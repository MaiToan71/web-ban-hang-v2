namespace Project.Data.Entities
{
    public class ProductImage
    {
        public int ProductId { get; set; }

        public Product Products { get; set; }

        public int ImageId { get; set; }
        public Image Images { get; set; }
    }
}
