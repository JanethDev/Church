using Church.Mobile.DataLayer.ApiModels;
using Church.Mobile.DataLayer.AuxiliaryModels;
using Church.Mobile.DataLayer.Models;
using Church.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Church.Mobile.DataServices
{
   public class PaymentsService
    {
        public string Url = Constants.WS_Url + "Payments/";

        public async Task<Response> CreateAsync(tblPayments model)
        {
            Response response = new Response();

            if (XPlatform.IsThereInternet)
            {
                response = await ApiClient.Post($"{Url}/Create", model);
                return response;
            }
            else
            {
                response.Result = Result.NETWORK_UNAVAILABLE;
            }
            return response;
        }
    }
}
