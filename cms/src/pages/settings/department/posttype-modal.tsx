import { Form, Input, Modal, Button, Flex } from "antd";
import { useEffect } from "react";
import { toast } from "sonner";
import posttype from "@/api/departments/index.tsx";
import { useMutation } from "@tanstack/react-query";
import { PostTypeEnum } from "#/enum";
export type PostTypeModalProps = {
  formValue: any;
  title: string;
  show: boolean;
  onOk: VoidFunction;
  onCancel: VoidFunction;
};
const { TextArea } = Input;
export default function PosttypeModal({
  title,
  show,
  formValue,
  onOk,
  onCancel,
}: PostTypeModalProps) {
  const [form] = Form.useForm();

  const addMutation = useMutation({
    mutationFn: posttype.addItem,
  });
  const updateMutation = useMutation({
    mutationFn: posttype.updateItem,
  });

  useEffect(() => {
    form.setFieldsValue({ ...formValue });
  }, [formValue, form]);
  const onFinish = async (values: any) => {
    values.SortOrder = 0;
    values.ParentId = 0;
    values.Status = 0;
    values.PostTypeEnum = PostTypeEnum.Brand;
    let res = {
      Status: false,
    };
    if (title != "Chỉnh sửa") {
      res = await addMutation.mutateAsync(values);
    } else {
      values.Id = formValue.Id;
      res = await updateMutation.mutateAsync(values);
      //callBack()
    }
    if (res.Status) {
      toast.success("Thành công", {
        position: "top-right",
      });
      form.setFieldsValue({
        Name: "",
        Description: "",
        Phonenumber: "",
      });
    } else {
      toast.error("Thất bại", {
        position: "top-right",
      });
    }
    onOk();
  };
  return (
    <Modal
      footer={null}
      forceRender
      title={title}
      open={show}
      onOk={onOk}
      onCancel={onCancel}
    >
      <Form
        initialValues={formValue}
        form={form}
        onFinish={onFinish}
        labelCol={{ span: 6 }}
        wrapperCol={{ span: 18 }}
        layout="horizontal"
      >
        <Form.Item<any>
          rules={[
            {
              required: true,
              message: "Nhập tên  phòng ban!",
            },
          ]}
          label="Tên phòng ban"
          name="Name"
        >
          <Input />
        </Form.Item>

        <Form.Item<any>
          rules={[
            {
              required: true,
              message: "Nhập thông tin phòng ban!",
            },
          ]}
          label="Mô tả"
          name="Description"
        >
          <TextArea rows={3} />
        </Form.Item>

        <Flex justify={"end"}>
          <Button className="mr-2" onClick={() => onCancel()}>
            Hủy
          </Button>
          <Button type="primary" htmlType="submit">
            Đồng ý
          </Button>
        </Flex>
      </Form>
    </Modal>
  );
}
