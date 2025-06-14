using Project.Enums;

namespace Project.ViewModels.Products.Manage
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public string? Search { get; set; }
        public bool? IsPublished { get; set; }
        public bool? IsShowHome { get; set; }
        public string? Keyword { get; set; }
        public PostTypeEnum? PostTypeEnum { get; set; }
        public int? PostTypeId { get; set; }
        public decimal? SellingPriceFrom { get; set; }
        public decimal? SellingPriceTo { get; set; }
        public Guid? UserId { get; set; }
        public Workflow? WorkflowId { get; set; }

        public List<int>? ProductIds { get; set; }

        public bool? IsDiscount { get; set; }
        public List<ProductAttributeSearchRequest>? ProductAttributes { get; set; }
    }

    public class ProductAttributeSearchRequest
    {
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
    }
}
