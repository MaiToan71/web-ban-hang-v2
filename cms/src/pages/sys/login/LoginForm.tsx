import { Button, Checkbox, Col, Form, Input, Row } from "antd";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import Logo from "@/assets/images/logo.png";
import type { SignInReq } from "@/api/services/userService";
import { useSignIn } from "@/store/userStore";
import { Image } from "antd";
import {
  LoginStateEnum,
  useLoginStateContext,
} from "./providers/LoginStateProvider";

function LoginForm() {
  const { t } = useTranslation();
  const [loading, setLoading] = useState(false);

  const { loginState, setLoginState } = useLoginStateContext();
  const signIn = useSignIn();

  if (loginState !== LoginStateEnum.LOGIN) return null;

  const handleFinish = async ({ Username, Password }: SignInReq) => {
    setLoading(true);
    try {
      await signIn({ Username, Password });
    } finally {
      setLoading(false);
    }
  };
  return (
    <>
      <div className="flex justify-center w-full">
        <Image width={150} src={Logo} />
      </div>
      <div className="mb-4 text-2xl font-bold xl:text-3xl">
        {t("sys.login.signInFormTitle")}
      </div>
      <Form
        name="login"
        size="large"
        initialValues={{
          RememberMe: true,
          Username: "",
          Password: "",
        }}
        onFinish={handleFinish}
      >
        <Form.Item
          name="Username"
          rules={[
            { required: true, message: t("sys.login.accountPlaceholder") },
          ]}
        >
          <Input placeholder="Email" />
        </Form.Item>
        <Form.Item
          name="Password"
          rules={[
            { required: true, message: t("sys.login.passwordPlaceholder") },
          ]}
        >
          <Input.Password type="password" placeholder="Mật khẩu" />
        </Form.Item>
        <Form.Item>
          <Row align="middle">
            <Col span={12}>
              <Form.Item name="RememberMe" valuePropName="checked" noStyle>
                <Checkbox>{t("sys.login.rememberMe")}</Checkbox>
              </Form.Item>
            </Col>
            <Col span={12} className="text-right">
              <Button
                type="link"
                className="!underline"
                onClick={() => setLoginState(LoginStateEnum.RESET_PASSWORD)}
              >
                {t("sys.login.forgetPassword")}
              </Button>
            </Col>
          </Row>
        </Form.Item>
        <Form.Item>
          <Button
            type="primary"
            htmlType="submit"
            className="w-full"
            loading={loading}
          >
            {t("sys.login.loginButton")}
          </Button>
        </Form.Item>

        <Row>
          <Col span={24} onClick={() => setLoginState(LoginStateEnum.REGISTER)}>
            <Button className="w-full !text-sm">
              {t("sys.login.signUpFormTitle")}
            </Button>
          </Col>
        </Row>
      </Form>
    </>
  );
}

export default LoginForm;
