import {
  AutoComplete,
  Form,
  Input,
  InputNumber,
  Modal,
  Radio,
  TreeSelect,
  Button,
  Flex,
} from "antd";
import { useCallback, useEffect, useState } from "react";
import { toast } from "sonner";

import helper from "@/api/helper";
import type { Permission } from "#/entity";
import { BasicStatus, PermissionType } from "#/enum";
import permission from "@/api/management/permission";
import { useMutation } from "@tanstack/react-query";
const ENTRY_PATH = "/src/pages";
const PAGES = import.meta.glob("/src/pages/**/*.tsx");
const PAGE_SELECT_OPTIONS = Object.entries(PAGES).map(([path]) => {
  const pagePath = path.replace(ENTRY_PATH, "");
  return {
    label: pagePath,
    value: pagePath,
  };
});

export type PermissionModalProps = {
  formValue: any;
  title: string;
  show: boolean;
  onOk: VoidFunction;
  onCancel: VoidFunction;
  permissions: any;
};

export default function PermissionModal({
  permissions,
  title,
  show,
  formValue,
  onOk,
  onCancel,
}: PermissionModalProps) {
  const [form] = Form.useForm();
  const addMutation = useMutation({
    mutationFn: permission.addItem,
  });
  const updateMutation = useMutation({
    mutationFn: permission.updateItem,
  });
  const [compOptions, setCompOptions] = useState(PAGE_SELECT_OPTIONS);

  const getParentNameById = useCallback(
    (parentId: string, data: Permission[] | undefined = permissions) => {
      let name = "";
      if (!data || !parentId) return name;
      for (let i = 0; i < data.length; i += 1) {
        if (data[i].id === parentId) {
          name = data[i].name;
        } else if (data[i].children) {
          name = getParentNameById(parentId, data[i].children);
        }
        if (name) {
          break;
        }
      }
      return name;
    },
    [permissions]
  );

  const updateCompOptions = (name: string) => {
    if (!name) return;
    setCompOptions(
      PAGE_SELECT_OPTIONS.filter((path) => {
        return path.value.includes(name.toLowerCase());
      })
    );
  };

  // biome-ignore lint/correctness/useExhaustiveDependencies: <explanation>
  useEffect(() => {
    form.setFieldsValue({ ...formValue });
    if (formValue.ParentId) {
      const parentName = getParentNameById(formValue.ParentId);
      updateCompOptions(parentName);
    }
  }, [formValue, form, getParentNameById]);
  const onFinish = async (values: any) => {
    if (values.Status == 1) {
      values.Status = true;
    } else {
      values.Status = false;
    }
    values.NormalizedName = "";
    values.PermissionType = values.Type;
    values.Id = formValue.Id;

    if (values.ParentId != "") {
      values.ParentId = values.ParentId;
    } else {
      values.ParentId = 0;
    }
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
        <Form.Item<any> label="Loại" name="Type" required>
          <Radio.Group optionType="button" buttonStyle="solid">
            <Radio value={PermissionType.MENU}>MENU</Radio>
            <Radio value={PermissionType.BUTTON}>BUTTON</Radio>
          </Radio.Group>
        </Form.Item>

        <Form.Item<any>
          label="Tên quyền"
          name="Name"
          required
          rules={[
            {
              required: true,
              message: "Nhập giá trị!",
            },
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item<any>
          label="Tiêu đề"
          name="Label"
          rules={[
            {
              required: true,
              message: "Nhập giá trị!",
            },
          ]}
          required
          tooltip="internationalization config"
        >
          <Input />
        </Form.Item>

        <Form.Item<any> label="Danh mục cha" name="ParentId">
          <TreeSelect
            fieldNames={{
              label: "Name",
              value: "Id",
              children: "children",
            }}
            allowClear
            treeData={helper.convertToTreeSelect(
              permissions.map((i: any) => {
                return {
                  ...i,
                  parentId: i.ParentId,
                  title: i.Title,
                  label: i.Label,
                  value: i.Id,
                };
              })
            )}
            onChange={(_value, labelList) => {
              updateCompOptions(labelList[0] as string);
            }}
          />
        </Form.Item>

        <Form.Item<any>
          label="Đường dẫn"
          rules={[
            {
              required: true,
              message: "Nhập giá trị!",
            },
          ]}
          name="Route"
          required
        >
          <Input />
        </Form.Item>

        <Form.Item
          noStyle
          shouldUpdate={(prevValues, currentValues) =>
            prevValues.Type !== currentValues.Type
          }
        >
          {({ getFieldValue }) => {
            if (getFieldValue("Type") === PermissionType.MENU) {
              return (
                <Form.Item<any>
                  label="Component"
                  name="Component"
                  rules={[
                    {
                      required: true,
                      message: "Nhập giá trị!",
                    },
                  ]}
                  required={getFieldValue("Type") === PermissionType.MENU}
                >
                  <AutoComplete
                    options={compOptions}
                    filterOption={(input, option) =>
                      ((option?.label || "") as string)
                        .toLowerCase()
                        .includes(input.toLowerCase())
                    }
                  />
                </Form.Item>
              );
            }
            return null;
          }}
        </Form.Item>

        <Form.Item<any>
          label="Icon"
          name="Icon"
          tooltip="local icon should start with ic"
        >
          <Input />
        </Form.Item>

        <Form.Item<any> label="Ẩn" name="Hide" tooltip="Ẩn thanh menu">
          <Radio.Group optionType="button" buttonStyle="solid">
            <Radio value={false}>Hiển thị</Radio>
            <Radio value>Ẩn</Radio>
          </Radio.Group>
        </Form.Item>

        <Form.Item<any> label="Số thứ tự" name="Order">
          <InputNumber style={{ width: "100%" }} />
        </Form.Item>

        <Form.Item<any> label="Trạng thái" name="Status" required>
          <Radio.Group optionType="button" buttonStyle="solid">
            <Radio value={BasicStatus.ENABLE}> Hiện </Radio>
            <Radio value={BasicStatus.DISABLE}> Ẩn </Radio>
          </Radio.Group>
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
