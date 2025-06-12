import Card from "@/components/card";
import { down, useMediaQuery } from "@/hooks";
import { useSettings } from "@/store/settingStore";

import type { EventClickArg } from "@fullcalendar/core";
import dayGridPlugin from "@fullcalendar/daygrid";
import interactionPlugin from "@fullcalendar/interaction";
import listPlugin from "@fullcalendar/list";
import FullCalendar from "@fullcalendar/react";
import timeGridPlugin from "@fullcalendar/timegrid";
import dayjs from "dayjs";
import { useMutation } from "@tanstack/react-query";

import { useEffect, useLayoutEffect, useRef, useState } from "react";
import moment from "moment";
import CalendarEvent from "./calendar-event";
import CalendarEventForm from "./calendar-event-form";
import CalendarHeader, {
  type HandleMoveArg,
  type ViewType,
} from "./calendar-header";

import { StyledCalendar } from "./styles";
import timeshift from "@/api/projects/timeshift";
const DefaultEventInitValue = {
  id: 0,
  title: "",
  description: "",
  allDay: false,
  start: dayjs(),
  end: dayjs(),
  color: "",
};

export default function Calendar() {
  const fullCalendarRef = useRef<FullCalendar>(null);
  const [view, setView] = useState<ViewType>("dayGridMonth");
  const [date, setDate] = useState(new Date());
  const [open, setOpen] = useState(false);
  const [eventInitValue, setEventInitValue] = useState<any>(
    DefaultEventInitValue
  );
  const [eventFormType, setEventFormType] = useState<"add" | "edit">("add");

  const { themeMode } = useSettings();
  const xsBreakPoint = useMediaQuery(down("xs"));

  useEffect(() => {
    if (xsBreakPoint) {
      setView("listWeek");
    }
  }, [xsBreakPoint]);

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

  useLayoutEffect(() => {
    const calendarApi = fullCalendarRef.current?.getApi();
    if (!calendarApi) return;
    setTimeout(() => {
      calendarApi.changeView(view);
    });
  }, [view]);

  /**
   * calendar event events
   */
  // click event and open modal
  const handleEventClick = (arg: EventClickArg) => {
    const { title, extendedProps, allDay, start, end, backgroundColor, id } =
      arg.event;
    setOpen(true);
    setEventFormType("edit");
    const newEventValue: any = {
      id,
      title,
      allDay,
      color: backgroundColor,
      description: extendedProps.description,
    };
    if (start) {
      newEventValue.start = dayjs(start);
    }

    if (end) {
      newEventValue.end = dayjs(end);
    }
    setEventInitValue(newEventValue);
  };
  const handleCancel = () => {
    setEventInitValue(DefaultEventInitValue);
    setOpen(false);
  };
  // edit event

  //events
  const [startDate, setStartDate] = useState<any>(moment().startOf("month"));
  const [endDate, setEndDate] = useState<any>(moment().endOf("month"));
  const [events, setEvents] = useState<any>([]);
  const searchUserMutation = useMutation({
    mutationFn: timeshift.searchItem,
  });
  const getTimeshifts = async () => {
    let param: any = {
      PageIndex: 1,
      PageSize: 10000,
      FromDate: startDate,
      ToDate: endDate,
    };
    const res: any = await searchUserMutation.mutateAsync(param);
    if (res.Status) {
      setEvents(
        res.Items.map((i: any) => {
          return {
            id: i.Id,
            title: i.Name,
            allDay: true,
            start: dayjs(i.StartTime).toISOString(),
            end: dayjs(i.EndTime).toISOString(),
            color: i.Color,
          };
        })
      );
    }
  };
  useEffect(() => {
    getTimeshifts();
  }, [startDate]);

  return (
    <Card className="h-full w-full">
      <div className="h-full w-full">
        <StyledCalendar $themeMode={themeMode}>
          <CalendarHeader
            now={date}
            onMove={handleMove}
            view={view}
            onCreate={() => {
              setEventFormType("add");
              setOpen(true);
            }}
            onViewTypeChange={handleViewTypeChange}
          />
          <FullCalendar
            locale="vi"
            ref={fullCalendarRef}
            plugins={[
              dayGridPlugin,
              timeGridPlugin,
              interactionPlugin,
              listPlugin,
            ]}
            datesSet={(arg) => {
              setStartDate(
                `${moment(arg.start).format(`YYYY-MM-DD`)}T00:00:00`
              ); //starting visible date
              setEndDate(`${moment(arg.end).format(`YYYY-MM-DD`)}T23:59:59`); //ending visible date
            }}
            initialDate={date}
            initialView={xsBreakPoint ? "listWeek" : view}
            events={events}
            eventContent={CalendarEvent}
            editable
            selectable
            selectMirror
            dayMaxEvents
            headerToolbar={false}
            select={() => {
              console.log("ok");
            }}
            eventClick={handleEventClick}
          />
        </StyledCalendar>
      </div>
      <CalendarEventForm
        component={"EDIT"}
        open={open}
        type={eventFormType}
        initValues={eventInitValue}
        onCancel={handleCancel}
        onCallBack={() => getTimeshifts()}
      />
    </Card>
  );
}
