using Project.Data.Entities;
using Project.Enums;
using Project.ViewModel.Usermanagers.Permissions;
using Project.ViewModel.Usermanagers.Roles;

namespace Project.ViewModel.Usermanagers.Users
{
    public class UserViewModel
    {

        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? FullName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? File { get; set; }
        public int CustomerCount { get; set; } = 0;
        public float Balance { get; set; } = 0;
        public bool IsActive { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public float Amount { get; set; } = 0;
        public Workflow Workflow { get; set; } = Workflow.CREATED;
        public UserType UserType { get; set; } = UserType.Employee;

        public List<PermissionViewModel> Permissions = new List<PermissionViewModel>();
        public List<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();
        public List<Department> Departments { get; set; } = new List<Department>();
    }
}
