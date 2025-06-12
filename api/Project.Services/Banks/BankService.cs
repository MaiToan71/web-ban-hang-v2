using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;

using Project.Utilities.Exceptions;
using Project.Utilities.Helper;
using Project.ViewModels;
using Project.ViewModels.BankConfig;


namespace Project.Services.Banks
{
    public class BankService : IBankService
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;

        public BankService(ProductDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddNew(BankCreateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = _mapper.Map<Data.Entities.BankConfig>(request);
            item.CreatedUser = CreatedUser;
            item.CreatedTime = DateTime.Now;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            item.CreatedId = CreatedUserId;
            _context.BankConfigs.Add(item);
            var res = await _context.SaveChangesAsync();

            return res > 0;
        }

        public async Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.BankConfigs.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {id}");

            item.IsDeleted = true;
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.BankConfigs.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<BankViewModel>> GetAllPaging(GetBankPagingRequest request)
        {
            //1. query

            var query = _context.BankConfigs
                .Where(m => m.IsDeleted == false).AsNoTracking();
            if (request.UserId != null)
            {
                query = query.Where(m => m.CreatedId == request.UserId);
            }
            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<BankViewModel>()
            {
                TotalRecord = datapaging.TotalItems,
                Items = datapaging.Data.Select(m => new BankViewModel
                {
                    Id = m.Id,
                    AccountName = m.AccountName,
                    BankId = m.BankId,
                    BankName = m.BankName,
                    AccountNo = m.AccountNo,
                    Template = m.Template,
                    Amount = m.Amount,
                    Description = m.Description,
                    QrCode = m.QrCode,

                }).ToList(),
            };

            await Task.CompletedTask;
            return pagedResult;
        }

        public async Task<Data.Entities.BankConfig> GetById(int id)
        {
            var item = await _context.BankConfigs.FindAsync(id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {id}");
            return item;
        }

        public async Task<string> GetQrCodeinBill(GetQrCodeRequest request)
        {
            //var paybill = await _context.PayBills.FirstOrDefaultAsync(m => m.Id == paybillId);
            //if (paybill == null)
            //{
            //    _logger.LogInformation("PaybillId Is Null");
            //    throw new Exception("PaybillId Is Null");
            //}
            var bank = await _context.BankConfigs.FindAsync(request.BankId);
            if (bank == null)
            {
                throw new Exception("BankId Is Null");
            }
            string qrCode = $"https://img.vietqr.io/image/{bank.BankId}-{bank.AccountNo}-compact2.jpg?amount={request.Amount}&addInfo=bill_{request.BillId}&accountName={bank.AccountName}";

            return qrCode;

        }





        public async Task<bool> Update(BankUpdateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var item = await _context.BankConfigs.FindAsync(request.Id);
            if (item == null) throw new ProductException($"Cannot find a Store with id: {request.Id}");
            _mapper.Map(request, item);
            item.ModifiedTime = DateTime.Now;
            item.ModifiedUser = CreatedUser;
            _context.BankConfigs.Update(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
