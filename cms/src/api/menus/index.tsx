import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum DepartmentApi {
  Search = "/Menus/search",
  Add = "/Menus/add",
  Update = "/Menus/update",
  GetById = "/Menus",
  Delete = "/Menus",
}

const searchItem = (data: any) =>
  apiClient.post({ url: DepartmentApi.Search, data });
const addItem = (data: any) => apiClient.post({ url: DepartmentApi.Add, data });
const updateItem = (data: any) =>
  apiClient.put({ url: DepartmentApi.Update, data });
const getById = (data: any) =>
  apiClient.get({ url: DepartmentApi.GetById + "/" + data });
const deleteItem = (data: any) =>
  apiClient.delete({ url: DepartmentApi.Delete + `/${data}`, data });

export default {
  searchItem,
  addItem,
  updateItem,
  deleteItem,
  getById,
};
