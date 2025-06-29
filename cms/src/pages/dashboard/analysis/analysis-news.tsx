import { faker } from "@faker-js/faker";
import { Avatar, List } from "antd";

const data = [
  {
    title: faker.lorem.words(),
  },
  {
    title: faker.lorem.words(),
  },
  {
    title: faker.lorem.words(),
  },
  {
    title: faker.lorem.words(),
  },
];
export default function AnalysisNews() {
  return (
    <List
      pagination={{ position: "bottom", align: "end" }}
      dataSource={data}
      renderItem={(item) => (
        <List.Item>
          <List.Item.Meta
            avatar={
              <Avatar
                shape="square"
                size={48}
                src={faker.image.urlLoremFlickr()}
              />
            }
            title={item.title}
            description={faker.lorem.sentence()}
          />
        </List.Item>
      )}
    />
  );
}
