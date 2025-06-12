using Project.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class GetHistoryPagingRequest : PagingRequestBase
    {
        public bool? IsAll { get; set; }
        public bool? SortOrder { get; set; }
        public string? Name { get; set; }
        public Status? Status { get; set; }
        public Guid? UserId { get; set; } = Guid.Empty; 
    }
}
