using Project.Data.Entities;
using Project.ViewModels;
using Project.ViewModels.TimeShifts;

namespace Project.Services.TimeShifts
{
    public interface ITimeShiftService
    {
        Task<PagedResult<TimeShift>> GetAllPaging(GetTimeShiftPagingRequest request);
        Task<TimeShiftDTOViewModel> GetById(int id);
        Task<bool> AddNew(TimeShiftCreateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Update(TimeShiftUpdateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser);
        Task<bool> UpdateWorkflow(TimeShiftUpdateRequest request, Guid CreatedUserId, string CreatedUser);
    }
}
