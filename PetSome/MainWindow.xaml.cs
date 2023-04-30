using PdfSharp.Pdf.Content.Objects;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;


namespace PetSome
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            this.Left = (SystemParameters.PrimaryScreenWidth / 12);
            this.Top = (SystemParameters.PrimaryScreenHeight / 2) - (this.Height / 2);

            Cursor = new Cursor(Application.GetResourceStream(new Uri("Resources/Paw.cur", UriKind.Relative)).Stream);

            Database dbObject = new Database();
            string query = "SELECT * FROM 'PetLibrary'";
            SQLiteCommand mySelect = new SQLiteCommand(query, dbObject.conn);

            dbObject.OpenConnection();

            SQLiteDataReader selectResult = mySelect.ExecuteReader();

            if (selectResult.HasRows)
            {
                Console.WriteLine("Selected Rows");
                int index = 0; 

                while (selectResult.Read())
                {
                    object typeObj = selectResult["Type"];
                    string type = typeObj == DBNull.Value ? null : (string)typeObj;
                    string icon = Type(type);
                    var newListBoxItem = new ListBoxItem();
                    newListBoxItem.Content = selectResult["Name"] + icon;
                    newListBoxItem.Selected += ListBoxItem_Selected; 
                    PetsList.Items.Insert(index, newListBoxItem);
                    newListBoxItem.Tag = selectResult["ID"].ToString();
                    index++;
                }
            }
            dbObject.CloseConnection();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            string name = (sender as ListBoxItem).Tag.ToString();
            Info info = new Info(name);
            info.Show();
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window1 newWindow = new Window1();
            newWindow.Show();
        }

        private void Item_Enter(object sender, MouseEventArgs e)
        {
            Cursor = new Cursor(Application.GetResourceStream(new Uri("Resources/PawLink.ani", UriKind.Relative)).Stream);
        }

        private void Item_Leave(object sender, MouseEventArgs e)
        {
            Cursor = new Cursor(Application.GetResourceStream(new Uri("Resources/Paw.cur", UriKind.Relative)).Stream);
        }

        private void AddNewListElement(object sender, RoutedEventArgs e)
        {
            Add add = new Add();
            add.Show();
            this.Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Options options = new Options();
            options.Show();
        }

        private string Type(string type)
        {
            string result = "";
            switch (type)
            {
                case "Pies":
                    result = "🐶";
                    break;
                case "Kot":
                    result = "🐱";
                    break;
                case "Królik":
                    result = "🐰";
                    break;
                case "Żółw":
                    result = "🐢";
                    break;
                case "Ryba":
                    result = "🐟";
                    break;
                case "Ptak":
                    result = "🐦";
                    break;
                default:
                    break;
            }

            return result;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            this.InvalidateVisual();
        }
    }
}
