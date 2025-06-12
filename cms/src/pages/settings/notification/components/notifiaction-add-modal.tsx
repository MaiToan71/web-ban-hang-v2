import { Button, Modal, Input, Flex, Form, Select } from "antd";
import { useState, useEffect } from "react";
import { useMutation } from "@tanstack/react-query";
import { toast } from "sonner";
import user from "@/api/management/user";
import notification from "@/api/settings/notification";
export default function NotificationAddModal({
  isModalOpen,
  setIsModalOpen,
  setIsReload,
}: any) {
  const searchUserMutation = useMutation({
    mutationFn: user.searchItem,
  });
  const addMutation = useMutation({
    mutationFn: notification.addItem,
  });
  const [users, setUsers] = useState<any>([]);
  const getUser = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: 1000,
    };
    const res: any = await searchUserMutation.mutateAsync(param);
    setUsers(res.Items);
  };
  useEffect(() => {
    if (isModalOpen) {
      getUser(1);
    }
  }, [isModalOpen]);
  const [form] = Form.useForm();

  const handleOk = () => {
    setIsModalOpen(false);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };
  const onFinish = async (values: any) => {
    values.IsSend = false;
    values.Workflow = 1;

    let res = await addMutation.mutateAsync(values);
    if (res.Status) {
      toast.success("Thành công", {
        position: "top-right",
      });
    } else {
      toast.error("Thất bại", {
        position: "top-right",
      });
    }
    setIsReload(true);
    handleOk();
  };
  return (
    <Modal
      title="Thêm mới thông báo"
      open={isModalOpen}
      onOk={handleOk}
      footer={null}
      onCancel={handleCancel}
    >
      <Form
        onFinish={onFinish}
        initialValues={{
          Name: "",
          Message: "",
          Users: [],
          ScheduleDate: "",
        }}
        form={form}
        layout="vertical"
      >
        <Form.Item
          label="Tiêu đề thông báo"
          rules={[
            {
              required: true,
              message: "Nhập giá trị!",
            },
          ]}
          name="Name"
          required
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Nội dung"
          name="Message"
          required
          rules={[
            {
              required: true,
              message: "Nhập giá trị!",
            },
          ]}
        >
          <Input.TextArea rows={6} />
        </Form.Item>
        <Form.Item
          label="Áp dụng cho CTV"
          name="Users"
          required
          rules={[
            {
              required: true,
              message: "Nhập giá trị!",
            },
          ]}
        >
          <Select
            mode="tags"
            style={{
              width: "100%",
            }}
            options={users.map((i: any) => {
              return {
                ...i,
                label: i.FullName,
                value: i.UserId,
              };
            })}
          />
        </Form.Item>
        <Form.Item
          label="Ngày gửi thông báo"
          name="ScheduleDate"
          rules={[
            {
              required: true,
              message: "Nhập ngày gửi thông báo!",
            },
          ]}
        >
          <Input type={"Date"} />
        </Form.Item>
        <Flex justify={"end"}>
          <Button className="mr-2" onClick={() => handleCancel()}>
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
