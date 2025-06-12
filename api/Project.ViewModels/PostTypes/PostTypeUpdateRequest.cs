using Project.Enums;

namespace Project.ViewModel.PostTypes
{
    public class PostTypeUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SortOrder { set; get; }
        public bool? IsShowOnHome { set; get; }
        public PostTypeEnum PostTypeEnum { set; get; }
        public int? ParentId { get; set; }
        public Status? Status { set; get; }
        public string? Description { set; get; }
        public string? Phonenumber { get; set; }
    }
}
