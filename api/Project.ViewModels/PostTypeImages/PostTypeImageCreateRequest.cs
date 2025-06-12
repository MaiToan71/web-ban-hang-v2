using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class PostTypeImageCreateRequest
    {
        public int PostTypeId { get; set; }
        public int ImageId { get; set; }
    }
}
