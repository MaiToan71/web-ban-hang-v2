using Project.Enums;

namespace Project.ViewModel.PostTypes
{
    public class PostTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SortOrder { set; get; }
        public bool IsShowOnHome { set; get; }
        public int ParentId { get; set; }
        public string? Phonenumber { get; set; }
        public PostTypeEnum PostTypeEnum { get; set; }
        public bool IsDelete { set; get; }
        public Status Status { set; get; }
        public int TotalOfPosts { set; get; } = 0;
    }
}
