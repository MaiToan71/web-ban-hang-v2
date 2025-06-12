using Project.ViewModel.Usermanagers.Permissions;

namespace Project.ViewModel.Usermanagers.Users
{
    public class UserInfoViewModel
    {

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }
        public float Balance { get; set; } = 0;
        public string PhoneNumber { get; set; }
        public DateTime CreatedTime { get; set; }

        public List<PermissionViewModel> Permissions { get; set; } = new List<PermissionViewModel>();
    }
}
