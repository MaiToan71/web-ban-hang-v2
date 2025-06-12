using Project.Data.Entities;
using Project.Enums;
using Project.ViewModel.PostTypes;
using Project.ViewModels.Images;

namespace Project.ViewModels.Posts
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid CreateUserId = Guid.Empty;
        public string Content { get; set; } = string.Empty;
        public string ShortContent { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string CoverImage { get; set; } = string.Empty;
        public Platform Platform { get; set; }
        public int PostTypeId { get; set; }
        public bool IsShowHome { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreateTime { get; set; }
        public string Author { get; set; } = string.Empty;
        public Workflow WorkflowId { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime InputDate { get; set; } = new DateTime();
        public DateTime ExportDate { get; set; } = new DateTime();
        public int Quantity { get; set; } = 0;
        public int QuantitySold { get; set; } = 0;

        public List<Department> Departments { get; set; } = new List<Department>();
        public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();
        public PostTypeViewModel PostType { get; set; } = new PostTypeViewModel();

    }
}
