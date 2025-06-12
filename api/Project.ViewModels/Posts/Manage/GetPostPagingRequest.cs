using Project.Enums;

namespace Project.ViewModels.Posts.Manage
{
    public class GetPostPagingRequest : PagingRequestBase
    {
        public bool? IsPublished { get; set; }
        public bool? IsShowHome { get; set; }
        public string? Keyword { get; set; }
        public PostTypeEnum? PostTypeEnum { get; set; }

        public int? PostTypeId { get; set; }

        public Guid? UserId { get; set; }

        public Workflow? WorkflowId { get; set; }
    }
}
