import { Suspense, lazy } from "react";
import { Navigate, Outlet } from "react-router";

import { SvgIcon } from "@/components/icon";
import { CircleLoading } from "@/components/loading";

import type { AppRouteObject } from "#/router";

const ProfilePage = lazy(() => import("@/pages/management/user/profile"));
const AccountPage = lazy(() => import("@/pages/management/user/account"));

const Blog = lazy(() => import("@/pages/management/blog"));

const management: AppRouteObject = {
  order: 2,
  path: "management",
  element: (
    <Suspense fallback={<CircleLoading />}>
      <Outlet />
    </Suspense>
  ),
  meta: {
    label: "sys.menu.management",
    icon: (
      <SvgIcon icon="ic-management" className="ant-menu-item-icon" size="24" />
    ),
    key: "/management",
  },
  children: [
    {
      index: true,
      element: <Navigate to="user" replace />,
    },
    {
      path: "user",
      meta: { label: "sys.menu.user.index", key: "/management/user" },
      children: [
        {
          index: true,
          element: <Navigate to="profile" replace />,
        },
        {
          path: "profile",
          element: <ProfilePage />,
          meta: {
            label: "sys.menu.user.profile",
            key: "/management/user/profile",
          },
        },
        {
          path: "account",
          element: <AccountPage />,
          meta: {
            label: "sys.menu.user.account",
            key: "/management/user/account",
          },
        },
      ],
    },

    {
      path: "blog",
      element: <Blog />,
      meta: { label: "sys.menu.blog", key: "/management/blog" },
    },
  ],
};

export default management;
