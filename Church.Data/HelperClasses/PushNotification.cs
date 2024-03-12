using Church.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data.HelperClasses
{
    public class PushNotification
    {
        public PushNotificationType PushNotificationType { get; set; }
        public string Data { get; set; }
    }
}
