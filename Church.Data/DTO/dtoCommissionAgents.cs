using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class dtoCommissionAgents
    {
        public int TotalCount { get; set; }
        public Int64 ROWNUM { get; set; }
        public int CommissionAgentID { get; set; }
        public string Name { get; set; }
        public string PSurname { get; set; }
        public int CathedralID { get; set; }
        public string Cathedral { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
