using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Utilities.Exceptions;
using Project.ViewModel.Usermanagers.Roles;
using Project.ViewModels;
using Project.ViewModels.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Menus
{
    public class MenuService : IMenusService
    {
        private readonly ProductDbContext _context;
        public MenuService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<int> DeleteRoleMenus(int menuId)
        {
            var items = _context.RoleMenus.Where(m => m.MenuId == menuId);
            _context.RoleMenus.RemoveRange(items);
            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> AddRoleMenus(List<int> roleIds, int menuId)
        {
            List<RoleMenu> items = new List<RoleMenu>();
            foreach (var i in roleIds)
            {
                var item = new RoleMenu
                {
                    RoleId = i,
                    MenuId = menuId
                };
                items.Add(item);
            }
            await _context.RoleMenus.AddRangeAsync(items);
            await _context.SaveChangesAsync();

            return 1;
        }
        public async Task<int> AddNew(MenuCreateRequest request)
        {
            var item = new Menu
            {
                Name = request.Name,
                ParentId = request.ParentId,
                SortOrder = request.SortOrder,
                Url = request.Url,
            };
            _context.Menus.Add(item);
            await _context.SaveChangesAsync();

            await this.AddRoleMenus(request.RoleIds, item.Id);

            return 1;

        }

        public async Task<int> Delete(int id)
        {
            var item = await _context.Menus.FindAsync(id);
            if (item == null)
            {
                throw new ProductException($"Cannot find a item with id: {id}");
            }
            _context.Menus.Remove(item);
            await _context.SaveChangesAsync();
            return 1;
        }
        public async Task<List<MenuViewModel>> GetAll(Guid UserId)
        {
            var roles =  _context.AppUsermangersRoles.Where(x  => x.UserId == UserId ).ToList().Select(m => m.RoleId);
            var data = _context.RoleMenus.Include(m => m.Menu).Where(m => roles.Any(x => x == m.RoleId)).AsQueryable();

            List<MenuViewModel> results = new List<MenuViewModel>();
            foreach(var d in data)
            {
                var obj = new MenuViewModel
                {
                    Id = d.MenuId,
                    Name = d.Menu.Name,
                    ParentId = d.Menu.ParentId,
                    SortOrder = d.Menu.SortOrder,
                    Url = d.Menu.Url,
                };
                results.Add(obj);   
            }

            results = results.OrderBy(m => m.SortOrder).Distinct().ToList();
            return results;
        }

        public async Task<PagedResult<MenuViewModel>> GetAllPaging(GetMenuPagingRequest request)
        {
            //1. query
            var query = _context.Menus.OrderBy(m => m.Id).AsQueryable();

            //2. filter


            //3. paging
            int totalRow = query.Count();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
               .Select(u => new MenuViewModel
               {
                   Id = u.Id,
                   Name = u.Name,
                   ParentId = u.ParentId,
                   SortOrder = u.SortOrder,
                   Url = u.Url,
                   Roles = (List<RoleViewModel>)(from r in u.RoleMenus
                                                 select new RoleViewModel
                                                 {
                                                     Description = r.Role.Description,
                                                     Id = r.RoleId,
                                                     Name = r.Role.Name,
                                                     NormalizedName = r.Role.NormalizedName,

                                                 })

               })
                .ToListAsync();
            //4 .Select and projecttion
            var pagedResult = new PagedResult<MenuViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
            };

            return pagedResult;
        }

        public async Task<MenuViewModel> GetById(int id)
        {
            var item = await _context.Menus.Select(u => new MenuViewModel
            {
                Id = u.Id,
                ParentId = u.ParentId,
                Name = u.Name,
                SortOrder = u.SortOrder,
                Url = u.Url,
                Roles = (List<RoleViewModel>)(from r in u.RoleMenus
                                              select new RoleViewModel
                                              {
                                                  Description = r.Role.Description,
                                                  Id = r.RoleId,
                                                  Name = r.Role.Name,
                                                  NormalizedName = r.Role.NormalizedName,

                                              })
            })
            .FirstOrDefaultAsync(m => m.Id == id);

            return item;
        }

        public async Task<int> Update(MenuUpdateRequest request)
        {
            var item = await _context.Menus.FindAsync(request.Id);
            if (item == null)
            {
                throw new ProductException($"Cannot find a item with id: {request.Id}");
            }
            item.ParentId = request.ParentId;
            item.Name = request.Name;
            item.Url = request.Url;
            item.SortOrder = request.SortOrder;

            await _context.SaveChangesAsync();

            await this.DeleteRoleMenus(item.Id);

            await this.AddRoleMenus(request.RoleIds, item.Id);
            return 1;
        }
    }
}
