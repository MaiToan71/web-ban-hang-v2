import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum DashboardApi {
  Statistical = "/dashboards/statistical",
  StatisticalRole = "/dashboards/statistical/role",
  StatisticalMoneyDate = "/dashboards/statistical/money/date",
}

const statistical = () =>
	apiClient.get({ url: DashboardApi.Statistical});
const statisticalRole = () =>
	apiClient.get({ url: DashboardApi.StatisticalRole});
const statisticalMoneyDate = (data: any) =>
	apiClient.post({ url: DashboardApi.StatisticalMoneyDate, data });

export default {
	statistical,
	statisticalRole,
	statisticalMoneyDate
};
