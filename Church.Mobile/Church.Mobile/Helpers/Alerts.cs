using Church.Mobile.Views.HelpViews;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Church.Mobile.Helpers
{
    public static class Alerts
    {
        public static void ShowAlert(string Title, string Msg)
        {
            Application.Current.MainPage.DisplayAlert(Title, Msg, "Aceptar");
        }

        public static Task<bool> ShowConfirmationAlert(string Title, string Msg)
        {
            return Application.Current.MainPage.DisplayAlert(Title, Msg, "Si", "No");
        }
        public static void ShowNetworkError()
        {
            Application.Current.MainPage.DisplayAlert("Alerta", "No tienes internet", "OK");
        }

        public static async Task ShowLoadingPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new Loading(), true) ;
        }

        public static async Task HideLoadingPageAsync()
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync(true);
        }
    }
}
