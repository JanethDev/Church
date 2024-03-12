using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class dtoNotificationsByUser
    {
        public int NotificationByUserID { get; set; }
        public int NotificationID { get; set; }
        public string Activity { get; set; }
        public int UserID { get; set; }
        public string NotificationKey { get; set; }
        public string Message { get; set; }
        public string Badge { get; set; }
        public List<int> lstUsers { get; set; }
    }
}
