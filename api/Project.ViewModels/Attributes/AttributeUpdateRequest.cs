using Project.Enums;

namespace Project.ViewModels.Attributes
{
    public class AttributeUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public AttributeEnum Type { get; set; }
    }
}
