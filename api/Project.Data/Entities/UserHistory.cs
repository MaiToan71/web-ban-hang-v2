namespace Project.Data.Entities
{
    public class UserHistory : BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public bool Status { get; set; }
    }
}
