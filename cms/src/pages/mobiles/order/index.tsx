import { Col, Row, Flex, Card } from "antd";
import { HistoryOutlined, CheckCircleOutlined } from "@ant-design/icons";

export default function Recharge() {
  return (
    <Row>
      <Col span={24}>
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
