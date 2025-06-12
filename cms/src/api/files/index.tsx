import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum ImageApi {
  Statistics = "/Images/statistics",
  Search = "/Images/search",
  Add = "/Images/add",
  Update = "/Images/update",
  GetById = "/Images",
  Delete = "/Images",
}
const searchItem = (data: any) =>
  apiClient.post({ url: ImageApi.Search, data });
const addItem = (data: any) =>
  apiClient.post({
    url: ImageApi.Add,
    data,
    headers: {
      "Content-Type": "multipart/form-data", // Send the file as binary data
    },
  });
const updateItem = (data: any) => apiClient.put({ url: ImageApi.Update, data });
const getById = (data: any) =>
  apiClient.get({ url: ImageApi.GetById + "/" + data });
const deleteItem = (data: any) =>
  apiClient.delete({ url: ImageApi.Delete + `/${data}`, data });
const getStatistics = (data: any) =>
  apiClient.post({ url: ImageApi.Statistics, data });

export default {
  searchItem,
  addItem,
  updateItem,
  deleteItem,
  getById,
  getStatistics,
};
