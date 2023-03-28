using System;
using System.Collections.Generic;
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


namespace PetSome
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            Cursor = new Cursor(Application.GetResourceStream(new Uri("Resources/Paw.cur", UriKind.Relative)).Stream);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            Info info = new Info();
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

        private void ListBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            Add add = new Add();
            add.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Options options = new Options();
            options.Show();
        }
    }
}
