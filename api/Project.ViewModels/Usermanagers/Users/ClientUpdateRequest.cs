namespace Project.ViewModel.Usermanagers.Users
{
    public class ClientUpdateRequest
    {
        public Guid? UserId { get; set; }
        public string FullName { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class ClientUpdatePasswordRequest
    {
        public Guid? UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class UpdatePasswordRequest
    {
        public Guid? UserId { get; set; }
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
