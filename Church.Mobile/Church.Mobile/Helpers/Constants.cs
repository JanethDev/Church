using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Church.Mobile.Helpers
{

    public static class Constants
    {
#if RELEASE

  public static string WS_Url = "https://pruebas.grupolan.com/churchapi/api/";

      
#else

        public static string WS_Url = "https://pruebas.grupolan.com/churchapi/api/";
        //public static string WS_Url = "http://192.168.0.192:45455/api/";
#endif

        public static string GCM_Notification_Url = "https://android.googleapis.com/gcm/notification";
        public static string GCM_Notification_Url_Verify = "https://fcm.googleapis.com/fcm/notification?notification_key_name=";
        public static string GM_Directions_Url = "https://maps.googleapis.com/maps/api/directions/json?origin={0}&destination={1}&key={2}";

        #region Colors
        public static Color ColorPrimary = Color.FromArgb(204, 163, 105);
        public static Color ColorWhite = Color.FromArgb(255, 255, 255);

        public static Color ColorRed = Color.FromArgb(153, 0, 0);
        public static Color ColorGreen = Color.FromArgb(0, 128, 0);
        public static Color ColorGray = Color.FromArgb(169, 169, 169);
        public static Color ColorBlue = Color.FromArgb(128, 128, 255);
        #endregion
    }

}
