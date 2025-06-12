using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class PostTypeImageUpdateRequest
    {
        public int Id { get; set; } 
        public int PostTypeId { get; set; }
        public int ImageId { get; set; }
    }
}
