using Project.Enums;
using Project.ViewModel.Usermanagers.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels.Menus
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; } = 0;
        public string Name { set; get; }
        public string Url { set; get; }
        public int SortOrder { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
