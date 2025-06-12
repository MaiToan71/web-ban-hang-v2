using Project.Enums;

namespace Project.ViewModel.PostTypes
{
    public class PostTypeCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public int SortOrder { set; get; }
        public bool IsShowOnHome { set; get; }

        public PostTypeEnum PostTypeEnum { set; get; }
        public int ParentId { get; set; }
        public Status Status { set; get; }
        public string? Description { set; get; }
        public string? Phonenumber { get; set; }
    }
}
