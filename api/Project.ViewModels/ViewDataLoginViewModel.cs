using Project.ViewModel.Usermanagers.Users;

namespace Project.ViewModels
{

    public class ViewDataLoginViewModel
    {
        public bool Status { get; set; } = true;
        public string Token { get; set; } = string.Empty;
        public UserInfoViewModel Data { get; set; }
        public dynamic Error { get; set; } = string.Empty;
        public dynamic Message { get; set; } = string.Empty;
    }
}
