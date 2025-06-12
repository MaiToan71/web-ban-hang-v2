import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum AttributeApi {
  Search = "/Attributes/search",
  Add = "/Attributes/add",
  Update = "/Attributes/update",
  GetById = "/Attributes",
  Delete = "/Attributes",
}

const searchItem = (data: any) =>
  apiClient.post({ url: AttributeApi.Search, data });
const addItem = (data: any) => apiClient.post({ url: AttributeApi.Add, data });
const updateItem = (data: any) =>
  apiClient.put({ url: AttributeApi.Update, data });
const getById = (data: any) =>
  apiClient.get({ url: AttributeApi.GetById + "/" + data });
const deleteItem = (data: any) =>
  apiClient.delete({ url: AttributeApi.Delete + `/${data}`, data });

export default {
  searchItem,
  addItem,
  updateItem,
  deleteItem,
  getById,
};
