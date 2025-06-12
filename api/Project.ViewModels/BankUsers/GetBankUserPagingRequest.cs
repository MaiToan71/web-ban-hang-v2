using Project.Enums;

namespace Project.ViewModels.BankUsers
{
    public class GetBankUserPagingRequest : PagingRequestBase
    {
        public Workflow? Workflow { get; set; }
        public Guid? UserId { get; set; }
    }
}
