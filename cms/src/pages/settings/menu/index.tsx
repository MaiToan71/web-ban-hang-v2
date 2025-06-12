import { Button, Card, } from "antd";

import { useState, useEffect } from "react";

import { useMutation } from "@tanstack/react-query";


import PosttypeModal from "./posttype-modal";
import posttype from "@/api/menus";

import helper from "@/api/helper";


const defaultBankValue: any = {
  Id: 0,
  BankId: "",
  BankName: "",
  AccountNo: "",
  AccountName: "",
  PostTypeEnum: 0,
};
import { Tree } from 'antd';
const { DirectoryTree } = Tree;
export default function DepartmentPage() {
  const searchMutation = useMutation({
    mutationFn: posttype.searchItem,
  });
  const [banks, setBanks] = useState<any>([]);
  const [items, setItems] = useState<any>([])
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE * 100);
  const currentPage = 1
  const getData = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
    };
    //console.log(searchMutation.mutateAsync)
    const res: any = await searchMutation.mutateAsync(param);
    setBanks(helper.convertToTree(res.Items.map((i: any) => {
      return {
        id: i.Id,
        title: i.Name,
        parentId: i.ParentId,
        name: i.Name
      }
    })));
    setItems(res.Items)

  };

  useEffect(() => {
    getData(1);
  }, []);

  const [bankModalProps, setBankModalProps] = useState<any>({
    formValue: { ...defaultBankValue },
    title: "Thêm mới",
    show: false,
    onOk: () => {
      console.log('pl')

      getData(currentPage);
      setBankModalProps((prev: any) => ({ ...prev, show: false }));
    },
    onCancel: () => {
      setBankModalProps((prev: any) => ({ ...prev, show: false }));
    },
  });



  const onCreate = () => {
    setBankModalProps((prev: any) => ({
      ...prev,
      show: true,
      ...defaultBankValue,
      title: "Thêm mới",
      formValue: { ...defaultBankValue },
    }));
  };

  const onEdit = (formValue: any) => {
    setBankModalProps((prev: any) => ({
      ...prev,
      show: true,
      title: "Chỉnh sửa",
      formValue,
    }));
  };





  const onSelect = (keys: any, info: any) => {
    let record = items.filter((i: any) => i.Id == info.node.id)[0]
    console.log(keys)
    onEdit({
      ...record,
      Id: record.Id,
      Type: record.PermissionType,
      Status: record.Status ? 1 : 0,
    })
  };

  return (
    <div className="grid grid-flow-col grid-rows-3 gap-4">
      <PosttypeModal {...bankModalProps} />

      <div className="row-span-3 ">
        <Card
          title="Thương hiệu"
          extra={
            <Button type="primary" onClick={() => onCreate()}>
              Thêm mới
            </Button>
          }
        >
          <DirectoryTree
            multiple

            defaultExpandAll
            onSelect={onSelect}
            treeData={banks}
          />
        </Card>
      </div>
    </div>

  );
}


