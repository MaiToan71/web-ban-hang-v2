import { Layout, Menu, type MenuProps } from "antd";
import { useEffect, useMemo, useState } from "react";
import { useMatches, useNavigate } from "react-router";

import Scrollbar from "@/components/scrollbar";
import {
  useFlattenedRoutes,
  usePathname,
  usePermissionRoutes,
  useRouteToMenuFn,
} from "@/router/hooks";
import { menuFilter } from "@/router/utils";
import { useSettingActions, useSettings } from "@/store/settingStore";

import { NAV_WIDTH } from "../config";

import NavLogo from "./nav-logo";

import { ThemeLayout, ThemeMode } from "#/enum";

const { Sider } = Layout;

type Props = {
  closeSideBarDrawer?: () => void;
};
export default function NavVertical(props: Props) {
  const navigate = useNavigate();
  const matches = useMatches();
  const pathname = usePathname();

  const settings = useSettings();
  const { themeLayout, themeMode, darkSidebar } = settings;
  const { setSettings } = useSettingActions();

  const routeToMenuFn = useRouteToMenuFn();
  const permissionRoutes = usePermissionRoutes();
  const flattenedRoutes = useFlattenedRoutes();

  const collapsed = useMemo(
    () => themeLayout === ThemeLayout.Mini,
    [themeLayout]
  );

  const menuList = useMemo(() => {
    const menuRoutes = menuFilter(permissionRoutes);
    return routeToMenuFn(menuRoutes);
  }, [routeToMenuFn, permissionRoutes]);

  const selectedKeys = useMemo(() => [pathname], [pathname]);

  const [openKeys, setOpenKeys] = useState<string[]>([]);
  useEffect(() => {
    if (!collapsed) {
      const keys = matches
        .filter(
          (match) => match.pathname !== "/" && match.pathname !== pathname
        )
        .map((match) => match.pathname);
      setOpenKeys(keys);
    }
  }, [collapsed, matches, pathname]);

  const handleToggleCollapsed = () => {
    setSettings({
      ...settings,
      themeLayout: collapsed ? ThemeLayout.Vertical : ThemeLayout.Mini,
    });
  };

  const onClick: MenuProps["onClick"] = ({ key }) => {
    const nextLink = flattenedRoutes?.find((e) => e.key === key);
    if (nextLink?.hideTab && nextLink?.frameSrc) {
      window.open(nextLink?.frameSrc, "_blank");
      return;
    }

    navigate(key);
    props?.closeSideBarDrawer?.();
  };

  const handleOpenChange: MenuProps["onOpenChange"] = (keys) => {
    setOpenKeys(keys);
  };

  const sidebarTheme = useMemo(() => {
    if (themeMode === ThemeMode.Dark) {
      return darkSidebar ? "light" : "dark";
    }
    return darkSidebar ? "dark" : "light";
  }, [themeMode, darkSidebar]);

  return (
    <Sider
      trigger={null}
      collapsible
      collapsed={collapsed}
      width={NAV_WIDTH}
      theme={sidebarTheme}
      className="!fixed left-0 top-0 h-screen border-r border-dashed border-border"
    >
      <div className="flex h-full flex-col">
        <NavLogo collapsed={collapsed} onToggle={handleToggleCollapsed} />

        <Scrollbar>
          <Menu
            mode="inline"
            items={menuList}
            theme={sidebarTheme}
            selectedKeys={selectedKeys}
            openKeys={openKeys}
            onOpenChange={handleOpenChange}
            className="!border-none"
            onClick={onClick}
          />
        </Scrollbar>
      </div>
    </Sider>
  );
}
