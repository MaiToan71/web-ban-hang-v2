using Project.ViewModel.PostTypes;
using Project.ViewModels.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class PostTypeImageViewModel
    {
        public int Id { get; set; } 
        public int PostTypeId { get; set; }
        public PostTypeViewModel PostTypeViewModel { get; set; }    
        public int ImageId { get; set; }
        public ImageViewModel ImageViewModel { get; set; }  
    }
}
