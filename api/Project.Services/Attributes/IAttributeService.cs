using Project.ViewModels;
using Project.ViewModels.Attributes;

namespace Project.Services.Attributes
{
    public interface IAttributeService
    {
        Task<PagedResult<Project.Data.Entities.Attribute>> GetAllPaging(GetAttributePagingRequest request);

        Task<bool> AddNew(AttributeCreateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Update(AttributeUpdateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser);

    }
}
