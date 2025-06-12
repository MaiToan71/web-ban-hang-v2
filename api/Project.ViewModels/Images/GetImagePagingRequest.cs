using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels.Images
{
    public class GetImagePagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
