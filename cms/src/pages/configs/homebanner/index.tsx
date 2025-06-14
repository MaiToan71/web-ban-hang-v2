import { Button, Card, Popconfirm, Flex, Image, Row, Col } from "antd";
import Table, { type ColumnsType } from "antd/es/table";
import { useState, useEffect } from "react";
import { useMutation } from "@tanstack/react-query";
import { GrSubtractCircle } from "react-icons/gr";

import file from "@/api/files";
import configs from "@/api/configs";
import Paging from "@/components/paging";
import helper from "@/api/helper";
import { ConfigEnum } from "#/enum";
import FileSystemModal from "./filestem-modal";
import moment from "moment";
import { toast } from "sonner";
import {
  PlusOutlined
} from '@ant-design/icons';
const defaultBankValue: any = {
  Id: 0,
  Caption: "",
  CategoryId: "",
};

export default function HomebannerPage() {
  const searchMutation = useMutation({
    mutationFn: file.searchItem,
  });

  const [total, setTotal] = useState(0);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);
  const [data, setData] = useState<any>([]);

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
    getBanners()
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

  const addBanner = async function (item: any) {
    const input: any = {
      "Order": 0,
      "ConfigEnum": ConfigEnum.BannerHome,
      "ConfigId": item.Id,
      "Url": item.ImagePath
    }
    var res: any = await configs.addItem(input)
    if (res.Status) {
      toast.success("Thành công", {
        position: "top-right",
      });
    } else {
      toast.error("Thất bại", {
        position: "top-right",
      });
    }
    getBanners()
  }


  const deleteBanner = async function (item: any) {
    var res: any = await configs.deleteItem(item.Id)
    if (res.Status) {
      toast.success("Thành công", {
        position: "top-right",
      });
    } else {
      toast.error("Thất bại", {
        position: "top-right",
      });
    }
    getBanners()
  }
  const [banners, setBanners] = useState<any>([])
  const getBanners = async () => {
    let param: any = {
      PageIndex: 1,
      "ConfigEnum": ConfigEnum.BannerHome,
      PageSize: 1000,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await configs.searchItem(param);
    setBanners(res.Items);
  }

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
      title: "Thao tác",
      key: "operation",
      align: "center",
      width: 100,
      render: (_: any, record: any) => (
        <div className="flex w-full justify-end text-gray">
          <Button onClick={() => addBanner(record)} icon={<PlusOutlined />}></Button>
        </div>
      ),
    },
  ];
  const columnBanners: ColumnsType<any> = [
    {
      title: "Số thứ tự",
      render: (_: any, record: any) => (
        <div>{record.Order}</div>
      ),
    },
    {
      title: "Ảnh",
      render: (_: any, record: any) => (
        <div>
          <Image
            width={80}
            src={
              import.meta.env.VITE_APP_BASE_API.slice(
                0,
                import.meta.env.VITE_APP_BASE_API.length - 4
              ) + `${record.Url}`
            }
          />
        </div>
      ),
    },
    {
      title: "Thao tác",
      key: "operation",
      align: "center",
      width: 100,
      render: (_: any, record: any) => (
        <div className="flex w-full justify-end text-gray">
          <Popconfirm
            onConfirm={() => deleteBanner(record)}
            title="Xóa ảnh"
            okText="Đồng ý"
            cancelText="Hủy"
            placement="left"
          >
            <Button icon={<GrSubtractCircle />}></Button>
          </Popconfirm>
        </div>
      ),
    },
  ];
  return (
    <div>
      <Row >
        <Col span={12}>
          <div >
            <Card
              title="Kho tài nguyên"
              extra={
                <Button type="primary" onClick={() => onCreate()}>
                  Thêm mới
                </Button>
              }
            >
              <Table
                scroll={{ y: 'calc(100vh - 240px)' }}
                rowKey="id"
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
        <Col span={1}>
        </Col>
        <Col span={11}>
          <div >
            <Card
              title="Danh sách banner hiển thị trang chủ"

            >
              <Table
                scroll={{ y: 'calc(100vh - 240px)' }}
                rowKey="id"
                pagination={false}
                columns={columnBanners}
                dataSource={banners}
              />

            </Card>
          </div>
        </Col>
      </Row>
    </div>
  );
}
