using System.ComponentModel.DataAnnotations;

namespace Project.Data.Entities
{
    public class BaseViewModel
    {
        [Key]
        public int Id { get; set; }

        public Guid CreatedId { get; set; } = Guid.NewGuid();
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string CreatedUser { get; set; } = "";
        public DateTime ModifiedTime { get; set; } = DateTime.Now;
        public string ModifiedUser { get; set; } = "";
        public Guid ModifiedId { get; set; } = Guid.NewGuid();
        public bool IsDeleted { get; set; } = false;
    }


}
