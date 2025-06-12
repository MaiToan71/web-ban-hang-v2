using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Enums;
using Project.Utilities.Exceptions;
using Project.Utilities.Helper;
using Project.ViewModel.Usermanagers.Users;
using Project.ViewModels;
using Project.ViewModels.TimeShifts;

namespace Project.Services.TimeShifts
{
    public class TimeShiftService : ITimeShiftService
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;
        public TimeShiftService(ProductDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AddNew(TimeShiftCreateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = _mapper.Map<Data.Entities.TimeShift>(request);
            item.CreatedUser = CreatedUser;
            item.CreatedTime = DateTime.Now;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            item.CreatedId = CreatedUserId;
            _context.TimeShifts.Add(item);
            var resId = await _context.SaveChangesAsync();
            if (request.UserIds != null)
            {
                await AddUserToTimeShift(request.UserIds, resId);



            }

            await setTotalTimeshif(item.TimeShiftTypeId);

            return resId > 0;
        }

        public async Task<int> setTotalTimeshif(int timeShiftType)
        {
            var totalTimeshifts = _context.TimeShifts.Where(m => m.TimeShiftTypeId == timeShiftType).Where(m => m.IsDeleted == false).Count();
            var checkTimeshiftType = await _context.TimeShiftTypes.FindAsync(timeShiftType);
            if (checkTimeshiftType != null)
            {
                checkTimeshiftType.TotalTimeShift = totalTimeshifts;
            }
            _context.SaveChanges();
            return 1;
        }

        public async Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.TimeShifts.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Item with id: {id}");

            item.IsDeleted = true;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.TimeShifts.Update(item);
            await _context.SaveChangesAsync();
            await setTotalTimeshif(item.TimeShiftTypeId);

            return true;
        }

        public async Task<PagedResult<TimeShift>> GetAllPaging(GetTimeShiftPagingRequest request)
        {
            //1. query
            var query = _context.TimeShifts.Where(m => m.IsDeleted == false).OrderBy(m => m.StartTime).AsNoTracking();
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(m => m.Name.Contains(request.Name));
            }
            if (request.FromDate != null && request.ToDate != null)
            {
                query = query.Where(m => (m.StartTime >= request.FromDate && m.EndTime <= request.ToDate) || (m.EndTime >= request.FromDate));
            }
            if (request.TimeShiftTypeId != null)
            {
                query = query.Where(m => m.TimeShiftTypeId == request.TimeShiftTypeId);
            }
            if (request.CurrentDate != null)
            {
                query = query.Where(m => m.StartTime >= request.CurrentDate);
            }
            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<Data.Entities.TimeShift>()
            {
                TotalRecord = datapaging.TotalItems,
                Items = datapaging.Data,
            };

            await Task.CompletedTask;
            return pagedResult;
        }

        public async Task<TimeShiftDTOViewModel> GetById(int id)
        {
            var response = new TimeShiftDTOViewModel();
            var item = await _context.TimeShifts.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Item with id: {id}");
            response.TimeShift = item;

            var timeShiftUsers = await _context.TimeShiftUsers.Where(m => m.TimeShiftId == id).ToListAsync();
            List<Guid> userIds = new List<Guid>();
            foreach (var user in timeShiftUsers)
            {
                userIds.Add(user.UserId);
            }
            if (item.PersonInCharge != null)
            {
                userIds.Add((Guid)item.PersonInCharge);
            }
            var users = await _context.AppUsers.Where(m => userIds.Contains(m.Id)).Select(m => new UserViewModel
            {
                UserId = m.Id,
                Id = m.Id,
                Email = m.Email,
                FullName = m.FullName
            }).ToListAsync();

            var personInCharge = users.FirstOrDefault(m => m.UserId == item.PersonInCharge);
            if (personInCharge != null)
            {
                response.User = personInCharge;
            }
            response.Users = users;
            return response;
        }

        public async Task<bool> Update(TimeShiftUpdateRequest request, Guid CreatedUserId, string CreatedUser)
        {

            var item = await _context.TimeShifts.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a Area with id: {request.Id}");
            _mapper.Map(request, item);
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.TimeShifts.Update(item);

            if (request.UserIds != null)
            {
                await RemoveUserInTimeShift(item.Id);
                await AddUserToTimeShift(request.UserIds, item.Id);

            }

            await _context.SaveChangesAsync();
            await setTotalTimeshif(item.TimeShiftTypeId);
            return true;
        }

        public async Task<bool> UpdateWorkflow(TimeShiftUpdateRequest request, Guid CreatedUserId, string CreatedUser)
        {

            var item = await _context.TimeShifts.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a Area with id: {request.Id}");
            item.ModifiedTime = DateTime.Now;
            item.ModifiedId = CreatedUserId;
            item.ModifiedUser = CreatedUser;
            item.Workflow = (Workflow)request.Workflow;
            _context.SaveChanges();
            return true;
        }
        public async Task<bool> RemoveUserInTimeShift(int timeShiftId)
        {
            var timeShifts = await _context.TimeShiftUsers.Where(m => m.TimeShiftId == timeShiftId).ToListAsync();
            if (timeShifts.Count > 0)
            {
                _context.TimeShiftUsers.RemoveRange(timeShifts);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> AddUserToTimeShift(List<Guid> userIds, int timeShiftId)
        {
            var list = new List<TimeShiftUser>();
            foreach (var userid in userIds)
            {
                var input = new TimeShiftUser
                {
                    UserId = userid,
                    TimeShiftId = timeShiftId
                };
                list.Add(input);
            }
            _context.TimeShiftUsers.AddRange(list);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
