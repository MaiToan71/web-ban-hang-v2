import { Table } from "antd";
const columns = [
  {
    title: "STT",
  },
  {
    title: "Bài đăng",
  },
  {
    title: "Đường dẫn",
  },
  {
    title: "Hình ảnh",
  },
  {
    title: "Tên nhóm",
  },
  {
    title: "Thao tác",
  },
];
const data: any = [];
const DaftPage = ({ tab }: any) => {
  console.log(tab);
  return <Table columns={columns} dataSource={data} />;
};
export default DaftPage;
