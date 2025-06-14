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
  Image,
} from "antd";
import moment from "moment";
import { useMutation } from "@tanstack/react-query";
import { fCurrency } from "@/utils/format-number";
import { PlusOutlined, SearchOutlined, CloseOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router";

import posttype from "@/api/posttypes/index.tsx";
import post from "@/api/products/index.tsx";

import { toast } from "sonner";
import { PostTypeEnum } from "#/enum";
import Paging from "@/components/paging";
import helper from "@/api/helper";
import { IconButton, Iconify } from "@/components/icon";
import defaultImage from "@/assets/images/default.png";
import WarehouseHistoryModal from "./warehouse-history";
export default function Draft() {
  const navigate = useNavigate();
  const [form] = Form.useForm();
  const [total, setTotal] = useState(0);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);
  const [data, setData] = useState<any>([]);
  const [keyword, setKeyword] = useState<any>("");
  const [open, setOpen] = useState(false);
  const [isOpenWarehouse, setIsOpenWarehouse] = useState<any>(false);
  const [editItem, setEditItem] = useState<any>();
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
      PostTypeEnum: PostTypeEnum.Brand,
    };
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
    formData.append("PostTypeEnum", PostTypeEnum.Brand);

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
    formData.append("Platform", 1);
    const res: any = await addPostMutation.mutateAsync(formData);
    if (res.Status) {
      navigate(`/warehouse/${res.Data}`);
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
      PostTypeEnum: PostTypeEnum.Brand,
      Keyword: keyword,
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
  // dữ liệu bài viết
  const columns: any = [
    {
      title: "Ảnh",
      render: (_: any, record: any) => (
        <div>
          {record.Images.length > 0 ? (
            <Image
              width={80}
              src={
                import.meta.env.VITE_APP_BASE_API.slice(
                  0,
                  import.meta.env.VITE_APP_BASE_API.length - 4
                ) + `${record.Images[0].ImagePath}`
              }
            />
          ) : (
            <Image width={80} src={defaultImage} />
          )}
        </div>
      ),
    },
    {
      title: "Tên mặt hàng",
      render: (_: any, record: any) => <div>{record.Title}</div>,
    },
    {
      title: "Danh mục",
      render: (_: any, record: any) => (
        <Tag color="green">{record.PostType.Name}</Tag>
      ),
    },
    {
      title: "Đã bán",
      render: (_: any, record: any) => (
        <Tag color="red">{record.QuantitySold}</Tag>
      ),
    },
    {
      title: "Tồn kho",
      render: (_: any, record: any) => (
        <Tag color="blue">{record.Quantity}</Tag>
      ),
    },
    {
      title: "Giá vốn",
      render: (_: any, record: any) => (
        <Tag color="blue">{fCurrency(record.CapitalPrice)}</Tag>
      ),
    },
    {
      title: "Giá bán",
      render: (_: any, record: any) => (
        <Tag color="blue">{fCurrency(record.SellingPrice)}</Tag>
      ),
    },
    {
      title: "Hạn sử dụng",
      render: (_: any, record: any) => (
        <Tag color="red">{moment(record.ExpireDate).format(" DD-MM-YYYY")}</Tag>
      ),
    },
    {
      title: "Ngày nhập",
      render: (_: any, record: any) => (
        <div> {moment(record.InputDate).format(" DD-MM-YYYY")}</div>
      ),
    },
    {
      title: "Ngày xuất",
      render: (_: any, record: any) => (
        <div>{moment(record.ExportDate).format(" DD-MM-YYYY")}</div>
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
      fixed: "right",
      width: 100,
      render: (_: any, record: any) => (
        <div className="flex w-full justify-end text-gray">
          <div>
            <Tooltip title="Chỉnh sửa">
              <IconButton onClick={() => navigate("/warehouse/" + record.Id)}>
                <Iconify icon="solar:pen-bold-duotone" size={18} />
              </IconButton>
            </Tooltip>
          </div>
          <div>
            <Tooltip title="Lịch sử">
              <IconButton
                onClick={() => {
                  setIsOpenWarehouse(true);
                  setEditItem(record);
                }}
              >
                <Iconify icon="material-symbols:history" size={18} />
              </IconButton>
            </Tooltip>
          </div>
          <Popconfirm
            title="Xóa sản phẩm"
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

  const searchData = () => {
    setCurrentPage(1);
    getData(1);
  };

  return (
    <div>
      <WarehouseHistoryModal
        editItem={editItem}
        setIsModalOpen={setIsOpenWarehouse}
        isModalOpen={isOpenWarehouse}
      />
      <Drawer title="Khởi tạo bài viết" onClose={onClose} open={open}>
        <Form
          className="w-full"
          layout="vertical"
          form={form}
          onFinish={onFinish}
        >
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
                message: "Bạn chưa Chọn danh mục!",
              },
            ]}
            label="Chọn danh mục"
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
                <div>
                  <Flex>
                    <Input
                      onChange={(e) => setKeyword(e.target.value)}
                      placeholder="Tìm kiếm tên sản phẩm"
                      style={{ marginRight: "8px", width: "250px" }}
                    />
                    <Button
                      type="primary"
                      onClick={() => searchData()}
                      icon={<SearchOutlined />}
                      style={{ marginRight: "8px" }}
                    >
                      Tìm kiếm
                    </Button>
                    <Tooltip title="Hủy tìm kiếm">
                      <Button
                        type="primary"
                        onClick={showDrawer}
                        danger
                        icon={<CloseOutlined />}
                      ></Button>{" "}
                    </Tooltip>
                  </Flex>
                </div>
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
