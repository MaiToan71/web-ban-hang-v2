using Project.Data.Entities;

namespace Project.ViewModels.BankUsers
{
    public class BankUserViewModel
    {
        public AppUser User { get; set; } = new AppUser();
        public Project.Data.Entities.BankConfig BankConfig { get; set; } = new Project.Data.Entities.BankConfig();
        public BankUser BankUser { get; set; } = new BankUser();
    }
}
