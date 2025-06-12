import { useState, useEffect } from "react";
import {
  Card,
  Flex,
  Progress,
  Row,
  Col,
  Tag,
  Popconfirm,
  DatePicker,
  Button,
} from "antd";
import { toast } from "sonner";
import dayjs from "dayjs";
import timeshift from "@/api/projects/timeshift";
import { SearchOutlined } from "@ant-design/icons";
import ChartLine from "./chart/chart-line";
import { Table } from "antd";
import { IconButton, Iconify } from "@/components/icon";
import moment from "moment";
import user from "@/api/management/user";
import { useMutation } from "@tanstack/react-query";
import Paging from "@/components/paging";
import helper from "@/api/helper";
const { RangePicker } = DatePicker;
import CalendarEventForm from "../../sys/others/calendar/calendar-event-form";
const DefaultEventInitValue = {
  id: 0,
  title: "",
  description: "",
  allDay: false,
  start: dayjs(),
  end: dayjs(),
  color: "",
};
const Individual = () => {
  const [eventInitValue, setEventInitValue] = useState<any>(
    DefaultEventInitValue
  );
  const [eventFormType, setEventFormType] = useState<any>("add");
  const [open, setOpen] = useState(false);
  const dateFormat = "DD/MM/YYYY";
  const customFormat = (value: any) => `${value.format(dateFormat)}`;
  const [total, setTotal] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = Number(import.meta.env.VITE_REACT_APP_API_SIZE);
  const [fromDate, setFromDate] = useState<any>(null);
  const [toDate, setToDate] = useState<any>(null);
  const chartlines: any = [];
  const handleCancel = () => {
    setEventInitValue(DefaultEventInitValue);
    setOpen(false);
  };
  useEffect(() => {
    getStatisticalJob();
    getTimeShiftTypes();
    getTimeShiftTags(1);
  }, []);
  const handlePagination = async (page: any) => {
    setCurrentPage(page);
    getTimeShiftTags(page);
  };
  const handleChange = (_: any, formattedDates: any) => {
    if (formattedDates.length == 2) {
      let start = formattedDates[0];
      if (start != "") {
        setFromDate(
          `${start.split("/")[2]}-${start.split("/")[1]}-${
            start.split("/")[0]
          }T00:00:00`
        );
      } else {
        setFromDate(null);
      }

      let end = formattedDates[1];
      if (end != "") {
        setToDate(
          `${end.split("/")[2]}-${end.split("/")[1]}-${
            end.split("/")[0]
          }T23:59:59`
        );
      } else {
        setToDate(null);
      }
    }
    console.log(formattedDates);
  };

  const onSearch = () => {
    setCurrentPage(1);
    getStatisticalJob();
    getTimeShiftTypes();
    getTimeShiftTags(1);
  };
  const [statisticalJob, setSatisticalJob] = useState<any>({
    Total: 0,
    TotalDone: 0,
    TotalFail: 0,
    TotalProcessing: 0,
    TotalOverdue: 0,
  });
  const querySatisticalJob = useMutation({
    mutationFn: user.statisticalJob,
  });
  const getStatisticalJob = async () => {
    let param: any = {
      FromDate: fromDate,
      ToDate: toDate,
    };
    const res: any = await querySatisticalJob.mutateAsync(param);
    if (res.Status) {
      console.log(res);
      setSatisticalJob(res.Data);
    }
  };

  const [timeShiftTypes, setTimeShiftTypes] = useState<any>([]);
  const queryTimeShiftType = useMutation({
    mutationFn: user.timeShiftType,
  });
  const getTimeShiftTypes = async () => {
    let param: any = {
      FromDate: fromDate,
      ToDate: toDate,
    };
    const res: any = await queryTimeShiftType.mutateAsync(param);
    if (res.Status) {
      setTimeShiftTypes(res.Data);
    }
  };

  const [timeShiftTags, setTimeShiftTags] = useState<any>([]);
  const queryTimeShiftTags = useMutation({
    mutationFn: user.timeShiftTags,
  });
  const getTimeShiftTags = async (page: any) => {
    let param: any = {
      PageIndex: page,
      PageSize: pageSize,
      FromDate: fromDate,
      ToDate: toDate,
    };
    const res: any = await queryTimeShiftTags.mutateAsync(param);
    if (res.Status) {
      console.log(res);
      setTotal(res.TotalRecord);
      setTimeShiftTags(
        res.Items.map((i: any) => {
          return {
            privateKey: i.TimeShift.Id,
            key: i.TimeShift.Id,
            ...i,
          };
        })
      );
    }
  };

  const [selectedRowKeys, setSelectedRowKeys] = useState([]);
  const onSelectChange = (newSelectedRowKeys: any) => {
    setSelectedRowKeys(newSelectedRowKeys);
  };
  const rowSelection = {
    selectedRowKeys,
    onChange: onSelectChange,
    selections: [
      {
        key: "odd",
        text: "Chưa hoàn thành",
        onSelect: (changeableRowKeys: any) => {
          let newSelectedRowKeys = [];
          newSelectedRowKeys = changeableRowKeys.filter(
            (_: any, index: any) => {
              if (index % 2 !== 0) {
                return false;
              }
              return true;
            }
          );
          setSelectedRowKeys(newSelectedRowKeys);
        },
      },
      {
        key: "even",
        text: "Đã hoàn thành",
        onSelect: (changeableRowKeys: any) => {
          let newSelectedRowKeys = [];
          newSelectedRowKeys = changeableRowKeys.filter((index: any) => {
            if (index % 2 !== 0) {
              return true;
            }
            return false;
          });
          setSelectedRowKeys(newSelectedRowKeys);
        },
      },
    ],
  };

  const workUpdate = useMutation({
    mutationFn: timeshift.updateWorkflow,
  });

  const handleWorkUpdate = async (record: any, workflow: any) => {
    let input = {
      Workflow: Number(workflow),
      Id: record.TimeShift.Id,
    };
    const res: any = await workUpdate.mutateAsync(input);
    if (res.Status) {
      toast.success("Thành công", {
        position: "top-right",
      });
    } else {
      toast.error("Thất bại", {
        position: "top-right",
      });
    }
    getTimeShiftTags(currentPage);
  };
  // click event and open modal
  const handleEventClick = (item: any) => {
    setOpen(true);
    setEventFormType("edit");
    const newEventValue: any = {
      id: item.Id,
      title: item.Name,
      allDay: true,
      color: "",
      description: "",
    };

    setEventInitValue(newEventValue);
  };

  const onCreate = () => {
    setOpen(true);
    setEventFormType("add");
    const newEventValue: any = {
      id: 0,
      title: "",
      allDay: true,
      color: "",
      description: "",
    };

    setEventInitValue(newEventValue);
  };
  const columns: any = [
    {
      title: "Bắt đầu ",
      render: (_: any, record: any) => (
        <p>{moment(record.TimeShift.StartTime).format("DD/MM/YYYY")}</p>
      ),
    },
    {
      title: "Kết thúc",
      render: (_: any, record: any) => (
        <p>{moment(record.TimeShift.EndTime).format("DD/MM/YYYY")}</p>
      ),
    },
    {
      title: "Công việc ",
      render: (_: any, record: any) => <p>{record.TimeShift.Name}</p>,
    },
    {
      title: "Dự án",
      render: (_: any, record: any) => (
        <Tag color="green">
          <p>{record.TimeShiftType.Name}</p>
        </Tag>
      ),
    },
    {
      title: "Phụ trách",
      render: (_: any, record: any) => <p>{record.User.FullName}</p>,
    },
    {
      title: "Trạng thái",
      render: (_: any, record: any) =>
        record.TimeShift.Workflow == 4 ? (
          <Tag color={`green`}>Hoàn thành</Tag>
        ) : (
          <Tag color={`red`}>Chưa hoàn thành</Tag>
        ),
    },
    {
      title: "Thao tác",
      key: "operation",
      align: "center",
      fixed: "right",
      width: 100,
      render: (_: any, record: any) => (
        <div className="flex w-full justify-end text-gray">
          <IconButton
            onClick={() => {
              handleEventClick({
                ...record.TimeShift,
              });
            }}
          >
            <Iconify icon="hugeicons:view" size={18} />
          </IconButton>
          {record.TimeShift.Workflow == 4 ? (
            <Popconfirm
              title="Hủy bỏ"
              okText="Đồng ý"
              cancelText="Hủy"
              placement="left"
              onConfirm={async () => {
                handleWorkUpdate(record, -1);
              }}
            >
              <IconButton>
                <Iconify
                  icon="icon-park-outline:doc-fail"
                  size={18}
                  className="text-[red]"
                />
              </IconButton>
            </Popconfirm>
          ) : (
            <Popconfirm
              title="Hoàn thành"
              okText="Đồng ý"
              cancelText="Hủy"
              placement="left"
              onConfirm={async () => {
                handleWorkUpdate(record, 4);
              }}
            >
              <IconButton>
                <Iconify
                  icon="pajamas:task-done"
                  size={18}
                  className="text-[green]"
                />
              </IconButton>
            </Popconfirm>
          )}
        </div>
      ),
    },
  ];
  return (
    <div>
      <CalendarEventForm
        component={"VIEW"}
        open={open}
        type={eventFormType}
        initValues={eventInitValue}
        onCancel={handleCancel}
        onCallBack={() => getTimeShiftTags(currentPage)}
      />
      <div className="ml-2">
        <Flex>
          <div className="mr-2">
            <RangePicker format={customFormat} onChange={handleChange} />
          </div>
          <Button type="primary" icon={<SearchOutlined />} onClick={onSearch}>
            Tìm kiếm
          </Button>
        </Flex>
      </div>
      <Row>
        <Col span={8} className="p-2">
          <Card>
            <p className="mt-0 mb-4">
              <span
                style={{
                  fontSize: "28px",
                  color: "#00b8d9",
                  fontWeight: "bold",
                }}
              >
                {statisticalJob?.Total}
              </span>{" "}
              <span>Công việc</span>
            </p>
            <div>
              <div>
                <Progress
                  percent={Number(
                    (
                      Number(statisticalJob?.TotalDone) /
                      Number(statisticalJob?.Total)
                    ).toFixed(2)
                  )}
                  status="active"
                  percentPosition={{
                    align: "center",
                    type: "inner",
                  }}
                  size={[`100%`, 20]}
                />
                <Flex>
                  <p className="mt-3 mb-3 mr-3">
                    <span
                      style={{
                        color: "#00b8d9",
                        fontWeight: "bold",
                        paddingRight: "4px",
                      }}
                    >
                      {statisticalJob?.TotalProcessing}
                    </span>
                    đang làm
                  </p>
                  <p className="mt-3 mb-3">
                    <span
                      style={{
                        color: "#00a76f",
                        fontWeight: "bold",
                        paddingRight: "4px",
                      }}
                    >
                      {statisticalJob?.TotalDone}
                    </span>
                    Hoàn thành
                  </p>
                </Flex>
              </div>
              <div>
                <hr className="mt-3 mb-3" />
                <Flex>
                  <p className="mt-2 mb-2 mr-3">
                    <span
                      style={{
                        color: "red",
                        fontWeight: "bold",
                        paddingRight: "4px",
                        fontSize: "18px",
                      }}
                    >
                      {statisticalJob?.TotalOverdue}
                    </span>
                    quá hạn
                  </p>
                  <p className="mt-2 mb-2">
                    <span
                      style={{
                        color: "red",
                        fontSize: "18px",
                        fontWeight: "bold",
                        paddingRight: "4px",
                      }}
                    >
                      0
                    </span>
                    khẩn cấp
                  </p>
                </Flex>
              </div>
            </div>
          </Card>
        </Col>
        <Col span={8} className="p-2">
          <Card>
            <h3
              style={{
                color: "#00a76f",
                fontSize: "18px",
                paddingRight: "4px",
              }}
            >
              Số lượng công việc hằng ngày
            </h3>
            <div>
              <ChartLine data={chartlines && chartlines} />
            </div>
          </Card>
        </Col>
        <Col span={8} className="p-2">
          <Card style={{ height: "247px" }}>
            <h3
              style={{
                color: "#00a76f",
                fontSize: "18px",
                paddingRight: "4px",
              }}
            >
              Phân chia theo dự án
            </h3>
            <div className="mt-2">
              {timeShiftTypes.map((i: any) => {
                return (
                  <div className="mt-2">
                    <Flex justify="space-between">
                      <div>
                        <strong>{i.Name}</strong>
                      </div>
                      <div>
                        <strong>{i.TotalTimeShift}</strong>{" "}
                        <span style={{ opacity: "0.5" }}>công việc</span>
                      </div>
                    </Flex>
                  </div>
                );
              })}
            </div>
          </Card>
        </Col>
        <Col span={24} className="p-2">
          <Card
            extra={
              <Button
                className="ml-2"
                type="primary"
                onClick={() => onCreate()}
              >
                <div className=" flex items-center justify-center">
                  <Iconify icon="material-symbols:add" size={24} />
                  Tạo mới
                </div>
              </Button>
            }
            title={
              <Flex>
                <div>
                  <p>Công việc của tôi</p>
                  <Flex style={{ marginTop: "4px" }}>
                    <Flex style={{ fontSize: "13px", fontWeight: "none" }}>
                      <Progress
                        type="circle"
                        percent={100}
                        format={() => "Done"}
                        size={10}
                      />
                      <p
                        style={{
                          marginLeft: "4px",
                          marginTop: "-2px",
                          color: "#00a76f",
                        }}
                      >
                        Đã hoàn thành
                      </p>
                    </Flex>
                    <Flex
                      style={{
                        fontSize: "13px",
                        fontWeight: "none",
                        marginLeft: "8px",
                      }}
                    >
                      <Progress
                        type="circle"
                        percent={100}
                        format={() => "Done"}
                        size={10}
                        strokeColor="red"
                      />
                      <p
                        style={{
                          marginLeft: "4px",
                          marginTop: "-2px",
                          color: "red",
                        }}
                      >
                        {" "}
                        Chưa hoàn thành
                      </p>
                    </Flex>
                  </Flex>
                </div>
              </Flex>
            }
          >
            <div>
              <Table
                rowKey="id"
                scroll={{ x: "max-content" }}
                pagination={false}
                rowSelection={rowSelection}
                columns={columns}
                dataSource={timeShiftTags || []}
              />
              <Flex justify={`space-between`} align={`center`}>
                <strong className="pt-2">
                  Tổng{" "}
                  <span style={{ color: "red" }}>
                    {" "}
                    {helper.currencyFormat(total)}
                  </span>{" "}
                  bản ghi
                </strong>
                <Paging
                  total={total}
                  currentPage={currentPage}
                  size={pageSize}
                  handlePagination={handlePagination}
                />
              </Flex>
            </div>
          </Card>{" "}
        </Col>
      </Row>
    </div>
  );
};

export default Individual;
