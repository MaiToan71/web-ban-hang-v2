using Project.Enums;

namespace Project.ViewModel.Usermanagers.Users
{
    public class UserCreateRequest
    {
        public string FullName { get; set; }
        public UserType UserType { get; set; }
        public Workflow Workflow { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool? IsCustomer { get; set; }
        public List<int>? RoleIds { get; set; }
        public string? IdToken { get; set; }
        public string? GoogleId { get; set; }
    }
}
