using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel.PostTypes
{
    public class PublicPostTypeViewModel
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public string Name { get; set; } = "";
        public string Title { get; set; } = "";
        public int ParentId { get; set; }
        public string Description { set; get; } = "";
        public string Note { set; get; } = "";

      //  public List<ImageViewModel> Banners { get; set; } = new List<ImageViewModel>();
      //  public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
        //public string Slug { get; set; }
    }
}
