import { Button, Popconfirm, Flex } from "antd";
import Table, { type ColumnsType } from "antd/es/table";
import { useState, useEffect } from "react";

import moment from "moment";
import { useMutation } from "@tanstack/react-query";
import { toast } from "sonner";
import { UserType, Workflow } from "#/enum";
import user from "@/api/management/user";
import Paging from "@/components/paging";
import helper from "@/api/helper";

export default function DaftPage({ tab }: any) {
  const [total, setTotal] = useState(0);
  const [users, setUsers] = useState<any>([]);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);

  const searchUserMutation = useMutation({
    mutationFn: user.searchItem,
  });
  const updateWorkUserMutation = useMutation({
    mutationFn: user.updateWorkflowItem,
  });
  const getUser = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
      UserType: UserType.CLIENT,
      Workflow: Workflow.PROCESSING,
    };
    const res: any = await searchUserMutation.mutateAsync(param);
    setUsers(res.Items);
    setTotal(res.TotalRecord);
  };
  const handlePagination = (page: any) => {
    setCurrentPage(page);
    getUser(page);
  };

  useEffect(() => {
    if (tab == 1) {
      setCurrentPage(1);
      getUser(1);
    }
  }, [tab]);

  const sendUser = async (input: any) => {
    let res = await updateWorkUserMutation.mutateAsync(input);
    await getUser(currentPage);
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
      dataIndex: "UserId",
    },
    {
      title: "Tên CTV",
      dataIndex: "FullName",
    },
    {
      title: "Địa chỉ",
      dataIndex: "Address",
    },

    {
      title: "Hồ sơ",
      dataIndex: "File",
    },
    {
      title: "Ngày yêu cầu",
      dataIndex: "UserName",
      render: (_: any, record: any) => (
        <div> {moment(record.CreatedTime).format("DD/MM/YYYY")}</div>
      ),
    },
    {
      title: "Phê duyệt",
      render: (_: any, record: any) => (
        <div>
          <Popconfirm
            title="Hủy"
            description="Hủy duyệt cộng tác viên?"
            onConfirm={async () => {
              let input: any = {
                UserId: record.UserId,
                Workflow: Workflow.CANCEL,
              };
              sendUser(input);
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
            description="Đồng ý duyệt cộng tác viên?"
            onConfirm={async () => {
              let input: any = {
                UserId: record.UserId,
                Workflow: Workflow.COMPLETED,
              };
              sendUser(input);
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
        dataSource={users}
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
