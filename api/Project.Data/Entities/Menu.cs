using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Entities
{
    public class Menu : BaseViewModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }

        public int ParentId { get; set; } = 0;
        public List<RoleMenu> RoleMenus { get; set; }
    }
}
