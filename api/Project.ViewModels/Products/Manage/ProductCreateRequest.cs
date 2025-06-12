using Microsoft.AspNetCore.Http;
using Project.Enums;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Project.ViewModels.Products.Manage
{
    public class ProductCreateRequest
    {
        public required string Title { get; set; }
        public required string SubTitle { get; set; }
        public required string Url { get; set; }
        public required string Description { get; set; }

        [DisplayFormat(HtmlEncode = true)]
        [AllowHtml]
        public required string Content { get; set; }
        public required string ShortContent { get; set; }
        public Platform Platform { get; set; }
        public required string Source { get; set; }
        public required string CoverImage { get; set; }
        public bool IsPublished { get; set; }
        public Workflow WorkflowId { get; set; }
        public bool IsShowHome { get; set; }
        public required int PostTypeId { get; set; }
        public PostTypeEnum PostTypeEnum { get; set; }
        public required IFormFile? SelectedFilesImage { get; set; }
    }
}
