using System;
using System.Collections.Generic;
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

namespace PetSome
{
    /// <summary>
    /// Logika interakcji dla klasy Info.xaml
    /// </summary>
    public partial class Info : Window
    {
        public Info()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);

            if (window != null)
            {
                // create a new PDF document
                Document document = new Document();

                // create a new PDF writer
                PdfWriter.GetInstance(document, new FileStream("output.pdf", FileMode.Create));

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
    }
}
