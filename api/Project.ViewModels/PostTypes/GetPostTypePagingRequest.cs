using Project.Enums;
using Project.ViewModels;

namespace Project.ViewModel.PostTypes
{
    public class GetPostTypePagingRequest : PagingRequestBase
    {

        public PostTypeEnum? PostTypeEnum { get; set; }
        public Workflow? Workflow { get; set; }
        public bool? IsAll { get; set; }
        public bool? SortOrder { get; set; }
        public string? Name { get; set; }
        public Status? Status { get; set; }

        public List<int>? Ids { get; set; }
    }
}
