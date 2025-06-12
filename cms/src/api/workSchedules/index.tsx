import { APIClient } from "../apiClient";

const apiClient = new APIClient();

export enum workScheduleApi {
  Update = "/WorkSchedule/update",
  UpdateAdmin = "/WorkSchedule/update-ad",
  Search = "/WorkSchedule/search",
  GetById = "/WorkSchedule/get_by_id",
  Delete = "/WorkSchedule",
}

const updateAdmin = (data: any) =>
  apiClient.post({ url: workScheduleApi.UpdateAdmin, data });
const updateEmployee = (data: any) =>
  apiClient.post({ url: workScheduleApi.Update, data });
const searchItem = (data: any) =>
  apiClient.post({ url: workScheduleApi.Search, data });
const getById = (data: any) =>
  apiClient.get({ url: workScheduleApi.GetById + "/" + data });
const deleteItem = (data: any) =>
  apiClient.delete({ url: workScheduleApi.Delete + "/" + data });
export default {
  searchItem,
  updateEmployee,
  updateAdmin,
  getById,
  deleteItem,
};
