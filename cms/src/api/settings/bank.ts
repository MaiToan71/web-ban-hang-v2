import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum BankApi {
	Search = "/banks/search",
	Add = "/banks/add",
	Update = "/banks/update",
	Delete = "/banks/delete",
}

const searchItem = (data: any) =>
	apiClient.post({ url: BankApi.Search, data });
const addItem = (data: any) =>
	apiClient.post({ url: BankApi.Add, data });
const updateItem = (data: any) =>
	apiClient.put({ url: BankApi.Update, data });
const deleteItem = (data: any) =>
	apiClient.delete({ url: BankApi.Delete+`?id=${data}`, data });

export default {
	searchItem,
	addItem,
	updateItem,
	deleteItem,
};
