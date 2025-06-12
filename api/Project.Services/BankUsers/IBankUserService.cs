using Project.ViewModels;
using Project.ViewModels.BankUsers;

namespace Project.Services.BankUsers
{
    public interface IBankUserService
    {
        Task<PagedResult<BankUserViewModel>> GetAllPaging(GetBankUserPagingRequest request);

        Task<bool> AddNew(BankUserCreateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<bool> Update(BankUserUpdateRequest request, Guid CreatedUserId, string CreatedUser);

        Task<bool> UpdateWorkflow(BankUserUpdateWorkflowRequest request, Guid CreatedUserId, string CreatedUser);


    }
}
