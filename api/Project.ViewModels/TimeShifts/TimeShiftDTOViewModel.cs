using Project.Data.Entities;
using Project.ViewModel.Usermanagers.Users;

namespace Project.ViewModels.TimeShifts
{
    public class TimeShiftDTOViewModel
    {
        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
        public TimeShift TimeShift { get; set; } = new TimeShift();
        public UserViewModel User { get; set; } = new UserViewModel();
        public TimeShiftType TimeShiftType { get; set; } = new TimeShiftType();
    }
}
