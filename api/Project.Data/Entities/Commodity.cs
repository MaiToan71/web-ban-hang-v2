namespace Project.Data.Entities
{
    public class Commodity : BaseViewModel
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Quantity { get; set; }
        public float? Price { get; set; }
        public DateTime? DateInput { get; set; }
        public DateTime? DateOutput { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? ImageUrl { get; set; }
    }
}
