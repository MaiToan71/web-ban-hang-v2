using Project.Enums;

namespace Project.ViewModels.BankUsers
{
    public class BankUserCreateRequest
    {
        public double Amount { get; set; }
        public int BankId { get; set; }
        public Guid UserId { get; set; }
        public Workflow Workflow { get; set; }
    }
}
