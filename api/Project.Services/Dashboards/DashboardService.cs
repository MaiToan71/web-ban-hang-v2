using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Enums;
using Project.ViewModels;

namespace Project.Services.Dashboards
{
    public class DashboardService : IDashboardService
    {
        private readonly ProductDbContext _context;

        public DashboardService(ProductDbContext context)
        {
            _context = context;
        }
        public DateTime FormatFromDateString(string date)
        {
            DateTime myDate = DateTime.ParseExact($"{date} 00:00:00", "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
            return myDate;
        }
        public DateTime FormatToDateDateString(string date)
        {
            DateTime myDate = DateTime.ParseExact($"{date} 23:59:59", "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
            return myDate;
        }
        public async Task<dynamic> GetStatistical()
        {
            var totalUsers = await _context.AppUsers.Where(m => m.UserType == UserType.Client && m.IsDelete == false).CountAsync();
            var totalBanks = await _context.BankConfigs.Where(m => m.IsDeleted == false).CountAsync();
            var totalUserRoles = await _context.AppUsers.Where(m => m.UserType != UserType.Client && m.IsDelete != false).CountAsync();
            var totalUserMoney = await _context.BankUsers.Where(m => m.IsDeleted != false).CountAsync();
            return new
            {
                Users = totalUsers,
                Banks = totalBanks,
                UserRoles = totalUserRoles,
                UserMoneys = totalUserMoney

            };
        }

        public List<StatisticalRole> GetStatisticalRole()
        {
            var roleUsers = _context.AppUsermangersRoles.GroupBy(
                p => p.RoleId,
                (key, g) => new StatisticalRole { RoleId = key, RoleName = key.ToString(), Total = g.Count() }).ToList();

            var roles = _context.Roles.Where(m => roleUsers.Select(p => p.RoleId).Contains(m.Id)).ToList();
            foreach (var r in roleUsers)
            {
                dynamic role = roles.FirstOrDefault(m => m.Id == r.RoleId);
                if (role != null)
                {
                    string roleName = (dynamic)role.Name.ToString();
                    r.RoleName = roleName != null ? roleName : "";
                }
            }

            return roleUsers;
        }

        public dynamic GetStatisticalUserMoneyInDate(DashboardRequest request)
        {
            var roleUsers = _context.BankUsers.Where(m => m.CreatedTime >= FormatFromDateString(request.FromDate)
            && m.CreatedTime <= FormatToDateDateString(request.ToDate) && m.IsDeleted == false && m.Workflow == Workflow.COMPLETED);

            var results = roleUsers.GroupBy(x => new { Year = x.CreatedTime.Year, Month = x.CreatedTime.Month, Day = x.CreatedTime.Day })
            .Select(x => new
            {
                Value = x.Sum(x => x.Amount),
                Year = x.Key.Year,
                Month = x.Key.Month,
                Day = x.Key.Day
            }).ToList();
            List<dynamic> selectedDates = new List<dynamic>();
            for (var date = FormatFromDateString(request.FromDate); date <= FormatToDateDateString(request.ToDate); date = date.AddDays(1))
            {
                float total = 0;
                var value = new
                {
                    Value = total,
                    Year = date.Year,
                    Month = date.Month,
                    Day = date.Day
                };
                var checkResult = results.FirstOrDefault(r => r.Year == value.Year && r.Month == value.Month && r.Day == value.Day);
                if (checkResult != null)
                {

                    selectedDates.Add(new
                    {
                        Value = Math.Round((decimal)checkResult.Value, 0),
                        Year = date.Year,
                        Month = date.Month,
                        Day = date.Day
                    });
                }
                else
                {
                    selectedDates.Add(value);
                }


            }
            return selectedDates;
        }
    }
}
