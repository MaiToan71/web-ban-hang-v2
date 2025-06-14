using Project.Enums;

namespace Project.ViewModels.Configs
{
    public class ConfigCreateRequest
    {
        public int Order { get; set; }
        public ConfigEnum ConfigEnum { get; set; }
        public int ConfigId { get; set; }
        public string Url { get; set; }
        public string? Title { get; set; }
    }
}
