import { Form, Input, Modal, Button, Select, Flex } from "antd";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import bank from "@/api/settings/bank";
import { useMutation } from "@tanstack/react-query";
import axios from "axios";
export type BankModalProps = {
  formValue: any;
  title: string;
  show: boolean;
  onOk: VoidFunction;
  onCancel: VoidFunction;
};

export default function BankModal({
  title,
  show,
  formValue,
  onOk,
  onCancel,
}: BankModalProps) {
  const [form] = Form.useForm();
  const [bankOptions, setBankOptions] = useState<any>([]);

  const fetchBanks = async () => {
    try {
      const response = await axios.get("https://api.vietqr.io/v2/banks");
      setBankOptions(response.data?.data);
    } catch (error) {
      console.error("Failed to fetch banks:", error);
    }
  };
  useEffect(() => {
    fetchBanks();
  }, [show]);
  const addMutation = useMutation({
    mutationFn: bank.addItem,
  });
  const updateMutation = useMutation({
    mutationFn: bank.updateItem,
  });

  useEffect(() => {
    form.setFieldsValue({ ...formValue });
  }, [formValue, form]);
  const onFinish = async (values: any) => {
    let shortName: any = bankOptions.filter(
      (i: any) => i.bin == values.BankId
    )[0]?.shortName;
    values.BankName = shortName;
    values.Id = formValue.Id;
    let res = {
      Status: false,
    };
    if (title != "Chỉnh sửa") {
      res = await addMutation.mutateAsync(values);
    } else {
      res = await updateMutation.mutateAsync(values);
      //callBack()
    }
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
              message: "Nhập tên ngân hàng!",
            },
          ]}
          label="Ngân hàng"
          name="BankId"
        >
          <Select
            showSearch
            placeholder="Select a person"
            optionFilterProp="label"
            options={bankOptions.map((i: any) => {
              return {
                label: `${i.shortName} - ${i.name}`,
                value: i.bin,
              };
            })}
          />
        </Form.Item>
        <Form.Item<any>
          rules={[
            {
              required: true,
              message: "Nhập số tài khoản ngân hàng!",
            },
          ]}
          label="Số tài khoản"
          name="AccountNo"
        >
          <Input />
        </Form.Item>
        <Form.Item<any>
          rules={[
            {
              required: true,
              message: "Nhập tên chủ số tài khoản ngân hàng!",
            },
          ]}
          label="Họ và tên"
          name="AccountName"
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
