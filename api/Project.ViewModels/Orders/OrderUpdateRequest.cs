using Project.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels.Orders
{
    public class OrderUpdateRequest
    {
        [Required]
        public int Id { get; set; }
        public string? Code { get; set; }
        public DateTime? DateOrder { get; set; }
        public Guid UserId { get; set; }
        public DateTime? DateDelivery { get; set; }
        public string UnitDelivery { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? Note { get; set; }
        public Workflow Workflow { get; set; }
        public int PostTypeId { get; set; }
        public string DeloveryMan { get; set; }
        public string DeloveryManPhonenumber { get; set; }
        public List<OrderDetailRequest> OrderDetails { get; set; }
    }
}
