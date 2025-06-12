using Project.Enums;

namespace Project.ViewModels.Notification
{
    public class NotificationUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime ScheduleDate { get; set; }
        public Workflow Workflow { get; set; }
        public bool IsSend { get; set; } = false;
        public List<Guid> Users { get; set; }
    }
}
