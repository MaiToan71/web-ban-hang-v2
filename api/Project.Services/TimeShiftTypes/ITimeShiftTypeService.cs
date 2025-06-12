using Project.ViewModels;
using Project.ViewModels.TimeShiftTypes;

namespace Project.Services.TimeShiftTypes
{
    public interface ITimeShiftTypeService
    {
        Task<PagedResult<TimeShiftTypeViewModel>> GetAllPaging(GetTimeShiftTypePagingRequest request);
        Task<TimeShiftTypeViewModel> GetById(int id);
        Task<bool> AddNew(TimeShiftTypeCreateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Update(TimeShiftTypeUpdateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser);
    }
}
