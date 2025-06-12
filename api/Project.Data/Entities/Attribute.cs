using Project.Enums;

namespace Project.Data.Entities
{
    public class Attribute : BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public AttributeEnum Type { get; set; }
    }
}
