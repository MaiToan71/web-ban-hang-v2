using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Project.ViewModels.Images
{
    public class ImageCreateRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public List<int> CategoryIds { get; set; } = new List<int>();
        public IFormFile? ImageFile { get; set; }
    }
}
