using Project.Enums.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Project.ViewModels.Schedule
{
    public class UpdateWorkRecordAdminRequest
    {
        [Required]
        public required List<UpdateCalendarEmployeeAdminDTO> LstCalendar { get; set; }
    }
    public class UpdateCalendarEmployeeAdminDTO : UpdateCalendarEmployeeDTO
    {
        public int? Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public decimal? TotalHours { get; set; } = 0;// tổng giờ

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataTypeAttribute(typeof(WorkType))]
        public WorkType? WorkType { get; set; } // loại hình làm việc, tăng ca hay thường
        public decimal? Multiplier { get; set; } = 1;// hệ số lương

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataTypeAttribute(typeof(WorkRecordStatusEnum))]
        public WorkRecordStatusEnum? WorkRecordStatus { get; set; } = WorkRecordStatusEnum.AdminAccept;
    }
}
