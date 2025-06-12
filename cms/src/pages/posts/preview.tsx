import { useState, useEffect } from "react";
import { Modal, Image, Flex } from "antd";
import post from "@/api/posts/index.tsx";
import { useMutation } from "@tanstack/react-query";
const Preview = ({ isModalOpen, setIsModalOpen, postId }: any) => {
  const handleOk = () => {
    setIsModalOpen(false);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };

  const [detail, setDetail] = useState<any>();
  const detailMutation = useMutation({
    mutationFn: post.getById,
  });
  const getDetail = async () => {
    const res: any = await detailMutation.mutateAsync(postId);
    if (res.Status) {
      setDetail(res.Data);
    }
  };

  useEffect(() => {
    if (isModalOpen == true) {
      getDetail();
    }
  }, [isModalOpen]);
  return (
    <>
      <Modal
        title={detail?.Title}
        open={isModalOpen}
        onOk={handleOk}
        width={650}
        onCancel={handleCancel}
        footer={false}
      >
        <div dangerouslySetInnerHTML={{ __html: detail?.Content }} />
        <div>
          <Flex justify="space-between" wrap>
            {detail?.Images.map((i: any) => {
              return (
                <div className="mb-[16px] w-[48%]">
                  <Image
                    src={
                      import.meta.env.VITE_APP_BASE_API.slice(
                        0,
                        import.meta.env.VITE_APP_BASE_API.length - 4
                      ) + `${i.ImagePath}`
                    }
                  />
                </div>
              );
            })}
          </Flex>
        </div>
      </Modal>
    </>
  );
};
export default Preview;
