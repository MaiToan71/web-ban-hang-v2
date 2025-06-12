using Project.Enums;

namespace Project.Data.Entities
{
    public class BankUser : BaseViewModel
    {
        public double Amount { get; set; }
        public int BankId { get; set; }
        public Guid UserId { get; set; }
        public Workflow Workflow { get; set; } = Workflow.WAITING;
    }
}
