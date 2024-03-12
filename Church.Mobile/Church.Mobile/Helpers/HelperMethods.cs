using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Church.Mobile.Helpers
{
    public static class HelperMethods
    {
        public static void OpenNewPage(Page page, bool ShowBackButton = false)
        {
            if (!ShowBackButton)
                App.Current.MainPage = new NavigationPage(page);
            else
                App.Current.MainPage.Navigation.PushAsync(page);
        }

        public static async Task NavigateToPageAsync(Page page)
        {
            await App.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
