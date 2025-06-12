using Project.Data.EF;
using Project.Data.Entities;
using Project.Utilities.Exceptions;
using Project.Utilities.Helper;
using Project.ViewModels;
using Project.ViewModels.Positions;

namespace Project.Services.Positions
{
    public class PositionService : IPositionService
    {
        private readonly ProductDbContext _context;
        public PositionService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNew(PositionCreateRequest request, Guid CreatedUserId, string CreatedUser)
        {

            var item = new Position
            {
                Description = request.Description,
                Name = request.Name,
                CreatedId = CreatedUserId,
                CreatedUser = CreatedUser,
                ModifiedUser = CreatedUser,
                ModifiedId = CreatedUserId,

            };
            await _context.Positions.AddAsync(item);
            await _context.SaveChangesAsync(); return true;
        }

        public async Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Positions.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a item with id: {id}");

            item.IsDeleted = true;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedId = CreatedUserId;
            item.ModifiedUser = CreatedUser;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<PositionViewModel>> GetAllPaging(GetPositionPagingRequest request)
        {
            //1. query
            var query = _context.Positions.Where(m => m.IsDeleted == false).Select(x => new PositionViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).AsQueryable();

            //2. filter


            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<PositionViewModel>()
            {
                TotalRecord = datapaging.TotalItems,
                Items = datapaging.Data,
            };

            return pagedResult;
        }

        public async Task<bool> Update(PositionUpdateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Positions.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a item with id: {request.Id}");
            item.Description = request.Description;
            item.Name = request.Name;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedId = CreatedUserId;
            item.ModifiedUser = CreatedUser;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
