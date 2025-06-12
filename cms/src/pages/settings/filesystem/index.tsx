import { Button, Card, Popconfirm, Flex, Image, Row, Col } from "antd";
import Table, { type ColumnsType } from "antd/es/table";
import { useState, useEffect } from "react";
import { useMutation } from "@tanstack/react-query";
import { IconButton, Iconify } from "@/components/icon";
import { toast } from "sonner";
import file from "@/api/files";
import Paging from "@/components/paging";
import helper from "@/api/helper";
import FileSystemModal from "./filestem-modal";
import moment from "moment";
const defaultBankValue: any = {
  Id: 0,
  Caption: "",
  CategoryId: "",
};

export default function FileSystemPage() {
  const searchMutation = useMutation({
    mutationFn: file.searchItem,
  });
  const getStatisticsMutation = useMutation({
    mutationFn: file.getStatistics,
  });
  const [total, setTotal] = useState(0);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);
  const [data, setData] = useState<any>([]);
  const [statistics, setStatistics] = useState<any>([]);
  const getStatistics = async () => {
    let param: any = {
      PageIndex: 1,
      PageSize: 1,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await getStatisticsMutation.mutateAsync(param);
    setStatistics(res.Items);
  };
  const getData = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
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
    getStatistics();
    getData(1);
  }, []);
  const [bankModalProps, setBankModalProps] = useState<any>({
    formValue: { ...defaultBankValue },
    title: "Thêm mới",
    show: false,
    onOk: () => {
      getData(currentPage);
      setBankModalProps((prev: any) => ({
        ...prev,
        show: false,
        formValue: { ...defaultBankValue },
      }));
    },
    onCancel: () => {
      setBankModalProps((prev: any) => ({ ...prev, show: false }));
    },
  });
  const onCreate = () => {
    setBankModalProps((prev: any) => ({
      ...prev,
      show: true,
      ...defaultBankValue,
      title: "Thêm mới",
      formValue: { ...defaultBankValue },
    }));
  };

  const deleteMutation = useMutation({
    mutationFn: file.deleteItem,
  });

  const columns: ColumnsType<any> = [
    {
      title: "Ngày tạo",
      render: (_: any, record: any) => (
        <div>{moment(record.DateCreated).format("HH:mm DD-MM-YYYY")}</div>
      ),
    },
    {
      title: "Ảnh",
      render: (_: any, record: any) => (
        <div>
          {" "}
          <Image
            width={80}
            src={
              import.meta.env.VITE_APP_BASE_API.slice(
                0,
                import.meta.env.VITE_APP_BASE_API.length - 4
              ) + `${record.ImagePath}`
            }
          />
        </div>
      ),
    },
    {
      title: "Tên ảnh",
      render: (_: any, record: any) => <div>{record.Caption}</div>,
    },
    {
      title: "Dung lượng",
      render: (_: any, record: any) => (
        <div>{(Number(record.FileSize) / 1024).toFixed(2)} kb</div>
      ),
    },
    {
      title: "Danh mục",
      render: (_: any, record: any) => (
        <div>
          {record.Categories.map((i: any) => {
            return i.Name;
          })}
        </div>
      ),
    },
    {
      title: "Người tạo",
      render: (_: any, record: any) => <div>{record.CreatedUser}</div>,
    },
    {
      title: "Thao tác",
      key: "operation",
      align: "center",
      width: 100,
      render: (_: any, record: any) => (
        <div className="flex w-full justify-end text-gray">
          <Popconfirm
            title="Xóa ảnh"
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
    <div>
      <Row>
        <Col span={24}>
          <Row gutter={16}>
            {statistics.map((i: any) => {
              return (
                <Col className="gutter-row" span={6}>
                  <Card>
                    <Flex justify="center">
                      <IconButton>
                        <Iconify
                          icon="flowbite:folder-solid"
                          size={82}
                          className="text-[#f7b84b]"
                        />
                      </IconButton>
                    </Flex>
                    <p className="text-center text-[18px]">
                      <strong>{i.Name}</strong>
                    </p>
                    <Flex justify="space-between">
                      <div>{i.TotalFile} files</div>
                      <div>
                        {Number(i.TotalFilesize) > 0
                          ? (Number(i.TotalFilesize) / 1073741824).toFixed(4)
                          : 0}{" "}
                        GB
                      </div>
                    </Flex>
                  </Card>
                </Col>
              );
            })}
          </Row>
          <div className="mt-[16px]">
            <Card
              title="Kho tài nguyên"
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
                dataSource={data}
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
              <FileSystemModal {...bankModalProps} />
            </Card>
          </div>
        </Col>
      </Row>
    </div>
  );
}
