using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Utilities.Exceptions;
using Project.Utilities.Helper;
using Project.ViewModels;
using Project.ViewModels.BankUsers;

namespace Project.Services.BankUsers
{
    public class BankUserService : IBankUserService
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;

        public BankUserService(ProductDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AddNew(BankUserCreateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = _mapper.Map<Data.Entities.BankUser>(request);
            item.CreatedUser = CreatedUser;
            item.CreatedTime = DateTime.Now;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            item.CreatedId = CreatedUserId;
            _context.BankUsers.Add(item);
            var res = await _context.SaveChangesAsync();

            return res > 0;
        }


        public async Task<PagedResult<BankUserViewModel>> GetAllPaging(GetBankUserPagingRequest request)
        {
            //1. query

            var query = _context.BankUsers
                .Where(m => m.IsDeleted == false).OrderByDescending(m => m.CreatedTime).AsNoTracking();
            if (request.Workflow != null)
            {
                query = query.Where(m => m.Workflow == request.Workflow);
            }
            if (request.UserId != null)
            {
                query = query.Where(m => m.UserId == request.UserId);
            }
            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<BankUserViewModel>()
            {
                TotalRecord = datapaging.TotalItems,


            };
            var banks = await _context.BankConfigs.Where(m => datapaging.Data.Select(d => d.BankId).Contains(m.Id)).ToListAsync();
            var users = await _context.AppUsers.Where(m => datapaging.Data.Select(d => d.UserId).Contains(m.Id)).ToListAsync();
            foreach (var item in datapaging.Data)
            {
                var input = new BankUserViewModel();
                var user = users.FirstOrDefault(m => m.Id == item.UserId);
                if (user != null)
                {
                    user.PasswordHash = "";
                    input.User = user;
                }
                var bank = banks.FirstOrDefault(m => m.Id == item.BankId);
                if (bank != null)
                {
                    input.BankConfig = bank;
                }
                input.BankUser = item;
                pagedResult.Items.Add(input);
            }

            await Task.CompletedTask;
            return pagedResult;
        }



        public async Task<bool> Update(BankUserUpdateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.BankUsers.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {request.Id}");
            _mapper.Map(request, item);
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.BankUsers.Update(item);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateWorkflow(BankUserUpdateWorkflowRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.BankUsers.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {request.Id}");
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            item.Workflow = request.Workflow;

            // var balanceUser = await _context.BankUsers.Where(m => m.UserId == item.UserId && m.Workflow == Enums.Workflow.COMPLETED).Select(m => m.Amount).SumAsync();
            var user = await _context.AppUsers.FirstOrDefaultAsync(m => m.Id == item.UserId);
            if (user != null)
            {
                user.Balance -= (float)item.Amount;
                user.Amount -= (float)item.Amount;
            }

            //   _context.BankUsers.Update(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
