using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Entities
{
    public class AppUsermangersRole
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
