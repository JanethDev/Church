using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Church.Mobile.DataLayer.ApiModels
{
    public class tblUsersDTO
    {
        [PrimaryKey, AutoIncrement]
        public int UserAutoID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PSurname { get; set; }
        public string MSurname { get; set; }
        public string NotificationKeyName { get; set; }
        public string NotificationKey { get; set; }

        public string FullName
            => $"{Name} {PSurname}";
    }
}
