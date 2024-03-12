using System;
using System.Collections.Generic;
using System.Text;

namespace Church.Mobile.DataLayer.AuxiliaryModels
{
    public class PushNotification
    {
        public PushNotificationType PushNotificationType { get; set; }
        public string Data { get; set; }
        public int Fragment { get; set; }
    }
}
