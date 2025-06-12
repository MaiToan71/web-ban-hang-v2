using Project.Enums;

namespace Project.ViewModels.Attributes
{
    public class GetAttributePagingRequest : PagingRequestBase
    {
        public AttributeEnum? AttributeEnum { get; set; }
    }
}
