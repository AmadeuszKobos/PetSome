using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace SQLite2
{
    internal class Database
    {
        public SQLiteConnection conn; //connection to database

        public Database()
        {
            conn = new SQLiteConnection("Data Source=petSomeDB.sqlite3");

            if (!File.Exists("./petSomeDB.sqlite3"))
            {
                SQLiteConnection.CreateFile("petSomeDB.sqlite3"); //if file named database.sqlite3 doesn't exist, we create one
            }
        }

        public void OpenConnection()
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

        }
        public void CloseConnection()
        {
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
            }
        }

    }
}
