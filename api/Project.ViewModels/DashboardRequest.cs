namespace Project.ViewModels
{
    public class DashboardRequest
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }

    public class StatisticalRole
    {
        public int RoleId { get; set; } = 0;
        public string RoleName { get; set; } = "";
        public int Total { get; set; } = 0;
    }
}
