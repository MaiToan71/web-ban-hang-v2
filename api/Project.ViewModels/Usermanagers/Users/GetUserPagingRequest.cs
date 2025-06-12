
using Project.Enums;
using Project.ViewModels;

namespace Project.ViewModel.Usermanagers.Users
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string? Username { get; set; }
        public bool? IsCustomer { get; set; }
        public UserType? UserType { get; set; }
        public Workflow? Workflow { get; set; }
        public string? Search { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public Guid? UserId { get; set; }

        public string? Name { get; set; }
        public int? TimeShiftTypeId { get; set; }
    }
}
