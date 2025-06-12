import { Button, Card, Popconfirm, Tag, Flex } from "antd";
import Table, { type ColumnsType } from "antd/es/table";
import { useState, useEffect } from "react";
import Paging from "@/components/paging";
import { IconButton, Iconify } from "@/components/icon";
import role from "@/api/management/role";
import { RoleModal, type RoleModalProps } from "./role-modal";
import helper from "@/api/helper";
import type { Role } from "#/entity";
import { BasicStatus } from "#/enum";
import { useMutation } from "@tanstack/react-query";
import { toast } from "sonner";
const DEFAULE_ROLE_VALUE: any = {
  Id: "",
  Name: "",
  Description: "",
  Status: BasicStatus.ENABLE,
  Permissions: [],
};
export default function RolePage() {
  const [total, setTotal] = useState(0);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);
  const searchMutation = useMutation({
    mutationFn: role.searchItem,
  });

  const [roles, setRoles] = useState<any>([]);
  const getData = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
    };
    const res: any = await searchMutation.mutateAsync(param);
    setRoles(res.Items);
    setTotal(res.TotalRecord);
  };
  const handlePagination = (page: any) => {
    setCurrentPage(page);
    getData(page);
  };

  useEffect(() => {
    getData(1);
  }, []);
  const [isUpdate, setIsUpdate] = useState<any>(false);
  const [roleModalPros, setRoleModalProps] = useState<RoleModalProps>({
    formValue: { ...DEFAULE_ROLE_VALUE },
    title: "Thêm mới",
    show: false,
    onOk: () => {
      setRoleModalProps((prev) => ({ ...prev, show: false }));
      setIsUpdate(true);
    },
    onCancel: () => {
      setRoleModalProps((prev) => ({ ...prev, show: false }));
    },
  });
  useEffect(() => {
    if (isUpdate) {
      getData(currentPage);
    }
  }, [isUpdate]);
  const deleteMutation = useMutation({
    mutationFn: role.deleteItem,
  });
  const columns: ColumnsType<Role> = [
    {
      title: "Tên",
      dataIndex: "Name",
      width: 300,
    },

    {
      title: "Trạng thái",
      dataIndex: "status",
      align: "center",
      width: 120,
      render: (status) => (
        <Tag color={status === BasicStatus.DISABLE ? "error" : "success"}>
          {status === BasicStatus.DISABLE ? "Vô hiệu hóa" : "Hoạt động"}
        </Tag>
      ),
    },
    { title: "Mô tả", dataIndex: "Description" },
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
          <Popconfirm
            onConfirm={async () => {
              let res = await deleteMutation.mutateAsync(record.Id);
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
            title="Xóa vai trò"
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

  const onCreate = () => {
    setRoleModalProps((prev) => ({
      ...prev,
      show: true,
      title: "Thêm mới",
      formValue: {
        ...prev.formValue,
        ...DEFAULE_ROLE_VALUE,
      },
    }));
    setIsUpdate(false);
  };

  const onEdit = (formValue: Role) => {
    setRoleModalProps((prev) => ({
      ...prev,
      show: true,
      title: "Chỉnh sửa",
      formValue,
    }));
    setIsUpdate(false);
  };

  return (
    <Card
      title="Danh sách vai trò"
      extra={
        <Button type="primary" onClick={onCreate}>
          Thêm mới
        </Button>
      }
    >
      <Table
        rowKey="id"
        scroll={{ x: "max-content" }}
        pagination={false}
        columns={columns}
        dataSource={roles}
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
      <RoleModal {...roleModalPros} />
    </Card>
  );
}
