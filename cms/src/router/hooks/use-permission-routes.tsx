import { isEmpty } from "ramda";
import { Suspense, lazy, useMemo } from "react";
import { Navigate, Outlet } from "react-router";

import { Iconify } from "@/components/icon";
import { CircleLoading } from "@/components/loading";

import { flattenTrees } from "@/utils/tree";
import { useUserPermission } from "@/store/userStore";
import { Tag } from "antd";
import type { Permission } from "#/entity";
import { BasicStatus, PermissionType } from "#/enum";
import type { AppRouteObject } from "#/router";
import { getRoutesFromModules } from "../utils";

const ENTRY_PATH = "/src/pages";
const PAGES = import.meta.glob("/src/pages/**/*.tsx");
const loadComponentFromPath = (path: string) => PAGES[`${ENTRY_PATH}${path}`];

/**
 * Build complete route path by traversing from current permission to root
 * @param {Permission} permission - current permission
 * @param {Permission[]} flattenedPermissions - flattened permission array
 * @param {string[]} segments - route segments accumulator
 * @returns {string} normalized complete route path
 */
function buildCompleteRoute(
  permission: Permission,
  flattenedPermissions: Permission[],
  segments: string[] = []
): string {
  //	return `/${segments.join("/")}`;
  // Add current route segment
  segments.unshift(permission.route);

  // Base case: reached root permission
  if (!permission.parentId) {
    return `/${segments.join("/")}`;
  }

  // Find parent and continue recursion
  const parent = flattenedPermissions.find((p) => p.id === permission.parentId);
  if (!parent) {
    console.warn(`Parent permission not found for ID: ${permission.parentId}`);
    return `/${segments.join("/")}`;
  }

  return buildCompleteRoute(parent, flattenedPermissions, segments);
}

// Components
function NewFeatureTag() {
  return (
    <Tag color="cyan" className="!ml-2">
      <div className="flex items-center gap-1">
        <Iconify icon="solar:bell-bing-bold-duotone" size={12} />
        <span className="ms-1">NEW</span>
      </div>
    </Tag>
  );
}
/*const fixPermissions = [
  {
    id: "1",
    parentId: "0",
    label: "Trang chủ",
    name: "Dashboard",
    icon: "material-symbols:home-outline",
    type: 1,
    order: 1,
    route: "dashboard",
    component: "/dashboard/analysis/index.tsx",
  },

  {
    id: "9",
    parentId: "",
    label: "Công việc",
    name: "products",
    icon: "eos-icons:project-outlined",
    route: "projects",
    type: 0,
    order: 3,
    children: [
      {
        id: "200",
        parentId: "9",
        label: "Thống kê",
        name: "Project",
        type: 1,
        order: 1,
        route: "dashboard",
        component: "/projects/product/index.tsx",
      },
      {
        id: "20",
        parentId: "9",
        label: "Dự án hiện tại",
        name: "Project",
        type: 1,
        order: 1,
        route: "project",
        component: "/projects/product/index.tsx",
      },

      {
        id: "21",
        parentId: "9",
        label: "Giao  cho tôi",
        name: "individual",
        type: 1,
        order: 2,
        route: "individual",
        component: "/projects/individual/index.tsx",
      },
      {
        id: "21",
        parentId: "9",
        label: "Đang theo dõi",
        name: "todo",
        type: 1,
        order: 3,
        route: "to-do",
        component: "/projects/kanban/index.tsx",
      },
      {
        id: "351",
        parentId: "9",
        label: "Lịch biểu",
        name: "calenar",
        type: 1,
        order: 4,
        route: "calendar",
        component: "/sys/others/calendar/index.tsx",
      },
    ],
  },
  {
    id: "20",
    parentId: "",
    label: "Mạng xã hội",
    name: "posts",
    icon: "eos-icons:project-outlined",
    route: "posts",
    type: 0,
    order: 3,
    children: [
      {
        id: "1",
        parentId: "20",
        label: "Facebook",
        name: "Facebook",
        type: 1,
        route: "facebook",
        component: "/posts/facebook.tsx",
      },
      {
        id: "2",
        parentId: "20",
        label: "Tiktok",
        name: "Tiktok",
        type: 1,
        route: "tiktok",
        component: "/posts/tiktok.tsx",
      },
    ],
  },
  {
    id: "2",
    parentId: "",
    label: "Danh sách Cộng tác viên",
    name: "Client",
    icon: "clarity:employee-line",
    type: 1,
    order: 4,
    route: "user/client",
    component: "/management/user/client/index.tsx",
  },

  {
    id: "3",
    parentId: "",
    label: "Tiền cọc",
    name: "Payment",
    icon: "grommet-icons:money",
    type: 1,
    order: 5,
    route: "user/payment",
    component: "/management/user/payment/index.tsx",
  },
  {
    id: "4",
    parentId: "",
    label: "Thông báo",
    name: "Notification",
    icon: "mynaui:notification-solid",
    type: 1,
    order: 6,
    route: "notification",
    component: "/settings/notification/index.tsx",
  },
  {
    id: "5",
    parentId: "",
    label: "Chậm tiến độ",
    name: "User",
    icon: "ri:admin-fill",
    type: 1,
    order: 7,
    route: "user",
    component: "/management/system/user/index.tsx",
  },
  {
    id: "8",
    parentId: "",
    label: "Cài đặt",
    name: "settings",
    icon: "uil:setting",
    route: "settings",
    type: 0,
    order: 8,
    children: [
      {
        id: "9",
        parentId: "8",
        label: "Ngân hàng",
        name: "Role",
        type: 1,
        route: "bank",
        component: "/settings/bank/index.tsx",
      },
      {
        id: "6",
        parentId: "8",
        label: "Vai trò",
        name: "Role",
        type: 1,
        route: "role",
        component: "/management/system/role/index.tsx",
      },
      {
        id: "7",
        parentId: "8",
        label: "Quyền",
        name: "permission",
        type: 1,
        route: "permission",
        component: "/management/system/permission/index.tsx",
      },
    ],
  },
  {
    id: "20",
    parentId: "",
    label: "Trang chủ",
    name: "mobile-dashboad",
    icon: "clarity:employee-line",
    type: 1,
    order: 9,
    route: "mobile/dashboad",
    component: "/mobiles/dashboard/index.tsx",
  },
  {
    id: "21",
    parentId: "",
    label: "Số dư",
    name: "mobile-recharge",
    icon: "clarity:employee-line",
    type: 1,
    order: 10,
    route: "mobile/recharge",
    component: "/mobiles/recharge/index.tsx",
  },
  {
    id: "21",
    parentId: "",
    label: "Thông báo",
    name: "mobile-recharge",
    icon: "clarity:employee-line",
    type: 1,
    order: 10,
    route: "mobile/notification",
    component: "/mobiles/notification/index.tsx",
  },
  {
    id: "22",
    parentId: "",
    label: "Tài khoản",
    name: "mobile-recharge",
    icon: "clarity:employee-line",
    type: 1,
    order: 12,
    route: "mobile/account",
    component: "/mobiles/account/index.tsx",
  },
];*/

