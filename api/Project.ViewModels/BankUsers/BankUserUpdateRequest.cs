using Project.Enums;

namespace Project.ViewModels.BankUsers
{
    public class BankUserUpdateRequest
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public int BankId { get; set; }
        public int UserId { get; set; }
        public Workflow Workflow { get; set; }
    }

    public class BankUserUpdateWorkflowRequest
    {
        public int Id { get; set; }

        public Workflow Workflow { get; set; }
    }
}
