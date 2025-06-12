import { Card, Col, Row, Typography } from "antd";

export default function TokenExpired() {
  return (
    <Card>
      <Row gutter={[16, 16]}>
        <Col span={24} md={12}>
          <Typography.Text>
            Clicking a button to simulate a token expiration scenario.
          </Typography.Text>
        </Col>
      </Row>
    </Card>
  );
}
