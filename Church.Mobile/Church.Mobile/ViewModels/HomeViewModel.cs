using Church.Mobile.DataLayer.ApiModels;
using Church.Mobile.DataLayer.AuxiliaryModels;
using Church.Mobile.DataLayer.Models;
using Church.Mobile.DataServices;
using Church.Mobile.Helpers;
using Church.Mobile.Localization;
using Church.Mobile.Views;
using Church.Mobile.Views.PurchaseRequest;
using Newtonsoft.Json;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Church.Mobile.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {

        private PurchasesRequestsService PurchasesRequestsService = new PurchasesRequestsService();

        public tblPurchasesRequestsDTO model { get; set; }
        public List<tblPurchasesRequestsDTO> Models { get; set; }

        public string Title { get; set; }

        public ICommand GoToCryptDetailsCommand { get; private set; }
        public ICommand LogOffCommand => new Command(() => LogOff());

        public HomeViewModel()
        {
            IsBusy = false;
            GetDataAsync();
            Title = $"Bienvenido {App.CurrentUser.FullName}";
            GoToCryptDetailsCommand = new Command<tblPurchasesRequestsDTO>(GoToCryptDetailsAsync);
        }

        #region Methods
        private void GoToCryptDetailsAsync(tblPurchasesRequestsDTO model)
        {
            //App.Current.MainPage.Navigation.PushAsync(new PurchaseRequestDetails(model));
            HelperMethods.OpenNewPage(new PurchaseRequestDetails(model), true);
        }


        public async void LogOff()
        {
            bool bResult = await Alerts.ShowConfirmationAlert("Notificación", "Cerrara sesión, ¿Desea continuar?");

            if (bResult)
            {
                App.UsersB.DeleteAll();
                HelperMethods.OpenNewPage(new Login());
            }
        }
        #endregion

        #region Api Request

        private async Task GetDataAsync()
        {
            IsBusy = true;

            var response = await PurchasesRequestsService.GetListByUserIdAndFilter(App.CurrentUser.UserID, 0, 0);

            if (response.Result != Result.NETWORK_UNAVAILABLE)
            {
                if (response.Result == Result.OK)
                {
                    Models = JsonConvert.DeserializeObject<List<tblPurchasesRequestsDTO>>(response.Data);
                }
                else
                {
                    Alerts.ShowAlert(string.Empty, response.Result.GetErrorMessage());
                }
            }
            else
            {
                Alerts.ShowNetworkError();
            }
        }
        #endregion
    }
}
