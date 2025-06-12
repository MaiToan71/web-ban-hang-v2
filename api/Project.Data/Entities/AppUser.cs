
using Microsoft.AspNetCore.Identity;
using Project.Enums;

namespace Project.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {

        public float Balance { get; set; } = 0;
        public string FullName { get; set; } = "";


        public string TelegramUrl { get; set; } = "";
        public string FacebookUrl { get; set; } = "";

        public string Salt { get; set; } = "";

        public string Address { get; set; } = "";

        public bool IsCustomer { get; set; } = false;

        public bool IsGoogle = false;
        public string IdGoogleToken { get; set; } = string.Empty;
        public string GoogleId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
        public DateTime CreatedTime { get; set; }
        public string CreatedUser { get; set; } = "";
        public DateTime ModifiedTime { get; set; }
        public string ModifiedUser { get; set; } = "";
        public string? FileAccount { get; set; }
        public string? LinkImage { get; set; }
        public float Amount { get; set; } = 0;
        public Workflow Workflow { get; set; } = Workflow.CREATED;
        public UserType UserType { get; set; } = UserType.Employee;
        public bool IsDeleted { get; set; } = false;
        public int DepartmentId { get; set; } = 0;
        public List<AppUsermangersRole> AppUsermangersRoles { get; set; }



        public List<Post> Posts { get; set; }

        public List<Product> Products { get; set; }

        public List<NotificationUser> NotificationUsers { get; set; }
    }
}
