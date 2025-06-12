using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Utilities.Helper;
using Project.ViewModels;
using Project.ViewModels.Schedule;

namespace Project.Services.Schedule
{
    public class WorkScheduleService : IWorkScheduleService
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;

        public WorkScheduleService(ProductDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AdminUpdateCalendar(UpdateWorkRecordAdminRequest request, Guid CreatedId, string CreatedUser)
        {
            //get calendar after month exist in db
            var date = DateTime.Now;
            if (request.LstCalendar.Count <= 0)
                return false;
            var lstCalendarId = request.LstCalendar.Select(x => x.Id).ToList();
            var lstCalendarHaveId = await _context.WorkRecords
                .AsNoTracking()
                .Where(x =>
                    (!x.IsDeleted)
                    && (lstCalendarId.Any(y => y == x.Id)))
                .ToListAsync();
            var lstEmployeeIdReq = request.LstCalendar.Select(x => x.EmployeeId).ToList();
            var minDate = request.LstCalendar.Select(x => x.FromWorkDateRegis).Min();
            var maxDate = request.LstCalendar.Select(x => x.ToWorkDateRegis).Min();
            var lstEmployeeDB = await _context.AppUsers.AsNoTracking().Where(x => lstEmployeeIdReq.Any(y => y == x.Id) && !x.IsDeleted).ToListAsync();
            var lstEmployeeIdDB = lstEmployeeDB.Select(x => x.Id).ToList();
            if (lstEmployeeDB.Count == 0)
                throw new Exception("Không tìm thấy người dùng");
            var lstCalendarDB = await _context.WorkRecords
                .AsNoTracking()
                .Where(x =>
                    (!x.IsDeleted)
                    && (lstEmployeeIdDB.Any(y => y == x.EmployeeId))
                    && (x.FromWorkDateRegis.Date >= minDate.Date)
                    && (x.ToWorkDateRegis.Date <= maxDate.Date))
                .ToListAsync();
            if (request.LstCalendar.Count == 0)
                return true;
            var lstWorkeRecordAdd = new List<WorkRecord>();
            var lstWorkeRecordUp = new List<WorkRecord>();
            foreach (var item in request.LstCalendar)
            {
                // valid date form and to
                if (((item.ToWorkDateRegis - item.FromWorkDateRegis).TotalDays > 1)
                    || (item.ToWorkDateRegis <= item.FromWorkDateRegis))
                    continue;
                if (item.Id == null || item.Id == 0)
                {
                    var workRecord = lstCalendarDB.FirstOrDefault(x =>
                        (x.DateOnly.Date == item.ToWorkDateRegis.Date)
                        &&
                        (
                            // ca đăng kí sau không được móc xích với ca đăng kí trước
                            (
                                    ((x.FromWorkDateRegis < item.FromWorkDateRegis) && (x.ToWorkDateRegis > item.FromWorkDateRegis))
                                    || ((x.FromWorkDateRegis < item.ToWorkDateRegis) && (x.ToWorkDateRegis > item.ToWorkDateRegis))
                            )
                              // hoặc ca đăng kí sau không được chứa hoặc trùng ca đăng kí trước đó
                              || ((x.FromWorkDateRegis >= item.FromWorkDateRegis) && (x.ToWorkDateRegis <= item.ToWorkDateRegis))
                          )
                    );
                    if (workRecord != null) continue;
                    var employee = lstEmployeeDB.FirstOrDefault(x => x.Id == item.EmployeeId);
                    if (employee == null)
                        continue;
                    if (workRecord == null)
                    {
                        workRecord = _mapper.Map<WorkRecord>(item);
                        workRecord.CreatedId = CreatedId;
                        workRecord.CreatedUser = CreatedUser;
                        workRecord.DateOnly = item.ToWorkDateRegis.Date;
                        lstWorkeRecordAdd.Add(workRecord);
                    }
                }
                else
                {
                    var workRecordHaveId = lstCalendarHaveId.FirstOrDefault(x => x.Id == item.Id);
                    if (workRecordHaveId == null) continue;
                    _mapper.Map(item, workRecordHaveId);
                    workRecordHaveId.ModifiedId = CreatedId;
                    workRecordHaveId.ModifiedUser = CreatedUser;
                    workRecordHaveId.ModifiedTime = date;
                    lstWorkeRecordUp.Add(workRecordHaveId);
                }

            }
            _context.WorkRecords.AddRange(lstWorkeRecordAdd);
            _context.WorkRecords.UpdateRange(lstWorkeRecordUp);
            if (lstWorkeRecordAdd.Count == 0 && lstWorkeRecordUp.Count == 0)
            {
                throw new Exception("Đã trùng thời gian trong ngày hoặc bị sang ngày mới");
            }
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> EmployeeCheckin(EmployeeCheckinRequest request, Guid CreatedId, string CreatedUser)
        {
            var date = DateTime.Now;
            var workingDay = await _context.WorkRecords.AsNoTracking()
                .Where(x => x.EmployeeId == CreatedId
                    && x.DateOnly == date.Date
                    && (x.WorkRecordStatus != Enums.Enums.WorkRecordStatusEnum.Checkin
                        || x.WorkRecordStatus != Enums.Enums.WorkRecordStatusEnum.Checkout)
                    && !x.IsDeleted)
                .ToListAsync();
            var lstCheckin = new List<WorkRecord>();
            foreach (var item in workingDay)
            {
                item.WorkRecordStatus = Enums.Enums.WorkRecordStatusEnum.Checkin;
                item.ModifiedTime = date;
                item.FromWorkDateEffect = date;
                item.ModifiedUser = CreatedUser;
                item.ModifiedId = CreatedId;
                lstCheckin.Add(item);
            }
            _context.WorkRecords.AddRange(lstCheckin);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> EmployeeCheckout(EmployeeCheckoutRequest request, Guid CreatedId, string CreatedUser)
        {
            var date = DateTime.Now;
            var workingDay = await _context.WorkRecords.AsNoTracking()
                .Where(x => x.EmployeeId == CreatedId
                    && (x.WorkRecordStatus != Enums.Enums.WorkRecordStatusEnum.Checkin
                        || x.WorkRecordStatus != Enums.Enums.WorkRecordStatusEnum.Checkout)
                    && x.DateOnly == date.Date
                    && !x.IsDeleted)
                .ToListAsync();
            var lstCheckout = new List<WorkRecord>();
            foreach (var item in workingDay)
            {
                var hourTemp = (item.ToWorkDateRegis - item.FromWorkDateRegis).TotalHours;
                var hourEffect = (date - (item.FromWorkDateEffect ?? date)).TotalHours;
                var hour = (hourEffect > hourTemp) ? hourTemp : hourEffect;
                item.WorkRecordStatus = Enums.Enums.WorkRecordStatusEnum.Checkout;
                item.ModifiedTime = date;
                item.ToWorkDateEffect = date;
                item.ModifiedUser = CreatedUser;
                item.ModifiedId = CreatedId;
                item.TotalHours = hour > 0 ? hour : 0;
                lstCheckout.Add(item);
            }
            _context.WorkRecords.AddRange(lstCheckout);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> EmployeeUpdateCalendar(UpdateWorkRecordRequest request, Guid CreatedId, string CreatedUser)
        {
            //get calendar after month exist in db
            var date = DateTime.Now;
            if (request.LstCalendar.Count <= 0)
                return false;
            var lstCalendarId = request.LstCalendar.Select(x => x.Id).ToList();
            var lstCalendarHaveId = await _context.WorkRecords
                .AsNoTracking()
                .Where(x =>
                    (!x.IsDeleted)
                    && (lstCalendarId.Any(y => y == x.Id)))
                .ToListAsync();
            var minDate = request.LstCalendar.Select(x => x.FromWorkDateRegis).Min();
            var maxDate = request.LstCalendar.Select(x => x.ToWorkDateRegis).Min();
            var employeeDB = await _context.AppUsers.FirstOrDefaultAsync(x => CreatedId == x.Id && !x.IsDeleted);
            if (employeeDB == null)
                throw new Exception("Not Found User");
            var lstCalendarDB = await _context.WorkRecords
                .AsNoTracking()
                .Where(x =>
                    (!x.IsDeleted)
                    && (CreatedId == x.EmployeeId)
                    && (x.FromWorkDateRegis.Date >= minDate.Date)
                    && (x.ToWorkDateRegis.Date <= maxDate.Date))
                .ToListAsync();
            if (request.LstCalendar.Count == 0)
                return true;
            var lstWorkeRecordAdd = new List<WorkRecord>();
            var lstWorkeRecordUp = new List<WorkRecord>();
            foreach (var item in request.LstCalendar)
            {
                // valid date form and to
                if (((item.ToWorkDateRegis - item.FromWorkDateRegis).TotalDays > 1)
                    || (item.ToWorkDateRegis <= item.FromWorkDateRegis))
                    continue;
                if (item.Id == null || item.Id == 0)
                {
                    var workRecord = lstCalendarDB.FirstOrDefault(x =>
                        (x.DateOnly.Date == item.ToWorkDateRegis.Date)
                        &&
                        (
                            // ca đăng kí sau không được móc xích với ca đăng kí trước
                            (
                                    ((x.FromWorkDateRegis < item.FromWorkDateRegis) && (x.ToWorkDateRegis > item.FromWorkDateRegis))
                                    || ((x.FromWorkDateRegis < item.ToWorkDateRegis) && (x.ToWorkDateRegis > item.ToWorkDateRegis))
                            )
                              // hoặc ca đăng kí sau không được chứa hoặc trùng ca đăng kí trước đó
                              || ((x.FromWorkDateRegis >= item.FromWorkDateRegis) && (x.ToWorkDateRegis <= item.ToWorkDateRegis))
                          )
                    );
                    if (workRecord != null) continue;
                    if (workRecord == null)
                    {
                        workRecord = _mapper.Map<WorkRecord>(item);
                        workRecord.CreatedId = CreatedId;
                        workRecord.CreatedUser = CreatedUser;
                        workRecord.WorkRecordStatus = Enums.Enums.WorkRecordStatusEnum.Register;
                        workRecord.DateOnly = item.ToWorkDateRegis.Date;
                        lstWorkeRecordAdd.Add(workRecord);
                    }
                }
                else
                {
                    var workRecordHaveId = lstCalendarHaveId.FirstOrDefault(x => x.Id == item.Id);
                    if (workRecordHaveId == null) continue;
                    _mapper.Map(item, workRecordHaveId);
                    workRecordHaveId.ModifiedId = CreatedId;
                    workRecordHaveId.ModifiedUser = CreatedUser;
                    workRecordHaveId.ModifiedTime = date;
                    lstWorkeRecordUp.Add(workRecordHaveId);
                }

            }
            _context.WorkRecords.AddRange(lstWorkeRecordAdd);
            _context.WorkRecords.AddRange(lstWorkeRecordUp);
            var res = await _context.SaveChangesAsync();

            return res > 0;
        }

        public async Task<PagedResult<WorkRecord>> GetAllPaging(GetWorkRecordPagingRequest request)
        {
            var query = _context.WorkRecords.Where(m => m.IsDeleted == false).AsNoTracking();
            if (request.LstWorkRecordStatus != null && request.LstWorkRecordStatus.Count > 0)
            {
                query = query.Where(x => x.WorkRecordStatus != null ? request.LstWorkRecordStatus.Any(y => y == x.WorkRecordStatus.Value) : false);
            }
            if (request.LstWorkType != null && request.LstWorkType.Count > 0)
            {
                query = query.Where(x => request.LstWorkType.Any(y => y == x.WorkType));
            }
            if (!string.IsNullOrEmpty(request.EmployeeName))
            {
                query = query.Where(x => x.EmployeeName.Contains(request.EmployeeName));
            }
            if (request.EmployeeId != null)
            {
                query = query.Where(x => x.EmployeeId == request.EmployeeId);
            }
            if (request.FromWorkDateRegis != null && request.ToWorkDateRegis != null)
            {
                query = query.Where(m => m.FromWorkDateRegis >= request.FromWorkDateRegis && m.ToWorkDateRegis <= request.ToWorkDateRegis);
            }


            query = query.OrderByDescending(x => x.DateOnly);

            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<Data.Entities.WorkRecord>()
            {
                TotalRecord = datapaging.TotalItems,
                Items = datapaging.Data,
            };
            await Task.CompletedTask;
            return pagedResult;
        }

        public async Task<WorkRecord> GetById(int id)
        {
            var item = await _context.WorkRecords.FindAsync(id);
            if (item == null) throw new Exception($"Cannot find a TableBL with id: {id}");
            return item;
        }

        public async Task<WorkRecord> Delete(int id)
        {
            var item = await _context.WorkRecords.FindAsync(id);

            if (item == null) throw new Exception($"Cannot find a TableBL with id: {id}");

            item.IsDeleted = true;
            _context.SaveChanges();
            return item;
        }
    }
}
