using Project.Enums;

namespace Project.Data.Entities
{
    public class Order : BaseViewModel
    {
        public string? Code { get; set; }
        public DateTime? DateOrder { get; set; }
        public Guid UserId { get; set; } = new Guid();
        public DateTime? DateDelivery { get; set; }
        public string UnitDelivery { get; set; } // đơn vị
        public PaymentMethod PaymentMethod { get; set; }
        public OrderType OrderType { get; set; }
        public Workflow Workflow { get; set; }
        public int PostTypeId { get; set; }
        public string DeloveryMan { get; set; } = string.Empty;
        public string DeloveryManPhonenumber { get; set; } = string.Empty;
        public string? Note { get; set; }
        public string? ProvinceCode { get; set; } = string.Empty;
        public string? Province { get; set; } = string.Empty;
        public string? DistrictCode { get; set; } = string.Empty;
        public string? District { get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
    }
}
