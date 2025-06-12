using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Usermager.Services.Interfaces;
using Project.Utilities.Exceptions;

using Project.ViewModel.Usermanagers.Permissions;
using Project.ViewModel.Usermanagers.Roles;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Usermager.Services
{
    public class RoleService : IRoleService
    {
        private readonly ProductDbContext _context;
        public RoleService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<int> DeleteRolePermission(int RoleId)
        {
            var items = _context.RolePermissions.Where(m => m.RoleId == RoleId);
            _context.RolePermissions.RemoveRange(items);
            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> AddRolePermission(List<int> PermissionIds, int RoleId)
        {
            List<RolePermission> items = new List<RolePermission>();
            foreach(var i in PermissionIds)
            {
                var item = new RolePermission
                {
                    PermissionId = i,
                    RoleId = RoleId
                };
                items.Add(item);
            }
            await _context.RolePermissions.AddRangeAsync(items);
            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> AddNew(RoleCreateRequest request)
        {
            var role = new Role
            {
                Description = request.Description,
                Name = request.Name,
                NormalizedName = request.NormalizedName,

            };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            await this.AddRolePermission(request.PermissionIds, role.Id);

            return 1;
        }

        public async Task<int> Delete(int id)
        {
            var item = await _context.Roles.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a role with id: {id}");
            _context.Roles.Remove(item);
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var query = await _context.Roles.Select(m => new RoleViewModel
            {
                Description = m.Description,
                Name = m.Name,
                Id = m.Id,
                NormalizedName = m.NormalizedName

            }).ToListAsync();
            return query;
        }

        public async Task<PagedResult<RoleViewModel>> GetAllPaging(GetRolePagingRequest request)
        {
            //1. query
            var query = _context.Roles.AsQueryable();

            //2. filter
            if (request.Name != null)
            {
                query = query.Where(m => m.Name.Contains(request.Name));
            }
            if (request.NormalizedName != null)
            {
                query = query.Where(m => m.NormalizedName.Contains(request.NormalizedName));
            }
            //3. paging
            int totalRow = query.Count();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new RoleViewModel
                {
                    Id = x.Id,
                    NormalizedName = x.NormalizedName,
                    Name = x.Name,
                    Description = x.Description,
                    Permissions = (List<PermissionViewModel>)(from p in x.RolePermissions select new PermissionViewModel
                    {
                        Description = p.Permission.Description,
                        Id = p.PermissionId,
                        Name = p.Permission.Name,
                        NormalizedName= p.Permission.NormalizedName
                    })
                })
                .ToListAsync();

            //4 .Select and projecttion
            var pagedResult = new PagedResult<RoleViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
            };

            return pagedResult;
        }

        public async Task<RoleViewModel> GetById(int id)
        {
            var item = await _context.Roles.Select(m => new RoleViewModel
            {
                Description = m.Description,
                Name = m.Name,
                Id = m.Id,
                NormalizedName = m.NormalizedName,
                Permissions= (List<PermissionViewModel>)(from p in m.RolePermissions select new PermissionViewModel
                {
                    Description = p.Permission.Description,
                    Name = p.Permission.Name,
                    NormalizedName= p.Permission.NormalizedName,
                    Id = p.Permission.Id
                })
            }).FirstOrDefaultAsync(m => m.Id == id);
            if (item == null) throw new ProductException($"Cannot find a role with id: {id}");

            return item;
        }

        public async Task<int> Update(RoleUpdateRequest request)
        {
            var item = await _context.Roles.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a role with id: {request.Id}");
            item.Description = request.Description;
            item.NormalizedName = request.NormalizedName;
            item.Name = request.Name;


            await _context.SaveChangesAsync();

            await this.DeleteRolePermission(request.Id);

            await this.AddRolePermission(request.PermissionIds, request.Id);
            return 1;
        }
    }
}
