import { useEffect } from "react";
import {
  Button,
  Modal,
  Form,
  Card,
  Input,
  Row,
  Col,
  Select,
  Flex,
  InputNumber,
  Popconfirm,
} from "antd";
import { toast } from "sonner";
const { TextArea } = Input;
import { useMutation } from "@tanstack/react-query";
import { Workflow, OrderType } from "#/enum";
import SelectProduct from "@/pages/components/selectProducts";
import order from "@/api/orders/index";
import moment from "moment";
import { DeleteOutlined } from "@ant-design/icons";
const OrderModal = ({
  editItem,
  isModalOpen,
  setIsModalOpen,
  onCallBack,
  type,
}: any) => {
  const [form] = Form.useForm();
  const addMutation = useMutation({
    mutationFn: order.addItem,
  });
  const updateMutation = useMutation({
    mutationFn: order.updateItem,
  });

  const handleOk = () => {
    setIsModalOpen(false);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };

  let userStore: any = localStorage.getItem("userStore");
  let userId: any = JSON.parse(userStore).state.userInfo.UserId;
  const onFinish = async (values: any) => {
    if (userId && userId) {
      values.UserId = userId;
      values.PaymentMethod = Number(values.PaymentMethod);
      //values.Workflow = Workflow.CREATED;
      values.OrderType = OrderType.Output;
      values.OrderDetails = values.OrderDetails.map((i: any) => {
        let productObj = i.Product;
        i.ProductId = `${productObj.value}`;
        return {
          ...i,
          CapitalPrice: 0,
          OrderId: 0,
        };
      });
      if (!(values.DateDelivery && values.DateDelivery)) {
        values.DateDelivery = null;
      }
      let res = {
        Status: false,
      };
      if (!(editItem && editItem)) {
        values.Workflow = Workflow.CREATED;
        res = await addMutation.mutateAsync(values);
      } else {
        values.PostTypeId = 0;
        values.Workflow = editItem.Order.Workflow;
        values.Id = editItem.Order.Id;
        res = await updateMutation.mutateAsync(values);
        //callBack()
      }
      if (res.Status) {
        toast.success("Thành công", {
          position: "top-right",
        });
        handleCancel();
        onCallBack();
      } else {
        toast.error("Thất bại", {
          position: "top-right",
        });
      }
    }
  };

  const SendWorkflow = async (workflow: any) => {
    let res: any = await order.deleteItem(editItem.Order.Id, workflow);
    if (res.Status) {
      toast.success("Thành công", {
        position: "top-right",
      });
      handleCancel();
      onCallBack();
    } else {
      toast.error("Thất bại", {
        position: "top-right",
      });
    }
  };

  useEffect(() => {
    if (editItem && editItem) {
      let newItem = { ...editItem.Order };
      if (newItem.DateDelivery && newItem.DateDelivery) {
        newItem.DateDelivery = moment(newItem.DateDelivery).format(
          "YYYY-MM-DD"
        );
      }

      newItem.DateOrder = moment(newItem.DateOrder).format("YYYY-MM-DD");

      newItem.OrderDetails = editItem.OrderDetails.map((i: any) => {
        let product = {
          label: i.Product.Title,
          value: i.Product.Id,
          key: i.Product.Id,
        };
        return {
          ...i.OrderDetail,
          Product: product,
        };
      });
      form.setFieldsValue(newItem);
    } else {
      form.setFieldsValue({
        Code: "",
        DateOrder: "",
        UserId: "",
        DateDelivery: "",
        UnitDelivery: "",
        PaymentMethod: 0,
        Note: "",
        DeloveryManPhonenumber: "",
        DeloveryMan: "",
        OrderDetails: [
          {
            OrderId: 0,
            Product: {
              label: "",
              value: 0,
              key: 0,
            },
            CapitalPrice: 0,
            SellingPrice: 0,
            Quantity: 0,
            Discount: 0,
            Fee: 0,
          },
        ],
      });
    }
  }, [editItem, form]);

  const action = () => {
    if (editItem && editItem) {
      if (editItem.Order.Workflow == Workflow.CREATED) {
        return (
          <>
            <Popconfirm
              title="Duyệt đơn hàng"
              okText="Đồng ý"
              cancelText="Hủy"
              placement="left"
              onConfirm={() => {
                SendWorkflow(Workflow.COMPLETED);
              }}
            >
              <Button style={{ marginLeft: "8px" }} type="primary">
                Duyệt đơn hàng
              </Button>
            </Popconfirm>
            {type == "edit" ? (
              <Button
                type="primary"
                htmlType="submit"
                style={{ marginLeft: "8px" }}
              >
                Cập nhật
              </Button>
            ) : (
              <></>
            )}
          </>
        );
      }
      if (editItem.Order.Workflow == Workflow.COMPLETED) {
        return (
          <>
            <Popconfirm
              title="Hủy đơn hàng"
              okText="Đồng ý"
              cancelText="Hủy"
              placement="left"
              onConfirm={() => {
                SendWorkflow(Workflow.CANCEL);
              }}
            >
              <Button type="primary" danger style={{ marginLeft: "8px" }}>
                Hủy đơn hàng
              </Button>
            </Popconfirm>
            {type == "edit" ? (
              <Button
                type="primary"
                htmlType="submit"
                style={{ marginLeft: "8px" }}
              >
                Cập nhật
              </Button>
            ) : (
              <></>
            )}
          </>
        );
      }
      if (editItem.Order.Workflow == Workflow.CANCEL) {
        return (
          <>
            <Popconfirm
              title="Duyệt đơn hàng"
              okText="Đồng ý"
              cancelText="Hủy"
              placement="left"
              onConfirm={() => {
                SendWorkflow(Workflow.COMPLETED);
              }}
            >
              <Button style={{ marginLeft: "8px" }} type="primary">
                Duyệt đơn hàng
              </Button>
            </Popconfirm>
            {type == "edit" ? (
              <Button
                type="primary"
                htmlType="submit"
                style={{ marginLeft: "8px" }}
              >
                Cập nhật
              </Button>
            ) : (
              <></>
            )}
          </>
        );
      }
    } else {
      return (
        <Button type="primary" htmlType="submit" style={{ marginLeft: "8px" }}>
          Khởi tạo
        </Button>
      );
    }
  };
  return (
    <Modal
      width={1200}
      title={`Chi tiết đơn hàng xuất kho`}
      open={isModalOpen}
      onOk={handleOk}
      onCancel={handleCancel}
      footer={null}
    >
      <Form
        form={form}
        onFinish={onFinish}
        layout="vertical"
        initialValues={{
          Code: "",
          DateOrder: "2025-03-11T16:15:04.360Z",
          DateDelivery: "2025-03-11T16:15:04.360Z",
          PaymentMethod: 1,
          Note: "",
          OrderDetails: [
            {
              OrderId: 0,
              Product: {
                label: "",
                value: 0,
                key: 0,
              },
              CapitalPrice: 0,
              SellingPrice: 0,
              Quantity: 0,
              Discount: 0,
              Fee: 0,
            },
          ],
        }}
      >
        <Card title="Thông tin chung">
          <Row>
            <Col span={8} className="p-1">
              <Form.Item
                label="Ngày đặt hàng"
                name="DateOrder"
                rules={[
                  {
                    required: true,
                    message: "Nhập ngày đặt hàng",
                  },
                ]}
              >
                <Input type="date" />
              </Form.Item>
            </Col>

            <Col span={8} className="p-1">
              <Form.Item label="Ngày giao hàng" name="DateDelivery">
                <Input type="date" />
              </Form.Item>
            </Col>
            <Col span={8} className="p-1">
              <Form.Item label="Đơn vị giao hàng" name="UnitDelivery">
                <Select
                  placeholder="Chọn đơn vị giao hàng"
                  options={[
                    {
                      value: "Không có",
                      label: "Không có",
                    },
                    {
                      value: "Giao Hàng Nhanh - GHN",
                      label: "Giao Hàng Nhanh - GHN",
                    },
                    {
                      value: "Best Express",
                      label: "Best Express",
                    },
                    {
                      value: "Giao Hàng Tiết Kiệm - GHTK",
                      label: "Giao Hàng Tiết Kiệm - GHTK",
                    },
                    {
                      value: "J&T Express",
                      label: "J&T Express",
                    },

                    {
                      value: "Vietnam Post",
                      label: "Vietnam Post",
                    },
                    {
                      value: "Nhat Tin Logistics",
                      label: "Nhat Tin Logistics",
                    },
                    {
                      value: "Ninja Van",
                      label: "Ninja Van",
                    },
                  ]}
                />
              </Form.Item>
            </Col>
            <Col span={8} className="p-1">
              <Form.Item
                label="Thanh toán"
                name="PaymentMethod"
                rules={[
                  {
                    required: true,
                    message: "Chọn đơn vị thanh toán",
                  },
                ]}
              >
                <Select
                  placeholder="Chọn hình thức thanh toán"
                  options={[
                    {
                      value: 1,
                      label: "Chuyển khoản",
                    },
                    {
                      value: 0,
                      label: "Tiền mặt",
                    },
                    {
                      value: 2,
                      label: "Nội bộ",
                    },
                    {
                      value: 3,
                      label: "Chưa thanh toán",
                    },
                  ]}
                />
              </Form.Item>
            </Col>

            <Col span={24} className="p-1">
              <Form.Item
                label="Ghi chú"
                name="Note"
                rules={[
                  {
                    required: true,
                    message: "Vui lòng nhập ghi chú",
                  },
                ]}
              >
                <TextArea rows={4} />
              </Form.Item>
            </Col>
          </Row>
        </Card>
        <div className="mt-3">
          <Card title="Thông tin người nhận">
            <Row>
              <Col span={12} className="p-1">
                <Form.Item
                  label="Họ tên  "
                  name="DeloveryMan"
                  rules={[
                    {
                      required: true,
                      message: "Nhập thông tin",
                    },
                  ]}
                >
                  <Input placeholder="Nhập họ và tên" />
                </Form.Item>
              </Col>
              <Col span={12} className="p-1">
                <Form.Item label="Số điện thoại" name="DeloveryManPhonenumber">
                  <Input placeholder="Nhập số điện thoại" />
                </Form.Item>
              </Col>
            </Row>
          </Card>
        </div>
        <div className="mt-3"></div>
        <Card title={<a>Dannh sách sản phẩm cùng loại </a>}>
          {/* OrderDetails List */}
          <Form.List
            name="OrderDetails"
            initialValue={[
              {
                OrderId: 0,
                Product: "",
                CapitalPrice: 0,
                SellingPrice: 0,
                Quantity: 0,
                Discount: 0,
                Fee: 0,
              },
            ]}
          >
            {(fields, { add, remove }) => (
              <>
                {fields.map(({ key, name, fieldKey, ...restField }: any) => (
                  <div key={key} style={{ marginBottom: 16 }}>
                    <Row
                      style={{
                        display: "flex",
                        alignItems: "baseline",
                      }}
                    >
                      <Col span={8} className="p-1">
                        <Form.Item
                          label="Sản phẩm"
                          rules={[
                            {
                              required: true,
                              message: "Vui lòng nhập tên sản phẩm!",
                            },
                          ]}
                          {...restField}
                          name={[name, "Product"]}
                          fieldKey={[fieldKey, "Product"]}
                        >
                          <SelectProduct
                            //value={personInCharge}
                            //setValue={setPersonInCharge}
                            disabled={type == "edit" ? true : false}
                            mode=""
                            placeholder={`Nhập tên sản phẩm`}
                          />
                        </Form.Item>
                      </Col>

                      <Col span={8} className="p-1">
                        <Form.Item
                          {...restField}
                          name={[name, "SellingPrice"]}
                          fieldKey={[fieldKey, "SellingPrice"]}
                          label="Giá bán"
                          rules={[
                            {
                              required: true,
                              message: "Nhập giá bán!",
                            },
                          ]}
                        >
                          <InputNumber
                            disabled={type == "edit" ? true : false}
                            min={0}
                            style={{ width: "100%", height: "64px" }}
                          />
                        </Form.Item>
                      </Col>
                      <Col span={8} className="p-1">
                        <Flex justify="space-between">
                          <Form.Item
                            {...restField}
                            name={[name, "Quantity"]}
                            fieldKey={[fieldKey, "Quantity"]}
                            label="Số lượng"
                            rules={[
                              {
                                required: true,
                                message: "Nhập số lượng!",
                              },
                            ]}
                          >
                            <InputNumber
                              disabled={type == "edit" ? true : false}
                              min={0}
                              style={{ width: "100%", height: "64px" }}
                            />
                          </Form.Item>
                          {type != "edit" ? (
                            <DeleteOutlined
                              style={{ color: "red" }}
                              onClick={() => remove(name)}
                            />
                          ) : (
                            <></>
                          )}
                        </Flex>
                      </Col>
                    </Row>
                  </div>
                ))}
                {type != "edit" ? (
                  <Form.Item>
                    <Button
                      type="dashed"
                      onClick={() => add()}
                      icon={<i className="anticon anticon-plus-circle" />}
                    >
                      Thêm sản phẩm
                    </Button>
                  </Form.Item>
                ) : (
                  <></>
                )}
              </>
            )}
          </Form.List>
        </Card>
        <div className="mt-4">
          <Flex justify={"end"}>
            <Button onClick={() => handleCancel()}>Hủy</Button>
            {action()}
          </Flex>
        </div>
      </Form>
    </Modal>
  );
};
export default OrderModal;
