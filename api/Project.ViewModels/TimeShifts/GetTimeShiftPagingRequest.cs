namespace Project.ViewModels.TimeShifts
{
    public class GetTimeShiftPagingRequest : PagingRequestBase
    {
        public string? Name { get; set; }
        public int? TimeShiftTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public DateTime? CurrentDate { get; set; }

    }
}
