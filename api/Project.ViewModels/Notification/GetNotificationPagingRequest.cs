using Project.Enums;

namespace Project.ViewModels.Notification
{
    public class GetNotificationPagingRequest : PagingRequestBase
    {
        public string? Search { get; set; }
        public Workflow? Workflow { get; set; }

        public Guid? UserId { get; set; }
    }
}
