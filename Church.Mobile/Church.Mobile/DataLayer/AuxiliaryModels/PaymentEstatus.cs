using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Church.Mobile.DataLayer.AuxiliaryModels
{
    public enum PaymentEstatus
    {
        [Display(Name = "Pendiente")]
        Pending = 1,
        [Display(Name = "En proceso de validacion")]
        ValidationProcess = 2,
        [Display(Name = "Pagado")]
        Paid = 3
    }
}
