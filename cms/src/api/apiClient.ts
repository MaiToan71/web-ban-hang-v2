import axios, { type AxiosRequestConfig, type AxiosError, type AxiosResponse } from "axios";

import { t } from "@/locales/i18n";
import userStore from "@/store/userStore";

import { toast } from "sonner";
import type { Result } from "#/api";


const axiosInstance = axios.create({
	baseURL: import.meta.env.VITE_APP_BASE_API,
	timeout: 50000,
	headers: { "Content-Type": "application/json;charset=utf-8" },
});
// content type

axiosInstance.interceptors.request.use(
	(config) => {
		const userStoreLocal:any =localStorage.getItem('userStore')
const token:any = userStoreLocal ? JSON.parse(userStoreLocal).state.userToken.accessToken : null;

		config.headers.Authorization = "Bearer "+token;
		return config;
	},
	(error) => {
		
		return Promise.reject(error);
	},
);
/**
 * Sets the default authorization
 * @param {*} token
 */
const setAuthorization = (token:any) => {
  axiosInstance.defaults.headers.common["Authorization"] = "Bearer " + token;
};

axiosInstance.interceptors.response.use(
	(res: any) => {
		if (res.data.Status == false) {
			toast.error(res.data.Error, {
			position: "top-center",
		});
		} 
		const hasSuccess = res.data.Status === true;
		const data=  res.data
		if (hasSuccess) {
			return data;
		}

		throw new Error(res.Message || t("sys.api.apiRequestFailed"));
	
	},
	(error: any) => {
		console.log(error)
		const { response, message } = error || {};

		const errMsg = response?.data?.message || message || t("sys.api.errorMessage");
		toast.error(errMsg, {
			position: "top-center",
		});

		const status = response?.status;
		if (status === 401) {
			userStore.getState().actions.clearUserInfoAndToken();
		}
		return Promise.reject(error);
	},
);

class APIClient {
	get<T = any>(config: AxiosRequestConfig): Promise<T> {
		return this.request({ ...config, method: "GET" });
	}

	post<T = any>(config: AxiosRequestConfig): Promise<T> {
		return this.request({ ...config, method: "POST" });
	}

	put<T = any>(config: AxiosRequestConfig): Promise<T> {
		return this.request({ ...config, method: "PUT" });
	}

	delete<T = any>(config: AxiosRequestConfig): Promise<T> {
		return this.request({ ...config, method: "DELETE" });
	}

	request<T = any>(config: AxiosRequestConfig): Promise<T> {
		return new Promise((resolve, reject) => {
			axiosInstance
				.request<any, AxiosResponse<Result>>(config)
				.then((res: AxiosResponse<Result>) => {
					resolve(res as unknown as Promise<T>);
				})
				.catch((e: Error | AxiosError) => {
					reject(e);
				});
		});
	}
}
export   {setAuthorization,APIClient};
