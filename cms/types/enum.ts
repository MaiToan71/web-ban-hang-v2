export enum BasicStatus {
	DISABLE = 0,
	ENABLE = 1,
}

export enum ResultEnum {
	SUCCESS = 1,
	ERROR = -1,
	TIMEOUT = 401,
}

export enum UserType {
	CLIENT = 0,
	EMPLOYEE = 1,
	SUPERADMIN = 2
}
export enum Workflow {
	CANCEL = -1,
	CREATED = 1,
	WAITING = 2,
	PROCESSING = 3,
	COMPLETED = 4,
}
export enum OrderType {

	Input = 0,
	Output = 1
}

export enum StorageEnum {
	UserInfo = "userInfo",
	UserToken = "userToken",
	Settings = "settings",
	I18N = "i18nextLng",
}

export enum ThemeMode {
	Light = "light",
	Dark = "dark",
}

export enum ThemeLayout {
	Vertical = "vertical",
	Horizontal = "horizontal",
	Mini = "mini",
}

export enum ThemeColorPresets {
	Default = "default",
	Cyan = "cyan",
	Purple = "purple",
	Blue = "blue",
	Orange = "orange",
	Red = "red",
}

export enum LocalEnum {
	en_US = "en_US",
	zh_CN = "zh_CN",
}

export enum MultiTabOperation {
	FULLSCREEN = "fullscreen",
	REFRESH = "refresh",
	CLOSE = "close",
	CLOSEOTHERS = "closeOthers",
	CLOSEALL = "closeAll",
	CLOSELEFT = "closeLeft",
	CLOSERIGHT = "closeRight",
}

export enum PermissionType {
	CATALOGUE = 0,
	MENU = 1,
	BUTTON = 2,
}

export enum PostTypeEnum {
	Folder = 0,
	Category = 1,
	Brand = 2,
}
export enum AttributeEnum {
	Tag = 0,
	Color = 1,
	Size = 2,
}
export enum ConfigEnum {
	ProductHome = 0,
	PostHome = 1,
	ProductSale = 2,
	BannerHome = 3,
	BannerCategory = 4,
}
export enum PlatformData {
	FACEBOOK = 0,
	TIKTOK = 1,
	YOUTUBE = 2,
	WEBSITE = 3
}