
using Project.ViewModel.Usermanagers.Permissions;
using Project.ViewModels;

namespace Project.Usermager.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<PagedResult<Data.Entities.Permission>> GetAllPaging(GetPermissionPagingRequest request);

        Task<PermissionViewModel> GetById(int id);
        Task<bool> AddNew(PermissionCreateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser);

        Task<bool> Update(PermissionUpdateRequest request, Guid CreatedUserId, string CreatedUser);
    }
}
