
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Usermager.Services.Interfaces;
using Project.Utilities.Exceptions;
using Project.Utilities.Helper;
using Project.ViewModel.Usermanagers.Permissions;
using Project.ViewModels;

namespace Project.Usermager.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;
        public PermissionService(ProductDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddNew(PermissionCreateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = _mapper.Map<Data.Entities.Permission>(request);
            item.CreatedUser = CreatedUser;
            item.CreatedTime = DateTime.Now;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.Permissions.Add(item);
            var resId = await _context.SaveChangesAsync();

            return resId > 0;
        }

        public async Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Permissions.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Item with id: {id}");

            item.IsDeleted = true;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.Permissions.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<PagedResult<Data.Entities.Permission>> GetAllPaging(GetPermissionPagingRequest request)
        {
            //1. query
            var query = _context.Permissions.Where(m => m.IsDeleted == false).AsNoTracking();

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
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<Data.Entities.Permission>()
            {
                TotalRecord = datapaging.TotalItems,
                Items = datapaging.Data,
            };

            await Task.CompletedTask;
            return pagedResult;

        }

        public async Task<PermissionViewModel> GetById(int id)
        {
            var item = await _context.Permissions.Select(m => new PermissionViewModel
            {
                // Description = m.Description,
                Name = m.Name,
                Id = m.Id,
                NormalizedName = m.NormalizedName

            }).FirstOrDefaultAsync(m => m.Id == id);
            if (item == null) throw new ProductException($"Cannot find a permission with id: {id}");

            return item;
        }

        public async Task<bool> Update(PermissionUpdateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Permissions.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a Area with id: {request.Id}");
            _mapper.Map(request, item);
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.Permissions.Update(item);



            await _context.SaveChangesAsync();
            return true;
        }
    }
}
