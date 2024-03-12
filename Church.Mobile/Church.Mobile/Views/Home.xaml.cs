using Church.Mobile.DataLayer.ApiModels;
using Church.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Church.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
            NavigationPage.SetHasBackButton(this, true);

        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var model = (tblPurchasesRequestsDTO)button.CommandParameter;

            if (model != null)
            {
                ((HomeViewModel)BindingContext).GoToCryptDetailsCommand.Execute(model);
            }
        }
    }
}