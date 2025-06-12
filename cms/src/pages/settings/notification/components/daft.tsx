import { Flex, Popconfirm, Tag, Table } from "antd";

import Paging from "@/components/paging";
import helper from "@/api/helper";
import { useState, useEffect } from "react";
import notification from "@/api/settings/notification";
import { useMutation } from "@tanstack/react-query";
import { IconButton, Iconify } from "@/components/icon";
import moment from "moment";
import NotificationUpdateModal from "./notifiaction-update-modal";
import { toast } from "sonner";
export default function DaftPage({ pageItem, isReload }: any) {
  const [total, setTotal] = useState<number>(0);
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [notifications, setNotifications] = useState<any>([]);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const handlePagination = (page: any) => {
    setCurrentPage(page);
    getData(page);
  };
  const searchMutation = useMutation({
    mutationFn: notification.searchItem,
  });
  const deleteMutation = useMutation({
    mutationFn: notification.deleteItem,
  });
  const getData = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
      Workflow: 1,
    };
    const res: any = await searchMutation.mutateAsync(param);
    setNotifications(res.Items);
    setTotal(res.TotalRecord);
  };
  useEffect(() => {
    if (Number(pageItem) == 1) {
      getData(1);
    }
  }, [pageItem]);
  useEffect(() => {
    if (isReload) {
      getData(currentPage);
    }
  }, [isReload]);
  const [editItem, setEditItem] = useState<any>();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const onEdit = (item: any, users: any) => {
    console.log(item);
    item.ScheduleDate = moment(item.ScheduleDate).format("YYYY-MM-DD");
    item.Users = users.map((i: any) => {
      return i.Id;
    });
    setEditItem(item);
    setIsModalOpen(true);
  };
  const columns: any = [
    {
      title: "STT",
      render: (_: any, record: any) => <p>{record.Notification.Id}</p>,
    },
    {
      title: "Tiêu đề",
      render: (_: any, record: any) => <p>{record.Notification.Name}</p>,
    },
    {
      title: "Nội dung",

      render: (_: any, record: any) => <p>{record.Notification.Message}</p>,
    },
    {
      title: "Áp dụng cho CTV",
      render: (_: any, record: any) => (
        <div>
          {record.Users.map((item: any) => {
            return <Tag color="cyan">{item.FullName}</Tag>;
          })}
        </div>
      ),
    },
    {
      title: "Ngày tạo",
      render: (_: any, record: any) => (
        <p>{moment(record.Notification.CreatedTime).format("DD/MM/YYYY")}</p>
      ),
    },
    {
      title: "Ngày gửi",

      render: (_: any, record: any) => (
        <p>{moment(record.Notification.ScheduleDate).format("DD/MM/YYYY")}</p>
      ),
    },
    {
      title: "Thao tác",
      key: "action",
      render: (_: any, record: any) => (
        <div className="flex w-full justify-center text-gray">
          <IconButton onClick={() => onEdit(record.Notification, record.Users)}>
            <Iconify icon="solar:pen-bold-duotone" size={18} />
          </IconButton>
          <Popconfirm
            onConfirm={async () => {
              let res = await deleteMutation.mutateAsync(
                record.Notification.Id
              );
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
            title="Xóa thông báo"
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
    <div>
      <NotificationUpdateModal
        editItem={editItem}
        isModalOpen={isModalOpen}
        setIsModalOpen={setIsModalOpen}
        title={"Thêm mới"}
        reloadEdit={() => getData(currentPage)}
      />
      <Table<any>
        scroll={{ x: "max-content" }}
        columns={columns}
        dataSource={notifications}
        pagination={false}
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
    </div>
  );
}
