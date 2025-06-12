using Project.Enums;

namespace Project.Data.Entities
{
    public class Notification : BaseViewModel
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime ScheduleDate { get; set; }
        public Workflow Workflow { get; set; }
        public bool IsSend { get; set; } = false;
        public List<NotificationUser> NotificationUsers { get; set; }
    }
}
