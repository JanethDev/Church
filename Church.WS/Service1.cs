using Church.Business;
using Church.Data.Enums;
using Church.Data.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Church.WS
{
    public partial class Service1 : ServiceBase
    {
        private string CheckOutTimer;

        HelpMethods hm = new HelpMethods();
        FCMManager FCMManager = new FCMManager();
        PurchaseRequestPaymentsB PurchaseRequestPaymentsB = new PurchaseRequestPaymentsB();

        public Service1()
        {
            InitializeComponent();
            CheckOutTimer = ConfigurationManager.AppSettings["CheckOutTimer"];
        }

        internal void TestStartupAndStop(string[] args)
        {
            OnStart(args);
            Console.ReadLine();
        }


        protected override void OnStart(string[] args)
        {
            Task.Delay(GetTimeSpan(CheckOutTimer)).ContinueWith((x) => PaymentsNotifications());
            //PaymentsNotifications();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        private void PaymentsNotifications()
        {
            var vCurrentDate = hm.GetDateTime();
            vCurrentDate = vCurrentDate.AddDays(10);
            var vPagosProximos = PurchaseRequestPaymentsB.GetListForNotifications(vCurrentDate);

            if (vPagosProximos?.Any() == true)
            {
                foreach (var item in vPagosProximos)
                {
                    string sMessage = item.RemainingDaysToPay > 2 ? "Su próximo pago es en menos de 1 semana."
                        : item.RemainingDaysToPay > 0 && item.RemainingDaysToPay <= 2 ? "Su próximo pago es en menos de 2 días."
                        : "Su próximo pago es hoy.";

                    var vPushNotification = new PushNotification
                    {
                        Data = sMessage,
                        PushNotificationType = PushNotificationType.Message
                    };
                    var response = FCMManager.SendNotificationAsync(vPushNotification, item.UserNotificationKey);
                }
            }

            Task.Delay(GetTimeSpan(CheckOutTimer)).ContinueWith((x) => PaymentsNotifications());
        }


        private TimeSpan GetTimeSpan(string TimerHour)
        {
            var DailyTime = TimerHour;
            var timeParts = DailyTime.Split(new char[1] { ':' });

            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day,
                       int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));

            TimeSpan ts;
            if (date > dateNow)
                ts = date - dateNow;
            else
            {
                date = date.AddDays(1);
                ts = date - dateNow;
            }

            return ts;
        }

    }
}
