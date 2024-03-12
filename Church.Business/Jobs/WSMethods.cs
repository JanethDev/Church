using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Church.Business.Jobs
{
    public static class WSMethods
    {
        //Sends a GET request
        public static async Task<string> Get(string url)
        {
            string jResponseContent = null;
            var uri = new Uri(url);
            HttpResponseMessage response = null;
            //==========
            // Para que funcione con conexiones no seguras
            //==========
            var httpClientHandler = new HttpClientHandler();

            //httpClientHandler.ServerCertificateCustomValidationCallback =
            //(message, cert, chain, errors) => { return true; };
            //==========
            // END
            //==========
            HttpClient client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Add("Authorization", "801fc41d27cd2fc55eb3f47bd7b015f5");

            try
            {
                response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    jResponseContent = await response.Content.ReadAsStringAsync();
                }
                return jResponseContent;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //Sends a POST request
        public static async Task<string> Post(string url, object objContent)
        {
            string jResponseContent = null;
            var uri = new Uri(url);
            var json = JsonConvert.SerializeObject(objContent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            //==========
            // Para que funcione con conexiones no seguras
            //==========
            var httpClientHandler = new HttpClientHandler();

            //httpClientHandler.ServerCertificateCustomValidationCallback =
            //(message, cert, chain, errors) => { return true; };
            //==========
            // END
            //==========
            HttpClient client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Add("Authorization", "801fc41d27cd2fc55eb3f47bd7b015f5");
            try
            {
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    jResponseContent = await response.Content.ReadAsStringAsync();
                }
                return jResponseContent;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Sends a PUT request
        public static async Task<string> Put(string url, object objContent)
        {
            string jResponseContent = null;

            var uri = new Uri(url);
            var json = JsonConvert.SerializeObject(objContent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;
            //==========
            // Para que funcione con conexiones no seguras
            //==========
            var httpClientHandler = new HttpClientHandler();

            //httpClientHandler.ServerCertificateCustomValidationCallback =
            //(message, cert, chain, errors) => { return true; };
            //==========
            // END
            //==========
            HttpClient client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Add("Authorization", "801fc41d27cd2fc55eb3f47bd7b015f5");

            try
            {
                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    jResponseContent = await response.Content.ReadAsStringAsync();
                }
                return jResponseContent;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
