using Church.Mobile.Interfaces;
using Church.Mobile.iOS;
using Foundation;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteiOS))]
namespace Church.Mobile.iOS
{
    public class SQLiteiOS : ISQLite
    {
        public SQLiteiOS()
        {
        }

        #region ISQLite implementation
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "Church2.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);

            var conn = new SQLiteConnection(path);

            return conn;
        }
        #endregion
    }
}