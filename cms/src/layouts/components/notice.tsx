import { Badge, Drawer } from "antd";
import { type CSSProperties, useState } from "react";

import CyanBlur from "@/assets/images/background/cyan-blur.png";
import RedBlur from "@/assets/images/background/red-blur.png";
import { IconButton, Iconify } from "@/components/icon";
import { themeVars } from "@/theme/theme.css";

export default function NoticeButton() {
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [count, setCount] = useState(4);

  const style: CSSProperties = {
    backdropFilter: "blur(20px)",
    backgroundImage: `url("${CyanBlur}"), url("${RedBlur}")`,
    backgroundRepeat: "no-repeat, no-repeat",
    backgroundColor: `rgba(${themeVars.colors.background.paperChannel}, 0.9)`,
    backgroundPosition: "right top, left bottom",
    backgroundSize: "50, 50%",
  };

  return (
    <div>
      <IconButton onClick={() => setDrawerOpen(true)}>
        <Badge
          count={count}
          styles={{
            root: { color: "inherit" },
            indicator: { color: themeVars.colors.common.white },
          }}
        >
          <Iconify icon="solar:bell-bing-bold-duotone" size={24} />
        </Badge>
      </IconButton>
      <Drawer
        placement="right"
        title="Thông báo"
        onClose={() => setDrawerOpen(false)}
        open={drawerOpen}
        closable={false}
        width={420}
        styles={{
          body: { padding: 0 },
          mask: { backgroundColor: "transparent" },
        }}
        style={style}
        extra={
          <IconButton
            style={{ color: themeVars.colors.palette.primary.default }}
            onClick={() => {
              setCount(0);
              setDrawerOpen(false);
            }}
          >
            <Iconify icon="solar:check-read-broken" size={20} />
          </IconButton>
        }
        footer={
          <div
            style={{ color: themeVars.colors.text.primary }}
            className="flex h-10 w-full items-center justify-center font-semibold"
          >
            Xem tất cả
          </div>
        }
      >
        <NoticeTab />
      </Drawer>
    </div>
  );
}

function NoticeTab() {
  return (
    <div className="flex flex-col px-6">
      {/*<Tabs defaultActiveKey="1" items={items} />*/}
    </div>
  );
}
