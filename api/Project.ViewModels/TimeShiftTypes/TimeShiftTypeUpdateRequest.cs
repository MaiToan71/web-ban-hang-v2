using Project.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels.TimeShiftTypes
{
    public class TimeShiftTypeUpdateRequest
    {
        [Required]
        public required int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? BackgroundColor { get; set; }
        public string? TextColor { get; set; }

        public string? Note { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? TimeShiftTypeId { get; set; }

        //  public Workflow? Workflow { get; set; }

        public Guid? PersonInCharge { get; set; }

        public PriorityEnum? PriorityEnum { get; set; }
    }
}
