import { useEffect } from "react";
import { Button, Modal, Row, Col, Form, Flex, Popconfirm } from "antd";
import { format } from "date-fns";
import { vi } from "date-fns/locale";

import { TimePicker } from "antd";
import dayjs from "dayjs";
import workScheduleApi from "@/api/workSchedules";

import { useMutation } from "@tanstack/react-query";
import moment from "moment";
const PopupAddEmployeeClickedShift = ({
  isOpen,
  toggle,
  infoEmployeesClicked,
  selectedDates,
  refetchData,
  workDateId,
}: any) => {
  const [form] = Form.useForm();

  const getByIdMutation = useMutation({
    mutationFn: workScheduleApi.getById,
  });
  const getDataById = async () => {
    const res: any = await getByIdMutation.mutateAsync(workDateId);
    if (res.Status) {
      form.setFieldsValue({
        StartTime: dayjs(
          moment(res.Data.FromWorkDateRegis).format("HH:mm"),
          "HH:mm"
        ), // Initial time: 2:00 PM
        EndTime: dayjs(
          moment(res.Data.ToWorkDateRegis).format("HH:mm"),
          "HH:mm"
        ), // Initial time: 2:00 PM });
      });
    }
  };
  useEffect(() => {
    if (workDateId != 0) {
      getDataById();
    }
  }, [workDateId]);

  const formatDate = (date: any) =>
    format(new Date(date), "eeee, dd 'tháng' MM 'năm' yyyy", { locale: vi });

  const dateRangeText =
    selectedDates?.length > 1
      ? `${formatDate(selectedDates[0])} - ${formatDate(
          selectedDates[selectedDates?.length - 2]
        )}`
      : `${formatDate(selectedDates[0])}`;

  const convertToHour = (timeString: any) => {
    const [hours, minutes] = timeString.split(":").map(Number);
    const totalHours = hours + minutes / 60;

    return Number(totalHours);
  };
  const calculateDuration = (start: any, end: any) => {
    console.log(start);
    return convertToHour(start) + convertToHour(end);
  };

  const onFinish = async (values: any) => {
    try {
      const datesToProcess =
        selectedDates?.length > 2 ? selectedDates.slice(0, -1) : selectedDates;

      for (const date of datesToProcess) {
        const formattedDate = format(new Date(date), "yyyy-MM-dd");
        const payload = {
          LstCalendar: [
            {
              FromWorkDateRegis: `${formattedDate}T${format(
                values.StartTime,
                "HH:mm:ss"
              )}`,
              ToWorkDateRegis: `${formattedDate}T${format(
                values.EndTime,
                "HH:mm:ss"
              )}`,
              FromWorkDateEffect: `${formattedDate}T${format(
                values.StartTime,
                "HH:mm:ss"
              )}`,
              ToWorkDateEffect: `${formattedDate}T${format(
                values.EndTime,
                "HH:mm:ss"
              )}`,
              IsDeleted: false,
              Id: workDateId,
              EmployeeId:
                infoEmployeesClicked?.resource?._resource?.id ||
                infoEmployeesClicked?._def?.resourceIds[0],
              EmployeeName:
                infoEmployeesClicked?.resource?._resource?.title || "",
              TotalHours: calculateDuration(
                format(values.StartTime, "HH:mm"),
                format(values.EndTime, "HH:mm")
              ),
              WorkType: "Normal",
              Multiplier: 1,
              WorkRecordStatus: "AdminAccept",
            },
          ],
        };

        await workScheduleApi.updateAdmin(payload);
      }

      /**  emitter.emit("FIRE_NOTIFICATION", {
        level: "success",
        message: "Cập nhật lịch làm việc thành công!",
      });
*/
      refetchData();
      toggle();
    } catch (error) {
      console.error("Lỗi API:", error);
      /** emitter.emit("FIRE_NOTIFICATION", {
        level: "error",
        message: "Có lỗi xảy ra khi cập nhật lịch làm việc.",
      }); */
    }
  };

  const onCancel = () => {
    toggle();
  };

  const onRemove = async () => {
    await workScheduleApi.deleteItem(workDateId);
    refetchData();
    toggle();
  };
  return (
    <Modal
      title="Giờ làm việc"
      open={isOpen}
      footer={false}
      onOk={onCancel}
      onCancel={onCancel}
    >
      <Form
        initialValues={{
          StartTime: dayjs("00:00", "HH:mm"), // Initial time: 2:00 PM
          EndTime: dayjs("00:00", "HH:mm"), // Initial time: 2:00 PM
        }}
        onFinish={onFinish}
        form={form}
        labelCol={{ span: 12 }}
        wrapperCol={{ span: 24 }}
        layout="horizontal"
      >
        <Row>
          <Col span={24}>
            <label className="text-nowrap font-medium ">{dateRangeText}</label>
          </Col>

          <Col span={24} className="mt-[16px]">
            <Flex>
              <Form.Item
                label="Bắt đầu"
                rules={[
                  {
                    required: true,
                    message: "Nhập giá trị!",
                  },
                ]}
                name="StartTime"
                required
              >
                <TimePicker
                  format="HH:mm"
                  allowClear={false}
                  placeholder="Giờ bắt đầu"
                />
              </Form.Item>
              <Form.Item
                label="Kết thúc"
                rules={[
                  {
                    required: true,
                    message: "Nhập giá trị!",
                  },
                ]}
                name="EndTime"
                required
              >
                <TimePicker
                  format="HH:mm"
                  allowClear={false}
                  placeholder="Giờ kết thúc"
                />
              </Form.Item>
            </Flex>
          </Col>
        </Row>
        <Flex justify={"end"}>
          <Button className="mr-2" onClick={() => onCancel()}>
            Hủy
          </Button>
          <Popconfirm
            title="Xóa dữ liệu"
            description="Bạn muốn xóa giờ làm việc?"
            onConfirm={() => onRemove()}
            okText="Đồng ý"
            cancelText="Hủy"
          >
            <Button className="mr-2" danger>
              Xóa
            </Button>
          </Popconfirm>
          <Button type="primary" htmlType="submit">
            Đồng ý
          </Button>
        </Flex>
      </Form>
    </Modal>
  );
};

export default PopupAddEmployeeClickedShift;
