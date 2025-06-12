using Project.Data.Entities;
using Project.ViewModels;
using Project.ViewModels.BankConfig;

namespace Project.Services.Banks
{
    public interface IBankService
    {
        Task<PagedResult<BankViewModel>> GetAllPaging(GetBankPagingRequest request);

        Task<BankConfig> GetById(int id);
        Task<bool> AddNew(BankCreateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Update(BankUpdateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Delete(int id, Guid CreatedUserId, string CreatedUser);

        Task<string> GetQrCodeinBill(GetQrCodeRequest request);
    }
}
