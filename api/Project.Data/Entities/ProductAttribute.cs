namespace Project.Data.Entities
{
    public class ProductAttribute : BaseViewModel
    {
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
        public decimal Price { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
    }
}
