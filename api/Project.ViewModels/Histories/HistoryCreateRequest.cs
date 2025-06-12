using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class HistoryCreateRequest
    {
        public string Action { get; set; }

        public string Detail { get; set; }

        public int PostIdRelate { get; set; }

        public int UserIdRelate { get; set; }

        
    }
}
