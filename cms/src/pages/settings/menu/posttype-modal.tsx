import { Form, Input, Modal, TreeSelect, Button, Flex, Popconfirm } from "antd";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import posttype from "@/api/menus/index.tsx";
import { IconButton, Iconify } from "@/components/icon";
import { useMutation } from "@tanstack/react-query";
import { PostTypeEnum } from "#/enum";
import helper from "@/api/helper";
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
  const [items, setItems] = useState<any>([])
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE * 100);
  const getData = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await posttype.searchItem(param);
    setItems(res.Items)
  };
  const deleteMutation = useMutation({
    mutationFn: posttype.deleteItem,
  });
  useEffect(() => {
    if (show) {
      getData(1);
    }
  }, [show]);

  useEffect(() => {
    form.setFieldsValue({ ...formValue });
  }, [formValue, form]);
  const onFinish = async (values: any) => {
    values.SortOrder = 0;
    values.Status = 0;
    values.RoleIds = []
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
        <Form.Item<any> label="Danh mục cha" name="ParentId">
          <TreeSelect
            fieldNames={{
              label: "Name",
              value: "Id",
              children: "children",
            }}
            allowClear
            treeData={helper.convertToTreeSelect(items.map((i: any) => {
              return {
                ...i,
                id: i.Id,
                title: i.Name,
                parentId: i.ParentId,
                name: i.Name,
                value: i.Id,
              }
            }))}

          />
        </Form.Item>
        <Form.Item<any>
          rules={[
            {
              required: true,
              message: "Nhập tên  menu!",
            },
          ]}
          label="Tên"
          name="Name"
        >
          <Input />
        </Form.Item>

        <Form.Item<any>
          rules={[
            {
              required: true,
              message: "Nhập thông tin đường dẫn!",
            },
          ]}
          label="Đường dẫn"
          name="Url"
        >
          <TextArea rows={3} />
        </Form.Item>

        <Flex justify={"end"}>
          <Popconfirm
            title="Xóa danh mục"
            okText="Đồng ý"
            cancelText="Hủy"
            placement="left"
            onConfirm={async () => {
              let res = await deleteMutation.mutateAsync(formValue.Id);
              await getData(1);
              if (res.Status) {
                toast.success("Thành công", {
                  position: "top-right",
                });
                onOk()
              } else {
                toast.error("Thất bại", {
                  position: "top-right",
                });
              }
            }}
          >
            <IconButton>
              <Iconify
                icon="mingcute:delete-2-fill"
                size={18}
                className="text-error"
              />
            </IconButton>
          </Popconfirm>
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
