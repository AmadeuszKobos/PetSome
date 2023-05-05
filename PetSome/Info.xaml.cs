using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using Paragraph = System.Windows.Documents.Paragraph;

namespace PetSome
{
    /// <summary>
    /// Logika interakcji dla klasy Info.xaml
    /// </summary>
    public partial class Info : Window
    {
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string gallery_id;
        public string GalleryId
        {
            get { return gallery_id; }
            set { gallery_id = value; }
        }


        public Info(string name)
        {
            InitializeComponent();

            this.Left = (SystemParameters.PrimaryScreenWidth / 2);
            this.Top = (SystemParameters.PrimaryScreenHeight / 2) - (this.Height / 2);

            Database dbObject = new Database();
            string query = "SELECT * FROM 'PetLibrary' WHERE ID=@name";
            string query2 = "UPDATE 'PetLibrary' SET 'GalleryID' = @gallery WHERE ID=@name";

            SQLiteCommand mySelect = new SQLiteCommand(query, dbObject.conn);
            SQLiteCommand myInsert = new SQLiteCommand(query2, dbObject.conn);
            mySelect.Parameters.AddWithValue("@name", name);
            myInsert.Parameters.AddWithValue("@name", name);


            string gallery = "";
            this.Id = name;

            dbObject.OpenConnection();

            SQLiteDataReader selectResult = mySelect.ExecuteReader();

            if (selectResult.HasRows)
            {
                selectResult.Read();

                string type = selectResult.IsDBNull(selectResult.GetOrdinal("Type")) ? "" : (string)selectResult["Type"];
                string icon = Type(type);
                foreach (ComboBoxItem item in PetType.Items)
                {
                    if ((string)item.Content == type)
                    {
                        PetType.SelectedItem = item;
                        break;
                    }
                }
                
                PetPet.Text = selectResult.IsDBNull(selectResult.GetOrdinal("Name")) ? "" : (string)selectResult["Name"] + icon;
                PetName.Text = selectResult.IsDBNull(selectResult.GetOrdinal("Name")) ? "" : (string)selectResult["Name"];


                PetRace.Text = selectResult.IsDBNull(selectResult.GetOrdinal("Race")) ? "" : (string)selectResult["Race"];
                PetFood.Text = selectResult.IsDBNull(selectResult.GetOrdinal("Food")) ? "" : (string)selectResult["Food"];

                PetVetVisit.SelectedDate = selectResult.IsDBNull(selectResult.GetOrdinal("LastVet")) ? null : (DateTime?)selectResult.GetDateTime(selectResult.GetOrdinal("LastVet"));
                PetBirthday.SelectedDate = selectResult.IsDBNull(selectResult.GetOrdinal("Birthday")) ? null : (DateTime?)selectResult.GetDateTime(selectResult.GetOrdinal("Birthday"));

                PetHeight.Text = selectResult.IsDBNull(selectResult.GetOrdinal("Height")) ? "" : selectResult["Height"].ToString();
                PetWeight.Text = selectResult.IsDBNull(selectResult.GetOrdinal("Weight")) ? "" : selectResult["Weight"].ToString();


                FlowDocument flowDoc = new FlowDocument();
                Paragraph para = new Paragraph();
                para.Inlines.Add(new Run(selectResult.IsDBNull(selectResult.GetOrdinal("More")) ? "" : (string)selectResult["More"]));
                flowDoc.Blocks.Add(para);

                PetMore.Document = flowDoc;

                if (selectResult["GalleryID"] == DBNull.Value)
                {
                    Random nr = new Random();
                    int myRandomNumber = nr.Next(100, 999);
                    gallery = PetName.Text + this.Id + myRandomNumber.ToString();
                    myInsert.Parameters.AddWithValue("@gallery", gallery);
                    var insertResult = myInsert.ExecuteNonQuery();
                    this.GalleryId = gallery;
                }
                else
                {
                    this.GalleryId = selectResult.IsDBNull(selectResult.GetOrdinal("GalleryID")) ? "" : (string)selectResult["GalleryID"];
                }


            }



            dbObject.CloseConnection();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);

            if (window != null)
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                appPath += "gallery";
                string galleryPath = System.IO.Path.Combine(appPath, this.GalleryId);
                if (!Directory.Exists(galleryPath))
                {
                    Directory.CreateDirectory(galleryPath);
                }

                // create a new PDF document
                Document document = new Document();

                // create a new PDF writer
                galleryPath += @"\wydruk_" + DateTime.Now.ToString("yyyy-MM-dd") + @".pdf";
                int counter = 0;

                while (File.Exists(galleryPath))
                {
                    counter++;
                    galleryPath = System.IO.Path.Combine(appPath, this.GalleryId);
                    galleryPath += @"\wydruk_" + DateTime.Now.ToString("yyyy-MM-dd") + "(" + counter + @").pdf";
                } 

