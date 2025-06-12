using Project.ViewModel.Usermanagers.Users;

namespace Project.ViewModels.Notification
{
    public class NotificationViewModel
    {
        public Project.Data.Entities.Notification Notification { get; set; } = new Project.Data.Entities.Notification();
        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    }
}
