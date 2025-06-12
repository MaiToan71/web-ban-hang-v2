import { Form, Input, Modal, Flex, Button, Radio, Tree } from "antd";
import { useEffect, useState } from "react";
import helper from "@/api/helper";

import { useMutation } from "@tanstack/react-query";

import user from "@/api/management/user";
import role from "@/api/management/role";
export type UserModalProps = {
  formValue: any;
  title: string;
  show: boolean;
  onOk: VoidFunction;
  onCancel: VoidFunction;
};
import { toast } from "sonner";
export function UserModal({
  title,
  show,
  formValue,
  onOk,
  onCancel,
}: UserModalProps) {
  const [form] = Form.useForm();
  const [roles, setRoles] = useState<any>([]);
  const [checkedKeys, setCheckedKeys] = useState<any>([]);
  const searchMutation = useMutation({
    mutationFn: role.searchItem,
  });
  const getRole = async () => {
    let param: any = {
      PageIndex: 1,
      PageSize: 1000,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await searchMutation.mutateAsync(param);
    setRoles(res.Items);
  };
  useEffect(() => {
    form.setFieldsValue({ ...formValue });
    setCheckedKeys(
      formValue?.Roles.map((item: any) => {
        return item.Id;
      })
    );
  }, [formValue, form]);
  useEffect(() => {
    getRole();
  }, [show]);

  const addMutation = useMutation({
    mutationFn: user.addItem,
  });
  const updateMutation = useMutation({
    mutationFn: user.updateItem,
  });

  const onCheck = (checkedKeys: any) => {
    setCheckedKeys(checkedKeys);
  };
  const onFinish = async (values: any) => {
    values.RoleIds = checkedKeys;
    values.UserId = formValue.UserId;
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
      title={title}
      footer={null}
      open={show}
      onOk={onOk}
      onCancel={onCancel}
    >
      <Form
        onFinish={onFinish}
        initialValues={formValue}
        form={form}
        labelCol={{ span: 5 }}
        wrapperCol={{ span: 17 }}
        layout="horizontal"
      >
        <Form.Item
          label="Họ và tên"
          rules={[
            {
              required: true,
              message: "Nhập giá trị!",
            },
          ]}
          name="FullName"
          required
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Email"
          rules={[
            {
              required: true,
              message: "Nhập giá trị!",
            },
          ]}
          name="Email"
          required
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Số điện thoại"
          rules={[
            {
              required: true,
              message: "Nhập giá trị!",
            },
          ]}
          name="PhoneNumber"
          required
        >
          <Input />
        </Form.Item>

        <Form.Item label="Trạng thái" name="IsActive" required>
          <Radio.Group optionType="button" buttonStyle="solid">
            <Radio value={true}>Hoạt động </Radio>
            <Radio value={false}> Vô hiệu hóa</Radio>
          </Radio.Group>
        </Form.Item>

        <Form.Item label="Vai trò" name="Eoles">
          <Tree
            checkable
            checkedKeys={checkedKeys}
            defaultCheckedKeys={checkedKeys}
            treeData={helper.convertToTreeSelect(
              roles.map((i: any) => {
                return {
                  ...i,
                  parentId: 0,
                  title: i.Name,
                  label: i.Name,
                  key: i.Id,
                  value: i.Id,
                };
              })
            )}
            onCheck={onCheck}
            fieldNames={{
              key: "value",
              children: "children",
              title: "title",
            }}
          />
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
