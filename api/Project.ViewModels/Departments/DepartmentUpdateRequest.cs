using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels.Departments
{
    public class DepartmentUpdateRequest
    {
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
