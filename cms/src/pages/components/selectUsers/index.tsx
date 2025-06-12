import React, { useMemo, useRef, useState } from "react";
import { Select, Spin } from "antd";
import type { SelectProps } from "antd";
import userApi from "@/api/management/user";
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

// Usage of DebounceSelect
interface UserValue {
  label: string;
  value: string;
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
        label: `${i.FullName}`,
        value: i.UserId,
      };
    });
  }
}

const SelectUser = ({ placeholder, mode, value, setValue }: any) => {
  return (
    <DebounceSelect
      showSearch
      defaultValue={value}
      mode={mode}
      value={value}
      placeholder={placeholder}
      optionFilterProp="label"
      fetchOptions={fetchUserList}
      onChange={(newValue: any) => {
        setValue(newValue as UserValue[]);
      }}
      style={{ width: "100%" }}
    />
  );
};

export default SelectUser;
