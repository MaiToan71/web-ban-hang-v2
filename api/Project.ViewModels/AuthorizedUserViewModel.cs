using Project.ViewModel.Usermanagers.Permissions;

namespace Project.ViewModels
{
    public class AuthenticatedUserInfoViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public float Amount { get; set; }
        public string Company { get; set; }
        public string AboutUs { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public List<PermissionViewModel> Permissions { get; set; } = new List<PermissionViewModel>();

        public DateTime CreatedTime { get; set; }

    }
    public class AuthorizedUserViewModel : AuthenticatedUserInfoViewModel
    {
        public AuthorizedUserViewModel(AuthenticatedUserInfoViewModel authenticatedUser)
        {
            UserId = authenticatedUser.UserId;
            UserName = authenticatedUser.UserName;
            FullName = authenticatedUser.FullName;
            Email = authenticatedUser.Email;
        }

        public AuthorizedUserViewModel() { }


    }
}
