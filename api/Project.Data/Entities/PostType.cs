using Project.Enums;

namespace Project.Data.Entities
{
    public class PostType : BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { set; get; }
        public bool IsShowOnHome { set; get; }
        public string? Phonenumber { get; set; }
        public int ParentId { get; set; }
        public Status Status { set; get; }
        public PostTypeEnum PostTypeEnum { get; set; } = PostTypeEnum.Folder;

        public List<Post> Posts { get; set; }

        public List<Product> Products { get; set; }
        public List<PostTypeImage> PostTypeImages { get; set; }
    }
}
