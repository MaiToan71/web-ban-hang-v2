namespace Project.Data.Entities
{
    public class ProductImportReference : BaseViewModel
    {
        public int ProductImportId { get; set; }
        public int ProductId { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
    }
}
