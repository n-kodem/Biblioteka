using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Logika interakcji dla klasy AddReaderControl.xaml
    /// </summary>
    public partial class AddReaderControl : UserControl
    {
        public AddReaderControl()
        {
            InitializeComponent();
        }

        private void addReader(object sender, RoutedEventArgs e)
        {
            var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location.ToString().Split('\\').ToList();
            dbPathList.RemoveRange(dbPathList.Count - 4, 4);
            var dbPath = string.Join("\\", dbPathList);

            SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");
            var exc = myConnection.CreateCommand();
            myConnection.Open();
            var ex = new SqlCommand($"INSERT INTO readers(imie,nazwisko,email,plec,adres) VALUES('{imie.Text}','{nazwisko.Text}','{email.Text}','{plec.Text}','{adres.Text}');", myConnection);
            ex.BeginExecuteNonQuery();
            myConnection.Close();
            myConnection.Dispose();

            imie.Text = "";
            nazwisko.Text = "";
            adres.Text = "";
            email.Text = "";
            plec.SelectedIndex = -1;

            MessageBox.Show("Użytkownik został dodany pomyślnie!", "", MessageBoxButton.OK);
            new DashboardPanel().reloadData();

        }

        private void resetData(object sender, RoutedEventArgs e)
        {
            imie.Text = "";
            nazwisko.Text = "";
            plec.Text = "";
            email.Text = "";
            adres.Text = "";
        }
    }
}
