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
    public class PopupMakePaymentViewModel : BaseViewModel
    {
        public PaymentsService PaymentsService { get; set; }
        public ImageSource SelectedImageSource { get; set; }
        public double ScreenHeight { get; set; }
        public Stream StreamFile { get; set; }
        public tblPurchaseRequestPaymentsDTO tblPurchaseRequestPaymentsDTO { get; set; }
        public byte[] FileArray { get; set; }

        public PopupMakePaymentViewModel(tblPurchaseRequestPaymentsDTO _tblPurchaseRequestPaymentsDTO)
        {
            IsBusy = false;
            tblPurchaseRequestPaymentsDTO = _tblPurchaseRequestPaymentsDTO;
            PaymentsService = new PaymentsService();
        }


        public ICommand PickPhotoCommand => new AsyncCommand(PickPhoto2);
        public ICommand UploadTicketCommand => new AsyncCommand(UploadTicket);


        async Task PickPhoto2()
        {
            SelectedImageSource = null;
            ScreenHeight = 0;
            var photo = await MediaPicker.PickPhotoAsync();

            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (StreamFile = await photo.OpenReadAsync())
            {
                if (StreamFile != null)
                {
                    byte[] buffer = new byte[16 * 1024];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = await StreamFile.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        FileArray = ms.ToArray();
                    }

                    Image image = new Image();
                    Stream stream = new MemoryStream(FileArray);

                    SelectedImageSource = ImageSource.FromStream(() => stream);
                    ScreenHeight = Application.Current.MainPage.Height / 2;
                }
            }

        }

        async Task PickPhoto()
        {

        }

        async Task UploadTicket()
        {
            if (SelectedImageSource == null)
            {
                Alerts.ShowAlert("Notificación", "Seleccione una foto para poder guardar.");
                return;
            }
            await Alerts.ShowLoadingPageAsync();
            tblPayments tblPayment = new tblPayments();
            tblPayment.FileBytes = FileArray;
            tblPayment.CreatedBy = tblPayment.UpdatedBy = App.CurrentUser.UserID;
            tblPayment.PurchaseRequestPaymentID = tblPurchaseRequestPaymentsDTO.PurchaseRequestPaymentID;
            tblPayment.IsInterest = tblPurchaseRequestPaymentsDTO.IsOverduePayment;
            tblPayment.Amount = tblPurchaseRequestPaymentsDTO.Amount;
            tblPayment.ExchangeRate = Global.Rate;
            var response = await PaymentsService.CreateAsync(tblPayment);

            await Alerts.HideLoadingPageAsync();
            if (response.Result == Result.OK)
            {
                MessagingCenter.Send(this, App.NEW_PAYMENT, "");
                Alerts.ShowAlert("Notificación", "Se subio correctamente el ticket, ahora se encuentra en proceso de validación.");
                await PopupNavigation.PopAsync();
            }
            else
            {
                Alerts.ShowAlert("Notificación", "Ocurrio un error al procesar la solicitud, intente de nuevo.");
            }
        }
    }
}
