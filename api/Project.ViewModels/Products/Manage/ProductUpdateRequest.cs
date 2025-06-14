using Microsoft.AspNetCore.Http;
using Project.Enums;
using Project.ViewModels.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Project.ViewModels.Products.Manage
{
    public class ProductUpdateRequest
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string SubTitle { get; set; }
        public required string Url { get; set; }
        public required string Description { get; set; }
        public string Code { get; set; }
        public Platform Platform { get; set; }
        [DisplayFormat(HtmlEncode = true)]
        [AllowHtml]
        public required string Content { get; set; }
        public required string ShortContent { get; set; }
        public required string Source { get; set; }
        public required string CoverImage { get; set; }
        public bool IsPublished { get; set; }
        public Workflow WorkflowId { get; set; }
        public bool IsShowHome { get; set; }
        public decimal Discount { get; set; }
        public required int PostTypeId { get; set; }
        public float CapitalPrice { get; set; } = 0;
        public float SellingPrice { get; set; } = 0;
        public DateTime? InputDate { get; set; }
        public DateTime? ExportDate { get; set; }
        public DateTime? ExpireDate { get; set; }

        public List<ProductAttributeRequest>? Attributes { get; set; }



        public required List<IFormFile>? SelectedFilesImages { get; set; }
    }


}
