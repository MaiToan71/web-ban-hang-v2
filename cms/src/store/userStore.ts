import { useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router";
import { create } from "zustand";
import { createJSONStorage, persist } from "zustand/middleware";
import { setAuthorization } from "@/api/apiClient";
import userService, { type SignInReq } from "@/api/services/userService";
import helper from "@/api/helper";
import { toast } from "sonner";
import type { UserInfo, UserToken } from "#/entity";
import { StorageEnum } from "#/enum";

const { VITE_APP_HOMEPAGE: HOMEPAGE } = import.meta.env;

type UserStore = {
	userInfo: Partial<UserInfo>;
	userToken: UserToken;
	actions: {
		setUserInfo: (userInfo: UserInfo) => void;
		setUserToken: (token: UserToken) => void;
		clearUserInfoAndToken: () => void;
	};
};

const useUserStore = create<UserStore>()(
	persist(
		(set) => ({
			userInfo: {},
			userToken: {},
			actions: {
				setUserInfo: (userInfo:any) => {
					set({ userInfo });
				},
				setUserToken: (userToken:any) => {
					set({ userToken });
				},
				clearUserInfoAndToken() {
					set({ userInfo: {}, userToken: {} });
				},
			},
		}),
		{
			name: "userStore", // name of the item in the storage (must be unique)
			storage: createJSONStorage(() => localStorage), // (optional) by default, 'localStorage' is used
			partialize: (state:any) => ({
				[StorageEnum.UserInfo]: state.userInfo,
				[StorageEnum.UserToken]: state.userToken,
			}),
		},
	),
);

export const useUserInfo = () => useUserStore((state) => state.userInfo);
export const useUserToken = () => useUserStore((state) => state.userToken);
export const useUserPermission = () =>
	useUserStore((state: any) => state?.userInfo?.Permissions);

export const useUserActions = () => useUserStore((state) => state.actions);


export const useSignIn = () => {
	const navigatge = useNavigate();
	const { setUserToken, setUserInfo } = useUserActions();

	const signInMutation = useMutation({
		mutationFn: userService.signin,
	});

	const signIn = async (data: SignInReq) => {
		try {
			localStorage.removeItem('userStore')
			const res: any = await signInMutation.mutateAsync(data);
		
			if (res.Status) {
				
				const user = res.Data;
				setAuthorization(res.Token);
				const accessToken = res.Token;
				const refreshToken = res.Token;

				const menus: any = [];

				res.Data.Permissions.map((i: any) => {
					console.log(i)
					menus.push({
						id: `${i.Id}`,
						parentId: `${i.ParentId}`,
						label: i.Name,
						name: i.Label,
						icon: i.Icon,
						type: i.PermissionType,
						order: i.Order,
						route: i.Route,
						hide:i.Hide,
						component: i.Component,
					});
				});


				const fixMenus: any = helper.convertToTree(menus);

				fixMenus.map((i: any) => {
					if (i.children.length == 0) {
						delete i['children']
					} else {
						i.type = 0;
					}
				})
				user.Permissions=fixMenus
				setUserToken({ accessToken, refreshToken });
				setUserInfo(user);
				navigatge(HOMEPAGE, { replace: true });
				toast.success("Sign in success!");
			} 
		} catch (err) {
			toast.error(err.message, {
				position: "top-center",
			});
		}
	};

	return signIn;
};

export default useUserStore;
