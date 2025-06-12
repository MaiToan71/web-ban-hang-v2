using Project.ViewModel.Usermanagers.Permissions;
using Project.ViewModel.Usermanagers.Roles;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Usermager.Services.Interfaces
{
    public interface IRoleService
    {
        Task<PagedResult<RoleViewModel>> GetAllPaging(GetRolePagingRequest request);
        Task<List<RoleViewModel>> GetAll();
        Task<RoleViewModel> GetById(int id);
        Task<int> AddNew(RoleCreateRequest request);
        Task<int> Update(RoleUpdateRequest request);
        Task<int> Delete(int id);
    }
}
