import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum TimeShiftApi {
	Search = "/TimeShifts/search",
	Add = "/TimeShifts/add",
	Update = "/TimeShifts/update",
	UpdateWorkflow = "/TimeShifts/UpdateWorkflow",
	GetById = "/TimeShifts",
	Delete = "/TimeShifts/delete",
}

const searchItem = (data: any) =>
	apiClient.post({ url: TimeShiftApi.Search, data });
const addItem = (data: any) =>
	apiClient.post({ url: TimeShiftApi.Add, data });
const updateItem = (data: any) =>
	apiClient.put({ url: TimeShiftApi.Update, data });
const updateWorkflow = (data: any) =>
	apiClient.put({ url: TimeShiftApi.UpdateWorkflow, data });
const getById = (data: any) =>
	apiClient.get({ url: TimeShiftApi.GetById+"/"+data });
const deleteItem = (data: any) =>
	apiClient.delete({ url: TimeShiftApi.Delete+`?id=${data}`, data });

export default {
	searchItem,
	addItem,
	updateItem,
	deleteItem,
	getById,
	updateWorkflow
};
