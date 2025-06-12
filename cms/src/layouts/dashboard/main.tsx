import { useSettings } from "@/store/settingStore";
import { themeVars } from "@/theme/theme.css";
import { cn } from "@/utils";
import { Content } from "antd/es/layout/layout";
import type { CSSProperties } from "react";
import { Outlet } from "react-router";
import { ThemeLayout } from "#/enum";
import { MULTI_TABS_HEIGHT } from "./config";

const Main = () => {
  const { themeLayout, multiTab } = useSettings();

  const mainStyle: CSSProperties = {
    paddingTop: multiTab ? MULTI_TABS_HEIGHT : 0,
    background: themeVars.colors.background.default,
    transition: "all 200ms cubic-bezier(0.4, 0, 0.2, 1) 0ms",
    width: "100%",
  };

  return (
    <Content style={mainStyle} className="flex">
      <div className="flex-grow overflow-auto size-full">
        <div
          className={cn(
            "m-auto size-full flex-grow p-5",
            themeLayout === ThemeLayout.Horizontal ? "flex-col" : "flex-row"
          )}
        >
          <Outlet />
        </div>
      </div>
    </Content>
  );
};

export default Main;
