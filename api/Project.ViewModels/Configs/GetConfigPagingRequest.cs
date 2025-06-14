using Project.Enums;

namespace Project.ViewModels.Configs
{
    public class GetConfigPagingRequest : PagingRequestBase
    {
        public ConfigEnum? ConfigEnum { get; set; }
    }
}
