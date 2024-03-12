using Church.Data.Enums;
using Church.Data.HelperClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Church.Business
{
    public class FCMManager
    {
        private string ApiKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data">Required</param>
        /// <param name="Token">Required if only one if notification is for one user</param>
        /// <param name="Tokens">Required if notification is for more than one user</param>
        /// <returns></returns>
        public async Task<bool> SendNotificationAsync(PushNotification Data, string Token)
        {
            ApiKey = WebConfigHelper.FCMApiKey;
            var jGcmData = new JObject();
            var jData = new JObject();
            var jNotification = new JObject();

            jData.Add("PushNotification", JsonConvert.SerializeObject(Data));

            jGcmData.Add("to", Token);

            jGcmData.Add("priority", "high");
            jGcmData.Add("data", jData);


            if (Data.PushNotificationType == PushNotificationType.Message)
            {
                jNotification.Add("title", "Parroquia");
                jNotification.Add("body", Data.Data);
            }
            jNotification.Add("sound", "Sounds/zsound.caf");
            jNotification.Add("content_available", true);
            jNotification.Add("icon", "ic_notification");
            jGcmData.Add("notification", jNotification);

            var url = new Uri("https://fcm.googleapis.com/fcm/send");
            try
            {

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + ApiKey);
                    var json = jGcmData.ToString();
                    var ddd = await client.PostAsync(url, new StringContent(json, Encoding.Default, "application/json"));
                    string jResponseContent = await ddd.Content.ReadAsStringAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to send GCM message:");
                Console.Error.WriteLine(e.StackTrace);
                return false;
            }
        }
    }
}
