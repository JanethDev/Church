using Church.Mobile.DataLayer.ApiModels;
using Church.Mobile.Interfaces;
using Church.Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Church.Mobile.Views.PurchaseRequest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupReceiptImage : PopupPage
    {
        public PopupReceiptImage(tblPurchaseRequestPaymentsDTO _tblPurchaseRequestPaymentsDTO)
        {
            InitializeComponent();
            this.BindingContext = new PopupReceiptImageViewModel(_tblPurchaseRequestPaymentsDTO);
        }
    }
}