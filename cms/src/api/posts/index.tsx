import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum PostApi {
  Search = "/Posts/search",
  Add = "/Posts/add",
  Update = "/Posts/update",
  GetById = "/Posts",
  Delete = "/Posts",
  DeleteImage = "/Posts/image",
}

const searchItem = (data: any) => apiClient.post({ url: PostApi.Search, data });
const addItem = (data: any) =>
  apiClient.post({
    url: PostApi.Add,
    data,
    headers: {
      "Content-Type": "multipart/form-data", // Send the file as binary data
    },
  });
const updateItem = (data: any) =>
  apiClient.put({
    url: PostApi.Update,
    data,
    headers: {
      "Content-Type": "multipart/form-data", // Send the file as binary data
    },
  });
const getById = (data: any) =>
  apiClient.get({ url: PostApi.GetById + "/" + data });
const deleteItem = (data: any) =>
  apiClient.delete({ url: PostApi.Delete + `/${data}`, data });
const deleteImage = (imageId: any, productId: any) =>
  apiClient.delete({
    url: `${PostApi.DeleteImage}?imageId=${imageId}&postId=${productId}`,
  });
export default {
  searchItem,
  addItem,
  updateItem,
  deleteItem,
  getById,
  deleteImage,
};
