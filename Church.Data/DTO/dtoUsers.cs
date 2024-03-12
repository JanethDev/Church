using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class dtoUsers
    {
        public int TotalCount { get; set; }
        public Int64 ROWNUM { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PSurname { get; set; }
        public string Rol { get; set; }
        public int RolID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
 }
