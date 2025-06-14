using Project.Enums;

namespace Project.ViewModels.Configs
{
    public class ConfigUpdateRequest
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public ConfigEnum ConfigEnum { get; set; }
        public int ConfigId { get; set; }
        public string Url { get; set; }
    }
}
