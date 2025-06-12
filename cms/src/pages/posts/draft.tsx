import { useState, useEffect } from "react";
import {
  Table,
  Select,
  Button,
  Flex,
  Row,
  Col,
  Card,
  Input,
  Tag,
  Drawer,
  Form,
  Popconfirm,
  Tooltip,
} from "antd";
import moment from "moment";
import { useMutation } from "@tanstack/react-query";
import { PlatformData } from "#/enum";
import { PlusOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router";
import { FaFacebook } from "react-icons/fa";
import Preview from "./preview";
import { FaYoutube } from "react-icons/fa";
import { FaPager } from "react-icons/fa";
import posttype from "@/api/posttypes/index.tsx";
import post from "@/api/posts/index.tsx";
import { FaTiktok } from "react-icons/fa";
import { toast } from "sonner";
import { PostTypeEnum } from "#/enum";
import Paging from "@/components/paging";
import helper from "@/api/helper";
import { IconButton, Iconify } from "@/components/icon";
export default function Draft() {
  const navigate = useNavigate();
  const [form] = Form.useForm();
  const [total, setTotal] = useState(0);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);
  const [data, setData] = useState<any>([]);

  const [platforms, setplatforms] = useState<any>([
    {
      title: "Facebook",
      key: PlatformData.FACEBOOK,
      isActive: true,
      color: "blue",
      icon: <FaFacebook className="mt-[6px]" style={{ color: "blue" }} />,
    },
    {
      title: "Tiktok",
      isActive: false,
      key: PlatformData.TIKTOK,
      icon: <FaTiktok className="mt-[6px]" />,
      isLeaf: true,
    },
    {
      isActive: false,
      title: "Youtube",
      color: "red",
      key: PlatformData.YOUTUBE,
      icon: <FaYoutube className="mt-[6px]" style={{ color: "red" }} />,
      isLeaf: true,
    },
    {
      isActive: false,
      title: "Website",
      key: PlatformData.WEBSITE,
      icon: <FaPager className="mt-[6px]" style={{ color: "green" }} />,
    },
  ]);

  const [open, setOpen] = useState(false);
  const showDrawer = () => {
    setOpen(true);
  };
  const onClose = () => {
    setOpen(false);
  };
  const searchPostTypeMutation = useMutation({
    mutationFn: posttype.searchItem,
  });
  const [postTypes, setPostTypes] = useState<any>([]);
  const getPostType = async () => {
    let param: any = {
      PageIndex: 1,
      PageSize: 1000,
      PostTypeEnum: PostTypeEnum.Category,
    };
    console.log(param);
    const res: any = await searchPostTypeMutation.mutateAsync(param);
    setPostTypes(res.Items);
  };
  useEffect(() => {
    getPostType();
  }, []);

  const addPostMutation = useMutation({
    mutationFn: post.addItem,
  });
  const onFinish = async (values: any) => {
    const formData: any = new FormData();
    formData.append("Title", values.Title);
    formData.append("PostTypeEnum", PostTypeEnum.Category);

    formData.append("SubTitle", values.Title);
    formData.append("Url", values.Title);
    formData.append("Description", values.Title);
    formData.append("Content", values.Title);
    formData.append("ShortContent", values.Title);
    formData.append("Source", values.Title);
    formData.append("CoverImage", values.Title);
    formData.append("IsPublished", false);
    formData.append("WorkflowId", 1);
    formData.append("IsShowHome", false);
    formData.append("PostTypeId", values.PostTypeId);
    let platform = platforms.filter((i: any) => i.isActive == true)[0];
    formData.append("Platform", Number(platform.key));
    const res: any = await addPostMutation.mutateAsync(formData);
    if (res.Status) {
      navigate(`/posts/${res.Data}`);
      toast.success("Thành công", {
        position: "top-right",
      });
    } else {
      toast.error("Thất bại", {
        position: "top-right",
      });
    }
  };
  const searchMutation = useMutation({
    mutationFn: post.searchItem,
  });
  const deletePostMutation = useMutation({
    mutationFn: post.deleteItem,
  });
  const getData = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
      WorkflowId: 1,
      PostTypeEnum: PostTypeEnum.Category,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await searchMutation.mutateAsync(param);
    setData(res.Items);
    setTotal(res.TotalRecord);
  };
  const handlePagination = (page: any) => {
    setCurrentPage(page);
    getData(page);
  };
  useEffect(() => {
    getData(1);
  }, []);
  const [postId, setPostId] = useState<any>(0);
  // dữ liệu bài viết
  const columns: any = [
    {
      title: "Nền tảng",
      render: (_: any, record: any) => (
        <div>
          {platforms.map((i: any) => {
            if (i.key == record.Platform) {
              return (
                <Flex>
                  {i.icon} <div className="mt-[2px] ml-1">{i.title}</div>
                </Flex>
              );
            }
          })}
        </div>
      ),
    },
    {
      title: "Tiêu đề",
      render: (_: any, record: any) => <div>{record.Title}</div>,
    },
    {
      title: "Lưu trữ",
      render: (_: any, record: any) => (
        <Tag color="green">{record.PostType.Name}</Tag>
      ),
    },
    {
      title: "Ngày tạo",
      render: (_: any, record: any) => (
        <div>{moment(record.DateCreated).format("HH:mm DD-MM-YYYY")}</div>
      ),
    },
    {
      title: "Người tạo",
      render: (_: any, record: any) => <div>{record.Author}</div>,
    },
    {
      title: "Thao tác",
      key: "operation",
      align: "center",
      width: 100,
      render: (_: any, record: any) => (
        <div className="flex w-full justify-end text-gray">
          <div>
            <Tooltip title="Xem trước">
              <IconButton
                onClick={() => {
                  setPostId(record.Id);
                  setIsModalOpen(true);
                }}
              >
                <Iconify icon="carbon:view-filled" size={18} />
              </IconButton>{" "}
            </Tooltip>
          </div>
          <div>
            <Tooltip title="Chỉnh sửa">
              <IconButton onClick={() => navigate("/posts/" + record.Id)}>
                <Iconify icon="solar:pen-bold-duotone" size={18} />
              </IconButton>
            </Tooltip>
          </div>
          <Popconfirm
            title="Xóa bài viết"
            okText="Đồng ý"
            cancelText="Hủy"
            placement="left"
            onConfirm={async () => {
              let res = await deletePostMutation.mutateAsync(record.Id);
              await getData(currentPage);
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
          >
            <IconButton>
              <Iconify
                icon="mingcute:delete-2-fill"
                size={18}
                className="text-error"
              />
            </IconButton>
          </Popconfirm>
        </div>
      ),
    },
  ];
  const [isModalOpen, setIsModalOpen] = useState(false);

  return (
    <div>
      <Preview
        isModalOpen={isModalOpen}
        setIsModalOpen={setIsModalOpen}
        postId={postId}
      />
      <Drawer title="Khởi tạo bài viết" onClose={onClose} open={open}>
        <Form
          className="w-full"
          layout="vertical"
          form={form}
          onFinish={onFinish}
        >
          <div>
            <label className="">Chọn nền tảng</label>
            <Row
              className="mt-[8px]"
              gutter={{
                xs: 5,
                sm: 5,
                md: 5,
                lg: 5,
              }}
            >
              {platforms.map((i: any) => {
                return (
                  <Col className="gutter-row" span={6}>
                    <div
                      className={`platform ${
                        i.isActive == true ? "active" : ""
                      }`}
                      onClick={() => {
                        let newPlatforms = [...platforms];
                        newPlatforms.map((p: any) => {
                          p.isActive = false;
                          if (p.key == i.key) {
                            p.isActive = true;
                          }
                          return p;
                        });
                        setplatforms(newPlatforms);
                      }}
                    >
                      <div>{i.icon}</div>
                      <div className="title"> {i.title}</div>
                    </div>
                  </Col>
                );
              })}
            </Row>
          </div>
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
            <Input className="w-full" placeholder="Nhập tiêu đề bài viết" />
          </Form.Item>
          <Form.Item<any>
            rules={[
              {
                required: true,
                message: "Bạn chưa chọn danh mục lưu trữ!",
              },
            ]}
            label="Kho lưu trữ"
            name="PostTypeId"
          >
            <Select
              placeholder="Chọn danh mục lưu trữ"
              options={postTypes.map((i: any) => {
                return {
                  value: i.Id,
                  label: i.Name,
                };
              })}
            />
          </Form.Item>
          <Flex justify={"end"}>
            <Button className="mr-2" onClick={() => onClose()}>
              Hủy
            </Button>
            <Button type="primary" htmlType="submit">
              Đồng ý
            </Button>
          </Flex>
        </Form>
      </Drawer>
      <Row>
        <Col span={24}>
          <Card>
            <div className="mb-3">
              <Flex justify="space-between">
                <Input
                  placeholder="Tìm kiếm bài viết"
                  style={{
                    width: "40%",
                  }}
                />
                <Button
                  type="primary"
                  onClick={showDrawer}
                  icon={<PlusOutlined />}
                >
                  Thêm mới
                </Button>
              </Flex>
            </div>
            <Table
              rowKey="id"
              scroll={{ x: "max-content" }}
              pagination={false}
              columns={columns}
              dataSource={data}
            />
            <Flex justify={`space-between`} align={`center`}>
              <strong className="pt-2">
                Tổng{" "}
                <span style={{ color: "red" }}>
                  {" "}
                  {helper.currencyFormat(total)}
                </span>{" "}
                bản ghi
              </strong>
              <Paging
                total={total}
                currentPage={currentPage}
                size={pageSize}
                handlePagination={handlePagination}
              />
            </Flex>
          </Card>
        </Col>
      </Row>
    </div>
  );
}
