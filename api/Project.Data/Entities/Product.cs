using Project.Enums;

namespace Project.Data.Entities
{
    public class Product : BaseViewModel
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Url { get; set; }
        public string Description { get; set; }

        public string Content { get; set; }

        public string ShortContent { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Source { get; set; }
        public string CoverImage { get; set; }
        public Workflow WorkflowId { get; set; }
        public bool IsPublished { get; set; } = false;
        public bool IsShowHome { get; set; } = false;

        public Platform Platform { get; set; } = Platform.Facebook;

        public PostTypeEnum PostTypeEnum { get; set; } = PostTypeEnum.Post;

        public Guid UserId { get; set; }

        public DateTime? InputDate { get; set; }
        public DateTime? ExportDate { get; set; }
        public decimal Quantity { get; set; } = 0;
        public decimal QuantitySold { get; set; } = 0;
        public DateTime? ExpireDate { get; set; }
        public float CapitalPrice { get; set; } = 0;
        public float SellingPrice { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public int BrandId { get; set; }
        public AppUser User { get; set; }

        public int PostTypeId { get; set; }

        public PostType PostType { get; set; }

        public List<ProductImage> ProductImages { get; set; }
    }
}
