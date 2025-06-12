using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Entities
{
    public class PostTypeImage
    {
        public int PostTypeId { get; set; }
        public PostType PostType { get; set; }

        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
