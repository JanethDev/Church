using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class dtoDiscounts
    {
        public int TotalCount { get; set; }
        public Int64 ROWNUM { get; set; }
        public int DiscountID { get; set; }
        public int Percentage { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
