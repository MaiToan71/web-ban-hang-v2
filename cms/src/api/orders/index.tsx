import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum OrderApi {
  Search = "/Orders/search",
  OrderdetailSearch = "/Orders/Orderdetail/search",
  Add = "/Orders/add",
  Update = "/Orders/update",
  GetById = "/Orders",
  Delete = "/Orders/SendWorkflow",
}
const searchOrderDetailItem = (data: any) =>
  apiClient.post({ url: OrderApi.OrderdetailSearch, data });
const searchItem = (data: any) =>
  apiClient.post({ url: OrderApi.Search, data });
const addItem = (data: any) => apiClient.post({ url: OrderApi.Add, data });
const updateItem = (data: any) => apiClient.put({ url: OrderApi.Update, data });
const getById = (data: any) =>
  apiClient.get({ url: OrderApi.GetById + "/" + data });
const deleteItem = (OrderId: any, workflow: any) =>
  apiClient.post({
    url: OrderApi.Delete + `?OrderId=${OrderId}&workflow=${workflow}`,
  });

export default {
  searchItem,
  addItem,
  updateItem,
  deleteItem,
  getById,
  searchOrderDetailItem,
};
