import { useState } from "react";

import { Col, Row, Flex, Card, Input, Button, Drawer } from "antd";
import {
  DollarOutlined,
  HistoryOutlined,
  CheckCircleOutlined,
} from "@ant-design/icons";

export default function Recharge() {
  const [open, setOpen] = useState(false);
  const showDrawer = () => {
    setOpen(true);
  };
  const onClose = () => {
    setOpen(false);
  };
  return (
    <Row>
      <Drawer
        title="Basic Drawer"
        placement={"bottom"}
        onClose={onClose}
        open={open}
      >
        <p>
          <strong>Số tiền cần nạp</strong>
        </p>
        <Input placeholder="Nhập số tiền cần nạp" />
        <div className="mt-4">
          <Flex justify="space-between">
            <Button style={{ width: "50%" }} className="mr-1">
              Hủy
            </Button>
            <Button type="primary" style={{ width: "50%" }} className="ml-1">
              Nạp tiền
            </Button>
          </Flex>
        </div>
      </Drawer>
      <Col span={24}>
        <Card>
          <h1 style={{ fontSize: "19px" }}>Số dư</h1>
          <Flex>
            <DollarOutlined />
            <strong>54.000.000</strong>
          </Flex>
          <Button
            type="primary"
            style={{ width: "100%" }}
            className="mt-3"
            onClick={showDrawer}
          >
            Nạp tiền
          </Button>
        </Card>
        <div className="mt-3" style={{ marginBottom: "8px" }}>
          <Flex justify="space-between">
            <div>
              <HistoryOutlined /> Lịch sử giao dịch
            </div>
            <p>Xem tất cả</p>
          </Flex>
        </div>

        <Card style={{ marginBottom: "8px" }}>
          <Flex justify="space-between">
            <Flex justify="start">
              <CheckCircleOutlined
                className="mr-2 mt-1"
                style={{ fontSize: "27px" }}
              />
              <div>
                <strong>Nạp tiền</strong>
                <p>20:04 15/10/2024</p>
              </div>
            </Flex>
            <div>
              <p>25.000đ</p>
              <p>Thành công</p>
            </div>
          </Flex>
        </Card>
        <Card style={{ marginBottom: "8px" }}>
          <Flex justify="space-between">
            <Flex justify="start">
              <CheckCircleOutlined
                className="mr-2 mt-1"
                style={{ fontSize: "27px" }}
              />
              <div>
                <strong>Nạp tiền</strong>
                <p>20:04 15/10/2024</p>
              </div>
            </Flex>
            <div>
              <p>25.000đ</p>
              <p>Thành công</p>
            </div>
          </Flex>
        </Card>
      </Col>
    </Row>
  );
}
