using Project.Data.Entities;
using Project.ViewModels;
using Project.ViewModels.Schedule;

namespace Project.Services.Schedule
{
    public interface IWorkScheduleService
    {
        Task<PagedResult<WorkRecord>> GetAllPaging(GetWorkRecordPagingRequest request);
        Task<WorkRecord> GetById(int id);
        Task<bool> EmployeeUpdateCalendar(UpdateWorkRecordRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> EmployeeCheckin(EmployeeCheckinRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> EmployeeCheckout(EmployeeCheckoutRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> AdminUpdateCalendar(UpdateWorkRecordAdminRequest request, Guid CreatedUserId, string CreatedUser);
        Task<WorkRecord> Delete(int id);
    }
}
