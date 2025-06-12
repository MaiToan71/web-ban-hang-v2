using Project.Data.Entities;

namespace Project.ViewModels.Orders
{
    public class OrderDetailDTOViewModel
    {
        public OrderDetail OrderDetail { get; set; } = new OrderDetail();
        public Order Order { get; set; } = new Order();
    }
    public class OrderDTOViewModel
    {
        public Order Order { get; set; } = new Order();
        public UserDetailDTOViewModel User { get; set; } = new UserDetailDTOViewModel();

        public List<ProductDetailDTOViewModel> OrderDetails { get; set; } = new List<ProductDetailDTOViewModel>();
    }
    public class UserDetailDTOViewModel
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string FullName { get; set; }
    }
    public class ProductDetailDTOViewModel
    {
        public OrderDetail OrderDetail { get; set; } = new OrderDetail();
        public ProductDTOViewModel Product { get; set; } = new ProductDTOViewModel();
    }
    public class ProductDTOViewModel
    {
        public string Title { get; set; }
        public int Id { get; set; }
    }
}
