import { useState, useEffect } from "react";
import { Table, Flex, Row, Col, Card, Input, Tag, Tooltip } from "antd";
import moment from "moment";
import { useMutation } from "@tanstack/react-query";
import { PlatformData } from "#/enum";

import { useNavigate } from "react-router";
import { FaFacebook } from "react-icons/fa";

import { FaYoutube } from "react-icons/fa";
import { FaPager } from "react-icons/fa";

import post from "@/api/posts/index.tsx";
import { FaTiktok } from "react-icons/fa";

import Paging from "@/components/paging";
import helper from "@/api/helper";
import { IconButton, Iconify } from "@/components/icon";
import Preview from "./preview";
export default function Completed() {
  const [isModalOpen, setIsModalOpen] = useState(false);

  const navigate = useNavigate();
  const [total, setTotal] = useState(0);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [currentPage, setCurrentPage] = useState(1);
  const [data, setData] = useState<any>([]);
  const platforms: any = [
    {
      title: "Facebook",
      key: PlatformData.FACEBOOK,
      isActive: true,
      color: "blue",
      icon: <FaFacebook className="mt-[6px]" style={{ color: "blue" }} />,
    },
    {
      title: "Tiktok",
      isActive: false,
      key: PlatformData.TIKTOK,
      icon: <FaTiktok className="mt-[6px]" />,
      isLeaf: true,
    },
    {
      isActive: false,
      title: "Youtube",
      color: "red",
      key: PlatformData.YOUTUBE,
      icon: <FaYoutube className="mt-[6px]" style={{ color: "red" }} />,
      isLeaf: true,
    },
    {
      isActive: false,
      title: "Website",
      key: PlatformData.WEBSITE,
      icon: <FaPager className="mt-[6px]" style={{ color: "green" }} />,
    },
  ];

  const searchMutation = useMutation({
    mutationFn: post.searchItem,
  });

  const getData = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
      WorkflowId: 4,
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
    getData(1);
  }, []);
  const [postId, setPostId] = useState<any>(0);
  // dữ liệu bài viết
  const columns: any = [
    {
      title: "Nền tảng",
      render: (_: any, record: any) => (
        <div>
          {platforms.map((i: any) => {
            if (i.key == record.Platform) {
              return (
                <Flex>
                  {i.icon} <div className="mt-[2px] ml-1">{i.title}</div>
                </Flex>
              );
            }
          })}
        </div>
      ),
    },
    {
      title: "Tiêu đề",
      render: (_: any, record: any) => <div>{record.Title}</div>,
    },
    {
      title: "Lưu trữ",
      render: (_: any, record: any) => (
        <Tag color="green">{record.PostType.Name}</Tag>
      ),
    },
    {
      title: "Ngày tạo",
      render: (_: any, record: any) => (
        <div>{moment(record.DateCreated).format("HH:mm DD-MM-YYYY")}</div>
      ),
    },
    {
      title: "Người tạo",
      render: (_: any, record: any) => <div>{record.Author}</div>,
    },
    {
      title: "Thao tác",
      key: "operation",
      align: "center",
      width: 100,
      render: (_: any, record: any) => (
        <div className="flex w-full justify-end text-gray">
          <Tooltip title="Xem trước">
            <IconButton
              onClick={() => {
                setPostId(record.Id);
                setIsModalOpen(true);
              }}
            >
              <Iconify icon="carbon:view-filled" size={18} />
            </IconButton>{" "}
          </Tooltip>
          <IconButton onClick={() => navigate("/posts/" + record.Id)}>
            <Iconify icon="solar:pen-bold-duotone" size={18} />
          </IconButton>
        </div>
      ),
    },
  ];
  return (
    <div>
      <Preview
        isModalOpen={isModalOpen}
        setIsModalOpen={setIsModalOpen}
        postId={postId}
      />
      <Row>
        <Col span={24}>
          <Card>
            <div className="mb-3">
              <Flex justify="space-between">
                <Input
                  placeholder="Tìm kiếm bài viết"
                  style={{
                    width: "40%",
                  }}
                />
              </Flex>
            </div>
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
          </Card>
        </Col>
      </Row>
    </div>
  );
}
