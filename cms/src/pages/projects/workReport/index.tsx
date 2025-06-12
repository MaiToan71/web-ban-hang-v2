import { Card, Tabs } from "antd";
import { Input } from "antd";
const { Search } = Input;
import type { TabsProps } from "antd";
import PendingPage from "./PendingPage";
import CompletePage from "./CompletePage";
import { useState } from "react";
import DaftPage from "./DaftPage";
const WorkReportPage = () => {
  const [tab, setTab] = useState<any>(1);
  const onChange = (key: string) => {
    setTab(key);
  };
  const items: TabsProps["items"] = [
    {
      key: "1",
      label: "Cá nhân",
      children: <DaftPage tab={tab} />,
    },
    {
      key: "2",
      label: "Chờ duyệt",
      children: <PendingPage tab={tab} />,
    },
    {
      key: "3",
      label: "Đã duyệt",
      children: <CompletePage tab={tab} />,
    },
  ];
  return (
    <Card
      title="Danh sách báo cáo công việc"
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
export default WorkReportPage;
