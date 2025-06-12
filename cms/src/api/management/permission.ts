import { APIClient } from "../apiClient";
const apiClient = new APIClient();
export enum PermissionApi {
	Search = "/permissions/search",
	Add = "/permissions/add",
	Update = "/permissions/update",
	Delete = "/permissions/delete",
}

const searchItem = (data: any) =>
	apiClient.post({ url: PermissionApi.Search, data });
const addItem = (data: any) =>
	apiClient.post({ url: PermissionApi.Add, data });
const updateItem = (data: any) =>
	apiClient.put({ url: PermissionApi.Update, data });
const deleteItem = (data: any) =>
	apiClient.delete({ url: PermissionApi.Delete+`?id=${data}`, data });

export default {
	searchItem,
	addItem,
	updateItem,
	deleteItem,
};
