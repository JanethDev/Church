
using Church.Mobile.DataLayer.AuxiliaryModels;
using Church.Mobile.DataServices;
using Church.Mobile.Helpers;
using Church.Mobile;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.FirebasePushNotification;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Church.Mobile.DataLayer.ApiModels;

namespace DentalHealthApp.DataServices
{
public class FirebaseManagerService
{
private string NOT_FOUND = "notification_key not found";
        private AccountServices AccountService = new AccountServices();

        private string GetFCMKeyPrefix()
        {
            if (Device.RuntimePlatform.Equals(Device.Android))
            {
                return "dh.android.beta.2.0";
            }
            else if (Device.RuntimePlatform.Equals(Device.iOS))
            {
                return "dh.ios.beta.2.0";
            }
            return "dh.none.beta.2.0";
        }

        //Creates a new group of devices for the current user
        public async void CreateDeviceGroup(string deviceToken)
        {
            if (App.UsersB.Get() != null)
            {
                var jGcmData = new JObject();

                JArray ja_registrationids = new JArray
                {
                    deviceToken
                };
                string UniqueKey = Guid.NewGuid().ToString();
                string notificationKeyName = string.Format("{0}{1}{2}", GetFCMKeyPrefix(), App.UsersB.Get().UserID, UniqueKey);
                jGcmData.Add("operation", "create");
                jGcmData.Add("notification_key_name", notificationKeyName);
                jGcmData.Add("registration_ids", ja_registrationids);


                var uri = new Uri(Constants.GCM_Notification_Url);


                var content = new StringContent(jGcmData.ToString(), Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", await SecureStorage.GetAsync("gcm.apikeyserver"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("project_id", await SecureStorage.GetAsync("gcm.project_id"));
                //var dsd = await SecureStorage.GetAsync("gcm.project_id");
                HttpResponseMessage response = await client.PostAsync(uri, content);

                string jResponseContent = await response.Content.ReadAsStringAsync();
                try
                {
                    NotificationErrors nkResponseRequest = JsonConvert.DeserializeObject<NotificationErrors>(jResponseContent);
                    if (!string.IsNullOrEmpty(nkResponseRequest.notification_key))
                    {
                        tblUsersDTO currentUser = App.UsersB.Get();
                        currentUser.NotificationKey = nkResponseRequest.notification_key;
                        currentUser.NotificationKeyName = string.Format("{0}{1}{2}", GetFCMKeyPrefix(), App.UsersB.Get().UserID, UniqueKey);
                        App.UsersB.Update(currentUser);
                        //var json = JsonConvert.SerializeObject(currentUser);
                        //content = new StringContent(json, Encoding.UTF8, "application/json");
                        string Json = JsonConvert.SerializeObject(currentUser);
                        var nmresponse = await AccountService.UpdateNotificationKeys(App.UsersB.Get().UserID, currentUser);
                        if (nmresponse.Result == Result.OK)
                        {

                        }
                    }
                    client.Dispose();
                }
                catch (Exception ex)
                {
                    client.Dispose();
                }
            }
        }

        //Retrieving a notification key
        public async System.Threading.Tasks.Task<bool> ValidNotificationKeyNameAsync(string notification_key_name)
        {
            string jResponseContent = null;

            try
            {
                using (var client = new System.Net.WebClient())
                {
                    client.Headers.Add("Accept", "application/json");
                    client.Headers.Add("Content-Type", "application/json; charset=utf-8");
                    client.Headers.Add("Authorization", await SecureStorage.GetAsync("gcm.apikeyserver"));
                    client.Headers.Add("project_id", await SecureStorage.GetAsync("gcm.project_id"));
                    jResponseContent = client.DownloadString(Constants.GCM_Notification_Url_Verify + notification_key_name);
                }


                NotificationErrors nkResponseRequest = JsonConvert.DeserializeObject<NotificationErrors>(jResponseContent);

                if (!string.IsNullOrEmpty(nkResponseRequest.error) && nkResponseRequest.error.Contains(NOT_FOUND))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        //Adds the current device to an specific group of devices
        public void AddDeviceToGroup(string DeviceToken)
        {
            GCM_Managing_Devices(DeviceToken, "add");
        }

        //Removes the current device of a group of devices
        public void RemoveDeviceFromGroup(string DeviceToken)
        {
            GCM_Managing_Devices(DeviceToken, "remove");
        }

        //Sends a POST request that provides a name for the group, and a list of registration tokens for the devices and the action. 
        private async void GCM_Managing_Devices(string DeviceToken, string Action)
        {
            var jGcmData = new JObject();

            JArray ja_registrationids = new JArray
            {
                DeviceToken
            };
            jGcmData.Add("operation", Action);
            jGcmData.Add("notification_key_name", App.UsersB.Get().NotificationKeyName);
            jGcmData.Add("notification_key", App.UsersB.Get().NotificationKey);
            jGcmData.Add("registration_ids", ja_registrationids);

            var uri = new Uri(Constants.GCM_Notification_Url);

            var json = JsonConvert.SerializeObject(jGcmData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", await SecureStorage.GetAsync("gcm.apikeyserver"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("project_id", await SecureStorage.GetAsync("gcm.project_id"));

            response = await client.PostAsync(uri, content);
            string jResponseContent = await response.Content.ReadAsStringAsync();
            try
            {
                NotificationErrors nkResponseRequest = JsonConvert.DeserializeObject<NotificationErrors>(jResponseContent);
                if (!string.IsNullOrEmpty(nkResponseRequest.error) && nkResponseRequest.error.Contains(NOT_FOUND))
                {
                    CreateDeviceGroup(DeviceToken);
                }
                client.Dispose();
            }
            catch (Exception ex)
            {
                client.Dispose();
            }
        }

        public Response SendRegistrationToServerAsync()
        {
            Response Response = new Response();
            Response.Result = Result.OK;
            try
            {
                var token = CrossFirebasePushNotification.Current.Token;
                tblUsersDTO current_user = App.CurrentUser;
                if (current_user != null && !string.IsNullOrEmpty(token))
                {
                    if (string.IsNullOrEmpty(current_user.NotificationKey))
                    {
                        CreateDeviceGroup(token);
                    }
                    else
                    {
                        if (ValidNotificationKeyNameAsync(current_user.NotificationKeyName).Result)
                        {
                            AddDeviceToGroup(token);
                        }
                        else
                        {
                            CreateDeviceGroup(token);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Result = Result.EXCEPTION;
                Response.Data = ex != null ? ex.Message : "Error al generar FCM Key";
            }
            return Response;
        }


        public Response DeleteTokenFromServerAsync()
        {
            Response Response = new Response
            {
                Result = Result.OK
            };

            return Response;
        }
    }
}
