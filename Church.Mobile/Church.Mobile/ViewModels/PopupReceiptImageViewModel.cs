using Church.Mobile.DataLayer.ApiModels;
using Church.Mobile.DataLayer.AuxiliaryModels;
using Church.Mobile.DataLayer.Models;
using Church.Mobile.DataServices;
using Church.Mobile.Helpers;
using Church.Mobile.Interfaces;
using Church.Mobile.Localization;
using Church.Mobile.Views;
using Newtonsoft.Json;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Church.Mobile.ViewModels
{
    public class PopupReceiptImageViewModel : BaseViewModel
    {
        public string ReceiptImageSource { get; set; }
        public double ScreenHeight { get; set; }
        public tblPurchaseRequestPaymentsDTO tblPurchaseRequestPaymentsDTO { get; set; }

        public PopupReceiptImageViewModel(tblPurchaseRequestPaymentsDTO _tblPurchaseRequestPaymentsDTO)
        {
            IsBusy = false;
            tblPurchaseRequestPaymentsDTO = _tblPurchaseRequestPaymentsDTO;

            ReceiptImageSource = tblPurchaseRequestPaymentsDTO.ReceiptUrl;

            ScreenHeight = Application.Current.MainPage.Height / 2;
        }
        public ICommand ClosePopupCommand => new AsyncCommand(ClosePopup);

        async Task ClosePopup()
        {
            await PopupNavigation.PopAsync();
        }
    }
}
