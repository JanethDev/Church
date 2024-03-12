using Church.Mobile.DataLayer.AuxiliaryModels;
using Church.Mobile.DataLayer.Models;
using Church.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Church.Mobile.DataServices
{
    public class PurchasesRequestsService
    {
        public string Url = Constants.WS_Url + "PurchaseRequest/";

        public async Task<Response> GetListByUserIdAndFilter(int UserId, int Filter, int PurReq)
        {
            Response response = new Response();

            if (XPlatform.IsThereInternet)
            {
                response = await ApiClient.Get($"{Url}GetByUserId/{UserId}/{Filter}/{PurReq}");
                return response;
            }
            else
            {
                response.Result = Result.NETWORK_UNAVAILABLE;
            }
            return response;
        }

        public async Task<Response> GetExchangeRate()
        {
            Response response = new Response();

            if (XPlatform.IsThereInternet)
            {
                response = await ApiClient.Get($"{Url}GetExchangeRate");
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
