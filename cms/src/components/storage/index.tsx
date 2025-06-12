import { useState, useEffect } from "react";
import { useMutation } from "@tanstack/react-query";
import { Modal, Flex, Checkbox } from "antd";
import { Image } from "antd";
import Paging from "@/components/paging";

import file from "@/api/files";
import helper from "@/api/helper";
const Storage = ({ isModalOpen, setIsModalOpen }: any) => {
  const handleOk = () => {
    setIsModalOpen(false);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };
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
    getData(1);
  }, []);
  return (
    <Modal
      title="Kho tài nguyên"
      open={isModalOpen}
      width={1250}
      onOk={handleOk}
      onCancel={handleCancel}
      okText="Đồng ý"
      cancelText="Hủy"
    >
      {data.map((record: any) => {
        return (
          <Checkbox key={record.Id} value={record.Id}>
            <Image
              style={{ cursor: "pointer" }}
              preview={false}
              width={120}
              className="p-[8px]"
              src={
                import.meta.env.VITE_APP_BASE_API.slice(
                  0,
                  import.meta.env.VITE_APP_BASE_API.length - 4
                ) + `${record.ImagePath}`
              }
            />
          </Checkbox>
        );
      })}

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
    </Modal>
  );
};

export default Storage;
