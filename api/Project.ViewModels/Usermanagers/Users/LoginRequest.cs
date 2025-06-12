namespace Project.ViewModels.Usermanagers.Users
{
    public class LoginGoogle
    {
        public string Name { get; set; }

        public string Email { get; set; }
        public string IdToken { get; set; }
        public string GoogleId { get; set; }
    }
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
