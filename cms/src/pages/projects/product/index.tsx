import { useState, useEffect } from "react";
import { Table, Card, Button, Popconfirm, Flex, Progress } from "antd";

import helper from "@/api/helper";
import Paging from "@/components/paging";
import timeShiftType from "@/api/projects/timeshiftType";
import { useMutation } from "@tanstack/react-query";
import ProductModal from "./product-modal";
import { IconButton, Iconify } from "@/components/icon";
import moment from "moment";
import { toast } from "sonner";
const ProductPage = () => {
  const [total, setTotal] = useState(0);
  const [products, setProjects] = useState<any>([]);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);
  const [open, setOpen] = useState(false);
  const [title, setTitle] = useState<any>("Thêm mới dự án");
  const [type, setType] = useState<any>("add");
  const [id, setId] = useState<any>(0);
  const onEdit = (formValue: any, PersonInCharge: any) => {
    setTitle(formValue.Name);
    setOpen(true);
    setType("edit");
    setId(formValue.Id);
    formValue.PersonInCharge = PersonInCharge;
  };
  const checkPriorityEnum = function (Priority: any) {
    if (Priority == 3) {
      return {
        color: "red",
        text: "Cần gấp và quan trọng",
      };
    }
    if (Priority == 2) {
      return {
        color: "yellow",
        text: "Quan trọng",
      };
    }
    if (Priority == 1) {
      return {
        color: "black",
        text: "Thoải mái",
      };
    }
    return {
      color: "black",
      text: "Thoải mái",
    };
  };

  const deleteMutation = useMutation({
    mutationFn: timeShiftType.deleteItem,
  });
  const columns: any = [
    {
      title: "Tên dự án",
      render: (_: any, record: any) => <p>{record.TimeShiftType.Name}</p>,
    },
    {
      title: "Độ ưu tiên",
      render: (_: any, record: any) => (
        <p
          style={{
            color: checkPriorityEnum(record.TimeShiftType.PriorityEnum)?.color,
          }}
        >
          {checkPriorityEnum(record.TimeShiftType.PriorityEnum)?.text}
        </p>
      ),
    },
    {
      title: "Tiến độ",
      width: 200,
      render: (_: any, record: any) => (
        <Progress
          percent={Number((record.TaskDone / record.TotalTask) * 100)}
          status="active"
        />
      ),
    },
    {
      title: "Người phụ trách",
      render: (_: any, record: any) => <p>{record.UserViewModel.FullName}</p>,
    },
    {
      title: "Ngày bắt đầu",
      render: (_: any, record: any) => (
        <p>{moment(record.TimeShiftType.StartTime).format("DD/MM/YYYY")}</p>
      ),
    },
    {
      title: "Ngày kết thúc",
      render: (_: any, record: any) => (
        <p>{moment(record.TimeShiftType.EndTime).format("DD/MM/YYYY")}</p>
      ),
    },
    {
      title: "Người tạo",
      render: (_: any, record: any) => (
        <p>{record.TimeShiftType.CreatedUser}</p>
      ),
    },
    {
      title: "Thao tác",
      key: "operation",
      align: "center",
      width: 100,
      render: (_: any, record: any) => (
        <div className="flex w-full justify-end text-gray">
          <IconButton
            onClick={() => onEdit(record.TimeShiftType, record.UserViewModel)}
          >
            <Iconify icon="solar:pen-bold-duotone" size={18} />
          </IconButton>
          <Popconfirm
            title="Xóa dự án"
            okText="Đồng ý"
            cancelText="Hủy"
            placement="left"
            onConfirm={async () => {
              let res = await deleteMutation.mutateAsync(
                record.TimeShiftType.Id
              );
              await getTimeshiftTypes(currentPage);
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

  const searchUserMutation = useMutation({
    mutationFn: timeShiftType.searchItem,
  });
  const getTimeshiftTypes = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
    };
    const res: any = await searchUserMutation.mutateAsync(param);
    if (res.Status) {
      setProjects(res.Items);
      setTotal(res.TotalRecord);
    }
  };
  useEffect(() => {
    setCurrentPage(1);
    getTimeshiftTypes(1);
  }, []);
  const handlePagination = (page: any) => {
    setCurrentPage(page);
    getTimeshiftTypes(page);
  };

  const showDrawer = () => {
    setTitle("Thêm mới dự án");
    setOpen(true);
    setType("add");
    setId(0);
  };

  return (
    <Card
      title="Danh sách dự án"
      extra={
        <Button type="primary" onClick={showDrawer}>
          Thêm mới
        </Button>
      }
    >
      <ProductModal
        id={id}
        type={type}
        setOpen={setOpen}
        open={open}
        title={title}
        onOk={() => {
          getTimeshiftTypes(currentPage);
        }}
      />
      <Table columns={columns} dataSource={products} pagination={false} />
      <Flex justify={`space-between`} align={`center`}>
        <strong className="pt-2">
          Tổng
          <span style={{ color: "red" }}>
            {helper.currencyFormat(total)}
          </span>{" "}
          bản ghi
        </strong>
        <Paging
          size={pageSize}
          total={total}
          currentPage={currentPage}
          handlePagination={handlePagination}
        />
      </Flex>
    </Card>
  );
};
export default ProductPage;
