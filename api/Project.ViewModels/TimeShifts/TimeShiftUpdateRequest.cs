using Project.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels.TimeShifts
{
    public class TimeShiftUpdateRequest
    {
        [Required]
        public required int Id { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? TimeShiftTypeId { get; set; }
        public string? Color { get; set; }
        public List<Guid>? UserIds { get; set; }

        public Workflow? Workflow { get; set; }

        public Guid? PersonInCharge { get; set; }

        public PriorityEnum? PriorityEnum { get; set; }
    }
}
