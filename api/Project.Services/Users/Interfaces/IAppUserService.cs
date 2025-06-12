using Project.ViewModel.Usermanagers.Users;
using Project.ViewModels;
using Project.ViewModels.TimeShifts;
using Project.ViewModels.TimeShiftTypes;
using Project.ViewModels.Usermanagers.Users;

namespace Project.Services.Users.Interfaces
{
    public interface IAppUserService
    {
        Task<dynamic> GetStatisticalJob(GetUserPagingRequest request);
        Task<string> AuthencateGoogle(LoginGoogle request);
        Task<ViewDataLoginViewModel> Authencate(LoginRequest request);

        Task<dynamic> Register(UserCreateRequest request);
        Task<bool> Update(UserUpdateRequest request);
        UserViewModel GetUserById(Guid userId);
        Task<UserViewModel> GetById(Guid userId);
        Task<PagedResult<UserViewModel>> GetAllPaging(GetUserPagingRequest request);
        Task<int> Delete(Guid id);
        Task<int> OpenUser(Guid id);
        Task<UserInfoViewModel> GetUserInfo(Guid userId);
        Task<bool> ClientUpdate(ClientUpdateRequest request);
        Task<ResponseViewModel> ClientChangePassword(ClientUpdatePasswordRequest request);
        Task<bool> UpdateWorkflow(UserUpdateworkdlowRequest request);
        Task<List<StatisticalTimeShiftTypeViewModel>> GetStatisticalTimeShiftType(GetUserPagingRequest request);
        Task<PagedResult<TimeShiftDTOViewModel>> GetListTimeShiftTags(GetUserPagingRequest request);
        Task<dynamic> GetStatisticalByDay(GetUserPagingRequest request);
        Task<ResponseViewModel> ChangePassword(UpdatePasswordRequest request);
    }
}
