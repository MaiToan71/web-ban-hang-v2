using Project.Enums;

namespace Project.Data.Entities
{
    public class TimeShift : BaseViewModel
    {
        public string? Name { get; set; }
        public string? Note { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TimeShiftTypeId { get; set; }

        public Workflow Workflow { get; set; }

        public Guid? PersonInCharge { get; set; }
        public string? Color { get; set; }

        public PriorityEnum? PriorityEnum { get; set; }
    }
}
