import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum PostTypeApi {
  Search = "/PostTypes/search",
  Add = "/PostTypes/add",
  Update = "/PostTypes/update",
  GetById = "/PostTypes",
  Delete = "/PostTypes",
}

const searchItem = (data: any) =>
  apiClient.post({ url: PostTypeApi.Search, data });
const addItem = (data: any) => apiClient.post({ url: PostTypeApi.Add, data });
const updateItem = (data: any) =>
  apiClient.put({ url: PostTypeApi.Update, data });
const getById = (data: any) =>
  apiClient.get({ url: PostTypeApi.GetById + "/" + data });
const deleteItem = (data: any) =>
  apiClient.delete({ url: PostTypeApi.Delete + `/${data}`, data });

export default {
  searchItem,
  addItem,
  updateItem,
  deleteItem,
  getById,
};
