using Project.ViewModels;
using Project.ViewModels.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Menus
{
    public interface IMenusService
    {
        Task<PagedResult<MenuViewModel>> GetAllPaging(GetMenuPagingRequest request);
        Task<List<MenuViewModel>> GetAll(Guid UserId);
        Task<MenuViewModel> GetById(int id);
        Task<int> AddNew(MenuCreateRequest request);
        Task<int> Update(MenuUpdateRequest request);
        Task<int> Delete(int id);
    }
}
