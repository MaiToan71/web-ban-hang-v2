import React, { useMemo, useRef, useState } from "react";
import { Select, Spin } from "antd";
import type { SelectProps } from "antd";
import userApi from "@/api/products/index";
import debounce from "lodash/debounce";

export interface DebounceSelectProps<ValueType = any>
  extends Omit<SelectProps<ValueType | ValueType[]>, "options" | "children"> {
  fetchOptions: (search: string) => Promise<ValueType[]>;
  debounceTimeout?: number;
}

function DebounceSelect<
  ValueType extends {
    key?: string;
    label: React.ReactNode;
    value: string | number;
  } = any
>({ fetchOptions, debounceTimeout = 800, ...props }: any) {
  const [fetching, setFetching] = useState(false);
  const [options, setOptions] = useState<ValueType[]>([]);
  const fetchRef = useRef(0);

  const debounceFetcher = useMemo(() => {
    const loadOptions = (value: string) => {
      fetchRef.current += 1;
      const fetchId = fetchRef.current;
      setOptions([]);
      setFetching(true);

      fetchOptions(value).then((newOptions: any) => {
        if (fetchId !== fetchRef.current) {
          // for fetch callback order
          return;
        }

        setOptions(newOptions);
        setFetching(false);
      });
    };

    return debounce(loadOptions, debounceTimeout);
  }, [fetchOptions, debounceTimeout]);

  return (
    <Select
      labelInValue
      filterOption={false}
      onSearch={debounceFetcher}
      notFoundContent={fetching ? <Spin size="small" /> : null}
      {...props}
      options={options}
    />
  );
}

async function fetchUserList(username: string) {
  let input: any = {
    Search: username,
    PageIndex: 1,
    PageSize: 100,
  };
  const res: any = await userApi.searchItem(input);
  if (res.Status) {
    return res.Items.map((i: any) => {
      return {
        label: (
          <div>
            <p>
              <strong>Tên sản phẩm :</strong>
              {i.Title}
            </p>
            <p>
              <strong>Số lượng :</strong>
              {i.Quantity}
            </p>
          </div>
        ),
        value: i.Id,
      };
    });
  }
}

const SelectProduct = ({
  placeholder,
  mode,
  value,
  name,
  fieldKey,
  onChange,
  disabled,
}: any) => {
  return (
    <DebounceSelect
      disabled={disabled}
      onChange={onChange}
      fieldKey={fieldKey}
      name={name}
      showSearch
      defaultValue={value}
      mode={mode}
      value={value}
      placeholder={placeholder}
      optionFilterProp="label"
      fetchOptions={fetchUserList}
      style={{ width: "100%", height: "64px" }}
    />
  );
};

export default SelectProduct;
