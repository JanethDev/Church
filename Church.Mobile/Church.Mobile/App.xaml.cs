using Church.Mobile.BusinessLayer;
using Church.Mobile.DataLayer.ApiModels;
using Church.Mobile.Helpers;
using Church.Mobile.Interfaces;
using Church.Mobile.Views;
using DentalHealthApp.DataServices;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotifications;
using SQLite;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Church.Mobile
{
    public partial class App : Application
    {
        private readonly SQLiteConnection dbConnection;
        public static UsersB UsersB { get; set; }
        public static tblUsersDTO CurrentUser { get { return UsersB.Get(); } }

        public static FirebaseManagerService FirebaseManagerService { get; set; }

        static readonly string CHANNEL_NAME = "ChurchChannel";
        public const string NEW_PAYMENT = nameof(NEW_PAYMENT);
        public static bool IsInForeground { get; set; } = false;

        public App()
        {
            InitializeComponent();
            try
            {
                SecureStorage.SetAsync("gl.appid", "99CD966A-2F58-47B2-AE92-A20B893EF540");
                SecureStorage.SetAsync("gl.appkey", "teZsT/JUxswcxs4ygO12CrfmtsbndrXqWpE7whE0xrE=");
                SecureStorage.SetAsync("gcm.apikeyserver", "key=AAAAWWISPXo:APA91bF5iCrZyb_PBFLo3HeuWVBVwNN_fsOV1IMnx66kxsPvTPBNnphSS0VCQw6DNfv_RJMm1HnLIWSwjPfNIYYyvV-XS5Qn4UcaDcdaQlwaU3el2jkjP9J-c4fq30mFILLeD9gLkWvb");
                SecureStorage.SetAsync("gcm.project_id", "383897451898");
            }
            catch (Exception ex)
            {

            }

            dbConnection = DependencyService.Get<ISQLite>().GetConnection();

            FirebaseManagerService = new FirebaseManagerService();
            UsersB = new UsersB(dbConnection);

            if (CurrentUser != null)
            {
                HelperMethods.OpenNewPage(new Home());
            }
            else
            {
                HelperMethods.OpenNewPage(new Login());
            }
        }

        protected override void OnStart()
        {
            IsInForeground = true;
            // Handle when your app starts

            CrossFirebasePushNotification.Current.Subscribe(CHANNEL_NAME);
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                var ds = CrossFirebasePushNotification.Current.Token;
                FirebaseManagerService.SendRegistrationToServerAsync();
                System.Diagnostics.Debug.WriteLine($"TOKEN: {CrossFirebasePushNotification.Current.Token}");
            };
            System.Diagnostics.Debug.WriteLine($"TOKEN: {CrossFirebasePushNotification.Current.Token}");

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    if (p.Data.ContainsKey("body"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var Message = $"{p.Data["body"]}";
                            SendNotificationOnlyMessage($"{p.Data["body"]}");
                        });

                    }

                    if (p.Data.ContainsKey("message"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var Message = $"{p.Data["message"]}";
                            SendNotificationOnlyMessage($"{p.Data["message"]}");
                        });

                    }
                }
                catch (Exception ex)
                {

                }
            };
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            IsInForeground = false;
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            IsInForeground = true;
        }

        private void SendNotificationOnlyMessage(string message)
        {
            CrossLocalNotifications.Current.Show("Parroquia", message);
        }
    }
}
