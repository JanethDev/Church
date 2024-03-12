using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data.DTO.Api
{
    public class tblUsersDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PSurname { get; set; }
        public string MSurname { get; set; }
        public string NotificationKeyName { get; set; }
        public string NotificationKey { get; set; }
    }
}
