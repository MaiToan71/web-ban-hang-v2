using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class PagedResult<T>
    {
        public List<T> Items { set; get; } = new List<T>();
        public int TotalRecord { get; set; }
        public bool Status { get; set; } = true;
        public string Message { get; set; }= string.Empty;

    }
}
