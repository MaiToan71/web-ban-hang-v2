using Project.Enums;

namespace Project.Data.Entities
{
    public class Config : BaseViewModel
    {
        public int Order { get; set; }
        public ConfigEnum ConfigEnum { get; set; }
        public int ConfigId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
