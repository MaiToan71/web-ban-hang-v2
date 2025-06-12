using Project.Data.Entities;
using Project.ViewModels;
using Project.ViewModels.Departments;

namespace Project.Services.Departments
{
    public interface IDepartmentService
    {
        Task<PagedResult<Department>> GetAllPaging(GetDepartmentPagingRequest request);

        Task<Department> GetById(int id);
        Task<bool> AddNew(DepartmentCreateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Update(DepartmentUpdateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser);

    }
}
