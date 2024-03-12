using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class dtoAshDeposits
    {
        public int TotalCount { get; set; }
        public Int64 ROWNUM { get; set; }
        public int AshDepositID { get; set; }
        public string Name { get; set; }
        public string DeathCertificate { get; set; }
        public string CremationCertificate { get; set; }
        public string Ticket { get; set; }
        public decimal FederalTax { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DeathDate { get; set; }
        public DateTime AshDepositDate { get; set; }
    }
}
