import { Upload as AntdUpload, Typography } from "antd";

import { StyledUpload } from "./styles";


import type { UploadProps } from "antd";

const { Dragger } = AntdUpload;
const { Text, Title } = Typography;

interface Props extends UploadProps {
  setFileList?: any;
}

export function Upload({ setFileList, fileList }: Props) {
  const handleChange = ({ fileList }: any) => {
    setFileList(fileList);
  };
  return (
    <StyledUpload className="w-[100%]">
      <Dragger
        fileList={fileList}
        onChange={handleChange}
        showUploadList={true}
        listType="picture"
        accept=".jpg,.png,jpeg"
        beforeUpload={() => false} // prevent auto-upload
      >
        <div className="opacity-100 hover:opacity-80 w-full">

          <Typography>
            <Title level={5} className="mt-4">
              Chọn ảnh/video
            </Title>
            <Text type="secondary">
              Nhấn
              <Text className="mx-2 !text-primary" underline>
                tài nguyên
              </Text>
              để chọn ảnh
            </Text>
          </Typography>
        </div>
      </Dragger>
    </StyledUpload>
  );
}
