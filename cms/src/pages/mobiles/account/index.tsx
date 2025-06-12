import { Col, Row, Flex, Card, Button } from "antd";

export default function Account() {
  return (
    <Row className="width-mobile">
      <Col span={24}>
        <Card style={{ marginBottom: "8px" }} title="Thông tin cá nhân">
          <Flex justify="space-between" style={{ marginBottom: "8px" }}>
            <div>Họ và tên</div>
            <div>
              <p>20:04 15/10/2024</p>
            </div>
          </Flex>
          <Flex justify="space-between" style={{ marginBottom: "8px" }}>
            <div>Địa chỉ</div>
            <div>
              <p>20:04 15/10/2024</p>
            </div>
          </Flex>
          <Flex justify="space-between" style={{ marginBottom: "8px" }}>
            <div>Số điện thoại</div>
            <div>
              <p>20:04 15/10/2024</p>
            </div>
          </Flex>
          <Flex justify="space-between" style={{ marginBottom: "8px" }}>
            <div>Số dư</div>
            <div>
              <p>20:04 15/10/2024</p>
            </div>
          </Flex>
          <Button style={{ width: "100%" }}>Chỉnh sửa</Button>
        </Card>

        <Button danger style={{ width: "100%", marginTop: "16px" }}>
          Đăng xuất
        </Button>
      </Col>
    </Row>
  );
}
