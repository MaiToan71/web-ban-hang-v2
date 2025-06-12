using Project.ViewModels;
using Project.ViewModels.Positions;

namespace Project.Services.Positions
{
    public interface IPositionService
    {
        Task<PagedResult<PositionViewModel>> GetAllPaging(GetPositionPagingRequest request);
        Task<bool> AddNew(PositionCreateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Update(PositionUpdateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser);
    }
}
