namespace Project.ViewModels.TimeShiftTypes
{
    public class TimeShiftTypeViewModel
    {
        public Project.Data.Entities.TimeShiftType TimeShiftType { get; set; } = new Project.Data.Entities.TimeShiftType();
        public Project.Data.Entities.AppUser UserViewModel { get; set; } = new Project.Data.Entities.AppUser();
        public int TotalTask { get; set; } = 0;
        public int TaskDone { get; set; } = 0;

        public List<Project.Data.Entities.TimeShift> TimeShifts { get; set; } = new List<Project.Data.Entities.TimeShift>();
    }

    public class StatisticalTimeShiftTypeViewModel
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public int TotalTimeShift { get; set; } = 0;
        public int TotalDone { get; set; } = 0;
    }
}
