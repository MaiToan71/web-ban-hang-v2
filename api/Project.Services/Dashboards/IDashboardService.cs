using Project.ViewModels;

namespace Project.Services.Dashboards
{
    public interface IDashboardService
    {
        Task<dynamic> GetStatistical();
        List<StatisticalRole> GetStatisticalRole();

        dynamic GetStatisticalUserMoneyInDate(DashboardRequest request);
    }
}
