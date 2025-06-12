using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Entities
{
    public class PostImage
    {
        public int PostId { get; set; }

        public Post Posts { get; set; }

        public int ImageId { get; set; }
        public Image Images { get; set; }
    }
}
