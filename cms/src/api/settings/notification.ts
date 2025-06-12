import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum NotificationApi {
	Search = "/notification/search",
	Add = "/notification/add",
	Update = "/notification/update",
	Delete = "/notification/delete",
}

const searchItem = (data: any) =>
	apiClient.post({ url: NotificationApi.Search, data });
const addItem = (data: any) =>
	apiClient.post({ url: NotificationApi.Add, data });
const updateItem = (data: any) =>
	apiClient.put({ url: NotificationApi.Update, data });
const deleteItem = (data: any) =>
	apiClient.delete({ url: NotificationApi.Delete+`?id=${data}`, data });

export default {
	searchItem,
	addItem,
	updateItem,
	deleteItem,
};
