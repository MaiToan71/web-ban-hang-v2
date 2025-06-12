using Project.Enums;

namespace Project.ViewModels.Attributes
{
    public class AttributeCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public AttributeEnum Type { get; set; }
    }

}