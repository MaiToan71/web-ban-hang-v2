namespace Project.Data.Entities
{
    public class OrderDetail : BaseViewModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; } // mã sản phẩm

        public decimal CapitalPrice { get; set; } //giá vốn
        public decimal SellingPrice { get; set; } // giá bán
        public decimal Quantity { get; set; } = 0;

        public decimal Discount { get; set; } = 0;
        public decimal Fee { get; set; } = 0;

        //   public Product ProductCode { get; set; } = new Product();
    }
}
