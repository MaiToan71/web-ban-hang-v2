import { Button, Card, Popconfirm, Flex, Image, Row, Col } from "antd";
import Table, { type ColumnsType } from "antd/es/table";
import { useState, useEffect } from "react";
import { useMutation } from "@tanstack/react-query";

import { GrSubtractCircle } from "react-icons/gr";
import product from "@/api/products";
import configs from "@/api/configs";
import Paging from "@/components/paging";
import helper from "@/api/helper";
import { ConfigEnum } from "#/enum";
import { toast } from "sonner";
import {
  PlusOutlined
} from '@ant-design/icons';


export default function HomeProductPage() {
  const searchMutation = useMutation({
    mutationFn: product.searchItem,
  });

  const [total, setTotal] = useState(0);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);
  const [data, setData] = useState<any>([]);

  const getData = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
      IsPublished: true
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



  const addBanner = async function (item: any) {
    const input: any = {
      "Order": 0,
      "ConfigEnum": ConfigEnum.ProductHome,
      "ConfigId": item.Id,
      "Url": "",
      "Title": item.Title
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
      "ConfigEnum": ConfigEnum.ProductHome,
      PageSize: 1000,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await configs.searchItem(param);
    setBanners(res.Items);
  }

  const columns: ColumnsType<any> = [
    {
      title: "Sản phẩm",
      render: (_: any, record: any) => (
        <div>{record.Title}</div>
      ),
    },
    {
      title: "Hình ảnh",
      render: (_: any, record: any) => (
        <div>
          {record.Images.length > 0 ? <Image
            width={80}
            src={
              import.meta.env.VITE_APP_BASE_API.slice(
                0,
                import.meta.env.VITE_APP_BASE_API.length - 4
              ) + `${record.Images[0].ImagePath}`
            }
          /> : <></>}
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
      title: "Sản phẩm",
      render: (_: any, record: any) => (
        <div>
          {record.Title}
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
              title="Kho nội dung"

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
            </Card>
          </div>
        </Col>
        <Col span={1}>
        </Col>
        <Col span={11}>
          <div >
            <Card
              title="Danh sách sản phẩm nổi bật hiển thị trang chủ"

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
