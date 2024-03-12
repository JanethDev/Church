using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class dtoPushNotificationsMessages
    {
        public int PushNotificationMessageID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Draft { get; set; }
        public string Estatus { get; set; }
        public int TotalCount { get; set; }
    }
}
