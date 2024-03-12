using Church.Mobile.DataLayer.ApiModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Church.Mobile.BusinessLayer
{
    public class UsersB
    {
        static object locker = new object();

        public SQLiteConnection database;

        //This method receives the connection to the database and creates the table if necessary
        public UsersB(SQLiteConnection conn)
        {
            database = conn;
            database.CreateTable<tblUsersDTO>();
        }


        public int Create(tblUsersDTO customer)
        {
            lock (locker)
            {
                return database.Insert(customer);
            }
        }


        public int Update(tblUsersDTO customer)
        {
            lock (locker)
            {
                return database.Update(customer);
            }
        }


        public int DeleteAll()
        {
            lock (locker)
            {
                return database.DeleteAll<tblUsersDTO>();
            }
        }


        public tblUsersDTO Get()
        {
            lock (locker)
            {
                return database.Table<tblUsersDTO>().FirstOrDefault();
            }
        }


        public tblUsersDTO Get(int UserID)
        {
            lock (locker)
            {
                return database.Table<tblUsersDTO>().FirstOrDefault(x => x.UserID == UserID);
            }
        }
    }
}
