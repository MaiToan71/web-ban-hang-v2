using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Entities
{
    public class Role: BaseViewModel
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
        public List<AppUsermangersRole> AppUsermangersRoles { get; set; }

        public List<RoleMenu> RoleMenus { get; set; }

    }
}
