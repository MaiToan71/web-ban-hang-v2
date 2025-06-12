import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum ProductApi {
  Search = "/Products/search",
  Add = "/Products/add",
  Update = "/Products/update",
  GetById = "/Products",
  Delete = "/Products",
  DeleteImage = "/Products/image",
}

const searchItem = (data: any) =>
  apiClient.post({ url: ProductApi.Search, data });
const addItem = (data: any) =>
  apiClient.post({
    url: ProductApi.Add,
    data,
    headers: {
      "Content-Type": "multipart/form-data", // Send the file as binary data
    },
  });
const updateImage = (data: any) =>
  apiClient.put({
    url: ProductApi.Update,
    data,
    headers: {
      "Content-Type": "multipart/form-data", // Send the file as binary data
    },
  });
const getById = (data: any) =>
  apiClient.get({ url: ProductApi.GetById + "/" + data });
const deleteItem = (data: any) =>
  apiClient.delete({ url: ProductApi.Delete + `/${data}`, data });
const deleteImage = (imageId: any, productId: any) =>
  apiClient.delete({
    url: `${ProductApi.DeleteImage}?imageId=${imageId}&productId=${productId}`,
  });
export default {
  searchItem,
  addItem,
  updateImage,
  deleteItem,
  getById,
  deleteImage,
};
