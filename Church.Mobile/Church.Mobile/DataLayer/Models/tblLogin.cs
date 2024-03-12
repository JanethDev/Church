using System;
using System.Collections.Generic;
using System.Text;

namespace Church.Mobile.DataLayer.Models
{
    public class tblLogin
    {
       
        public string UserName { get; set; }
      
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
