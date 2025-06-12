using Project.ViewModels;
using Project.ViewModels.Notification;


namespace Project.Services.Notification
{
    public interface INotificationService
    {
        Task<PagedResult<NotificationViewModel>> GetAllPaging(GetNotificationPagingRequest request);
        Task<bool> AddNew(NotificationCreateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Update(NotificationUpdateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser);
    }
}