// Route Transformers
const createBaseRoute = (
  permission: Permission,
  completeRoute: string
): AppRouteObject => {
  const {
    route,
    label,
    icon,
    order,
    hide,
    hideTab,
    status,
    frameSrc,
    newFeature,
  } = permission;

  const baseRoute: AppRouteObject = {
    path: route,
    meta: {
      label,
      key: completeRoute,
      hideMenu: !!hide,
      hideTab,
      disabled: status === BasicStatus.DISABLE,
    },
  };

  if (order) baseRoute.order = order;
  if (baseRoute.meta) {
    if (icon) baseRoute.meta.icon = icon;
    if (frameSrc) baseRoute.meta.frameSrc = frameSrc;
    if (newFeature) baseRoute.meta.suffix = <NewFeatureTag />;
  }

  return baseRoute;
};

const createCatalogueRoute = (
  permission: Permission,
  flattenedPermissions: Permission[]
): AppRouteObject => {
  const baseRoute = createBaseRoute(
    permission,
    buildCompleteRoute(permission, flattenedPermissions)
  );

  if (baseRoute.meta) {
    baseRoute.meta.hideTab = true;
  }

  const { parentId, children = [] } = permission;
  if (!parentId) {
    baseRoute.element = (
      <Suspense fallback={<CircleLoading />}>
        <Outlet />
      </Suspense>
    );
  }

  baseRoute.children = transformPermissionsToRoutes(
    children,
    flattenedPermissions
  );

  if (!isEmpty(children)) {
    baseRoute.children.unshift({
      index: true,
      element: <Navigate to={children[0].route} replace />,
    });
  }

  return baseRoute;
};

const createMenuRoute = (
  permission: Permission,
  flattenedPermissions: Permission[]
): AppRouteObject => {
  const baseRoute = createBaseRoute(
    permission,
    buildCompleteRoute(permission, flattenedPermissions)
  );

  if (permission.component) {
    const Element = lazy(loadComponentFromPath(permission.component) as any);

    if (permission.frameSrc) {
      baseRoute.element = <Element src={permission.frameSrc} />;
    } else {
      baseRoute.element = (
        <Suspense fallback={<CircleLoading />}>
          <Element />
        </Suspense>
      );
    }
  }

  return baseRoute;
};

function transformPermissionsToRoutes(
  permissions: Permission[],
  flattenedPermissions: Permission[]
): AppRouteObject[] {
  return permissions.map((permission) => {
    if (permission.type === PermissionType.CATALOGUE) {
      return createCatalogueRoute(permission, flattenedPermissions);
    }
    return createMenuRoute(permission, flattenedPermissions);
  });
}

const ROUTE_MODE = import.meta.env.VITE_APP_ROUTER_MODE;
export function usePermissionRoutes() {
  if (ROUTE_MODE === "module") {
    return getRoutesFromModules();
  }

  //const permissions = useUserPermission();
  const permissions: any = useUserPermission();
  return useMemo(() => {
    if (!permissions) return [];

    const flattenedPermissions: any = flattenTrees(permissions);
    return transformPermissionsToRoutes(permissions, flattenedPermissions);
  }, [permissions]);
}
