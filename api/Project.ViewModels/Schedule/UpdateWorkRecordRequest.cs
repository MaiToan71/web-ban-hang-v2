using Project.Enums.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels.Schedule
{
    public class UpdateWorkRecordRequest
    {
        [Required]
        public required List<UpdateCalendarEmployeeDTO> LstCalendar { get; set; }
    }
    public class UpdateCalendarEmployeeDTO
    {
        public int? Id { get; set; }
        public string? ImgUrlCheckin { get; set; }
        public string? ImgUrlCheckout { get; set; }
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
        public bool? IsDeleted { get; set; } = false;
    }
}
