import { useState, useEffect } from "react";

import { SvgIcon } from "@/components/icon";
import SelectUser from "@/pages/components/selectUsers";
import { themeVars } from "@/theme/theme.css";
import {
  Button,
  Drawer,
  Form,
  Flex,
  Radio,
  Space,
  Input,
  DatePicker,
  Timeline,
  Tabs,
} from "antd";
import dayjs from "dayjs";
import { useMutation } from "@tanstack/react-query";
import { toast } from "sonner";

import timeshiftType from "@/api/projects/timeshiftType";
import moment from "moment";
const { TextArea } = Input;
const { RangePicker } = DatePicker;
const ProductModal = ({ open, setOpen, title, onOk, type, id }: any) => {
  const [editItem, setEditItem] = useState<any>();
  const onClose = () => {
    setOpen(false);
  };
  const [form] = Form.useForm();
  const [personInCharge, setPersonInCharge] = useState<any>("");

  const addMutation = useMutation({
    mutationFn: timeshiftType.addItem,
  });
  const updateMutation = useMutation({
    mutationFn: timeshiftType.updateItem,
  });
  const getByIdMutation = useMutation({
    mutationFn: timeshiftType.getById,
  });
  const getById = async (id: any) => {
    const res: any = await getByIdMutation.mutateAsync(id);
    if (res.Status) {
      setEditItem(res.Data);
    }
  };
  useEffect(() => {
    if (open) {
      if (type == "edit") {
        if (id != 0) {
          getById(id);
        }
      } else {
        setEditItem(null);
      }
    }
  }, [open]);
  useEffect(() => {
    if (editItem && editItem) {
      setPersonInCharge({
        ...editItem.UserViewModel,
        label: editItem.UserViewModel.FullName,
        value: editItem.UserViewModel.Id,
        key: editItem.UserViewModel.Id,
      });
      form.setFieldsValue({
        ...editItem.TimeShiftType,
        PriorityEnum: editItem.TimeShiftType.PriorityEnum.toString(),
        RangePicker: [
          dayjs(
            moment(editItem.TimeShiftType.StartTime).format("DD-MM-YYYY"),
            "DD/MM/YYYY"
          ),
          dayjs(
            moment(editItem.TimeShiftType.EndTime).format("DD-MM-YYYY"),
            "DD/MM/YYYY"
          ),
        ],
      });
    } else {
      setPersonInCharge("");
      form.setFieldsValue({
        Name: "",
        RangePicker: [],
        PriorityEnum: 0,
        Description: "",
      });
    }
  }, [editItem, form]);
  const onFinish = async (values: any) => {
    let check: any = true;

    if (!(personInCharge && personInCharge)) {
      check = false;
    }
    if (check) {
      let input: any = {
        Name: values.Name,
        Description: values.Description,
        BackgroundColor: "string",
        TextColor: "string",
        Note: "string",
        StartTime: moment(values.RangePicker[0].$d).format("YYYY-MM-DD"),
        EndTime: moment(values.RangePicker[1].$d).format("YYYY-MM-DD"),
        Workflow: 4,
        PersonInCharge: personInCharge.value,
        PriorityEnum: Number(values.PriorityEnum),
      };
      let res = {
        Status: false,
      };
      if (!(editItem && editItem)) {
        res = await addMutation.mutateAsync(input);
      } else {
        input.Id = editItem.TimeShiftType.Id;
        res = await updateMutation.mutateAsync(input);
        //callBack()
      }
      if (res.Status) {
        toast.success("Thành công", {
          position: "top-right",
        });
        onOk();
      } else {
        toast.error("Thất bại", {
          position: "top-right",
        });
      }
      onClose();
    }
  };

  const items = [
    {
      key: "1",
      label: "Chi tiết dự án",
      children: (
        <Form
          onFinish={onFinish}
          initialValues={editItem}
          form={form}
          labelCol={{ span: 7 }}
          wrapperCol={{ span: 18 }}
          layout="horizontal"
        >
          <Form.Item
            label="Tên dự án"
            name="Name"
            rules={[
              {
                required: true,
                message: "Nhập tên dự án",
              },
            ]}
          >
            <TextArea rows={2} />
          </Form.Item>
          <Form.Item label="Người phụ trách" name="PersonInCharge">
            <SelectUser
              value={personInCharge}
              setValue={setPersonInCharge}
              mode=""
              placeholder={`Chọn người phụ trách`}
            />
            <span style={{ color: "red" }}>
              {personInCharge && personInCharge ? "" : "Chọn  người phụ trách"}
            </span>
          </Form.Item>

          <Form.Item
            label="Độ ưu tiên"
            name="PriorityEnum"
            rules={[
              {
                required: true,
                message: "Chọn độ ưu tiên",
              },
            ]}
          >
            <Radio.Group>
              <Space>
                <Radio.Button value="3">
                  <SvgIcon
                    icon="ic_rise"
                    size={20}
                    color={themeVars.colors.palette.warning.default}
                  />
                  <span>Cao</span>
                </Radio.Button>

                <Radio.Button value="2">
                  <SvgIcon
                    icon="ic_rise"
                    size={20}
                    color={themeVars.colors.palette.success.default}
                    className="rotate-90"
                  />
                  <span>Trung bình</span>
                </Radio.Button>

                <Radio.Button value="1">
                  <SvgIcon
                    icon="ic_rise"
                    size={20}
                    color={themeVars.colors.palette.info.default}
                    className="rotate-180"
                  />
                  <span>Thấp</span>
                </Radio.Button>
              </Space>
            </Radio.Group>
          </Form.Item>
          <Form.Item
            label="Thời gian thực hiện"
            name="RangePicker"
            rules={[
              {
                required: true,
                message: "Nhập thời gian thực hiện!",
              },
            ]}
          >
            <RangePicker />
          </Form.Item>

          <Form.Item label="Mô tả" name="Description">
            <TextArea rows={4} />
          </Form.Item>

          <Flex justify={"end"}>
            <Button className="mr-2" onClick={() => onClose()}>
              Hủy
            </Button>
            <Button type="primary" htmlType="submit">
              Đồng ý
            </Button>
          </Flex>
        </Form>
      ),
    },
    {
      key: "2",
      label: "Đang thực hiện",
      children: (
        <div className="m-2">
          <Timeline
            items={editItem?.TimeShifts.map((i: any) => {
              return {
                color: i.Workflow == 4 ? "green" : "red",
                children: (
                  <p
                    style={{
                      textDecoration: i.Workflow == 4 ? "line-through" : "none",
                    }}
                  >
                    {i.Name}
                  </p>
                ),
              };
            })}
          />
        </div>
      ),
    },
  ];
  return (
    <>
      <Drawer title={title} width="600px" onClose={onClose} open={open}>
        <Tabs defaultActiveKey="1" items={items} />
      </Drawer>
    </>
  );
};
export default ProductModal;
