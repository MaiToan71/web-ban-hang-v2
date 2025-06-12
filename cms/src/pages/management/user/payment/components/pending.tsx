import { Button, Popconfirm, Flex } from "antd";
import Table, { type ColumnsType } from "antd/es/table";
import { useState, useEffect } from "react";

import moment from "moment";
import { useMutation } from "@tanstack/react-query";
import { toast } from "sonner";
import { Workflow } from "#/enum";
import bankuser from "@/api/settings/bankuser";
import Paging from "@/components/paging";
import helper from "@/api/helper";

export default function PendingPage({ tab }: any) {
  const [total, setTotal] = useState(0);
  const [bankUsers, setBankUsers] = useState<any>([]);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);

  const searchUserMutation = useMutation({
    mutationFn: bankuser.searchItem,
  });
  const updateWorkUserMutation = useMutation({
    mutationFn: bankuser.updateWorkflowItem,
  });
  const getBankUser = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
      Workflow: Workflow.WAITING,
    };
    const res: any = await searchUserMutation.mutateAsync(param);
    setBankUsers(res.Items);
    setTotal(res.TotalRecord);
  };
  const handlePagination = (page: any) => {
    setCurrentPage(page);
    getBankUser(page);
  };

  useEffect(() => {
    if (tab == 1) {
      setCurrentPage(1);
      getBankUser(1);
    }
  }, [tab]);

  const sendPayment = async (input: any) => {
    let res = await updateWorkUserMutation.mutateAsync(input);
    await getBankUser(currentPage);
    if (res.Status) {
      toast.success("Thành công", {
        position: "top-right",
      });
    } else {
      toast.error("Thất bại", {
        position: "top-right",
      });
    }
  };
  const columns: ColumnsType<any> = [
    {
      title: "Mã yêu cầu",
      render: (_: any, record: any) => <div> {record.BankUser.Id}</div>,
    },
    {
      title: "Tên CTV",
      render: (_: any, record: any) => <div> {record.User.FullName}</div>,
    },
    {
      title: "Địa chỉ",
      render: (_: any, record: any) => <div> {record.User.Address}</div>,
    },
    {
      title: "Ngân hàng",
      render: (_: any, record: any) => <div> {record.BankConfig.BankName}</div>,
    },
    {
      title: "Ngày yêu cầu",
      render: (_: any, record: any) => (
        <div> {moment(record.User.CreatedTime).format("DD/MM/YYYY")}</div>
      ),
    },
    {
      title: "Số tiền",
      render: (_: any, record: any) => (
        <div> {helper.currencyFormat(record.BankUser.Amount)}</div>
      ),
    },

    {
      title: "Phê duyệt",
      render: (_: any, record: any) => (
        <div>
          <Popconfirm
            title="Hủy"
            description="Hủy tiền cọc ?"
            onConfirm={async () => {
              let input: any = {
                Id: record.BankUser.Id,
                Workflow: Workflow.CANCEL,
              };
              sendPayment(input);
            }}
            okText="Đồng ý"
            cancelText="Hủy"
          >
            <Button type="primary" danger className="m-1">
              Từ chối
            </Button>
          </Popconfirm>
          <Popconfirm
            title="Phê duyệt"
            description="Đồng ý duyệt tiền cọc?"
            onConfirm={async () => {
              let input: any = {
                Id: record.BankUser.Id,
                Workflow: Workflow.COMPLETED,
              };
              console.log(input);

              sendPayment(input);
            }}
            okText="Đồng ý"
            cancelText="Hủy"
          >
            <Button type="primary">Phê duyệt</Button>
          </Popconfirm>
        </div>
      ),
    },
  ];
  return (
    <div>
      <Table
        rowKey="id"
        scroll={{ x: "max-content" }}
        pagination={false}
        columns={columns}
        dataSource={bankUsers}
      />
      <Flex justify={`space-between`} align={`center`}>
        <strong className="pt-2">
          Tổng{" "}
          <span style={{ color: "red" }}> {helper.currencyFormat(total)}</span>{" "}
          bản ghi
        </strong>
        <Paging
          size={pageSize}
          total={total}
          currentPage={currentPage}
          handlePagination={handlePagination}
        />
      </Flex>
    </div>
  );
}
