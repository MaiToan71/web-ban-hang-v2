import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum BankUserApi {
	Search = "/bankusers/search",
	Add = "/bankusers/add",
	Update = "/bankusers/update",
	UpdateWorkflow = "/bankusers/update/workflow",
	Delete = "/bankusers/delete",
}

const searchItem = (data: any) =>
	apiClient.post({ url: BankUserApi.Search, data });
const addItem = (data: any) =>
	apiClient.post({ url: BankUserApi.Add, data });
const updateItem = (data: any) =>
	apiClient.put({ url: BankUserApi.Update, data });
const updateWorkflowItem = (data: any) =>
	apiClient.put({ url: BankUserApi.UpdateWorkflow, data });
const deleteItem = (data: any) =>
	apiClient.delete({ url: BankUserApi.Delete+`?id=${data}`, data });

export default {
	searchItem,
	addItem,
	updateItem,
	deleteItem,
	updateWorkflowItem
};
