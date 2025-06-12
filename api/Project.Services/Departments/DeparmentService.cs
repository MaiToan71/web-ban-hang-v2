using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Utilities.Exceptions;
using Project.Utilities.Helper;
using Project.ViewModels;
using Project.ViewModels.Departments;

namespace Project.Services.Departments
{
    public class DeparmentService : IDepartmentService
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;

        public DeparmentService(ProductDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AddNew(DepartmentCreateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = _mapper.Map<Data.Entities.Department>(request);
            item.CreatedUser = CreatedUser;
            item.CreatedTime = DateTime.Now;
            item.ModifiedTime = DateTime.Now;
            item.CreatedId = CreatedUserId;
            item.ModifiedUser = CreatedUser;
            _context.Departments.Add(item);
            var res = await _context.SaveChangesAsync();

            return res > 0;
        }

        public async Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Departments.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {id}");

            item.IsDeleted = true;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.Departments.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<Department>> GetAllPaging(GetDepartmentPagingRequest request)
        {
            //1. query

            var query = _context.Departments
                .Where(m => m.IsDeleted == false).AsNoTracking();
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(m => m.Name.Contains(request.Name));
            }
            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<Department>()
            {
                TotalRecord = datapaging.TotalItems,
                Items = datapaging.Data
            };

            await Task.CompletedTask;
            return pagedResult;
        }

        public async Task<Department> GetById(int id)
        {
            var item = await _context.Departments.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {id}");
            return item;
        }

        public async Task<bool> Update(DepartmentUpdateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Departments.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {request.Id}");
            _mapper.Map(request, item);
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.Departments.Update(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
