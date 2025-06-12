import { Button, Card, Popconfirm, Tag, Flex } from "antd";
import Table, { type ColumnsType } from "antd/es/table";
import { isNil } from "ramda";
import { useState, useEffect } from "react";
import { useTranslation } from "react-i18next";
import { useMutation } from "@tanstack/react-query";
import { IconButton, Iconify, SvgIcon } from "@/components/icon";
import { toast } from "sonner";
import PermissionModal from "./permission-modal";
import permission from "@/api/management/permission";
import { BasicStatus, PermissionType } from "#/enum";
import Paging from "@/components/paging";
import helper from "@/api/helper";
const defaultPermissionValue: any = {
  Id: 0,
  ParentId: "",
  Name: "",
  Label: "",
  Route: "",
  Component: "",
  Icon: "",
  Order: 0,
  Hide: false,
  Status: BasicStatus.ENABLE,
  Type: PermissionType.MENU,
};

export default function PermissionPage() {
  const searchMutation = useMutation({
    mutationFn: permission.searchItem,
  });
  const [permissions, setPermissions] = useState<any>([]);
  const [total, setTotal] = useState(0);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE) * 100;
  const [currentPage, setCurrentPage] = useState(1);
  const { t } = useTranslation();
  const getData = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
    };

    //console.log(searchMutation.mutateAsync)
    const res: any = await searchMutation.mutateAsync(param);
    setPermissions(res.Items);
    setTotal(res.TotalRecord);
  };
  const handlePagination = async (page: any) => {
    setCurrentPage(page);
    getData(page);
  };

  useEffect(() => {
    setCurrentPage(1);
    getData(1);
  }, []);
  const onCreate = () => {
    setIsUpdate(false);
    setPermissionModalProps((prev: any) => ({
      ...prev,
      show: true,
      permissions: permissions,
      ...defaultPermissionValue,
      title: "Thêm mới",
      formValue: { ...defaultPermissionValue },
    }));
  };

  const onEdit = (formValue: any) => {
    setIsUpdate(false);
    setPermissionModalProps((prev: any) => ({
      ...prev,
      show: true,
      permissions: permissions,
      title: "Chỉnh sửa",
      formValue,
    }));
  };
  const [isUpdate, setIsUpdate] = useState<any>(false);
  const [permissionModalProps, setPermissionModalProps] = useState<any>({
    formValue: { ...defaultPermissionValue },
    title: "Thêm mới",
    show: false,
    permissions: permissions,
    onOk: () => {
      setPermissionModalProps((prev: any) => ({ ...prev, show: false }));
      setIsUpdate(true);
    },
    onCancel: () => {
      setPermissionModalProps((prev: any) => ({ ...prev, show: false }));
    },
  });
  useEffect(() => {
    if (isUpdate) {
      getData(currentPage);
    }
  }, [isUpdate]);

  const deleteMutation = useMutation({
    mutationFn: permission.deleteItem,
  });

  const columns: ColumnsType<any> = [
    {
      title: "Tên quyền",
      dataIndex: "Name",
      width: 150,
      render: (_: any, record: any) => <div>{t(record.Name)}</div>,
    },
    {
      title: "Loại ",
      dataIndex: "PermissionEnum",
      width: 60,
      render: (_: any, record: any) => (
        <Tag color="processing">{PermissionType[record.PermissionType]}</Tag>
      ),
    },
    {
      title: "Icon",
      dataIndex: "Icon",
      width: 60,
      render: (_: any, record: any) => {
        if (isNil(record.Icon)) return "";
        if (record.Icon.startsWith("ic")) {
          return (
            <SvgIcon
              icon={record.Icon}
              size={18}
              className="ant-menu-item-icon"
            />
          );
        }
        return (
          <Iconify
            icon={record.Icon}
            size={18}
            className="ant-menu-item-icon"
          />
        );
      },
    },
    {
      title: "Component",
      dataIndex: "Component",
    },
    { title: "Route", dataIndex: "Route", width: 60 },
    {
      title: "Trạng thái",
      dataIndex: "Status",
      align: "center",
      width: 120,
      render: (status: any) => (
        <Tag color={status === BasicStatus.DISABLE ? "error" : "success"}>
          {status === BasicStatus.DISABLE ? "Vô hiệu hóa" : "Hoạt động"}
        </Tag>
      ),
    },

    {
      title: "Thao tác",
      key: "operation",
      align: "center",
      width: 100,
      render: (_: any, record: any) => (
        <div className="flex w-full justify-end text-gray">
          {record?.PermissionType === PermissionType.CATALOGUE && (
            <IconButton onClick={() => onCreate()}>
              <Iconify icon="gridicons:add-outline" size={18} />
            </IconButton>
          )}
          <IconButton
            onClick={() => {
              onEdit({
                ...record,
                Id: record.Id,
                Type: record.PermissionType,
                Status: record.Status ? 1 : 0,
              });
            }}
          >
            <Iconify icon="solar:pen-bold-duotone" size={18} />
          </IconButton>
          <Popconfirm
            title="Xóa quyền"
            okText="Đồng ý"
            cancelText="Hủy"
            placement="left"
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
      title="Danh sách quyền"
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
        dataSource={permissions}
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
      <PermissionModal {...permissionModalProps} />
    </Card>
  );
}
