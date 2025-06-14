
import { Tabs, } from 'antd';
import { FaFolderOpen } from "react-icons/fa6";



import HomeProductPage from "./homeproduct"
import "./setting.css"
import HomebannerPage from './homebanner'

export default function Setting() {
   const tabPosition: any = 'left';

   const items: any = [
      {
         label: <div className="flex"><FaFolderOpen className="mt-[4px] mr-[4px]" /> Banner trang chủ</div>,
         key: 1,
         children: <div><HomebannerPage /></div>,
      },
      {
         label: <div className="flex"><FaFolderOpen className="mt-[4px] mr-[4px]" /> Sản phẩm nổi bật</div>,
         key: 2,
         children: <div><HomeProductPage /></div>,
      },
   ]
   return (
      <>

         <Tabs className="h-full"
            tabPosition={tabPosition}
            items={items}
         />
      </>
   );
}
