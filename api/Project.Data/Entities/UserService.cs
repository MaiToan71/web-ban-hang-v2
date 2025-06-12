namespace Project.Data.Entities
{
    public class UserService : BaseViewModel
    {
        public int ServiceId { get; set; }
        public Guid UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
