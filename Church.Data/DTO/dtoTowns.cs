using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class dtoTowns
    {
        public int TotalCount { get; set; }
        public Int64 ROWNUM { get; set; }
        public int StateID { get; set; }
        public int TownID { get; set; }
        public string Name { get; set; }
        public string State { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