                PdfWriter.GetInstance(document, new FileStream(galleryPath, FileMode.Create));

                // open the document
                document.Open();

                // create a new RenderTargetBitmap object
                RenderTargetBitmap bitmap = new RenderTargetBitmap((int)window.Width, (int)window.Height, 96, 96, PixelFormats.Pbgra32);

                // render the WPF window to the bitmap
                bitmap.Render(window);

                // create a new PNG encoder
                PngBitmapEncoder encoder = new PngBitmapEncoder();

                // add the bitmap to the encoder
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                // create a new memory stream
                MemoryStream stream = new MemoryStream();

                // save the encoder to the stream
                encoder.Save(stream);

                // create a new iTextSharp image object from the stream
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(stream.GetBuffer());

                // set the size of the image to the size of the window
                image.ScaleToFit((float)window.Width, (float)window.Height);

                // add the image to the document
                document.Add(image);

                // close the document
                document.Close();


            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Czy na pewno chcesz zapisać zmiany?", "Zapisz zmiany", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Database dbObject = new Database();
                string query = "UPDATE 'PetLibrary' SET Name=@name, Type=@type, Race=@race, Food=@food, Height=@height, Weight=@weight, Birthday=@birthday, LastVet=@vet WHERE ID = @id";
                SQLiteCommand myInsert = new SQLiteCommand(query, dbObject.conn);

                dbObject.OpenConnection();
                string newPetName = PetName.Text;
                string newPetRace = PetRace.Text;
                string newPetFood = PetFood.Text;
                string newPetHeight = PetHeight.Text;
                string newPetWeight = PetWeight.Text;
                string newPetType = ((ComboBoxItem)PetType.SelectedItem).Content.ToString();

                DateTime? newPetVet = PetVetVisit.SelectedDate;

                DateTime? newPetBirthday = PetBirthday.SelectedDate;


                myInsert.Parameters.AddWithValue("@id", this.Id);
                myInsert.Parameters.AddWithValue("@name", newPetName);
                myInsert.Parameters.AddWithValue("@type", newPetType);
                myInsert.Parameters.AddWithValue("@race", newPetRace);
                myInsert.Parameters.AddWithValue("@food", newPetFood);
                myInsert.Parameters.AddWithValue("@vet", newPetVet);
                myInsert.Parameters.AddWithValue("@birthday", newPetBirthday);
                myInsert.Parameters.AddWithValue("@height", newPetHeight);
                myInsert.Parameters.AddWithValue("@weight", newPetWeight);

                var insertResult = myInsert.ExecuteNonQuery();

                dbObject.CloseConnection();

                Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MenuWindow");
                ItemEnabled(window);
                this.Close();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Czy na pewno chcesz usunąć ten rekord?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Database dbObject = new Database();
                string query = "DELETE FROM 'PetLibrary' WHERE ID = @id";
                SQLiteCommand myDelete = new SQLiteCommand(query, dbObject.conn);

                myDelete.Parameters.AddWithValue("@id", this.Id);

                dbObject.OpenConnection();
                var deleteResult = myDelete.ExecuteNonQuery();

                dbObject.CloseConnection();

                Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MenuWindow");
                if (window != null)
                { 
                    ListBox listBox = window.FindName("PetsList") as ListBox;
                    ListBoxItem lbi = listBox.Items.Cast<ListBoxItem>().FirstOrDefault(item => item.Tag != null && string.Equals(item.Tag.ToString(), this.Id));
                    if (lbi != null)
                    {
                        MessageBox.Show("Usunięto pomyślnie");
                        listBox.Items.Remove(lbi);
                    }
                    else
                    {
                        MessageBox.Show("Coś poszło nie tak :O");
                    }

                }

                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                appPath += "gallery";
                string galleryPath = System.IO.Path.Combine(appPath, this.GalleryId);
                if (Directory.Exists(galleryPath))
                {
                    Directory.Delete(galleryPath, true);
                }


                this.Close();

            }
        }

        private void Home_Button(object sender, RoutedEventArgs e)
        {
            Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "MenuWindow");
            ItemEnabled(window);

            this.Close();

        }

        private void Move_Object(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
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

        private void ItemEnabled(Window window)
        {
            if (window != null)
            {
                ListBox listBox = window.FindName("PetsList") as ListBox;
                ListBoxItem lbi = listBox.Items.Cast<ListBoxItem>().FirstOrDefault(item => item.Tag != null && string.Equals(item.Tag.ToString(), this.Id));
                if (lbi != null)
                {
                    lbi.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Coś poszło nie tak :O");
                }

            }
        }

        private void Gallery_Click(object sender, RoutedEventArgs e)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            appPath += "gallery";
            string galleryPath = System.IO.Path.Combine(appPath, this.GalleryId);
            if (!Directory.Exists(galleryPath))
            {
                Directory.CreateDirectory(galleryPath);
            }
            Process.Start("explorer.exe", galleryPath);
        }
    }
}
