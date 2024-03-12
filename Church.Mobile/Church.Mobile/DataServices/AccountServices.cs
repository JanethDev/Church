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
   public class AccountServices
    {
        public string Url = Constants.WS_Url + "Account/";

        public async Task<Response> UserLoginAsync(tblLogin Credentials)
        {
            Response response = new Response();

            if (XPlatform.IsThereInternet)
            {
                response = await ApiClient.Post($"{Url}user/Login", Credentials);
                return response;
            }
            else
            {
                response.Result = Result.NETWORK_UNAVAILABLE;
            }
            return response;
        }

        public async Task<Response> UpdateNotificationKeys(int UserID, tblUsersDTO Patient)
        {
            Response response = new Response();

            if (XPlatform.IsThereInternet)
            {
                response = await ApiClient.Put($"{Url}UpdateNotificationKeys/{UserID}", Patient);
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
