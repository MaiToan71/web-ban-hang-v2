import { Button, Card, Tabs } from "antd";
import type { TabsProps } from "antd";
import ApprovePage from "./components/approve";
import DaftPage from "./components/daft";
import { useState, useEffect } from "react";
import NotificationAddModal from "./components/notifiaction-add-modal";
export default function NotificationPage() {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isReload, setIsReload] = useState(false);
  useEffect(() => {
    setIsReload(false);
  }, [isModalOpen]);
  const [key, setKey] = useState<any>("1");

  const onChange = (key: any) => {
    setKey(key);
  };

  const items: TabsProps["items"] = [
    {
      key: "1",
      label: "Chưa gửi",
      children: (
        <DaftPage
          pageItem={key}
          isReload={isReload}
          setIsReload={setIsReload}
        />
      ),
    },
    {
      key: "2",
      label: "Đã gửi",
      children: <ApprovePage pageItem={key} />,
    },
  ];

  return (
    <Card
      title="Danh thông báo"
      extra={
        <Button type="primary" onClick={() => setIsModalOpen(true)}>
          Thêm mới thông báo
        </Button>
      }
    >
      <NotificationAddModal
        isModalOpen={isModalOpen}
        setIsModalOpen={setIsModalOpen}
        setIsReload={setIsReload}
        title={"Thêm mới"}
      />
      <Tabs defaultActiveKey={key} items={items} onChange={onChange} />
    </Card>
  );
}
