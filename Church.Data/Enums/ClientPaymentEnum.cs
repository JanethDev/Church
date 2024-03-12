using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data.Enums
{
    public enum ClientPaymentEnum
    {
        [Display(Name = "Pendiente")]
        Pending = 1,
        [Display(Name = "Aprobado")]
        Approved = 2,
        [Display(Name = "Rechazado")]
        Rejected = 3
    }
}
