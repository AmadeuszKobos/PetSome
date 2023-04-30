
using System;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace PetSome
{
    /// <summary>
    /// Logika interakcji dla klasy Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        public Add()
        {
            InitializeComponent();
        }

        public void AddNewPet(object sender, RoutedEventArgs e)
        {
            string newPetName = nameField.Text;
            ComboBoxItem typeItem = (ComboBoxItem)typeField.SelectedItem;   
            string newPetType = typeItem.Content.ToString();
            if (nameField.Text == "")
            {
                nameField.Text = newPetName.ToString();
                nameField.BorderBrush = new SolidColorBrush(Colors.Red);
                Console.WriteLine(newPetName);
            }
            else
            {
                Database dbObject = new Database();

                string query = "INSERT INTO 'PetLibrary' ('Type','Name') VALUES (@type, @name)";
                SQLiteCommand myInsert = new SQLiteCommand(query, dbObject.conn);

                dbObject.OpenConnection();

                myInsert.Parameters.AddWithValue("@type", newPetType);
                myInsert.Parameters.AddWithValue("@name", newPetName);

                var insertResult = myInsert.ExecuteNonQuery();

                dbObject.CloseConnection();

                Window window = Application.Current.MainWindow.FindName("MenuWindow") as Window;
                if (window != null)
                {
                    ListBox listBox = (ListBox)window.FindName("PetsList");
                    listBox.Items.Insert(0, newPetName);
                }

                MainWindow menu = new MainWindow();
                menu.Show();
                this.Close();

            }





        }
    }





}
