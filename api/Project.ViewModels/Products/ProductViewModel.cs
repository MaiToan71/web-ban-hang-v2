using Project.Enums;
using Project.ViewModel.PostTypes;
using Project.ViewModels.Attributes;
using Project.ViewModels.Images;

namespace Project.ViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
        public string ShortContent { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string CoverImage { get; set; } = string.Empty;
        public Platform Platform { get; set; }
        public int PostTypeId { get; set; }
        public bool IsShowHome { get; set; } = false;
        public bool IsHot { get; set; } = false;
        public bool IsPublished { get; set; } = false;
        public DateTime CreateTime { get; set; }
        public string Author { get; set; } = string.Empty;
        public Workflow WorkflowId { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? InputDate { get; set; }
        public DateTime? ExportDate { get; set; }
        public decimal Quantity { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public decimal QuantitySold { get; set; } = 0;
        public float CapitalPrice { get; set; } = 0;
        public float SellingPrice { get; set; } = 0;
        public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();
        public PostTypeViewModel PostType { get; set; } = new PostTypeViewModel();

        public List<ProductAttributeViewModel> ProductAttributes { get; set; }

    }
}
