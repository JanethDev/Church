using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Church.Mobile.Interfaces
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
