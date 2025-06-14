using Project.ViewModels;
using Project.ViewModels.Configs;

namespace Project.Services.Configs
{
    public interface IConfigService
    {
        Task<PagedResult<Project.Data.Entities.Config>> GetAllPaging(GetConfigPagingRequest request);
        Task<bool> AddNew(ConfigCreateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Update(ConfigUpdateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser);
    }
}
