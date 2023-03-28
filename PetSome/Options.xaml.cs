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
using System.Windows.Shapes;

namespace PetSome
{
    public partial class Options : Window
    {
        public Options()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == LightRadioButton)
            {
                SolidColorBrush brush = new SolidColorBrush(Colors.Black);
                Application.Current.Resources["Cosik"] = brush;

                SolidColorBrush backgroundcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5DEB3"));
                Application.Current.Resources["BackgroundColor"] = backgroundcolor;

                SolidColorBrush listcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2C94C"));
                Application.Current.Resources["ListColor"] = listcolor;
            }
            else if (sender == DarkRadioButton)
            {
                SolidColorBrush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2C94C"));
                Application.Current.Resources["Cosik"] = brush;

                SolidColorBrush backgroundcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B5A2B"));
                Application.Current.Resources["BackgroundColor"] = backgroundcolor;

                SolidColorBrush listcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#573615"));
                Application.Current.Resources["ListColor"] = listcolor;
            }
        }

        private void WindowSize_Radionbutton(object sender, RoutedEventArgs e)
        {
            if(sender == SmallButton)
            {

            }else if(sender == MediumButton)
            {

            }else if(sender == LargeButton)
            {

            }
        }

    }
    }

