using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public string Action { get; set; }

        public string Detail { get; set; }

        public int PostIdRelate { get; set; }

        public int UserIdRelate { get; set; }

        public Guid UserId { get; set; }

        public AppUser User { get; set; }
    }
}
