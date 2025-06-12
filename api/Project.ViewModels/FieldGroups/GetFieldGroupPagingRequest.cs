using Project.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels.FieldGroups
{
    public class GetFieldGroupPagingRequest : PagingRequestBase
    {
        public string? Name { get; set; }

    }
}
