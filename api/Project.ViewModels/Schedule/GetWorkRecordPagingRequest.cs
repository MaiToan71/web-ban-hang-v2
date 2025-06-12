using Project.Enums.Enums;

namespace Project.ViewModels.Schedule
{
    public class GetWorkRecordPagingRequest : PagingRequestBase
    {
        public Guid? EmployeeId { get; set; }
        public List<Guid>? EmployeeIds { get; set; }
        public string? EmployeeName { get; set; }
        public List<WorkRecordStatusEnum>? LstWorkRecordStatus { get; set; }
        public List<WorkType>? LstWorkType { get; set; }
        /// <summary>
        /// ngày giờ bắt đầu đăng kí
        /// </summary>
        public DateTime? FromWorkDateRegis { get; set; }
        /// <summary>
        /// ngày giờ kết thúc đăng kí
        /// </summary>
        public DateTime? ToWorkDateRegis { get; set; }
        public int? StoreId { get; set; }
    }
}
