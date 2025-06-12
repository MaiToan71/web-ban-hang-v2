import { useState, useEffect } from "react";
import glass_bag from "@/assets/images/glass/ic_glass_bag.png";
import glass_buy from "@/assets/images/glass/ic_glass_buy.png";
import glass_message from "@/assets/images/glass/ic_glass_message.png";
import glass_users from "@/assets/images/glass/ic_glass_users.png";

import ChartLine from "./chart/chart-line";
import ChartPie from "@/pages/components/chart/view/chart-pie";
import { themeVars } from "@/theme/theme.css";
import { Card, Col, Row, Typography } from "antd";
import AnalysisCard from "./analysis-card";

import dashboard from "@/api/dashboards";
import { useMutation } from "@tanstack/react-query";
import moment from "moment";
function Analysis() {
  const [statistical, setStatistical] = useState<any>({
    Banks: 0,
    UserMoneys: 0,
    UserRoles: 0,
    Users: 0,
  });
  const searchStatisticalMutation = useMutation({
    mutationFn: dashboard.statistical,
  });
  const getStatistical = async () => {
    const res: any = await searchStatisticalMutation.mutateAsync();
    if (res.Status) {
      setStatistical(res.Data);
    }
  };

  const [statisticalRole, setStatisticalRole] = useState<any>([]);
  const searchStatisticalRoleMutation = useMutation({
    mutationFn: dashboard.statisticalRole,
  });
  const getStatisticalRole = async () => {
    const res: any = await searchStatisticalRoleMutation.mutateAsync();
    if (res.Status) {
      setStatisticalRole(res.Data);
    }
  };

  //chart line
  const [chartlines, setChartlines] = useState<any>([]);
  const searchChartLineMutation = useMutation({
    mutationFn: dashboard.statisticalMoneyDate,
  });
  const getChartLine = async () => {
    let params: any = {
      FromDate: moment().add(-7, "days").format("yyyy-MM-DD"),
      ToDate: moment().format("yyyy-MM-DD"),
    };
    const res: any = await searchChartLineMutation.mutateAsync(params);
    if (res.Status) {
      setChartlines(res.Data);
    }
  };
  useEffect(() => {
    getStatistical();
    getStatisticalRole();
    getChartLine();
  }, []);
  return (
    <div className="p-2">
      <Typography.Title level={2}>Hi, Welcome back 👋</Typography.Title>
      <Row gutter={[16, 16]} justify="center">
        <Col lg={6} md={12} span={24}>
          <AnalysisCard
            cover={glass_bag}
            title="Tổng sản phẩm"
            subtitle={`${statistical.Users} hoạt động`}
            style={{
              color: themeVars.colors.palette.success.dark,
              backgroundColor: `rgba(${themeVars.colors.palette.success.defaultChannel}, .2)`,
            }}
          />
        </Col>
        <Col lg={6} md={12} span={24}>
          <AnalysisCard
            cover={glass_users}
            title="Tin tức"
            subtitle={`${statistical.UserMoneys} hoạt động`}
            style={{
              color: themeVars.colors.palette.info.dark,
              backgroundColor: `rgba(${themeVars.colors.palette.info.defaultChannel}, .2)`,
            }}
          />
        </Col>
        <Col lg={6} md={12} span={24}>
          <AnalysisCard
            cover={glass_buy}
            title="Đơn hàng"
            subtitle={`${statistical.UserRoles} hoạt động`}
            style={{
              color: themeVars.colors.palette.warning.dark,
              backgroundColor: `rgba(${themeVars.colors.palette.warning.defaultChannel}, .2)`,
            }}
          />
        </Col>
        <Col lg={6} md={12} span={24}>
          <AnalysisCard
            cover={glass_message}
            title="Người đăng ký"
            subtitle={`${statistical.Banks} hoạt động`}
            style={{
              color: themeVars.colors.palette.error.dark,
              backgroundColor: `rgba(${themeVars.colors.palette.error.defaultChannel}, .2)`,
            }}
          />
        </Col>
      </Row>

      <Row gutter={[16, 16]} className="mt-8" justify="center">
        <Col span={24} lg={12} xl={8}>
          <Card title="Tài khoản">
            <ChartPie data={statisticalRole && statisticalRole} />
          </Card>
        </Col>
        <Col span={24} lg={12} xl={16}>
          <Card title="Biểu đồ đơn hàng">
            <ChartLine data={chartlines && chartlines} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export default Analysis;
