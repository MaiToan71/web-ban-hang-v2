import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum ConfigApi {
  Search = "/Configs/search",
  Add = "/Configs/add",
  Update = "/Configs/update",
  GetById = "/Configs",
  Delete = "/Configs",
}

const searchItem = (data: any) =>
  apiClient.post({ url: ConfigApi.Search, data });
const addItem = (data: any) => apiClient.post({ url: ConfigApi.Add, data });
const updateItem = (data: any) =>
  apiClient.put({ url: ConfigApi.Update, data });
const getById = (data: any) =>
  apiClient.get({ url: ConfigApi.GetById + "/" + data });
const deleteItem = (data: any) =>
  apiClient.delete({ url: ConfigApi.Delete + `/${data}`, data });

export default {
  searchItem,
  addItem,
  updateItem,
  deleteItem,
  getById,
};
