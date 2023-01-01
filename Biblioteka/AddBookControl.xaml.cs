using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Biblioteka
{
    /// <summary>
    /// Logika interakcji dla klasy AddBookControl.xaml
    /// </summary>
    public partial class AddBookControl : UserControl
    {
        public AddBookControl()
        {
            InitializeComponent();
        }

        private void resetData(object sender, RoutedEventArgs e)
        {
            title.Text = "";
            author.Text = "";
            genre.Text = "";
        }

        private void submitData(object sender, RoutedEventArgs e)
        {
            //******
            var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location.ToString().Split('\\').ToList();
            dbPathList.RemoveRange(dbPathList.Count - 4, 4);
            var dbPath = string.Join("\\", dbPathList);

            SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");
            var exc = myConnection.CreateCommand();
            myConnection.Open();
            var ex = new SqlCommand($"INSERT INTO books(tytul,autor,gatunek) VALUES('{title.Text}','{author.Text}','{genre.Text}');", myConnection);
            ex.ExecuteReader();
            myConnection.Close();
            myConnection.Dispose();

            title.Text = "";
            author.Text = "";
            genre.Text = "";

            MessageBox.Show("Książka została dodana pomyślnie!", "", MessageBoxButton.OK);
            //*****
        }
    }
}
