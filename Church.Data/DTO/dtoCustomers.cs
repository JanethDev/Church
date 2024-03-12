using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class dtoCustomers
    {
        public int TotalCount { get; set; }
        public Int64 ROWNUM { get; set; }
        public int CustomerID { get; set; }
        public string Email { get; set; }
        public string CelPhone { get; set; }
        public string Name { get; set; }
        public string PSurname { get; set; }
        public string CivilStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public int NumberQuotation { get; set; }
    }
}
