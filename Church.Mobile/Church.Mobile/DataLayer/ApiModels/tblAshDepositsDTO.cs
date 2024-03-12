using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using Church.Mobile.DataLayer.AuxiliaryModels;
using Church.Mobile.Helpers;

namespace Church.Mobile.DataLayer.ApiModels
{
    public class tblAshDepositsDTO
    {
        public string FullName { get; set; }
        public DateTime DeathDate { get; set; }
      
        public string DeathDateFormat
        {
            get
            {
                DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
                string sMonth = dtinfo.GetMonthName(DeathDate.Month);
                sMonth = sMonth[0].ToString().ToUpper() + sMonth.Substring(1, sMonth.Length - 1);
                string value = $"{DeathDate.Day} de {sMonth}";

                return value;
            }
        }

    }
}
