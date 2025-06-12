import { Col, Row, Flex, Card } from "antd";

export default function Notification() {
  return (
    <Row>
      <Col span={24}>
        <Card style={{ marginBottom: "8px" }}>
          <Flex justify="space-between">
            <Flex justify="start">
              <div>
                <strong>Nạp tiền</strong>
                <p>
                  Lorem ipsum dolor sit amet consectetur adipisicing elit. Animi
                  dignissimos impedit quidem, nisi quo unde suscipit aspernatur
                  sed necessitatibus maxime ut. Animi, id. Amet, quasi nesciunt
                  perferendis perspiciatis tempore ipsam.
                </p>
              </div>
            </Flex>
            <div>
              <p>20:04 15/10/2024</p>
            </div>
          </Flex>
        </Card>
      </Col>
    </Row>
  );
}
