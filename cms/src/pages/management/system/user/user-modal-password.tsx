import { Form, Modal, Button } from "antd";
import { Input, Flex } from "antd";

import user from "@/api/management/user";
import { toast } from "sonner";
export default function UserPasswordModal({
  isOpenPassword,
  setIsOpenPassword,
  editItem,
}: any) {
  const [form] = Form.useForm();
  const onOk = () => {
    setIsOpenPassword(false);
  };
  const onCancel = () => {
    setIsOpenPassword(false);
  };
  const validatePassword = (_: any, value: any) => {
    if (!value) {
      return Promise.reject("Bạn chưa nhập mật khẩu!");
    }
    if (!/[A-Z]/.test(value)) {
      return Promise.reject("Mật khẩu phải chứa ít nhất một chữ in hoa");
    }
    if (value.length < 12) {
      return Promise.reject("Mật khẩu của bạn phải có ít nhất 12 ký tự!");
    }
    if (!/\d/.test(value) || !/[a-zA-Z]/.test(value)) {
      return Promise.reject("Mật khẩu phải chứa cả chữ cái và số!");
    }
    if (!/[!@#$%^&*()]/.test(value)) {
      return Promise.reject(
        "Mật khẩu phải chứa ít nhất một ký tự đặc biệt (!@#$%^&*())."
      );
    }
    return Promise.resolve();
  };
  const onFinish = async (values: any) => {
    let input: any = {
      UserId: editItem.UserId,
      NewPassword: values.Password,
      ConfirmPassword: values.ConfirmPassword,
    };
    const res: any = await user.updatePassword(input);
    if (res.Status) {
      toast.success("Thành công", {
        position: "top-right",
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
      title={`Chỉnh sửa thông tin mật khẩu`}
      footer={null}
      open={isOpenPassword}
      onOk={onOk}
      onCancel={onCancel}
    >
      <Form onFinish={onFinish} form={form} layout="horizontal">
        <Form.Item name="Password" rules={[{ validator: validatePassword }]}>
          <Input.Password type="password" placeholder="Nhập mật khẩu" />
        </Form.Item>
        <Form.Item
          name="ConfirmPassword"
          rules={[
            {
              required: true,
              message: "Xác nhận mật khẩu",
            },
            ({ getFieldValue }) => ({
              validator(_: any, value) {
                if (!value || getFieldValue("Password") === value) {
                  return Promise.resolve();
                }
                return Promise.reject(new Error("Mật khẩu không trùng khớp"));
              },
            }),
          ]}
        >
          <Input.Password type="password" placeholder="Xác nhận mật khẩu " />
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
