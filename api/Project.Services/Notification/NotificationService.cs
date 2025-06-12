using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Utilities.Exceptions;
using Project.Utilities.Helper;
using Project.ViewModel.Usermanagers.Users;
using Project.ViewModels;
using Project.ViewModels.Notification;

namespace Project.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly ProductDbContext _context; private readonly IMapper _mapper;
        public NotificationService(ProductDbContext context, IMapper mapper)
        {
            _context = context; _mapper = mapper;
        }
        public async Task<bool> AddNew(NotificationCreateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = _mapper.Map<Data.Entities.Notification>(request);
            item.CreatedUser = CreatedUser;
            item.CreatedTime = DateTime.Now;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.Notifications.Add(item);
            var inputUsers = new List<Data.Entities.NotificationUser>();
            var res = await _context.SaveChangesAsync();

            foreach (var user in request.Users)
            {
                var input = new Data.Entities.NotificationUser
                {
                    UserId = user,
                    NotificationId = item.Id,
                    IsRead = false
                };
                inputUsers.Add(input);
            }
            _context.NotificationUsers.AddRange(inputUsers);
            _context.SaveChanges();
            return res > 0;
        }

        public async Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Notifications.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Item with id: {id}");

            item.IsDeleted = true;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.Notifications.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<NotificationViewModel>> GetAllPaging(GetNotificationPagingRequest request)
        {
            //1. query
            var query = _context.Notifications.Where(m => m.IsDeleted == false).AsNoTracking();

            //2. search 
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(m => m.Name.Contains(request.Search) || m.Message.Contains(request.Search));
            }
            if (request.Workflow != null)
            {
                query = query.Where(m => m.Workflow == request.Workflow);
            }
            if (request.UserId != null)
            {
                var notiUserItems = await _context.NotificationUsers.Where(m => m.UserId == request.UserId).ToListAsync();
                query = query.Where(m => notiUserItems.Select(i => i.NotificationId).Contains(m.Id)).OrderByDescending(m => m.ScheduleDate);
            }
            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);
            var data = new List<NotificationViewModel>();

            var notiUsers = _context.NotificationUsers.Where(m => datapaging.Data.Select(i => i.Id).Contains(m.NotificationId)).ToList();
            var users = _context.AppUsers.Where(m => notiUsers.Select(u => u.UserId).Contains(m.Id)).ToList();
            foreach (var item in datapaging.Data)
            {
                var notiMapping = notiUsers.Where(m => m.NotificationId == item.Id);
                var userNapping = users.Where(m => notiMapping.Select(n => n.UserId).Contains(m.Id)).Select(m => new UserViewModel()
                {
                    UserId = m.Id,
                    Id = m.Id,
                    FullName = m.FullName,
                    UserName = m.UserName
                }).ToList();
                var obj = new NotificationViewModel()
                {
                    Notification = item,
                    Users = userNapping
                };
                data.Add(obj);
            }
            //4 .Select and projecttion
            var pagedResult = new PagedResult<NotificationViewModel>()
            {
                TotalRecord = datapaging.TotalItems,
                Items = data,
            };
            await Task.CompletedTask;
            return pagedResult; ;
        }

        public async Task<bool> Update(NotificationUpdateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.Notifications.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a Item with id: {request.Id}");
            _mapper.Map(request, item);
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            var notificationUsers = await _context.NotificationUsers.Where(m => m.NotificationId == item.Id).ToListAsync();
            if (notificationUsers.Count() > 0)
            {
                _context.NotificationUsers.RemoveRange(notificationUsers);
                _context.SaveChanges();
            }

            var inputUsers = new List<Data.Entities.NotificationUser>();
            foreach (var user in request.Users)
            {
                var input = new Data.Entities.NotificationUser
                {
                    UserId = user,
                    NotificationId = item.Id,
                    IsRead = false
                };
                inputUsers.Add(input);
            }
            _context.NotificationUsers.AddRange(inputUsers);
            _context.Notifications.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
