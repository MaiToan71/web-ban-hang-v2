import { Card, Input } from "antd";
import { useEffect, useLayoutEffect, useRef, useState } from "react";

import { StyledCalendar } from "./styles";
import moment from "moment";
import { useMutation } from "@tanstack/react-query";
import { SearchOutlined } from "@ant-design/icons";
import user from "@/api/management/user";
import { UserType, Workflow } from "#/enum";

import dayGridPlugin from "@fullcalendar/daygrid";
import interactionPlugin from "@fullcalendar/interaction";
import listPlugin from "@fullcalendar/list";
import FullCalendar from "@fullcalendar/react";
import resourceTimeGridPlugin from "@fullcalendar/resource-timegrid";
import resourceTimelinePlugin from "@fullcalendar/resource-timeline";
import timeGridPlugin from "@fullcalendar/timegrid";
import { useSettings } from "@/store/settingStore";
import workScheduleApi from "@/api/workSchedules";
import PopupAddEmployeeClickedShift from "./popupAddEmployeeClickedShift";

import { formatISO, parseISO } from "date-fns";
import CalendarHeader, {
  type HandleMoveArg,
  type ViewType,
} from "./calendar-header";
export default function WorkSchedule() {
  const fullCalendarRef = useRef<FullCalendar>(null);
  const [view, setView] = useState<ViewType>("resourceTimelineWeek");
  const [date, setDate] = useState(new Date());

  const [workDateId, setWorkDateId] = useState<any>(0);
  const [isOpenPopupEmployee, setIsOpenPopupEmployee] = useState(false);
  const [infoEmployeesClicked, setInfoEmployeesClicked] = useState(null);

  const [startDate, setStartDate] = useState<any>(moment().startOf("month"));
  const [endDate, setEndDate] = useState<any>(moment().endOf("month"));
  const [selectedDates, setSelectedDates] = useState<any>([]);
  const [events, setEvents] = useState<any>([]);
  const [filteredResources, setFilteredResources] = useState<any>();
  const [filterText, setFilterText] = useState<any>("");
  const searchUserMutation = useMutation({
    mutationFn: user.searchItem,
  });
  const getUser = async () => {
    let param: any = {
      PageIndex: 1,
      PageSize: 1000,
      UserType: UserType.CLIENT,
      Workflow: Workflow.COMPLETED,
      Search: filterText,
    };
    const res: any = await searchUserMutation.mutateAsync(param);

    const resource = res.Items.map((item: any) => ({
      id: item.UserId,
      title: item.FullName,
    }));

    setFilteredResources(
      resource?.filter((resource: any) =>
        resource?.title?.toLowerCase().includes(filterText?.toLowerCase())
      )
    );

    await fetchData();
  };
  const searchTimeshiftMutation = useMutation({
    mutationFn: workScheduleApi.searchItem,
  });
  const fetchData = async () => {
    let param: any = {
      PageIndex: 1,
      PageSize: 10000,
      FromWorkDateRegis: startDate,
      ToWorkDateRegis: endDate,
    };
    const res: any = await searchTimeshiftMutation.mutateAsync(param);
    const transformedEvents = res.Items.map((item: any) => ({
      resourceId: item.EmployeeId,
      title:
        item.WorkType === "Normal"
          ? ` ${formatISO(parseISO(item.FromWorkDateEffect), {
              representation: "time",
            }).slice(0, 5)} - ${formatISO(parseISO(item.ToWorkDateEffect), {
              representation: "time",
            }).slice(0, 5)}`
          : item.WorkType,
      time: `${formatISO(parseISO(item.FromWorkDateEffect), {
        representation: "time",
      }).slice(0, 5)} - ${formatISO(parseISO(item.ToWorkDateEffect), {
        representation: "time",
      }).slice(0, 5)}`,
      start: item.FromWorkDateEffect,
      end: item.ToWorkDateEffect,
      className: "event-task",
      type: "task",
      borderColor: "#cee6da",
      backgroundColor: "#cee6da",
      textColor: "green",
      idEvent: item.Id,
    }));
    setEvents(transformedEvents);
  };
  useEffect(() => {
    if (filterText && filterText) {
      getUser();
    }
  }, [filterText]);
  useEffect(() => {
    getUser();
  }, []);
  useEffect(() => {
    fetchData();
  }, [startDate]);
  useLayoutEffect(() => {
    const calendarApi = fullCalendarRef.current?.getApi();
    if (!calendarApi) return;
    setTimeout(() => {
      calendarApi.changeView(view);
    });
  }, [view]);
  const { themeMode } = useSettings();
  const handleMove = (action: HandleMoveArg) => {
    const calendarApi = fullCalendarRef.current?.getApi();
    if (!calendarApi) return;
    switch (action) {
      case "prev":
        calendarApi.prev();
        break;
      case "next":
        calendarApi.next();
        break;
      case "today":
        calendarApi.today();
        break;
      default:
        break;
    }
    setDate(calendarApi.getDate());
  };
  const handleViewTypeChange = (view: ViewType) => {
    setView(view);
  };
  const handleDateOrRangeSelection = (arg: any) => {
    let selectedDays = [];

    if (arg.start && arg.end) {
      let currentDate = new Date(arg.start);
      while (currentDate <= arg.end) {
        selectedDays.push(new Date(currentDate));
        currentDate.setDate(currentDate.getDate() + 1);
      }
    } else {
      const date = arg.date;
      selectedDays.push(
        new Date(date.getFullYear(), date.getMonth(), date.getDate())
      );
    }
    setWorkDateId(0);
    setSelectedDates(selectedDays);
    setIsOpenPopupEmployee(true);
    console.log(arg);
    setInfoEmployeesClicked(arg);
  };

  const handleEventClick = (arg: any) => {
    let selectedDays = [];
    if (arg.event._instance.range.start && arg.event._instance.range.end) {
      let currentDate = new Date(arg.event._instance.range.start);
      while (currentDate <= arg.event._instance.range.end) {
        selectedDays.push(new Date(currentDate));
        currentDate.setDate(currentDate.getDate() + 1);
      }
    } else {
      const date = arg.event._instance.range.event.start;
      selectedDays.push(
        new Date(date.getFullYear(), date.getMonth(), date.getDate())
      );
    }
    setWorkDateId(arg.event._def.extendedProps.idEvent);
    setSelectedDates(selectedDays);
    setIsOpenPopupEmployee(true);
    setInfoEmployeesClicked(arg.event);
  };
  return (
    <Card>
      <Input
        prefix={<SearchOutlined />}
        placeholder="Tìm kiếm nhân sự"
        onChange={(e) => setFilterText(e.target.value)}
      />
      <StyledCalendar $themeMode={themeMode}>
        <CalendarHeader
          now={date}
          onMove={handleMove}
          view={view}
          onViewTypeChange={handleViewTypeChange}
        />
        <FullCalendar
          plugins={[
            dayGridPlugin,
            timeGridPlugin,
            interactionPlugin,
            listPlugin,
            resourceTimelinePlugin,
            resourceTimeGridPlugin,
          ]}
          ref={fullCalendarRef}
          locale="vi"
          initialView="resourceTimelineWeek"
          slotLabelInterval={{ days: 1 }}
          slotDuration={"24:00:00"} // Khoảng thời gian 1 ngày cho mỗi slot
          headerToolbar={false}
          //  themeSystem="bootstrap"
          buttonText={{
            // resourceTimelineMonth: "Tháng",
            today: "Hôm nay",
            resourceTimelineWeek: "Tuần",
            resourceTimelineDay: "Ngày",
          }}
          datesSet={(arg) => {
            setStartDate(`${moment(arg.start).format(`YYYY-MM-DD`)}T00:00:00`); //starting visible date
            setEndDate(`${moment(arg.end).format(`YYYY-MM-DD`)}T23:59:59`); //ending visible date
          }}
          height={"auto"}
          events={events}
          resources={filteredResources}
          resourceAreaWidth={200}
          stickyHeaderDates={true}
          resourceAreaHeaderContent={"Nhân viên"}
          editable={true}
          droppable={true}
          selectable={true}
          select={handleDateOrRangeSelection}
          dateClick={handleDateOrRangeSelection}
          eventClick={handleEventClick}
          schedulerLicenseKey="CC-Attribution-NonCommercial-NoDerivatives"
          // drop={onDrop}
        />
      </StyledCalendar>

      {isOpenPopupEmployee && (
        <PopupAddEmployeeClickedShift
          workDateId={workDateId}
          isOpen={isOpenPopupEmployee}
          infoEmployeesClicked={infoEmployeesClicked}
          selectedDates={selectedDates}
          refetchData={fetchData}
          toggle={() => setIsOpenPopupEmployee(false)}
        />
      )}
    </Card>
  );
}
