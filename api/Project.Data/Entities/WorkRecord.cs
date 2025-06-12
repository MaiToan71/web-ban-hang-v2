using Project.Enums.Enums;

namespace Project.Data.Entities
{
    public class WorkRecord : BaseViewModel
    {
        public Guid? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? ImgUrlCheckin { get; set; }
        public string? ImgUrlCheckout { get; set; }
        /// <summary>
        /// ngày đăng kí
        /// </summary>
        public DateTime DateOnly { get; set; } = DateTime.Now;
        /// <summary>
        /// ngày giờ bắt đầu đăng kí
        /// </summary>
        public DateTime FromWorkDateRegis { get; set; }
        /// <summary>
        /// ngày giờ kết thúc đăng kí
        /// </summary>
        public DateTime ToWorkDateRegis { get; set; }
        /// <summary>
        /// ngày giờ bắt đầu thực tế
        /// </summary>
        public DateTime? FromWorkDateEffect { get; set; }
        /// <summary>
        /// ngày giờ kết thúc thực tế
        /// </summary>
        public DateTime? ToWorkDateEffect { get; set; }
        public double? TotalHours { get; set; } = 0;// tổng giờ
        public WorkType WorkType { get; set; } // loại hình làm việc, tăng ca hay thường
        public decimal? Multiplier { get; set; } = 1;// hệ số lương
        public WorkRecordStatusEnum? WorkRecordStatus { get; set; }
    }
}
