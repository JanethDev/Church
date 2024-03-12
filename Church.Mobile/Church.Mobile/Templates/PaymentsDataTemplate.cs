using Church.Mobile.DataLayer.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Church.Mobile.Templates
{
    public class PaymentsDataTemplate : DataTemplateSelector
    {
        public DataTemplate OverduePayments { get; set; }

        public DataTemplate PendingPayments { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var obj = (tblPurchaseRequestPaymentsDTO)item;
            return obj.OverduePayment ? OverduePayments : PendingPayments;
        }
    }
}
