import { useState, useEffect } from "react";
import { Modal, Tag, Flex, Table } from "antd";

import { useMutation } from "@tanstack/react-query";
import { Workflow } from "#/enum";

import order from "@/api/orders/index";
import moment from "moment";
import Paging from "@/components/paging";
import helper from "@/api/helper";

const WarehouseHistoryModal = ({
  editItem,
  isModalOpen,
  setIsModalOpen,
}: any) => {
  const [total, setTotal] = useState(0);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);
  const searchMutation = useMutation({
    mutationFn: order.searchOrderDetailItem,
  });
  const [data, setData] = useState<any>([]);
  const getData = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
      ProductId: editItem.Id,
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
    if (isModalOpen) {
      getData(1);
    }
  }, [isModalOpen]);
  const handleOk = () => {
    setIsModalOpen(false);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };
  const columns = [
    {
      title: "Hành động",
      render: (_: any, record: any) => (
        <div>{record.Order.OrderType != 0 ? "Xuất kho" : "Nhập kho"}</div>
      ),
    },
    {
      title: "Số lượng",
      render: (_: any, record: any) => (
        <Tag>
          {record.Order.OrderType != 0
            ? `-${record.OrderDetail.Quantity}`
            : `+${record.OrderDetail.Quantity}`}
          {}
        </Tag>
      ),
    },
    {
      title: "Ngày thao tác",
      render: (_: any, record: any) => (
        <div>{record.Order.OrderType != 0 ? "Xuất kho" : "Nhập kho"}</div>
      ),
    },

    {
      title: "Chi tiết",
      render: (_: any, record: any) => (
        <ul>
          <li>
            {record.Order.OrderType == 0
              ? `Ngày nhập hàng : ${moment(record.Order.DateOrder).format(
                  "DD/MM/YYYY"
                )}`
              : `Ngày đặt hàng : ${moment(record.Order.DateOrder).format(
                  "DD/MM/YYYY"
                )}`}
          </li>
          <li>{`Trạng thái đơn hàng : ${[
            {
              value: Workflow.CANCEL,
              label: "Đã hủy",
              color: "red",
            },
            {
              value: Workflow.CREATED,
              label: "Khởi tạo",
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
          ]
            .map((i: any) => {
              if (i.value == record.Order.Workflow) {
                return i.label;
              }
            })
            .join("")
            .replace(",", "")}`}</li>
          <li>{`Thanh toán: ${[
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
          ]
            .map((i: any) => {
              if (i.value == record.Order.PaymentMethod) {
                return i.label;
              }
            })
            .join("")
            .replace(",", "")}`}</li>
        </ul>
      ),
    },
  ];
  return (
    <Modal
      width={1200}
      title={`Thông tin lịch sử nhập/xuất sản phẩm`}
      open={isModalOpen}
      onOk={handleOk}
      onCancel={handleCancel}
      footer={null}
    >
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
          <span style={{ color: "red" }}> {helper.currencyFormat(total)}</span>{" "}
          bản ghi
        </strong>
        <Paging
          total={total}
          currentPage={currentPage}
          size={pageSize}
          handlePagination={handlePagination}
        />
      </Flex>
    </Modal>
  );
};
export default WarehouseHistoryModal;
