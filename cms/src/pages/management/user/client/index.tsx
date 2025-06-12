import { Card, Tabs } from "antd";
import { Input } from "antd";
const { Search } = Input;
import type { TabsProps } from "antd";

import { useState } from "react";
import DaftPage from "./components/daft";
import CompletePage from "./components/complete";
import FailPage from "./components/fail";
export default function ClientPage() {
  const [tab, setTab] = useState<any>(1);
  const onChange = (key: string) => {
    setTab(key);
  };

  const items: TabsProps["items"] = [
    {
      key: "1",
      label: "Chờ duyệt",
      children: <DaftPage tab={tab} />,
    },
    {
      key: "2",
      label: "Đã duyệt",
      children: <CompletePage tab={tab} />,
    },
    {
      key: "3",
      label: "Từ chối",
      children: <FailPage tab={tab} />,
    },
  ];

  return (
    <Card
      title="Danh sách cộng tác viên"
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
}
