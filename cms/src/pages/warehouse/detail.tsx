import { useEffect, useState } from "react";
import { Upload } from "@/components/upload";
import {
  Col,
  Form,
  Input,
  Row,
  Flex,
  Select,
  Button,
  Image,
  Popconfirm,
  InputNumber,
  Checkbox
} from "antd";
import Editor from "../../components/editor";
import Card from "@/components/card";
import { CloseOutlined } from "@ant-design/icons";
import post from "@/api/products/index.tsx";
import { FaBackspace } from "react-icons/fa";
import { toast } from "sonner";
import { IoSaveSharp } from "react-icons/io5";
import { DeleteOutlined } from "@ant-design/icons";
import posttype from "@/api/posttypes/index.tsx";
import Storage from "@/components/storage";
import attribute from "@/api/attributes";
import { useNavigate } from "react-router";
import { useMutation } from "@tanstack/react-query";
import { PostTypeEnum } from "#/enum";
import { AttributeEnum } from "#/enum";
import { useParams } from "react-router";
import moment from "moment";
const DetailPage = () => {
  const navigate = useNavigate();
  const [form] = Form.useForm();
  const [fileList, setFileList] = useState([]);
  const [content, setContent] = useState<any>("");
  const [workflow, setWorkflow] = useState<any>(1);
  let { id } = useParams();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const searchMutation = useMutation({
    mutationFn: posttype.searchItem,
  });
  const [posttypes, setPosttypes] = useState<any>([]);
  const getPostTypes = async () => {
    let param: any = {
      PageIndex: 1,
      PageSize: 1000,
      PostTypeEnum: PostTypeEnum.Brand,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await searchMutation.mutateAsync(param);
    setPosttypes(res.Items);
  };

  const [colors, setColors] = useState<any>([])
  const [tags, setTags] = useState<any>([])
  const getAttribute = async () => {
    let param: any = {
      PageIndex: 1,
      PageSize: 1000,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await attribute.searchItem(param);
    let newColors: any = []
    res.Items.map((i: any) => {
      if (i.Type == AttributeEnum.Color) {
        newColors.push(i)
      }
    })
    setColors(newColors)



    let newTags: any = []
    res.Items.map((i: any) => {
      if (i.Type == AttributeEnum.Tag) {
        newTags.push(i)
      }
    })
    setTags(newTags)

  }


  const [detail, setDetail] = useState<any>();
  const detailMutation = useMutation({
    mutationFn: post.getById,
  });
  const getDetail = async () => {
    const res: any = await detailMutation.mutateAsync(id);
    if (res.Status) {
      setContent(res.Data.Content);
      if (res.Data.InputDate && res.Data.InputDate) {
        res.Data.InputDate = moment(res.Data.InputDate).format("YYYY-MM-DD");
      }
      if (res.Data.ExportDate && res.Data.ExportDate) {
        res.Data.ExportDate = moment(res.Data.ExportDate).format("YYYY-MM-DD");
      }
      if (res.Data.ExpireDate && res.Data.ExpireDate) {
        res.Data.ExpireDate = moment(res.Data.ExpireDate).format("YYYY-MM-DD");
      }
      setDetail(res.Data);
      form.setFieldsValue({
        ...res.Data,
        Tags: res.Data.ProductAttributes.filter((i: any) => i.Type === AttributeEnum.Tag).map((i: any) => {
          if (i.Type === AttributeEnum.Tag) {
            return {
              label: i.Name,
              value: i.AttributeId
            }
          }
        }),
        Sizes: res.Data.ProductAttributes.filter((i: any) => i.Type === AttributeEnum.Size),
        Colors: res.Data.ProductAttributes.filter((i: any) => i.Type === AttributeEnum.Color)
      });
    }
  };

  useEffect(() => {
    getPostTypes();
    getDetail();
    getAttribute();
  }, []);

  if (!(detail && detail)) {
    return <div className="text-center">Không có dữ liệu</div>;
  }

  const onFinish = async (values: any) => {
    const formData: any = new FormData();
    formData.append("Id", detail.Id);
    formData.append("Code", values.Code);
    formData.append("Title", values.Title);
    formData.append("SubTitle", values.Title);
    formData.append("Url", values.Title);
    formData.append("Description", values.Title);
    formData.append("Content", content);
    formData.append("ShortContent", values.Title);
    formData.append("Source", values.Title);
    formData.append("CoverImage", values.Title);
    formData.append("IsPublished", values.IsPublished);
    formData.append("WorkflowId", workflow);
    formData.append("IsShowHome", false);
    formData.append("PostTypeId", values.PostTypeId);
    formData.append("Platform", 1);
    formData.append("Discount", values.Discount);
    formData.append("Quantity", values.Quantity);
    formData.append("QuantitySold", values.QuantitySold);
    formData.append("CapitalPrice", values.CapitalPrice);
    formData.append("SellingPrice", values.SellingPrice);
    let attributeInputs: any = []
    if (values.Colors != null) {
      attributeInputs = [...values.Colors]

    }
    if (values.Sizes != null) {
      values.Sizes.forEach((attr: any) => {
        attributeInputs.push(attr)
      })
    }
    if (values.Tags != undefined) {
      values.Tags.forEach((attr: any) => {
        if (attr.value == undefined) {
          attributeInputs.push({
            AttributeId: attr,
            Price: 0,
            Quantity: 0,
            Description: "1"
          })
        } else {
          attributeInputs.push({
            AttributeId: attr.value,
            Price: 0,
            Quantity: 0,
            Description: "1"
          })
        }
      });
    }
    attributeInputs.forEach((attr: any, index: any) => {
      formData.append(`Attributes[${index}].AttributeId`, attr.AttributeId);
      formData.append(`Attributes[${index}].Price`, attr.Price);
      formData.append(`Attributes[${index}].Quantity`, attr.Quantity);
      formData.append(`Attributes[${index}].Description`, attr.Description);
    });



    if (values.InputDate && values.InputDate) {
      formData.append("InputDate", values.InputDate);
    }
    if (values.ExportDate && values.ExportDate) {
      formData.append("ExportDate", values.ExportDate);
    }
    if (values.ExpireDate && values.ExpireDate) {
      formData.append("ExpireDate", values.ExpireDate);
    }
    fileList.map((f: any) => {
      formData.append("SelectedFilesImages", f.originFileObj);
    });
    let res: any = await post.updateImage(formData);
    if (res.Status) {
      toast.success("Thành công", {
        position: "top-right",
      });
      setFileList([]);
    } else {
      toast.error("Thất bại", {
        position: "top-right",
      });
    }
    getDetail();
    /**
   
    if (res.Status) {
      toast.success("Thành công", {
        position: "top-right",
      });
    } else {
      toast.error("Thất bại", {
        position: "top-right",
      });
    }
     */
    //
  };

  return (
    <div>
      <Storage isModalOpen={isModalOpen} setIsModalOpen={setIsModalOpen} />

      <Form
        initialValues={detail}
        className="w-full "
        layout="vertical"
        form={form}
        onFinish={onFinish}
      >
        <div className="mb-3 fixed z-10 right-0 top-0 h-[65px] bg-[white] pr-[50px] pt-[16px]">
          <Flex justify={"space-between"}>
            <Button icon={<FaBackspace />} onClick={() => navigate(-1)}>
              Quay lại
            </Button>
            <div>
              <Button
                onClick={() => setWorkflow(detail.WorkflowId)}
                type="primary"
                icon={<IoSaveSharp />}
                htmlType="submit"
                className="ml-[20px]"
              >
                Lưu trữ
              </Button>
            </div>
          </Flex>
        </div>
        <Row>
          <Col span={16}>
            <Card>
              <Flex vertical={true} style={{ width: "100%" }}>
                <h1 className="font-bold text-[18px]">Thông tin sản phẩm</h1>

                <Form.Item<any>
                  label="Tên mặt hàng"
                  name="Title"
                  className="w-full"
                  rules={[
                    {
                      required: true,
                      message: "Bạn nhập tên mặt hàng",
                    },
                  ]}
                >
                  <Input className="w-full" placeholder="Nhập tên mặt hàng" />
                </Form.Item>
                <Form.Item<any>
                  label="Mã sản phẩm"
                  name="Code"
                  className="w-full"
                  rules={[
                    {
                      required: true,
                      message: "Bạn nhập mã sản phẩm",
                    },
                  ]}
                >
                  <Input className="w-full" placeholder="Nhập mã mặt hàng" />
                </Form.Item>
                <Row gutter={16}>
                  <Col span={12}>
                    <Form.Item<any>
                      label="Ngày nhập"
                      name="InputDate"
                      className="w-full"
                    >
                      <Input type="date" style={{ width: "100%" }} />
                    </Form.Item>
                  </Col>
                  <Col span={12}>
                    <Form.Item<any>
                      label="Ngày xuất"
                      name="ExportDate"
                      className="w-full"
                    >
                      <Input type="date" style={{ width: "100%" }} />
                    </Form.Item>
                  </Col>
                  <Col span={12}>
                    <Form.Item<any>
                      label="Hạn sử dụng"
                      name="ExpireDate"
                      className="w-full"
                    >
                      <Input type="date" style={{ width: "100%" }} />
                    </Form.Item>
                  </Col>
                </Row>

                <Row gutter={16}>
                  <Col span={12}>
                    <Form.Item<any>
                      label="Giá vốn"
                      name="CapitalPrice"
                      className="w-full"
                      rules={[
                        {
                          required: true,
                          message: "Vui lòng  nhập giá vốn",
                        },
                      ]}
                    >
                      <Input type="number" />
                    </Form.Item>
                  </Col>
                  <Col span={12}>
                    <Form.Item<any>
                      label="Giá bán"
                      name="SellingPrice"
                      className="w-full"
                      rules={[
                        {
                          required: true,
                          message: "Bạn nhập giá bán",
                        },
                      ]}
                    >
                      <Input type="number" />
                    </Form.Item>
                  </Col>
                  <Col span={12}>
                    <Form.Item<any>
                      label="Giảm giá (%)"
                      name="Discount"
                      className="w-full"
                      rules={[
                        {
                          required: true,
                          message: "Nhập giảm giá",
                        },
                      ]}
                    >
                      <Input type="number" />
                    </Form.Item>
                  </Col>
                </Row>
                <div>
                  <h2 className="mb-2">Nhập mô tả sản phẩm</h2>
                  <Editor
                    value={content}
                    onChange={(e: any) => {
                      setContent(e);
                    }}
                  />
                </div>
              </Flex>
            </Card>
            <Card className="mt-[16px] ">
              {/* OrderDetails List */}
              <div className="flex flex-col justify-start">
                <h1 className="font-bold text-[18px]">Thông tin size</h1>
                <Form.List
                  name="Sizes"
                  initialValue={[
                    {
                      AttributeId: 0,
                      Price: 0,
                      Quantity: 0,
                      Description: ""
                    },
                  ]}
                >
                  {(fields, { add, remove }) => (
                    <div className="flex flex-col">
                      {fields.map(({ key, name, fieldKey, ...restField }: any) => (
                        <div key={key} className="flex" style={{ marginBottom: 16 }}>
                          <Row

                          >
                            <Col span={8} className="p-1">
                              <Form.Item
                                label="Kích cõ"
                                rules={[
                                  {
                                    required: true,
                                    message: "Vui lòng chọn kích cỡ!",
                                  },
                                ]}
                                {...restField}
                                name={[name, "AttributeId"]}
                                fieldKey={[fieldKey, "AttributeId"]}
                              >
                                <Select
                                  options={colors.map((i: any) => {
                                    return {
                                      value: i.Id,
                                      label: i.Name
                                    }
                                  })}
                                />
                              </Form.Item>
                            </Col>
                            <Col span={8} className="p-1">
                              <Form.Item
                                {...restField}
                                name={[name, "Description"]}
                                fieldKey={[fieldKey, "Description"]}
                                label="Mô tả (gam, cm ,...)"

                              >
                                <Input
                                  min={0}
                                  style={{ width: "100%", }}
                                />
                              </Form.Item>
                            </Col>
                            <Col span={8} className="p-1">
                              <Form.Item
                                {...restField}
                                name={[name, "Price"]}
                                fieldKey={[fieldKey, "Price"]}
                                label="Giá bán"
                                rules={[
                                  {
                                    required: true,
                                    message: "Nhập giá bán!",
                                  },
                                ]}
                              >
                                <InputNumber
                                  min={0}
                                  style={{ width: "100%", }}
                                />
                              </Form.Item>
                            </Col>
                            <Col span={8} className="p-1">
                              <Flex justify="space-between">
                                <Form.Item
                                  {...restField}
                                  name={[name, "Quantity"]}
                                  fieldKey={[fieldKey, "Quantity"]}
                                  label="Số lượng"
                                  rules={[
                                    {
                                      required: true,
                                      message: "Nhập số lượng!",
                                    },
                                  ]}
                                >
                                  <InputNumber
                                    min={0}
                                    style={{ width: "100%", }}
                                  />
                                </Form.Item>
                                <DeleteOutlined
                                  className="ml-[16px] text-[18px]"
                                  style={{ color: "red" }}
                                  onClick={() => remove(name)}
                                />

                              </Flex>
                            </Col>
                          </Row>
                        </div>
                      ))}
                      <Form.Item>
                        <Button
                          type="dashed"
                          onClick={() => add()}
                          icon={<i className="anticon anticon-plus-circle" />}
                        >
                          Thêm thuộc tính
                        </Button>
                      </Form.Item>

                    </div>
                  )}
                </Form.List>
              </div>
            </Card>


          </Col>
          <Col span={8} className="pl-3">
            <Card>

              <Form.Item name="IsPublished" valuePropName="checked">
                <Checkbox>Hiển thị</Checkbox>
              </Form.Item>
            </Card>
            <Card className="mt-3">

              <Form.Item<any>
                rules={[
                  {
                    required: true,
                    message: "Vui lòng  Chọn danh mục!",
                  },
                ]}
                className="w-full"
                label="Danh mục"
                name="PostTypeId"
              >
                <Select
                  className="w-full"
                  style={{ width: "100%" }}
                  placeholder="Chọn danh mục lưu trữ"
                  options={posttypes.map((i: any) => {
                    return {
                      value: i.Id,
                      label: i.Name,
                    };
                  })}
                />
              </Form.Item>
            </Card>
            <Card className="mt-3">
              <Form.Item<any>
                className="w-full"
                label="Từ khóa sản phẩm"
                name="Tags"
              >
                <Select
                  mode="multiple"
                  allowClear
                  className="w-full"
                  style={{ width: "100%" }}
                  placeholder="Chọn từ khóa sản phẩm"
                  options={tags.map((i: any) => {
                    return {
                      value: i.Id,
                      label: i.Name,
                    };
                  })}
                />
              </Form.Item>
            </Card>
            <Card className="mt-3">
              <Upload fileList={fileList} setFileList={setFileList} />
            </Card>
            <Card className="mt-3">
              <div>
                <label className="mb-2">Ảnh sản phẩm</label>
                <div className="text-[red] ml-[16px]">
                  {detail.Images.length > 0
                    ? ""
                    : "Vui lòng  cập nhật ảnh sản phẩm"}
                </div>

                <div>
                  <Flex justify="space-between" wrap>
                    {detail.Images.map((i: any) => {
                      return (
                        <Flex>
                          <Image
                            className="p-2"
                            style={{ width: "200px" }}
                            src={
                              import.meta.env.VITE_APP_BASE_API.slice(
                                0,
                                import.meta.env.VITE_APP_BASE_API.length - 4
                              ) + `${i.ImagePath}`
                            }
                          />
                          <Popconfirm
                            title="Xóa ảnh"
                            description="Bạn muốn xóa ảnh trên sản phẩm?"
                            onConfirm={async () => {
                              let res = await post.deleteImage(i.Id, detail.Id);
                              await getDetail();
                              if (res.Status) {
                                toast.success("Thành công", {
                                  position: "top-right",
                                });
                              } else {
                                toast.error("Thất bại", {
                                  position: "top-right",
                                });
                              }
                            }}
                            okText="Đồng ý"
                            cancelText="Hủy"
                          >
                            <Button
                              danger
                              style={{
                                marginTop: "15px",

                                marginLeft: "-44px",
                                height: "25px",
                                width: "25px",
                              }}
                            >
                              <CloseOutlined />
                            </Button>{" "}
                          </Popconfirm>
                        </Flex>
                      );
                    })}
                  </Flex>
                </div>
              </div>
            </Card>
          </Col>
          <Col span={24}>

          </Col>
        </Row>
      </Form>
    </div >
  );
};
export default DetailPage;
