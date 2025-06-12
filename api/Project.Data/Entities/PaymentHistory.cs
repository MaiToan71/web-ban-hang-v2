using Project.Enums;

namespace Project.Data.Entities
{
    public class PaymentHistory : BaseViewModel
    {
        public Guid UserId { get; set; }
        public float Amount { get; set; }
        public Workflow Workflow { get; set; }

    }
}
