using Project.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Entities
{
    public class NotificationUser
    {

        public Guid UserId { get; set; }

        public AppUser AppUser { get; set; }

        public int NotificationId { get; set; }

        public Notification Notification { get; set; }

        public bool IsRead { get; set; }    = false;
    }

}
