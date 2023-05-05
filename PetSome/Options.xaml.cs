using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.util;
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
            FontSizeSlider.Value = Properties.Settings.Default.SliderValue;
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

        private void Home_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowSize_Radionbutton(object sender, RoutedEventArgs e)
        {
            Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MenuWindow");
            if(sender == SmallButton)
            {
                if(window != null)
                {
                    window.Height -= 200;
                    window.Width -= 200;
                }


                this.Height -= 100;
                this.Width -= 100;
            }
            else if(sender == MediumButton)
            {

                if( window != null)
                {
                    window.Height = 680;
                    window.Width = 620;
                }

                this.Height = 400;
                this.Width = 525;
            }
            else if(sender == LargeButton)
            {
                if(window != null)
                {
                    window.Height += 200;
                    window.Width += 200;
                }


                this.Height += 100;
                this.Width += 100;
            }
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            //window.UpdateLayout();
            this.UpdateLayout();
        }

        private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int defSliderValue = Properties.Settings.Default.SliderValue;

            Properties.Settings.Default.MidFont = defSliderValue+2;
            Properties.Settings.Default.HugeFont = defSliderValue+4;

            Properties.Settings.Default.SliderValue = defSliderValue;

            Properties.Settings.Default.Save();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
    }

