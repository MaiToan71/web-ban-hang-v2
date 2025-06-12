namespace Project.Data.Entities
{
    public class TimeShiftComment : BaseViewModel
    {
        public string? Content { get; set; }
        public int ParentId { get; set; } = 0;
    }
}
