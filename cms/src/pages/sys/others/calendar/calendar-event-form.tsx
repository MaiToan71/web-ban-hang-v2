import { themeVars } from "@/theme/theme.css";
import {
  ColorPicker,
  DatePicker,
  Form,
  Input,
  Drawer,
  Radio,
  Space,
  Flex,
  Button,
  Select,
} from "antd";
import dayjs from "dayjs";
import { Workflow } from "#/enum";
import { SvgIcon } from "@/components/icon";

import { useEffect, useState } from "react";
import SelectUser from "@/pages/components/selectUsers";

const { RangePicker } = DatePicker;

import { useMutation } from "@tanstack/react-query";
import timeShiftType from "@/api/projects/timeshiftType";
import timeshift from "@/api/projects/timeshift";
import moment from "moment";
import { toast } from "sonner";

type Props = {
  type: "edit" | "add" | "view";
  open: boolean;
  onCancel: VoidFunction;
  initValues: any;
  onCallBack: any;
  component: any;
};

export default function CalendarEventForm({
  component,
  type,
  open,
  onCancel,
  initValues,
  onCallBack,
}: Props) {
  const title = type === "add" ? "Thêm mới chiến dịch" : "Chỉnh sửa chiến dịch";
  const [form] = Form.useForm();
  const [personInCharge, setPersonInCharge] = useState<any>();
  const [tags, setTags] = useState<any>([]);
  const [editItem, setEditItem] = useState<any>();
  let userStore: any = localStorage.getItem("userStore");
  let userId: any = JSON.parse(userStore).state.userInfo.UserId;
  useEffect(() => {
    if (type == "edit" || type == "view") {
      if (editItem && editItem) {
        setPersonInCharge({
          label: editItem.User.FullName,
          value: editItem.User.Id,
          key: editItem.User.Id,
        });
        setTags(
          editItem.Users.map((i: any) => {
            return {
              label: i.FullName,
              value: i.Id,
              key: i.Id,
            };
          })
        );
        form.setFieldsValue({
          ...editItem.TimeShift,

          Description: editItem.TimeShift.Note,
          PriorityEnum: editItem.TimeShift.PriorityEnum.toString(),
          RangePicker: [
            dayjs(
              moment(editItem.TimeShift.StartTime).format("DD-MM-YYYY"),
              "DD/MM/YYYY"
            ),
            dayjs(
              moment(editItem.TimeShift.EndTime).format("DD-MM-YYYY"),
              "DD/MM/YYYY"
            ),
          ],
        });
      }
    } else {
      setPersonInCharge("");
      setTags([]);
      form.setFieldsValue({
        Workflow: Workflow.WAITING,
        TimeShiftTypeId: 0,
        Description: "",
        PriorityEnum: 0,
        RangePicker: [],
        Name: "",
        Color: "#598543",
      });
    }
  }, [editItem, form, type]);

  //dự án
  const [products, setProducts] = useState<any>([]);
  const searchUserMutation = useMutation({
    mutationFn: timeShiftType.searchItem,
  });
  const getTimeshiftTypes = async () => {
    let param: any = {
      PageIndex: 1,
      PageSize: 1000,
    };
    const res: any = await searchUserMutation.mutateAsync(param);
    if (res.Status) {
      setProducts(res.Items);
      console.log(products);
    }
  };

  //

  const getByIdMutation = useMutation({
    mutationFn: timeshift.getById,
  });
  const getById = async (id: any) => {
    const res: any = await getByIdMutation.mutateAsync(id);
    if (res.Status) {
      setEditItem(res.Data);
    }
  };
  useEffect(() => {
    if (open) {
      getTimeshiftTypes();
      if (type == "edit" || type == "view") {
        if (initValues.id != 0) {
          getById(initValues.id);
        }
      } else {
        setEditItem(null);
      }
    }
  }, [open]);

  const addMutation = useMutation({
    mutationFn: timeshift.addItem,
  });
  const updateMutation = useMutation({
    mutationFn: timeshift.updateItem,
  });
  const onFinish = async (values: any) => {
    let check: any = true;

    if (!(personInCharge && personInCharge)) {
      check = false;
    }
    if (tags.length == 0) {
      check = false;
    }
    if (check) {
      let input: any = {
        Name: values.Name,
        Note: values.Description,
        StartTime: moment(values.RangePicker[0].$d).format("YYYY-MM-DD"),
        EndTime: moment(values.RangePicker[1].$d).format("YYYY-MM-DD"),
        TimeShiftTypeId: Number(values.TimeShiftTypeId),
        UserIds: tags.map((i: any) => {
          return i.value;
        }),
        Workflow: Number(values.Workflow),
        PersonInCharge: personInCharge.value,
        PriorityEnum: Number(values.PriorityEnum),
        Color: values.Color,
      };
      let res = {
        Status: false,
      };
      if (type == "add") {
        res = await addMutation.mutateAsync(input);
      } else {
        input.Id = editItem.TimeShift.Id;
        res = await updateMutation.mutateAsync(input);
        //callBack()
      }
      if (res.Status) {
        toast.success("Thành công", {
          position: "top-right",
        });
        onCancel();
        onCallBack();
      } else {
        toast.error("Thất bại", {
          position: "top-right",
        });
      }
    }
  };
  return (
    <Drawer open={open} title={title} width="700px" onClose={onCancel}>
      <Form
        onFinish={onFinish}
        form={form}
        labelCol={{ span: 7 }}
        wrapperCol={{ span: 18 }}
        initialValues={initValues}
      >
        <Form.Item<any>
          label="Trạng thái"
          name="Workflow"
          required
          rules={[
            {
              required: true,
              message: "Chọn trạng thái ",
            },
          ]}
        >
          <Radio.Group optionType="button" buttonStyle="solid">
            {type == "edit" ? (
              <Radio value={Workflow.CANCEL}> Hủy bỏ </Radio>
            ) : (
              <></>
            )}
            <Radio value={Workflow.WAITING}> Sẽ làm </Radio>
            <Radio value={Workflow.PROCESSING}> Đang tiến hành </Radio>
            {type == "edit" ? (
              <Radio value={Workflow.COMPLETED}> Hoàn thành </Radio>
            ) : (
              <></>
            )}
          </Radio.Group>
        </Form.Item>
        <Form.Item<any>
          label="Tên chiến dịch"
          name="Name"
          rules={[{ required: true, message: "Nhập tên chiến dịch!" }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Dự án"
          name="TimeShiftTypeId"
          rules={[
            {
              required: true,
              message: "Chọn dự án",
            },
          ]}
        >
          <Select
            showSearch
            placeholder="Chọn dự án"
            options={products.map((i: any) => {
              return {
                value: i.TimeShiftType.Id,
                label: i.TimeShiftType.Name,
              };
            })}
          />
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
        <Form.Item label="Người liên quan" name="Tags">
          <SelectUser
            value={tags}
            setValue={setTags}
            mode="multiple"
            placeholder={`Chọn liên quan`}
          />
          <span style={{ color: "red" }}>
            {personInCharge && personInCharge ? "" : "Chọn liên quan"}
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
                <span>Cần gấp</span>
              </Radio.Button>

              <Radio.Button value="2">
                <SvgIcon
                  icon="ic_rise"
                  size={20}
                  color={themeVars.colors.palette.success.default}
                  className="rotate-90"
                />
                <span>Quan trọng</span>
              </Radio.Button>

              <Radio.Button value="1">
                <SvgIcon
                  icon="ic_rise"
                  size={20}
                  color={themeVars.colors.palette.info.default}
                  className="rotate-180"
                />
                <span>Thoải mái</span>
              </Radio.Button>
            </Space>
          </Radio.Group>
        </Form.Item>
        <Form.Item<any>
          label="Hạn chót"
          name="RangePicker"
          rules={[{ required: true, message: "Nhập thời gian thực hiện!" }]}
        >
          <RangePicker />
        </Form.Item>

        <Form.Item<any> label="Mô tả" name="Description">
          <Input.TextArea />
        </Form.Item>

        <Form.Item<any>
          label="Màu sắc"
          name="Color"
          getValueFromEvent={(e) => e.toHexString()}
        >
          <ColorPicker />
        </Form.Item>
        {type != "view" ? (
          <Flex justify={"end"}>
            <Button className="mr-2" onClick={() => onCancel()}>
              Hủy
            </Button>
            {component == "EDIT" ? (
              <Button type="primary" htmlType="submit">
                Đồng ý
              </Button>
            ) : userId != personInCharge?.value ? (
              <></>
            ) : (
              <Button type="primary" htmlType="submit">
                Đồng ý
              </Button>
            )}
          </Flex>
        ) : (
          <></>
        )}
      </Form>
    </Drawer>
  );
}
