using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel.Usermanagers.Roles
{
    public class RoleCreateRequest
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}
