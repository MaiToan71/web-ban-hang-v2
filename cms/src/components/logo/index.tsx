import { NavLink } from "react-router";

import logo from "@/assets/images/logo.png";
import { Image } from "antd";
interface Props {
  size?: number | string;
}
function Logo({ size = 50 }: Props) {
  return (
    <NavLink to="/">
      <Image width={size} src={logo} />
    </NavLink>
  );
}

export default Logo;
