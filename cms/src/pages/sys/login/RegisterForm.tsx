import { useMutation } from "@tanstack/react-query";
import { Button, Form, Input } from "antd";
import { useTranslation } from "react-i18next";

import userService from "@/api/services/userService";
import { toast } from "sonner";
import { ReturnButton } from "./components/ReturnButton";
import {
  LoginStateEnum,
  useLoginStateContext,
} from "./providers/LoginStateProvider";

function RegisterForm() {
  const { t } = useTranslation();
  const signUpMutation = useMutation({
    mutationFn: userService.signup,
  });

  const { loginState, backToLogin } = useLoginStateContext();
  if (loginState !== LoginStateEnum.REGISTER) return null;
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
    values.UserType = 0;
    values.Workflow = 0;
    let res: any = await signUpMutation.mutateAsync(values);
    if (res.Status) {
      toast.success("Thành công", {
        position: "top-right",
      });
      backToLogin();
    } else {
      toast.error(res.Data, {
        position: "top-right",
      });
    }
  };

  return (
    <>
      <div className="mb-4 text-2xl font-bold xl:text-3xl">
        {t("sys.login.signUpFormTitle")}
      </div>
      <Form
        name="normal_login"
        size="large"
        initialValues={{ remember: true }}
        onFinish={onFinish}
      >
        <p className="text-[red] mt-[4px] mb-[4px]">
          Lưu ý: Tên đăng nhập viết liền không dấu
        </p>
        <Form.Item
          name="UserName"
          rules={[{ required: true, message: "Nhập tên đăng nhập" }]}
        >
          <Input placeholder={"Tên đăng nhập(Viết liền không dấu)"} />
        </Form.Item>
        <Form.Item
          name="FullName"
          rules={[{ required: true, message: "Nhập họ và tên" }]}
        >
          <Input placeholder={"Nhập họ và tên"} />
        </Form.Item>
        <Form.Item
          name="Email"
          rules={[{ required: true, message: "Nhập địa chỉ email" }]}
        >
          <Input placeholder="Email" type="email" />
        </Form.Item>
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
        <Form.Item>
          <Button type="primary" htmlType="submit" className="w-full">
            {t("sys.login.registerButton")}
          </Button>
        </Form.Item>

        <ReturnButton onClick={backToLogin} />
      </Form>
    </>
  );
}

export default RegisterForm;
