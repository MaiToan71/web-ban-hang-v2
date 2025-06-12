import { APIClient } from "../apiClient";
const apiClient = new APIClient();
import type { UserInfo, UserToken } from "#/entity";

export interface SignInReq {
	Username: string;
	Password: string;

}

export interface SignUpReq extends SignInReq {
	email: string;
}
export type SignInRes = UserToken & { user: UserInfo };

export enum UserApi {
	SignIn = "/auths/login",
	SignUp = "/auths/register",
	Logout = "/auths/logout",
	Refresh = "/auth/refresh",
	User = "/user",
}

const signin = (data: SignInReq) =>
	apiClient.post<SignInRes>({ url: UserApi.SignIn, data });
const signup = (data: any) =>
	apiClient.post({ url: UserApi.SignUp, data });
const logout = () => apiClient.get({ url: UserApi.Logout });
const findById = (id: string) =>
	apiClient.get<UserInfo[]>({ url: `${UserApi.User}/${id}` });

export default {
	signin,
	signup,
	findById,
	logout,
};
