using Project.Enums;

namespace Project.ViewModels.Attributes
{
    public class ProductAttributeRequest
    {
        public int AttributeId { get; set; }
        public decimal Price { get; set; } = 0;
        public int Quantity { get; set; }
        public string? Description { get; set; }
    }

    public class ProductAttributeViewModel
    {
        public int AttributeId { get; set; }
        public decimal Price { get; set; } = 0;
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public AttributeEnum Type { get; set; }
    }
}
