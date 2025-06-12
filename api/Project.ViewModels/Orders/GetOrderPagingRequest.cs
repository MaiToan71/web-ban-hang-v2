using Project.Enums;

namespace Project.ViewModels.Orders
{
    public class GetOrderPagingRequest : PagingRequestBase
    {
        public Guid? UserId { get; set; }
        public OrderType? OrderType { get; set; }

        public DateTime? FromDateOrder { get; set; }
        public int? ProductId { get; set; }
        public DateTime? ToDateOrder { get; set; }
    }
}
