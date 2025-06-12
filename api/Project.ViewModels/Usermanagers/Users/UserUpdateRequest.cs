using Project.Enums;

namespace Project.ViewModel.Usermanagers.Users
{
    public class UserUpdateRequest
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }

        public UserType UserType { get; set; }
        public Workflow Workflow { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public List<int>? RoleIds { get; set; }
        public List<int>? DepartmentIds { get; set; }
        public string? IdToken { get; set; }
    }
    public class UserUpdateworkdlowRequest
    {
        public Guid UserId { get; set; }
        public Workflow Workflow { get; set; }
    }
}
