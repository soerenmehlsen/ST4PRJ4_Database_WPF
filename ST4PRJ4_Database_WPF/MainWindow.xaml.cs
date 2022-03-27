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
using Microsoft.Win32;
using System.Data.SqlClient;
using System.IO;

namespace ST4PRJ4_Database_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string imagePath { get; set; }
        private SqlDataReader dataReader;
        private byte[] output;

        private void LoadButton_Click(object sender, RoutedEventArgs e) //Click on the load button
        {
            OpenFileDialog openFile = new OpenFileDialog(); //create object for Filedialog
            

            openFile.FileName = ""; // 

            openFile.Filter = "Supported Images|*.jpg;*.jpeg.*png"; // Filter only supported pictures

            if (openFile.ShowDialog() == true)
            {
                imagePath = openFile.FileName;
                AddImage.Source = new BitmapImage(new Uri(openFile.FileName)); //Bitmapimage?
                
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) //Click on the add button
        {
            
            string connString = "Server=tcp:st4prj4.database.windows.net,1433;Initial Catalog=ST4PRJ4;Persist Security Info=False;User ID=azureuser;Password=Katrinebjerg123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"; //Connection string for the database

            SqlConnection connection = new SqlConnection(connString);

            connection.Open();

            SqlCommand command = connection.CreateCommand();

            // Convert image to byte array
            byte[] data;

            data = File.ReadAllBytes(imagePath);

            //Define image parameter
            command.Parameters.AddWithValue("@image", data);

            //query
            command.CommandText = "INSERT INTO Test_table(PersonID, FirstName, LastName, Image) VALUES(4, 'Magnus', 'Andersen', @image)";

            if (command.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Billedet blev uploadet til database");
            }
            else
            {
                MessageBox.Show("Billedet blev ikke uploadet til database");
            }
            connection.Close();

        }

        private void ShowButton_Click(object sender, RoutedEventArgs e) 
        {
            string connString = "Server=tcp:st4prj4.database.windows.net,1433;Initial Catalog=ST4PRJ4;Persist Security Info=False;User ID=azureuser;Password=Katrinebjerg123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"; //Connection string for the database

            SqlConnection connection = new SqlConnection(connString);

            connection.Open();

            SqlCommand command = connection.CreateCommand();

            command.CommandText = "Select Image from Test_table WHERE PersonID=4";

            dataReader = command.ExecuteReader();


            while (dataReader.Read())
            {
                output = (byte[])dataReader.GetValue(0);
            }

            connection.Close();

            Stream streamObj = new MemoryStream(output);
            BitmapImage bitObj = new BitmapImage();
            
            bitObj.BeginInit();
            bitObj.StreamSource = streamObj;
            bitObj.EndInit();

            ShowImage.Source = bitObj;

            //https://stackoverflow.com/questions/14337071/convert-array-of-bytes-to-bitmapimage
            //https://sqltutorialtips.blogspot.com/2016/11/image-vs-varbinarymax.html
            //https://www.c-sharpcorner.com/UploadFile/0f100d/storing-and-retrieving-image-from-sql-server-database-in-wpf/
            //https://www.youtube.com/watch?v=XMhcF-zex6k
            //https://www.youtube.com/watch?v=HIv1_P98Ne8&list=LL&index=2&t=182s
        }

    }
}
