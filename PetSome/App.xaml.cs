using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Globalization;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Controls;
using PetSome.Properties;

namespace PetSome
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }


    internal class Database
    {
        public SQLiteConnection conn; //connection to database

        public Database()
        {
            conn = new SQLiteConnection("Data Source=petSomeDB.sqlite3");

            if (!File.Exists("./petSomeDB.sqlite3"))
            {
                SQLiteConnection.CreateFile("petSomeDB.sqlite3");
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
