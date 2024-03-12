using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class dtoAnniversary
    {
        public int TotalCount { get; set; }
        public Int64 ROWNUM { get; set; }
        public int AnniversaryID { get; set; }
        public string Name { get; set; }
        public string Hour { get; set; }
        public DateTime Date { get; set; }
    }
}
