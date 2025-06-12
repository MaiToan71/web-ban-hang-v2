import { APIClient } from "../apiClient";
const apiClient = new APIClient();
export enum RoleApi {
	Search = "/roles/search",
	Add = "/roles/add",
	Update = "/roles/update",
	Delete = "/roles/delete",
}

const searchItem = (data: any) =>
	apiClient.post({ url: RoleApi.Search, data });
const addItem = (data: any) =>
	apiClient.post({ url: RoleApi.Add, data });
const updateItem = (data: any) =>
	apiClient.put({ url: RoleApi.Update, data });
const deleteItem = (data: any) =>
	apiClient.delete({ url: RoleApi.Delete+`?id=${data}`, data });

export default {
	searchItem,
	addItem,
	updateItem,
	deleteItem,
};
