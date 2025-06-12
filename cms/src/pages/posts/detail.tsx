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
} from "antd";
import { CloseOutlined } from "@ant-design/icons";
import Editor from "../../components/editor";
import Card from "@/components/card";
import { PlatformData } from "#/enum";
import post from "@/api/posts/index.tsx";
import { FaBackspace } from "react-icons/fa";
import { toast } from "sonner";
import { IoSaveSharp } from "react-icons/io5";
import { BsFillSendFill } from "react-icons/bs";
import posttype from "@/api/posttypes/index.tsx";
import Storage from "@/components/storage";
import { TiArrowBack } from "react-icons/ti";
import { useNavigate } from "react-router";
import { useMutation } from "@tanstack/react-query";
import { PostTypeEnum } from "#/enum";
import { useParams } from "react-router";

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
      PostTypeEnum: PostTypeEnum.Category,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await searchMutation.mutateAsync(param);
    setPosttypes(res.Items);
  };

  const [detail, setDetail] = useState<any>();
  const detailMutation = useMutation({
    mutationFn: post.getById,
  });
  const getDetail = async () => {
    const res: any = await detailMutation.mutateAsync(id);
    if (res.Status) {
      setDetail(res.Data);
      setContent(res.Data.Content);
      form.setFieldsValue({ ...res.Data });
    }
  };

  useEffect(() => {
    getPostTypes();
    getDetail();
  }, []);

  const updateMutation = useMutation({
    mutationFn: post.updateItem,
  });
  if (!(detail && detail)) {
    return <div className="text-center">Không có dữ liệu</div>;
  }
  const onFinish = async (values: any) => {
    const formData: any = new FormData();
    formData.append("Id", detail.Id);
    formData.append("Title", values.Title);
    formData.append("SubTitle", values.Title);
    formData.append("Url", values.Title);
    formData.append("Description", values.Title);
    formData.append("Content", content);
    formData.append("ShortContent", values.Title);
    formData.append("Source", values.Title);
    formData.append("CoverImage", values.Title);
    formData.append("IsPublished", false);
    formData.append("WorkflowId", workflow);
    formData.append("IsShowHome", false);
    formData.append("PostTypeId", values.PostTypeId);
    formData.append("Platform", detail.Platform);
    fileList.map((f: any) => {
      formData.append("SelectedFilesImages", f.originFileObj);
    });
    let res: any = await updateMutation.mutateAsync(formData);
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
        className="w-full"
        layout="vertical"
        form={form}
        onFinish={onFinish}
      >
        <div className="mb-3 fixed z-10 right-0 top-0 h-[65px] bg-[white] pr-[50px] pt-[16px]">
          <Flex justify={"space-between"}>
            <Button
              icon={<FaBackspace />}
              onClick={() => navigate(-1)}
              style={{ marginRight: "16px" }}
            >
              Quay lại
            </Button>
            <div>
              {detail.WorkflowId != 4 ? (
                <Button
                  style={{ marginRight: "16px" }}
                  color="cyan"
                  variant="solid"
                  icon={<BsFillSendFill />}
                  htmlType="submit"
                  onClick={() => setWorkflow(4)}
                >
                  Gửi duyệt
                </Button>
              ) : (
                <></>
              )}
              {detail.WorkflowId == 4 ? (
                <Button
                  style={{ marginRight: "16px" }}
                  variant="solid"
                  color="red"
                  icon={<TiArrowBack />}
                  htmlType="submit"
                  onClick={() => setWorkflow(1)}
                >
                  Trả lại
                </Button>
              ) : (
                <></>
              )}
              <Button
                onClick={() => setWorkflow(detail.WorkflowId)}
                type="primary"
                icon={<IoSaveSharp />}
                htmlType="submit"
              >
                Lưu trữ
              </Button>
            </div>
          </Flex>
        </div>
        <Row>
          <Col span={24} lg={16}>
            <Card>
              <Flex vertical={true}>
                <Form.Item<any>
                  label="Tiêu đề"
                  name="Title"
                  className="w-full"
                  rules={[
                    {
                      required: true,
                      message: "Bạn nhập tiêu đề bài viết",
                    },
                  ]}
                >
                  <Input
                    className="w-full"
                    placeholder="Nhập tiêu đề bài viết"
                  />
                </Form.Item>
                <div>
                  <h2 className="mb-2">Nhập nội dung</h2>
                  <Editor
                    value={content}
                    onChange={(e: any) => {
                      setContent(e);
                    }}
                  />
                </div>
              </Flex>
            </Card>
          </Col>
          <Col span={24} lg={8} className="pl-3">
            <Card>
              <Form.Item<any>
                label="Chọn nền tảng (facebook, tiktok, youtube, website)"
                name="Platform"
                className="w-full"
                rules={[
                  {
                    required: true,
                    message: "Bạn chưa chọn nền tảng",
                  },
                ]}
              >
                <Select
                  defaultValue={-1}
                  options={[
                    {
                      value: -1,
                      label: "Tất cả",
                    },
                    {
                      value: PlatformData.FACEBOOK,
                      label: "Facebook",
                    },
                    {
                      value: PlatformData.TIKTOK,
                      label: "Tiktok",
                    },
                    {
                      value: PlatformData.YOUTUBE,
                      label: "Youtube",
                    },
                    {
                      value: PlatformData.WEBSITE,
                      label: "Website",
                    },
                  ]}
                />
              </Form.Item>
            </Card>

            <Card className="mt-3">
              <Form.Item<any>
                rules={[
                  {
                    required: true,
                    message: "Bạn chưa chọn danh mục lưu trữ!",
                  },
                ]}
                className="w-full"
                label="Kho lưu trữ (Lưu trữ nội dung kiểm duyệt)"
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
                        <div className="mb-[16px] w-[48%]">
                          <Flex>
                            <Image
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
                                let res = await post.deleteImage(
                                  i.Id,
                                  detail.Id
                                );
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
                              <Button danger style={{ marginLeft: "-45px" }}>
                                <CloseOutlined />
                              </Button>{" "}
                            </Popconfirm>
                          </Flex>
                        </div>
                      );
                    })}
                  </Flex>
                </div>
              </div>
            </Card>
          </Col>
        </Row>
      </Form>
    </div>
  );
};
export default DetailPage;
