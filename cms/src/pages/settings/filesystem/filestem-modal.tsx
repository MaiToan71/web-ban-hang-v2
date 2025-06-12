import {
  Form,
  Input,
  Modal,
  Button,
  Select,
  Flex,
  Upload,
  message,
} from "antd";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import posttype from "@/api/posttypes/index.tsx";
import { useMutation } from "@tanstack/react-query";
import { UploadOutlined } from "@ant-design/icons";
import file from "@/api/files";
export type PostTypeModalProps = {
  formValue: any;
  title: string;
  show: boolean;
  onOk: VoidFunction;
  onCancel: VoidFunction;
};

export default function FileSystemModal({
  title,
  show,
  formValue,
  onOk,
  onCancel,
}: PostTypeModalProps) {
  const searchMutation = useMutation({
    mutationFn: posttype.searchItem,
  });
  const [form] = Form.useForm();

  const addMutation = useMutation({
    mutationFn: file.addItem,
  });

  const [data, setData] = useState<any>([]);
  const getData = async () => {
    let param: any = {
      PageIndex: 1,
      PageSize: 1000,
      PostTypeEnum: 0,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await searchMutation.mutateAsync(param);
    setData(res.Items);
  };
  useEffect(() => {
    if (show) {
      getData();
    }
  }, [show]);
  useEffect(() => {
    if (formValue && formValue) {
      if (formValue.Categories && formValue.Categories) {
        formValue.CategoryId = formValue.Categories[0].Id;
      }
    }
    form.setFieldsValue({ ...formValue });
  }, [formValue, form]);
  const handleUpload = (file: any) => {
    // Custom file validation or processing can be done here
    const isJpgOrPng = file.type === "image/jpeg" || file.type === "image/png";
    if (!isJpgOrPng) {
      message.error("You can only upload JPG/PNG file!");
    }
    return isJpgOrPng;
  };
  const onFinish = async (values: any) => {
    const { Caption, upload, CategoryId } = values;
    const formData = new FormData();
    formData.append("Title", Caption);
    let newCategoryId: any = Number(CategoryId);
    formData.append("CategoryIds[]", newCategoryId);
    if (upload && upload[0]) {
      // console.log(upload);
      formData.append("ImageFile", upload[0].originFileObj); // Assuming single file upload
    }
    let res = {
      Status: false,
    };
    res = await addMutation.mutateAsync(formData);
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
        labelCol={{ span: 8 }}
        wrapperCol={{ span: 16 }}
        layout="horizontal"
      >
        <Form.Item<any>
          rules={[
            {
              required: true,
              message: "Nhập tên  tên file!",
            },
          ]}
          label="Tên file"
          name="Caption"
        >
          <Input />
        </Form.Item>
        <Form.Item<any>
          rules={[
            {
              required: true,
              message: "Bạn chưa chọn danh mục lưu trữ!",
            },
          ]}
          label="Kho lưu trữ"
          name="CategoryId"
        >
          <Select
            placeholder="Chọn danh mục lưu trữ"
            options={data.map((i: any) => {
              return {
                value: i.Id,
                label: i.Name,
              };
            })}
          />
        </Form.Item>
        <Form.Item
          name="upload"
          label="Tải ảnh"
          valuePropName="fileList"
          getValueFromEvent={(e) => e?.fileList}
          rules={[{ required: true, message: "Please upload a file!" }]}
        >
          <Upload
            name="file"
            action="/upload.do" // replace with your server's upload URL
            beforeUpload={handleUpload}
            listType="picture"
            accept=".jpg,.png"
          >
            <Button icon={<UploadOutlined />}>Click to Upload</Button>
          </Upload>
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
