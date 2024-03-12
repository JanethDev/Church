using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Church.Mobile.DataLayer.AuxiliaryModels;
using Church.Mobile.DataLayer.Models;
using Newtonsoft.Json;

namespace Church.Mobile.DataServices
{
    public class ApiClient
    {


        //Sends a GET request and returns a string
        public static async Task<string> GetString(string url)
        {
            string jResponseContent = "Exception";
            var uri = new Uri(url);
            HttpResponseMessage response = null;

            AuthDelegatingHandler authDelegatingHandler = new AuthDelegatingHandler();
            HttpClient client = new HttpClient(authDelegatingHandler);


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
                return jResponseContent;
            }
        }

        //Sends a GET request
        public static async Task<Response> Get(string url)
        {
            string jResponseContent = null;
            var uri = new Uri(url);
            HttpResponseMessage response = null;

            AuthDelegatingHandler authDelegatingHandler = new AuthDelegatingHandler();
            HttpClient client = new HttpClient(authDelegatingHandler);

            Response Response = new Response();

            try
            {
                response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    jResponseContent = await response.Content.ReadAsStringAsync();
                    Response = JsonConvert.DeserializeObject<Response>(jResponseContent);
                }
                else
                {
                    Response.Result = Result.ERROR_GETTING_DATA;
                }
                return Response;
            }
            catch (Exception ex)
            {
                Response.Result = Result.SERVICE_EXCEPTION;
                return Response;
            }
        }

        //Sends a POST request
        public static async Task<Response> Post(string url, object objContent)
        {
            Response Response = new Response();
            string jResponseContent = null;
            var uri = new Uri(url);
            var json = JsonConvert.SerializeObject(objContent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            AuthDelegatingHandler authDelegatingHandler = new AuthDelegatingHandler();
            HttpClient client = new HttpClient(authDelegatingHandler);

            try
            {
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    jResponseContent = await response.Content.ReadAsStringAsync();
                    Response = JsonConvert.DeserializeObject<Response>(jResponseContent);
                }
                else
                {
                    Response.Result = Result.ERROR_GETTING_DATA;
                }
            }
            catch (Exception ex)
            {
                Response.Result = Result.SERVICE_EXCEPTION;
            }
            return Response;
        }

        //Sends a PUT request
        public static async Task<Response> Put(string url, object objContent)
        {
            Response Response = new Response();
            string jResponseContent = null;

            var uri = new Uri(url);
            var json = JsonConvert.SerializeObject(objContent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            AuthDelegatingHandler authDelegatingHandler = new AuthDelegatingHandler();
            HttpClient client = new HttpClient(authDelegatingHandler);

            try
            {
                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    jResponseContent = await response.Content.ReadAsStringAsync();
                    Response = JsonConvert.DeserializeObject<Response>(jResponseContent);
                }
                else
                {
                    Response.Result = Result.ERROR_GETTING_DATA;
                }
            }
            catch (Exception ex)
            {
                Response.Result = Result.SERVICE_EXCEPTION;
            }

            return Response;
        }

    }
}
