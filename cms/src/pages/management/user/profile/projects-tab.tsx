
import { Avatar, Col, Divider, Row, Tag } from "antd";
import dayjs from "dayjs";


import Card from "@/components/card";
import { IconButton, Iconify } from "@/components/icon";

export default function ProjectsTab() {
const items:any=[]
	return (
		<Row gutter={[16, 16]}>
			{items.map((item:any) => (
				<Col span={24} md={12} key={item.name}>
					<Card className="w-full flex-col">
						<header className="flex w-full items-center">
							{item.icon}

							<div className="flex flex-col">
								<span className="ml-4 text-xl opacity-70">{item.name}</span>
								<span className="text-md ml-4 opacity-50">
									Client: {item.client}
								</span>
							</div>

							<div className="ml-auto flex opacity-70">
								<IconButton>
									<Iconify icon="fontisto:more-v-a" size={18} />
								</IconButton>
							</div>
						</header>

						<main className="mt-4 w-full">
							<div className="my-2 flex justify-between">
								<span>
									Start Date:
									<span className="ml-2 opacity-50">
										{item.startDate.format("DD/MM/YYYY")}
									</span>
								</span>

								<span>
									Deadline:
									<span className="ml-2 opacity-50">
										{item.deadline.format("DD/MM/YYYY")}
									</span>
								</span>
							</div>
							<span className="opacity-70">{item.desc}</span>
						</main>

						<Divider />

						<footer className="flex w-full  flex-col items-center">
							<div className="mb-4 flex w-full justify-between">
								<span>
									All Hours:
									<span className="ml-2 opacity-50">{item.allHours}</span>
								</span>

								<Tag color="warning">
									{item.deadline.diff(dayjs(), "day")} days left
								</Tag>
							</div>
							<div className="flex w-full ">
								<Avatar.Group max={{ count: 4 }}>
									{item.members.map((memberAvatar:any) => (
										<Avatar src={memberAvatar} key={memberAvatar} />
									))}
								</Avatar.Group>
								<div className="ml-auto flex items-center opacity-50">
									<Iconify icon="solar:chat-round-line-linear" size={24} />
									<span className="ml-2">{item.messages}</span>
								</div>
							</div>
						</footer>
					</Card>
				</Col>
			))}
		</Row>
	);
}
