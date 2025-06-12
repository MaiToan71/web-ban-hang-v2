import { faker } from "@faker-js/faker";
import { Avatar, Col, Row, Tag } from "antd";

import Card from "@/components/card";
import { IconButton, Iconify } from "@/components/icon";

export default function TeamsTab() {
	const items:any = []
	return (
		<Row gutter={[16, 16]}>
			{items.map((item:any) => (
				<Col span={24} md={12} key={item.name}>
					<Card className="w-full flex-col">
						<header className="flex w-full items-center">
							{item.icon}
							<span className="ml-4 text-xl opacity-70">{item.name}</span>

							<div className="ml-auto flex opacity-70">
								<IconButton>
									<Iconify icon="solar:star-line-duotone" size={18} />
								</IconButton>
								<IconButton>
									<Iconify icon="fontisto:more-v-a" size={18} />
								</IconButton>
							</div>
						</header>
						<main className="my-4 opacity-70">{item.desc}</main>
						<footer className="flex w-full items-center">
							<Avatar.Group max={{ count: 4 }}>
								{item.members.map((memberAvatar:any) => (
									<Avatar src={memberAvatar} key={memberAvatar} />
								))}
							</Avatar.Group>
							<div className="ml-auto flex items-center">
								{item.tags.map((tag:any) => (
									<Tag color={faker.color.rgb()} key={tag}>
										{tag}
									</Tag>
								))}
							</div>
						</footer>
					</Card>
				</Col>
			))}
		</Row>
	);
}
