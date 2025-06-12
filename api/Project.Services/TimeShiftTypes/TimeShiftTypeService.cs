using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Utilities.Exceptions;
using Project.Utilities.Helper;
using Project.ViewModels;
using Project.ViewModels.TimeShiftTypes;

namespace Project.Services.TimeShiftTypes
{
    public class TimeShiftTypeService : ITimeShiftTypeService
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;
        public TimeShiftTypeService(ProductDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AddNew(TimeShiftTypeCreateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = _mapper.Map<Data.Entities.TimeShiftType>(request);
            item.CreatedUser = CreatedUser;
            item.CreatedTime = DateTime.Now;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            item.CreatedId = CreatedUserId;
            _context.TimeShiftTypes.Add(item);
            var res = await _context.SaveChangesAsync();

            return res > 0;
        }

        public async Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.TimeShiftTypes.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Item with id: {id}");

            item.IsDeleted = true;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.TimeShiftTypes.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<TimeShiftTypeViewModel>> GetAllPaging(GetTimeShiftTypePagingRequest request)
        {
            //1. query
            var query = _context.TimeShiftTypes.Where(m => m.IsDeleted == false).AsNoTracking();

            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<TimeShiftTypeViewModel>()
            {
                TotalRecord = datapaging.TotalItems,
            };

            var users = await _context.AppUsers.Where(m => datapaging.Data.Select(d => d.PersonInCharge).Contains(m.Id)).ToListAsync();
            foreach (var item in datapaging.Data)
            {
                var tasks = _context.TimeShifts.Where(m => m.TimeShiftTypeId == item.Id).AsQueryable();
                var input = new TimeShiftTypeViewModel
                {
                    TimeShiftType = item,
                    TotalTask = await tasks.CountAsync(),
                    TaskDone = await tasks.Where(m => m.Workflow == Enums.Workflow.COMPLETED).CountAsync(),
                };
                if (users.FirstOrDefault(m => m.Id == item.PersonInCharge) != null)
                {
                    input.UserViewModel = users.FirstOrDefault(m => m.Id == item.PersonInCharge);
                }

                pagedResult.Items.Add(input);
            }
            await Task.CompletedTask;
            return pagedResult;
        }

        public async Task<TimeShiftTypeViewModel> GetById(int id)
        {
            var item = await _context.TimeShiftTypes.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Item with id: {id}");

            var tasks = _context.TimeShifts.Where(m => m.TimeShiftTypeId == item.Id).AsQueryable();
            var user = await _context.AppUsers.FirstOrDefaultAsync(m => m.Id == item.PersonInCharge);
            var response = new TimeShiftTypeViewModel
            {
                TimeShiftType = item,
                TotalTask = await tasks.CountAsync(),
                TaskDone = await tasks.Where(m => m.Workflow == Enums.Workflow.COMPLETED).CountAsync(),
                UserViewModel = user,
                TimeShifts = await _context.TimeShifts.Where(m => m.TimeShiftTypeId == item.Id).ToListAsync(),
            };
            return response;
        }

        public async Task<bool> Update(TimeShiftTypeUpdateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.TimeShiftTypes.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a Item with id: {request.Id}");
            _mapper.Map(request, item);
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.TimeShiftTypes.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
