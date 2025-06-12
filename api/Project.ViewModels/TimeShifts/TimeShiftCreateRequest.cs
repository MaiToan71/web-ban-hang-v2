using Project.Enums;

namespace Project.ViewModels.TimeShifts
{
    public class TimeShiftCreateRequest
    {
        public string? Name { get; set; }
        public string? Note { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? TimeShiftTypeId { get; set; }

        public List<Guid>? UserIds { get; set; }
        public string? Color { get; set; }
        public Workflow? Workflow { get; set; }

        public Guid? PersonInCharge { get; set; }

        public PriorityEnum? PriorityEnum { get; set; }
    }
}
