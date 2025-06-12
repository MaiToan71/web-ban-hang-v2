namespace Project.Data.Entities
{
    public class TimeShiftTypeComment : BaseViewModel
    {
        public string? Content { get; set; }
        public int ParentId { get; set; } = 0;
    }
}
