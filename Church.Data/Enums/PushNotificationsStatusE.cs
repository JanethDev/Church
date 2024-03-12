using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data.Enums
{
    public enum PushNotificationsStatusE
    {
        [Display(Name = "En borrador")]
        Borrador = 1,
        [Display(Name = "Envíado")]
        Envíado = 2
    }
}
