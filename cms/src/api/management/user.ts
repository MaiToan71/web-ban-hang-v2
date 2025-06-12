import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum UserApi {
	Search = "/users/search",
	Add = "/users/add",
	UserInfo = "/users/info",
	Update = "/users/update",
	UpdatePassword = "/users/changepassword",
	UpdateWorkflow = "/users/update/workflow",
	Delete = "/users/delete",
	StatisticalJob = "/users/statistical/job",
	TimeShiftType = "/users/statistical/TimeShiftType",
		TimeShiftTags = "/users/TimeShiftTags",
}
const userInfo = () =>
	apiClient.get({ url: UserApi.UserInfo });
const searchItem = (data: any) =>
	apiClient.post({ url: UserApi.Search, data });
const addItem = (data: any) =>
	apiClient.post({ url: UserApi.Add, data });
const updateItem = (data: any) =>
	apiClient.put({ url: UserApi.Update, data });
const updatePassword = (data: any) =>
	apiClient.put({ url: UserApi.UpdatePassword, data });
const updateWorkflowItem = (data: any) =>
	apiClient.put({ url: UserApi.UpdateWorkflow, data });
const deleteItem = (data: any) =>
	apiClient.delete({ url: UserApi.Delete+`?id=${data}`, data });
const statisticalJob = (data: any) =>
	apiClient.post({ url: UserApi.StatisticalJob, data });
const timeShiftType = (data: any) =>
	apiClient.post({ url: UserApi.TimeShiftType, data });

const timeShiftTags = (data: any) =>
	apiClient.post({ url: UserApi.TimeShiftTags, data });
export default {
	searchItem,
	addItem,
	updateItem,
	deleteItem,
	updateWorkflowItem,
	statisticalJob,
	timeShiftType,
	timeShiftTags,
	userInfo,
	updatePassword
};
