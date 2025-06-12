using Project.ViewModel.Usermanagers.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel.Usermanagers.Roles
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; }= string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<PermissionViewModel> Permissions { get; set; } = new List<PermissionViewModel>();
    }
}
