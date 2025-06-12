import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum TimeShiftTypeApi {
	Search = "/TimeShiftTypes/search",
	Add = "/TimeShiftTypes/add",
	Update = "/TimeShiftTypes/update",
	UpdateWorkflow = "/TimeShiftTypes/update/workflow",
	Delete = "/TimeShiftTypes/delete",
	GetById = "/TimeShiftTypes",
}

const searchItem = (data: any) =>
	apiClient.post({ url: TimeShiftTypeApi.Search, data });
const addItem = (data: any) =>
	apiClient.post({ url: TimeShiftTypeApi.Add, data });
const updateItem = (data: any) =>
	apiClient.put({ url: TimeShiftTypeApi.Update, data });
const updateWorkflowItem = (data: any) =>
	apiClient.put({ url: TimeShiftTypeApi.UpdateWorkflow, data });
const deleteItem = (data: any) =>
	apiClient.delete({ url: TimeShiftTypeApi.Delete + `?id=${data}`, data });
const getById = (data: any) =>
	apiClient.get({ url: TimeShiftTypeApi.GetById+"/"+data });

export default {
	searchItem,
	addItem,
	updateItem,
	deleteItem,
	updateWorkflowItem,
	getById
};
