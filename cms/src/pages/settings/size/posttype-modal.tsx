import { Form, Input, Modal, Button, Flex } from "antd";
import { useEffect } from "react";
import { toast } from "sonner";
import posttype from "@/api/attributes/index.tsx";
import { useMutation } from "@tanstack/react-query";
import { AttributeEnum } from "#/enum";
export type PostTypeModalProps = {
  formValue: any;
  title: string;
  show: boolean;
  onOk: VoidFunction;
  onCancel: VoidFunction;
};

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
    values.Color = "";
    values.Description = ""
    values.Type = AttributeEnum.Size;
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
              message: "Nhập size!",
            },
          ]}
          label="Size"
          name="Name"
        >
          <Input />
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
