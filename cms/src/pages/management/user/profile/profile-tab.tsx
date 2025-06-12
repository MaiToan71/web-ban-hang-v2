import { useState, useEffect } from "react";
import {
  Col,
  Row,
  Timeline,
  Typography,
  Calendar,
  Badge,
  ConfigProvider,
} from "antd";
import viVN from "antd/locale/vi_VN";

import userApi from "@/api/management/user";
import Card from "@/components/card";
import { Iconify } from "@/components/icon";

import moment from "moment";
import workScheduleApi from "@/api/workSchedules";
import { themeVars } from "@/theme/theme.css";

import { useMutation } from "@tanstack/react-query";
import notification from "@/api/settings/notification";
export default function ProfileTab() {
  const [user, setUser] = useState<any>();
  const getInfo = async () => {
    const res: any = await userApi.userInfo();
    if (res.Status) {
      setUser(res.Data);
      getNotifications(res.Data.UserId);
      getWorkScheudule(res.Data.UserId);
    }
  };
  useEffect(() => {
    getInfo();
  }, []);
  const [notifications, setNotifications] = useState<any>([]);
  const searchMutation = useMutation({
    mutationFn: notification.searchItem,
  });
  const getNotifications = async (userId: any) => {
    let param: any = {
      PageIndex: 1,
      PageSize: 25,
      Workflow: 4,
      UserId: userId,
    };
    const res: any = await searchMutation.mutateAsync(param);
    setNotifications(res.Items);
  };
  const searchTimeshiftMutation = useMutation({
    mutationFn: workScheduleApi.searchItem,
  });
  const [startDate, setStartDate] = useState<any>(moment().startOf("month"));
  const [endDate, setEndDate] = useState<any>(moment().endOf("month"));
  const [events, setEvents] = useState<any>([]);
  const getWorkScheudule = async (userId: any) => {
    let param: any = {
      PageIndex: 1,
      PageSize: 10000,
      FromWorkDateRegis: startDate,
      ToWorkDateRegis: endDate,
      EmployeeId: userId,
    };
    const res: any = await searchTimeshiftMutation.mutateAsync(param);
    if (res.Status) {
      setEvents(
        res.Items.map((i: any) => {
          return {
            date: moment(i.FromWorkDateRegis).format("YYYY-MM-DD"),
            type: "success",
            content: `${moment(i.FromWorkDateRegis).format("HH:mm")}-${moment(
              i.ToWorkDateRegis
            ).format("HH:mm")}`,
          };
        })
      );
    }
  };

  const AboutItems = [
    {
      icon: <Iconify icon="fa-solid:user" size={18} />,
      label: "Họ và tên",
      val: user?.FullName,
    },

    {
      icon: <Iconify icon="tabler:location-filled" size={18} />,
      label: "Địa chỉ",
      val: user?.Address,
    },
    {
      icon: <Iconify icon="ion:language" size={18} />,
      label: "Ngôn ngữ",
      val: "Tiếng việt",
    },
    {
      icon: <Iconify icon="ph:phone-fill" size={18} />,
      label: "Số điện thoại",
      val: user?.PhoneNumber,
    },
    {
      icon: <Iconify icon="ic:baseline-email" size={18} />,
      label: "Tên đăng nhập",
      val: user?.UserName,
    },
  ];
  const getEventsForDate = (date: any) => {
    const formatted = date.format("YYYY-MM-DD");
    return events.filter((event: any) => event.date === formatted);
  };
  const dateCellRender = (date: any) => {
    const dayEvents = getEventsForDate(date);
    return (
      <ul style={{ listStyle: "none", padding: 0 }}>
        {dayEvents.map((item: any, index: any) => (
          <li key={index}>
            <Badge status={item.type} text={item.content} />
          </li>
        ))}
      </ul>
    );
  };
  useEffect(() => {
    if (user && user) {
      getWorkScheudule(user.UserId);
    }
  }, [startDate]);
  const handlePanelChange = (value: any) => {
    setStartDate(value.startOf("month").format("YYYY-MM-DDTHH:mm:ss"));
    setEndDate(value.endOf("month").format("YYYY-MM-DDTHH:mm:ss"));
  };
  return (
    <>
      <Row gutter={[16, 16]}>
        <Col span={24} md={12} lg={8}>
          <Card className="flex-col">
            <div className="flex w-full flex-col ">
              <Typography.Title level={5}>Thông tin cá nhân</Typography.Title>

              <div className="mt-2 flex flex-col gap-4 h-[310px]">
                {AboutItems.map((item) => (
                  <div className="flex" key={item.label}>
                    <div className="mr-2">{item.icon}</div>
                    <div className="mr-2">{item.label}:</div>
                    <div className="opacity-50">{item.val}</div>
                  </div>
                ))}
              </div>
            </div>
          </Card>
        </Col>

        <Col span={24} md={12} lg={16}>
          <Card className="flex-col !items-start">
            <Typography.Title level={5}>
              Dòng thời gian thông báo
            </Typography.Title>
            <Timeline
              className="!mt-4 w-full overflow-y-auto h-[300px] "
              items={[
                ...notifications.map((i: any) => {
                  return {
                    color: themeVars.colors.palette.error.default,
                    children: (
                      <div className="flex flex-col mt-[6px]">
                        <div className="flex items-center justify-between">
                          <Typography.Text strong>
                            {i.Notification.Name}
                          </Typography.Text>
                          <div className="opacity-50">
                            {moment(i.Notification.ScheduleDate).format(
                              "DD/MM/YYYY"
                            )}
                          </div>
                        </div>
                        <Typography.Text type="secondary" className="text-xs">
                          Người gửi: {i.Notification.CreatedUser}
                        </Typography.Text>
                      </div>
                    ),
                  };
                }),
              ]}
            />
          </Card>
        </Col>
      </Row>
      <Card className="flex-col !items-start mt-[16px]">
        <Typography.Title level={5}>
          Lịch chấm công tháng {moment(startDate).format("M")}
        </Typography.Title>
        <ConfigProvider locale={viVN}>
          {" "}
          <Calendar
            dateCellRender={dateCellRender}
            onPanelChange={handlePanelChange}
          />{" "}
        </ConfigProvider>
      </Card>
    </>
  );
}
