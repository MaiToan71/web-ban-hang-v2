namespace Project.ViewModels.Orders
{
    public class OrderDetailRequest
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public float CapitalPrice { get; set; } = 0;//giá vốn
        public float SellingPrice { get; set; } // giá bán
        public decimal Quantity { get; set; } = 0;

        public float Discount { get; set; } = 0;
        public float Fee { get; set; } = 0;
    }
}
