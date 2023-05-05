using iTextSharp.text.pdf;
using PetSome.Properties;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PetSome
{
    /// <summary>
    /// Logika interakcji dla klasy Calendar.xaml
    /// </summary>
    public partial class Calendar : Window
    {
        public Calendar()
        {
            InitializeComponent();

            int counter = 0;
            Database dbObject = new Database();
            string query = "SELECT * FROM 'Calendar'";
            SQLiteCommand mySelect = new SQLiteCommand(query, dbObject.conn);

            dbObject.OpenConnection();

            SQLiteDataReader selectResult = mySelect.ExecuteReader();

            if (selectResult.HasRows)
            {
                Console.WriteLine("Selected Rows");

                StackPanel Notes_panel = (StackPanel)FindName("Notes_panel");


                while (selectResult.Read())
                {
                    int id = Convert.ToInt32(selectResult["ID_Event"]);

                    Style style = new Style(typeof(TextBlock));
                    style.Setters.Add(new Setter(TextBlock.FontSizeProperty, 18.0));

                    Style borderStyle = new Style(typeof(Border));
                    borderStyle.Setters.Add(new Setter(Border.BackgroundProperty, Brushes.DarkGoldenrod));
                    borderStyle.Setters.Add(new Setter(Border.BorderBrushProperty, Brushes.Black));
                    borderStyle.Setters.Add(new Setter(Border.BorderThicknessProperty, new Thickness(3)));
                    borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(5)));
                    borderStyle.Setters.Add(new Setter(Border.OpacityProperty, 0.3));
                    borderStyle.Setters.Add(new Setter(Border.PaddingProperty, new Thickness(10)));

                    TextBlock textBlock1 = new TextBlock() { Text = (string)selectResult["Note"] + "\n\n", FontWeight = FontWeights.Bold };
                    textBlock1.SetBinding(TextBlock.FontSizeProperty, new Binding("MidFont") { Source = Settings.Default });
                    TextBlock smallTextBlock = new TextBlock() { Text = (string)selectResult["Date"], FontSize = 12, Margin = new Thickness(0, 0, 5, 5), HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom };
                    Button delButton = new Button() { Content = "X", Height = 20, Width = 20, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Tag = id };
                    delButton.Click += DeleteNote_Click;
                    delButton.Style = (Style)Resources["buttonStyle"];
                    Grid grid1 = new Grid() { Children = { textBlock1, smallTextBlock, delButton } };
                    Border border1 = new Border() { Child = grid1 };
                    border1.Style = borderStyle;
                    Notes_panel.Children.Add(border1);

                    counter++;
                }
            }
            dbObject.CloseConnection();

            Counter_TB.Text = counter + " tasks left";
        }


        private void Move_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void txtNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNote.Text) && txtNote.Text.Length > 0)
            {
                labelNote.Visibility = Visibility.Collapsed;
            }
            else
            {
                labelNote.Visibility = Visibility.Visible;
            }
        }

        private void labelNote_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNote.Focus();
        }

        private void NoteBtn_Click(object sender, RoutedEventArgs e)
        {

            DateTime? dateNoteValue = dateNote.SelectedDate;
            string noteText = txtNote.Text;

            if (txtNote.Text == "" || dateNoteValue == null)
            {
                string errorMessage = "Uzupełnij oba pola!";
                string errorTitle = "Błąd";

                if (dateNoteValue != null)
                {
                    dateNote.BorderBrush = Brushes.Red;
                }

                if (txtNote.Text == "")
                {
                    txtNote.BorderBrush = Brushes.Red;
                }

                MessageBox.Show(errorMessage, errorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Database dbObject = new Database();

                string query = "INSERT INTO 'Calendar' ('Note','Date') VALUES (@note, @date)";
                SQLiteCommand myInsert = new SQLiteCommand(query, dbObject.conn);

                dbObject.OpenConnection();

                myInsert.Parameters.AddWithValue("@note", noteText);
                myInsert.Parameters.AddWithValue("@date", dateNoteValue);

                var insertResult = myInsert.ExecuteNonQuery();

                dbObject.CloseConnection();

                txtNote.Text = "";
                dateNote.SelectedDate = null;

            }
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Czy na pewno chcesz usunąć ten rekord?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Database dbObject = new Database();
                string query = "DELETE FROM 'Calendar' WHERE ID_Event = @id";
                SQLiteCommand myDelete = new SQLiteCommand(query, dbObject.conn);

                Button note = (Button)sender;
                int noteID = (int)note.Tag;

                myDelete.Parameters.AddWithValue("@id", noteID);

                dbObject.OpenConnection();
                var deleteResult = myDelete.ExecuteNonQuery();

                dbObject.CloseConnection();

                Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MenuWindow");
                if (window != null)
                {


                }
            }
        }

        private void Home_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
