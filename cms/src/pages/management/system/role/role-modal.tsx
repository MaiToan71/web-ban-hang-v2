import { Form, Input, Modal, Flex, Button, Radio, Tree } from "antd";
import { useEffect, useState } from "react";
import helper from "@/api/helper";
import { useMutation } from "@tanstack/react-query";
import type { Role } from "#/entity";
import { BasicStatus } from "#/enum";
import permission from "@/api/management/permission";
import role from "@/api/management/role";
export type RoleModalProps = {
  formValue: Role;
  title: string;
  show: boolean;
  onOk: VoidFunction;
  onCancel: VoidFunction;
};
import { toast } from "sonner";
export function RoleModal({
  title,
  show,
  formValue,
  onOk,
  onCancel,
}: RoleModalProps) {
  const [form] = Form.useForm();
  const [permissions, setPermissions] = useState<any>([]);
  const [checkedKeys, setCheckedKeys] = useState<any>([]);
  const searchMutation = useMutation({
    mutationFn: permission.searchItem,
  });
  const getPermission = async () => {
    let param: any = {
      PageIndex: 1,
      PageSize: 1000,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await searchMutation.mutateAsync(param);
    setPermissions(res.Items);
  };
  useEffect(() => {
    form.setFieldsValue({ ...formValue });
    setCheckedKeys(
      formValue?.Permissions.map((item: any) => {
        return item.Id;
      })
    );
  }, [formValue, form]);
  useEffect(() => {
    getPermission();
  }, [show]);

  const addMutation = useMutation({
    mutationFn: role.addItem,
  });
  const updateMutation = useMutation({
    mutationFn: role.updateItem,
  });

  const onCheck = (checkedKeys: any) => {
    console.log(checkedKeys);
    if (checkedKeys.checked && checkedKeys.checked) {
      setCheckedKeys(checkedKeys.checked);
    }
  };
  const onFinish = async (values: any) => {
    values.NormalizedName = "";

    values.PermissionIds = checkedKeys;
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
          label="Tên vai trò"
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

        <Form.Item label="Trạng thái" name="Status" required>
          <Radio.Group optionType="button" buttonStyle="solid">
            <Radio value={BasicStatus.ENABLE}>Hoạt động </Radio>
            <Radio value={BasicStatus.DISABLE}> Vô hiệu hóa</Radio>
          </Radio.Group>
        </Form.Item>
        <Form.Item label="Mô tả" name="Description">
          <Input.TextArea />
        </Form.Item>
        <Form.Item label="Quyền">
          <Tree
            checkable
            checkedKeys={checkedKeys}
            defaultCheckedKeys={checkedKeys}
            checkStrictly
            treeData={helper.convertToTreeSelect(
              permissions.map((i: any) => {
                return {
                  ...i,
                  parentId: i.ParentId,
                  title: i.Name,
                  label: i.Name,
                  key: i.Id,
                  value: i.Id,
                  id: i.Id,
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
