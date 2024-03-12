using Church.Mobile.DataLayer.ApiModels;
using Church.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Church.Mobile.Views.PurchaseRequest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseRequestDetails : TabbedPage
    {
        public PurchaseRequestDetails(tblPurchasesRequestsDTO model)
        {
            InitializeComponent();
            BindingContext = new PurchaseRequestDetailsViewModel(model);
        }
    }
}