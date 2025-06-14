import { Button, Card, Flex, Tag } from "antd";
import Table, { type ColumnsType } from "antd/es/table";
import Paging from "@/components/paging";
import helper from "@/api/helper";
import { useState, useEffect } from "react";
import { useMutation } from "@tanstack/react-query";
import order from "@/api/orders/index";
import OrderModal from "./order-modal";
import { useTranslation } from "react-i18next";
import { IconButton, Iconify } from "@/components/icon";
import { DatePicker } from "antd";
const { RangePicker } = DatePicker;
import { fCurrency } from "@/utils/format-number";
import moment from "moment";
import { SearchOutlined } from "@ant-design/icons";
import { Workflow, OrderType } from "#/enum";
const OrderPage = () => {
  const [type, setType] = useState<any>("show");
  const searchMutation = useMutation({
    mutationFn: order.searchItem,
  });
  const getByIdMutation = useMutation({
    mutationFn: order.getById,
  });
  const { t } = useTranslation();
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [orders, setOrders] = useState<any>([]);
  const [total, setTotal] = useState(0);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editItem, setEditItem] = useState<any>();
  const [currentPage, setCurrentPage] = useState(1);
  const [fromDateOrder, setFromDateOrder] = useState<any>();
  const [toDateOrder, setToDateOrder] = useState<any>();
  const dateFormat = "DD/MM/YYYY";
  const customFormat = (value: any) => `${value.format(dateFormat)}`;
  const onChangeDateOrder = (_: any, formattedDates: any) => {
    if (formattedDates.length == 2) {
      let start = formattedDates[0];
      if (start != "") {
        setFromDateOrder(
          `${start.split("/")[2]}-${start.split("/")[1]}-${start.split("/")[0]
          }T00:00:00`
        );
      } else {
        setFromDateOrder(null);
      }

      let end = formattedDates[1];
      if (end != "") {
        setToDateOrder(
          `${end.split("/")[2]}-${end.split("/")[1]}-${end.split("/")[0]
          }T23:59:59`
        );
      } else {
        setToDateOrder(null);
      }
    }
  };
  const getData = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
      OrderType: OrderType.Output, //input
    };
    let check: any = true;
    if (!(toDateOrder && toDateOrder) || !(fromDateOrder && fromDateOrder)) {
      check = false;
    }
    if (check == true) {
      param.FromDateOrder = fromDateOrder;
      param.ToDateOrder = toDateOrder;
    }
    //console.log(searchMutation.mutateAsync)
    const res: any = await searchMutation.mutateAsync(param);
    setOrders(res.Items);
    setTotal(res.TotalRecord);
  };
  const handlePagination = (page: any) => {
    setCurrentPage(page);
    getData(page);
  };

  const onSearch = () => {
    getData(1);
    setCurrentPage(1);
  };
  useEffect(() => {
    getData(1);
  }, []);

  const onCreate = () => {
    setEditItem(undefined);
    setIsModalOpen(true);
    setType("show");
  };

  const getById = async (id: any) => {
    const res: any = await getByIdMutation.mutateAsync(id);
    if (res.Status == true) {
      setEditItem(res.Data);
      setType("edit");
    }
  };

  const columns: ColumnsType<any> = [
    {
      title: "Mã đơn hàng",
      render: (_: any, record: any) => <div>{t(record.Order.Id)}</div>,
    },

    {
      title: "Ngày đặt hàng ",
      render: (_: any, record: any) => (
        <div>{moment(record.Order.DateOrder).format("DD/MM/YYYY")}</div>
      ),
    },
    {
      title: "Sản phẩm",
      render: (_: any, record: any) => (
        <div>
          {record.OrderDetails.map((i: any) => {
            return (
              <div className=" p-[4px]">
                <ul>
                  <li>
                    <strong>Sản phẩm</strong>: {i.Product.Title}
                  </li>
                  <li>
                    <strong>Số lượng</strong> : {i.OrderDetail.Quantity}
                  </li>
                  <li>
                    <strong>Giá bán</strong> :{" "}
                    {fCurrency(i.OrderDetail.SellingPrice)}
                  </li>
                </ul>
              </div>
            );
          })}
        </div>
      ),
    },
    {
      title: "Thanh toán ",
      render: (_: any, record: any) => (
        <div>
          {[
            {
              value: 1,
              label: "Chuyển khoản",
            },
            {
              value: 0,
              label: "Tiền mặt",
            },
            {
              value: 2,
              label: "Nội bộ",
            },
            {
              value: 3,
              label: "Chưa thanh toán",
            },
          ].map((i: any) => {
            if (i.value == record.Order.PaymentMethod) {
              return i.label;
            }
          })}
        </div>
      ),
    },

    {
      title: "Trạng thái đơn hàng ",
      render: (_: any, record: any) => (
        <div>
          {[
            {
              value: Workflow.CANCEL,
              label: "Đã hủy",
              color: "red",
            },
            {
              value: Workflow.CREATED,
              label: "Chờ duyệt",
            },
            {
              value: Workflow.WAITING,
              label: "Chờ duyệt",
            },
            {
              value: Workflow.PROCESSING,
              label: "Đang duyệt",
            },
            {
              value: Workflow.COMPLETED,
              label: "Hoàn thành",
              color: "green",
            },
          ].map((i: any) => {
            if (i.value == record.Order.Workflow) {
              return <Tag color={i.color}>{i.label}</Tag>;
            }
          })}
        </div>
      ),
    },
    {
      title: "Người nhận",
      render: (_: any, record: any) => (
        <div>
          <p>{record.Order.FullName}</p>
          <p>
            {record.Order.Phone != ""
              ? `SĐT: ${record.Order.Phone}`
              : ""}
          </p>
        </div>
      ),
    },

    {
      title: "Ngày tạo ",
      render: (_: any, record: any) => (
        <div>{moment(record.Order.CreatedTime).format("HH:mm DD/MM/YYYY")}</div>
      ),
    },
    {
      title: "Thao tác",
      key: "operation",
      align: "center",
      width: 100,
      fixed: "right",
      render: (_: any, record: any) => (
        <div className="flex w-full justify-end text-gray">
          <IconButton
            onClick={() => {
              getById(record.Order.Id);
              setIsModalOpen(true);
            }}
          >
            <Iconify icon="solar:pen-bold-duotone" size={18} />
          </IconButton>
        </div>
      ),
    },
  ];

  return (
    <div>
      <div className="mb-3">
        <Card>
          <div className="flex justify-between">
            <div className="flex flex-col">
              <label className="font-medium">Ngày đặt hàng</label>
              <RangePicker format={customFormat} onChange={onChangeDateOrder} />
            </div>
            <div>
              <Button
                icon={<SearchOutlined />}
                type="primary"
                onClick={() => onSearch()}
              >
                Tìm kiếm
              </Button>
            </div>
          </div>
        </Card>
      </div>
      <Card
        title="Danh sách đơn hàng xuất kho"
        extra={
          <Button type="primary" onClick={() => onCreate()}>
            Thêm mới
          </Button>
        }
      >
        <Table
          rowKey="id"
          scroll={{ x: "max-content" }}
          pagination={false}
          columns={columns}
          dataSource={orders}
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
        <OrderModal
          type={type}
          onCallBack={() => getData(currentPage)}
          editItem={editItem}
          isModalOpen={isModalOpen}
          setIsModalOpen={setIsModalOpen}
        />
      </Card>
    </div>
  );
};

export default OrderPage;
