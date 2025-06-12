import { Button, Card, Popconfirm, Tag, Flex } from "antd";
import Table, { type ColumnsType } from "antd/es/table";
import { useState, useEffect } from "react";
import { IconButton, Iconify } from "@/components/icon";

import { useMutation } from "@tanstack/react-query";
import { toast } from "sonner";
import UserPasswordModal from "./user-modal-password";
import user from "@/api/management/user";
import Paging from "@/components/paging";
import helper from "@/api/helper";

import { UserModal, type UserModalProps } from "./user-modal";
const DEFAULE_USER_VALUE: any = {
  Id: "",
  FullName: "",
  Email: "",
  PhoneNumber: "",
  Departments: [],
  IsActive: false,
  Roles: [],
};
export default function UserPage() {
  const [editItem, setEditItem] = useState<any>();
  const [isUpdate, setIsUpdate] = useState<any>(false);
  const [userModalPros, setUserModalProps] = useState<UserModalProps>({
    formValue: { ...DEFAULE_USER_VALUE },
    title: "Thêm mới",
    show: false,
    onOk: () => {
      setIsUpdate(true);
      setUserModalProps((prev) => ({ ...prev, show: false }));
    },
    onCancel: () => {
      setUserModalProps((prev) => ({ ...prev, show: false }));
    },
  });

  const [total, setTotal] = useState(0);
  const [users, setUsers] = useState<any>([]);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);

  const searchUserMutation = useMutation({
    mutationFn: user.searchItem,
  });
  const getUser = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
      //  Workflow: 4,
    };
    const res: any = await searchUserMutation.mutateAsync(param);
    if (res.Status) {
      setUsers(res.Items);
      setTotal(res.TotalRecord);
    }
  };

  const handlePagination = (page: any) => {
    setCurrentPage(page);
    getUser(page);
  };

  useEffect(() => {
    getUser(1);
  }, []);
  useEffect(() => {
    if (isUpdate) {
      getUser(currentPage);
    }
  }, [isUpdate]);
  const deleteMutation = useMutation({
    mutationFn: user.deleteItem,
  });
  const onEdit = (formValue: any) => {
    setIsUpdate(false);
    setUserModalProps((prev) => ({
      ...prev,
      show: true,

      title: "Chỉnh sửa",
      formValue,
    }));
  };

  const [isOpenPassword, setIsOpenPassword] = useState<any>(false);
  const columns: ColumnsType<any> = [
    {
      title: "Họ và tên",
      dataIndex: "FullName",
    },
    {
      title: "Tên đăng nhập",
      dataIndex: "UserName",
    },
    {
      title: "Số điện thoại",
      dataIndex: "PhoneNumber",
    },

    {
      title: "Duyệt",
      render: (_: any, record: any) => (
        <Tag color={record.Workflow != 4 ? "error" : "success"}>
          {record.Workflow == 4 ? "Đã duyệt" : "Chưa duyệt"}
        </Tag>
      ),
    },
    {
      title: "Vai trò",
      dataIndex: "Roles",
      align: "center",
      render: (roles: any) => (
        <div>
          {roles.map((item: any) => {
            return <Tag color="cyan">{item.Name}</Tag>;
          })}
        </div>
      ),
    },
    {
      title: "Trạng thái",
      dataIndex: "IsActive",
      align: "center",
      width: 120,
      render: (status: any) => (
        <Tag color={status !== true ? "error" : "success"}>
          {status !== true ? "Vô hiệu hóa" : "Hoạt động"}
        </Tag>
      ),
    },

    {
      title: "Thao tác",
      key: "operation",
      align: "center",
      width: 100,
      render: (_: any, record: any) => (
        <div className="flex w-full justify-center text-gray">
          <IconButton onClick={() => onEdit(record)}>
            <Iconify icon="solar:pen-bold-duotone" size={18} />
          </IconButton>
          <IconButton
            onClick={() => {
              setIsOpenPassword(true);
              setEditItem(record);
            }}
          >
            <Iconify icon="mdi:password" size={18} />
          </IconButton>
          <Popconfirm
            onConfirm={async () => {
              let res = await deleteMutation.mutateAsync(record.UserId);
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
            }}
            title="Xóa người dùng"
            okText="Đồng ý"
            cancelText="Hủy"
            placement="left"
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
  return (
    <Card
      title="Danh sách người dùng"
      extra={<Button type="primary">Thêm người dùng</Button>}
    >
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
      <UserPasswordModal
        editItem={editItem}
        setIsOpenPassword={setIsOpenPassword}
        isOpenPassword={isOpenPassword}
      />
      <UserModal {...userModalPros} />
    </Card>
  );
}
