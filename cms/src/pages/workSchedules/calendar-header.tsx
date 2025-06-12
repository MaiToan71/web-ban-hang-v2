import { Button, Dropdown, type MenuProps } from "antd";
import dayjs from "dayjs";
import { type ReactNode, useMemo } from "react";

import { IconButton, Iconify } from "@/components/icon";
import { up } from "@/hooks";
import { useMediaQuery } from "@/hooks";

export type HandleMoveArg = "next" | "prev" | "today";
export type ViewType = "resourceTimelineWeek" | "resourceTimelineDay";
type ViewTypeMenu = {
  key: string;
  label: string;
  view: ViewType;
  icon: ReactNode;
};

export default function CalendarHeader({
  now,
  view,
  onMove,

  onViewTypeChange,
}: any) {
  const LgBreakPoint = useMediaQuery(up("lg"));

  const items = useMemo<ViewTypeMenu[]>(
    () => [
      {
        key: "1",
        label: "Tháng",
        view: "resourceTimelineWeek",
        icon: <Iconify icon="mdi:calendar-month-outline" size={18} />,
      },

      {
        key: "3",
        label: "Ngày",
        view: "resourceTimelineDay",
        icon: <Iconify icon="mdi:calendar-today-outline" size={18} />,
      },
    ],
    []
  );

  const handleMenuClick: MenuProps["onClick"] = (e) => {
    const selectedViewType = items.find((item) => item.key === e.key);
    if (selectedViewType) {
      onViewTypeChange(selectedViewType.view);
    }
  };

  const viewTypeMenu = (view: ViewType) => {
    const viewTypeItem = items.find((item) => item.view === view);
    if (!viewTypeItem) {
      return null;
    }
    const { icon, label } = viewTypeItem;
    return (
      <div className="flex items-center">
        {icon}
        <span className="mx-1 !text-sm font-medium">{label}</span>
        <Iconify icon="solar:alt-arrow-down-outline" size={20} />
      </div>
    );
  };

  return (
    <div className="relative flex items-center justify-between py-5">
      {LgBreakPoint && (
        <Dropdown menu={{ items, onClick: handleMenuClick }}>
          <Button type="text">{viewTypeMenu(view)}</Button>
        </Dropdown>
      )}

      <div className="flex cursor-pointer items-center justify-center">
        <IconButton>
          <Iconify
            icon="solar:alt-arrow-left-outline"
            onClick={() => onMove("prev")}
            size={20}
          />
        </IconButton>
        <span className="mx-2 text-base font-bold">
          {dayjs(now).format("DD MMM YYYY")}
        </span>
        <IconButton>
          <Iconify
            icon="solar:alt-arrow-right-outline"
            onClick={() => onMove("next")}
            size={20}
          />
        </IconButton>
      </div>

      <div className="flex items-center"></div>
    </div>
  );
}
