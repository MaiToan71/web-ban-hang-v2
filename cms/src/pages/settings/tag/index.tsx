import { Button, Card, Popconfirm, Flex } from "antd";
import Table, { type ColumnsType } from "antd/es/table";
import { useState, useEffect } from "react";
import { useTranslation } from "react-i18next";
import { useMutation } from "@tanstack/react-query";
import { IconButton, Iconify } from "@/components/icon";
import { toast } from "sonner";
import PosttypeModal from "./posttype-modal";
import posttype from "@/api/attributes";
import Paging from "@/components/paging";
import helper from "@/api/helper";
import { AttributeEnum } from "#/enum";
const defaultBankValue: any = {
  Id: 0,
  BankId: "",
  BankName: "",
  AccountNo: "",
  AccountName: "",
  PostTypeEnum: 0,
};

export default function PosttypePage() {
  const searchMutation = useMutation({
    mutationFn: posttype.searchItem,
  });
  const [banks, setBanks] = useState<any>([]);
  const [total, setTotal] = useState(0);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);
  const { t } = useTranslation();
  const getData = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
      AttributeEnum: AttributeEnum.Tag,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await searchMutation.mutateAsync(param);
    setBanks(res.Items);
    setTotal(res.TotalRecord);
  };
  const handlePagination = (page: any) => {
    setCurrentPage(page);
    getData(page);
  };
  useEffect(() => {
    getData(1);
  }, []);

  const [bankModalProps, setBankModalProps] = useState<any>({
    formValue: { ...defaultBankValue },
    title: "Thêm mới",
    show: false,
    onOk: () => {
      getData(currentPage);
      setBankModalProps((prev: any) => ({ ...prev, show: false }));
    },
    onCancel: () => {
      setBankModalProps((prev: any) => ({ ...prev, show: false }));
    },
  });

  const deleteMutation = useMutation({
    mutationFn: posttype.deleteItem,
  });
  const columns: ColumnsType<any> = [
    {
      title: "Tên từ khóa",
      render: (_: any, record: any) => <div>{t(record.Name)}</div>,
    },

    {
      title: "Mô tả từ khóa",
      width: "550px",
      render: (_: any, record: any) => <div>{record.Description}</div>,
    },

    {
      title: "Thao tác",
      key: "operation",
      align: "center",
      width: 100,
      render: (_: any, record: any) => (
        <div className="flex w-full justify-end text-gray">
          <IconButton
            onClick={() =>
              onEdit({
                ...record,
                Id: record.Id,
                Type: record.PermissionType,
                Status: record.Status ? 1 : 0,
              })
            }
          >
            <Iconify icon="solar:pen-bold-duotone" size={18} />
          </IconButton>
          <Popconfirm
            title="Xóa danh mục"
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

  const onCreate = () => {
    setBankModalProps((prev: any) => ({
      ...prev,
      show: true,
      ...defaultBankValue,
      title: "Thêm mới",
      formValue: { ...defaultBankValue },
    }));
  };

  const onEdit = (formValue: any) => {
    setBankModalProps((prev: any) => ({
      ...prev,
      show: true,
      title: "Chỉnh sửa",
      formValue,
    }));
  };
  return (
    <Card
      title="Từ khóa"
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
        dataSource={banks}
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
      <PosttypeModal {...bankModalProps} />
    </Card>
  );
}
