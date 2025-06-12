using Project.Enums;

namespace Project.Data.Entities
{
    public class SocialToken : BaseViewModel
    {
        public Platform Platform { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
    }
}
