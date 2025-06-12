import { useState } from "react";
import { Card, Tabs } from "antd";
import { Input } from "antd";
const { Search } = Input;
import PendingPage from "./components/pending";
import CompletePage from "./components/complete";
import FailPage from "./components/fail";
const PaymentPage = () => {
  const [tab, setTab] = useState<any>("1");
  const onChange = (key: any) => {
    setTab(key);
  };
  const items = [
    {
      key: "1",
      label: "Chờ duyệt",
      children: <PendingPage tab={tab} />,
    },
    {
      key: "2",
      label: "Đã duyệt",
      children: <CompletePage tab={tab} />,
    },
    {
      key: "3",
      label: "Đã hủy",
      children: <FailPage tab={tab} />,
    },
  ];
  return (
    <Card
      title="Danh sách yêu cầu tiền cọc"
      extra={
        <Search
          placeholder="Tìm kiếm"
          style={{
            width: 200,
          }}
        />
      }
    >
      <Tabs defaultActiveKey="1" items={items} onChange={onChange} />
    </Card>
  );
};
export default PaymentPage;
