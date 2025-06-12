
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel.Usermanagers.Roles
{
    public class GetRolePagingRequest : PagingRequestBase
    {
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
    }
}
