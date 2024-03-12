using Church.Mobile.DataLayer.ApiModels;
using Church.Mobile.ViewModels;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Church.Mobile.Views.PurchaseRequest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabPayments : ContentPage
    {
        public TabPayments()
        {
            InitializeComponent();
        }
        public ICommand OpenModalToMakePaymentCommand => new Command<tblPurchaseRequestPaymentsDTO>(OpenModalToMakePayment);
        private void OpenModalToMakePayment(tblPurchaseRequestPaymentsDTO item)
        {
            var popupProperties = new PopupMakePayment(item);
            var scaleAnimation = new ScaleAnimation
            {
                PositionIn = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Right,
                PositionOut = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Left
            };

            popupProperties.Animation = scaleAnimation;
            popupProperties.CloseWhenBackgroundIsClicked = true;

            Application.Current.MainPage.Navigation.PushPopupAsync(popupProperties);
        }

        public ICommand OpenReceiptCommand => new Command<tblPurchaseRequestPaymentsDTO>(OpenReceipt);
        private async void OpenReceipt(tblPurchaseRequestPaymentsDTO item)
        {
            if(item.ReceiptUrl.Contains(".pdf"))
            {
                await Browser.OpenAsync(item.ReceiptUrl, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.AliceBlue,
                    PreferredControlColor = Color.Violet
                });
            }
            else
            {
                var popupProperties = new PopupReceiptImage(item);
                var scaleAnimation = new ScaleAnimation
                {
                    PositionIn = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Right,
                    PositionOut = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Left
                };

                popupProperties.Animation = scaleAnimation;
                popupProperties.CloseWhenBackgroundIsClicked = true;

                Application.Current.MainPage.Navigation.PushPopupAsync(popupProperties);
            }
           
        }
    }
}