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
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Church.Mobile.ViewModels
{
    public class PurchaseRequestDetailsViewModel : BaseViewModel
    {
        public string FilterEstatus { get; set; }
        public bool PaymentsVisible { get; set; } = false;

        public ObservableCollection<tblPurchaseRequestPaymentsDTO> lstPayments { get; set; }

        public bool ShowAshDeposits { get; set; } = false;

        public bool ShowMessageNotAshDeposits => !ShowAshDeposits;

        public string LoggedUser { get; set; }

        private PurchasesRequestsService PurchasesRequestsService = new PurchasesRequestsService();

        public ICommand GoToCryptDetailsCommand => new Command(GoToCryptDetailsAsync);


        public bool HasOverduePayment
            => model.lstOverduePayments?.Any() == true;

        public bool HasNextPayment
            => model.tblNextPayments != null;

        public string TotalToPay { get; set; }
        public tblPurchasesRequestsDTO model { get; set; }
        public PurchaseRequestDetailsViewModel ThisBindingContext { get; set; }
        public PurchaseRequestDetailsViewModel(tblPurchasesRequestsDTO _model)
        {
            IsBusy = false;
            InitializeAsync(_model);
            _model.Rate = Global.Rate;
            Global.ID = _model.PurchasesRequestID;
            model = _model;
        }

        private async void InitializeAsync(tblPurchasesRequestsDTO _model)
        {
            Response response2 = await GetExchangeRateAsync();
            Global.Rate = response2.Decimal;
            _model.Rate = Global.Rate;
            GetDataAsync();
            LoggedUser = App.CurrentUser.FullName;
            ThisBindingContext = this;

            MessagingCenter.Subscribe<PopupMakePaymentViewModel, string>(this, App.NEW_PAYMENT, (sender, data) =>
            {
                GetDataAsync();
            });
        }

        private async Task<Response> GetExchangeRateAsync()
        {
            // Llamar a GetExchangeRate y esperar el resultado
            return await PurchasesRequestsService.GetExchangeRate();
        }


        #region Methods
        private int GetFilterEstatus()
        {
            int value = 0;
            switch (FilterEstatus)
            {
                case "Pagados":
                    value = 1;
                    break;
                case "Pendientes":
                    value = 2;
                    break;
            }
            return value;
        }
        private void GoToCryptDetailsAsync()
        {
            HelperMethods.OpenNewPage(new Login());
        }


        public void OnFilterEstatusChanged()
        {
            GetDataAsync();
        }
        #endregion

        #region Api Request

        private async Task GetDataAsync()
        {
            IsBusy = true;

            var response = await PurchasesRequestsService.GetListByUserIdAndFilter(App.CurrentUser.UserID, GetFilterEstatus(), Global.ID);

            if (response.Result != Result.NETWORK_UNAVAILABLE)
            {
                if (response.Result == Result.OK)
                {
                    try
                    {
                        var model = JsonConvert.DeserializeObject<List<tblPurchasesRequestsDTO>>(response.Data);
                        lstPayments = new ObservableCollection<tblPurchaseRequestPaymentsDTO>(model[0].lstPayments ?? new List<tblPurchaseRequestPaymentsDTO>());

                        if (GetFilterEstatus() != 1)
                            TotalToPay = (lstPayments?.Where(r => r.Estatus != PaymentEstatus.Paid).Sum(r => r.Amount) ?? 0).ToString("C2");

                        PaymentsVisible = model[0].PaymentMethod == PaymentMethods.Meses12 ||
                                          model[0].PaymentMethod == PaymentMethods.Meses18 ||
                                          model[0].PaymentMethod == PaymentMethods.Meses24 ||
                                          model[0].PaymentMethod == PaymentMethods.Meses36 ||
                                          model[0].PaymentMethod == PaymentMethods.Meses48;

                        ShowAshDeposits = model[0].lstAshDeposits?.Any() == true;
                    }
                    catch (JsonException ex)
                    {
                        // Manejar la excepción de deserialización aquí
                        Console.WriteLine("Error de deserialización: " + ex.Message);
                    }
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