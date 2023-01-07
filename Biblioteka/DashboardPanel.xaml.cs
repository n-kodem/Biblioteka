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
    /// Logika interakcji dla klasy DashboardPanel.xaml
    /// </summary>
    public partial class DashboardPanel : UserControl
    {
        public DashboardPanel()
        {
            InitializeComponent();
        }

        private void logOut(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).logOut();
        }


        public class DataRow
        {
            public string? id { set; get; }
            public string? tytul { set; get; }
            public string? autor { set; get; }
            public string? gatunek { set; get; }
            public string? imie { set; get; }
            public string? nazwisko { set; get; }
            public string? email { set; get; }
            public string? plec { set; get; }
            public string? adres { set; get; }
            public string? book { set; get; }
            public string? borrowDate { set; get; }
            public string? returnDate { set; get; }





        }
        public List<DataRow> searchFor(int colNum,string text)
        {
            List<string> tableNames = new List<string> { "books","borrowings","readers"};

            List<DataRow> output = new List<DataRow>();
            List<List<string>> columns = new List<List<string>> { 
                new List<string>{ "\"tytul\"", "\"autor\"","\"gatunek\""}, 
                new List<string>{ "borrowings.id", "readers.imie", "readers.nazwisko", "books.autor", "books.gatunek", "borrowings.borrowDate", "borrowings.returnDate" }, 
                new List<string> { "imie", "nazwisko", "email", "plec", "adres" } 
            };
            var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location.ToString().Split('\\').ToList();
            dbPathList.RemoveRange(dbPathList.Count - 4, 4);
            var dbPath = string.Join("\\", dbPathList);

            SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");
            myConnection.Open();
            
            var ex = new SqlCommand($"SELECT * FROM {tableNames[colNum]} WHERE id like (\'%{text}%\') or {string.Join($" like (\'%{text}%\') or ", columns[colNum])} LIKE (\'%{text}%\');", myConnection);
            if (colNum == 1)
                ex = new SqlCommand($"SELECT borrowings.id, readers.imie, readers.nazwisko, books.autor, books.gatunek, borrowings.borrowDate, borrowings.returnDate FROM borrowings INNER JOIN readers ON borrowings.userId = readers.id INNER JOIN books ON borrowings.bookId = books.id WHERE borrowings.id like (\'%{text}%\') or {string.Join($" like (\'%{text}%\') or  ", columns[colNum])} LIKE (\'%{text}%\');",myConnection);
            var reader = ex.ExecuteReader();

            while (reader.Read())
            {
                if(colNum==0)
                    output.Add(
                        new DataRow
                        {
                            id = reader["id"].ToString(),
                            tytul = reader["tytul"].ToString(),
                            autor = reader["autor"].ToString(),
                            gatunek = reader["gatunek"].ToString(),
                        }
                    );
                else if (colNum == 1)
                    output.Add(
                        new DataRow
                        {
                            imie = reader["imie"].ToString(),
                            nazwisko = reader["nazwisko"].ToString(),
                            book = reader["autor"].ToString()+", "+ reader["gatunek"].ToString(),
                            borrowDate = reader["borrowDate"].ToString(),
                            returnDate = reader["returnDate"].ToString(),
                        }
                    );
                else if (colNum == 2)
                    output.Add(
                        new DataRow
                        {
                            id = reader["id"].ToString(),
                            imie = reader["imie"].ToString(),
                            nazwisko = reader["nazwisko"].ToString(),
                            email = reader["email"].ToString(),
                            plec = reader["plec"].ToString(),
                            adres = reader["adres"].ToString(),

                        }
                    );
            }

            myConnection.Close();
            myConnection.Dispose();

            return output;
        }

        public void reloadData()
        {
            bookBase.Items.Clear();
            readersBase.Items.Clear();
            borrowingsBase.Items.Clear();




            var books = new List<DataRow> {};
            var readers = new List<DataRow> {};
            var borrowings = new List<DataRow> {};

            //
            var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location.ToString().Split('\\').ToList();
            dbPathList.RemoveRange(dbPathList.Count - 4, 4);
            var dbPath = string.Join("\\", dbPathList);

            SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");
            myConnection.Open();
            var ex = new SqlCommand($"SELECT * FROM books;", myConnection);
            var reader = ex.ExecuteReader();

            while (reader.Read())
            {
                books.Add(new DataRow { id = reader[0].ToString(), tytul = reader[1].ToString(), autor = reader[2].ToString(), gatunek = reader[3].ToString() });
            }

            ex = new SqlCommand($"SELECT * FROM readers;", myConnection);
            reader.Close();
            reader = ex.ExecuteReader();

            while (reader.Read())
            {
                readers.Add(new DataRow { id = reader[0].ToString(), imie = reader[1].ToString(), nazwisko = reader[2].ToString(), email = reader[3].ToString(), plec = reader[4].ToString(), adres = reader[5].ToString() });
            }


            ex = new SqlCommand($"SELECT borrowings.id, readers.imie, readers.nazwisko, books.autor, books.gatunek, borrowings.borrowDate, borrowings.returnDate FROM borrowings INNER JOIN readers ON borrowings.userId = readers.id INNER JOIN books ON borrowings.bookId = books.id;", myConnection);
            reader.Close();
            reader = ex.ExecuteReader();

            while (reader.Read())
            {
                borrowings.Add(new DataRow
                {
                    imie = reader["imie"].ToString(),
                    nazwisko = reader["nazwisko"].ToString(),
                    book = reader["autor"].ToString() + ", " + reader["gatunek"].ToString(),
                    borrowDate = reader["borrowDate"].ToString(),
                    returnDate = reader["returnDate"].ToString(),
                });
            }


            foreach (var borrow in borrowings)
            {
                borrowingsBase.Items.Add(borrow);
            }
            foreach (var book in books)
            {
                bookBase.Items.Add(book);
            }

            foreach (var readerp in readers)
            {
                readersBase.Items.Add(readerp);
            }

            // add borrowings
            


            myConnection.Close();
            myConnection.Dispose();



            //bookBase.Items.Add(books);
            //readersBase.Items.Add(readers);


        }

        private void loadData(object sender, DependencyPropertyChangedEventArgs e)
        {
            reloadData();
        }

        private void truncateReaders(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąć wszystkie dane czytelników?\nUwaga tej czynności nie da się cofnąć!", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // readers TABLE TRUNCATION
                var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location.ToString().Split('\\').ToList();
                dbPathList.RemoveRange(dbPathList.Count - 4, 4);
                var dbPath = string.Join("\\", dbPathList);

                SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");
                myConnection.Open();
                new SqlCommand($"TRUNCATE TABLE readers;", myConnection).ExecuteNonQuery();
                myConnection.Close();
                myConnection.Dispose();
            }
        }

        private void truncateBooks(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąć wszystkie dane książek?\nUwaga tej czynności nie da się cofnąć!", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // books TABLE TRUNCATION
                var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location.ToString().Split('\\').ToList();
                dbPathList.RemoveRange(dbPathList.Count - 4, 4);
                var dbPath = string.Join("\\", dbPathList);

                SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");
                myConnection.Open();
                new SqlCommand($"TRUNCATE TABLE books;", myConnection).ExecuteNonQuery();
                myConnection.Close();
                myConnection.Dispose();
            }
        }
        private void truncateBorrowings(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąć wszystkie dane wyporzyczeń?\nUwaga tej czynności nie da się cofnąć!", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // books TABLE TRUNCATION
                var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location.ToString().Split('\\').ToList();
                dbPathList.RemoveRange(dbPathList.Count - 4, 4);
                var dbPath = string.Join("\\", dbPathList);

                SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");
                myConnection.Open();
                new SqlCommand($"TRUNCATE TABLE borrowings;", myConnection).ExecuteNonQuery();
                myConnection.Close();
                myConnection.Dispose();
            }
        }
        private void ksiazka_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void imie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (imie.SelectedIndex != -1)
                nazwisko.IsEnabled = true;
            else
                nazwisko.IsEnabled = false;

            nazwisko.SelectedIndex = -1;
        }

        private void readersSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            readersBase.Items.Clear();
            foreach (var item in searchFor(2, (sender as TextBox).Text.ToString()))
            {
                readersBase.Items.Add(item);
            }
        }

        private void booksSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bookBase.Items.Clear();
            foreach (var item in searchFor(0, (sender as TextBox).Text.ToString()))
            {
                bookBase.Items.Add(item);
            }
        }

        private void borrowingsSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            borrowingsBase.Items.Clear();
            foreach (var item in searchFor(1, (sender as TextBox).Text.ToString()))
            {
                borrowingsBase.Items.Add(item);
            }
        }

        private void nazwisko_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void load_borrow_imie_data(object sender, EventArgs e)
        {
            //
            imie.Items.Clear();
            var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location.ToString().Split('\\').ToList();
            dbPathList.RemoveRange(dbPathList.Count - 4, 4);
            var dbPath = string.Join("\\", dbPathList);

            SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");
            myConnection.Open();
            var ex = new SqlCommand($"SELECT imie FROM readers;", myConnection);
            var reader = ex.ExecuteReader();

            while (reader.Read())
            {
                imie.Items.Add(reader[0].ToString());
            }
            myConnection.Close();
            myConnection.Dispose();
        }

        private void load_borrow_nazwisko_data(object sender, EventArgs e)
        {
            //
            nazwisko.Items.Clear();
            var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location
                .ToString()
                .Split('\\')
                .ToList();
            dbPathList.RemoveRange(dbPathList.Count - 4, 4);
            var dbPath = string.Join("\\", dbPathList);

            SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");
            myConnection.Open();
            var ex = new SqlCommand($"SELECT nazwisko FROM readers WHERE imie = '{imie.SelectedValue}';", myConnection);
            var reader = ex.ExecuteReader();

            while (reader.Read())
            {
                nazwisko.Items.Add(reader[0].ToString());
            }
            myConnection.Close();
            myConnection.Dispose();
        }

        private void load_borrow_ksiazka_data(object sender, EventArgs e)
        {
            ksiazka.Items.Clear();
            var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location.ToString().Split('\\').ToList();
            dbPathList.RemoveRange(dbPathList.Count - 4, 4);
            var dbPath = string.Join("\\", dbPathList);

            SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");
            myConnection.Open();
            var ex = new SqlCommand($"SELECT tytul FROM books;", myConnection);
            var reader = ex.ExecuteReader();

            while (reader.Read())
            {
                ksiazka.Items.Add(reader[0].ToString());
            }
            myConnection.Close();
            myConnection.Dispose();
        }

        private void addBorrowing(object sender, RoutedEventArgs e)
        {
            // Sprawdzenie poprawności danych
            if (imie.Text == "" || nazwisko.Text == "" || ksiazka.Text == "" || data_wybor_wyp.Text == "")
            {
                MessageBox.Show("Nie wszystkie pola zostały uzupełnione.");
                return;
            }

            var userId = "";
            var bookId = "";
            var borrowDate = DateTime.Now.Date;

            var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location.ToString().Split('\\').ToList();
            dbPathList.RemoveRange(dbPathList.Count - 4, 4);
            var dbPath = string.Join("\\", dbPathList);

            SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");
            myConnection.Open();


            var ex = new SqlCommand($"SELECT id FROM books WHERE tytul = '{ksiazka.Text}';", myConnection);
            var reader = ex.ExecuteReader();

            while (reader.Read())
            {
                bookId = reader[0].ToString();
            }

            reader.Close();
            ex = new SqlCommand($"SELECT id FROM readers WHERE imie = '{imie.Text}' and nazwisko = '{nazwisko.Text}';", myConnection);
            reader = ex.ExecuteReader();

            while (reader.Read())
            {
                userId = reader[0].ToString();
            }

            reader.Close();
            ex = new SqlCommand($"INSERT INTO borrowings(bookId,userId,borrowDate,returnDate) VALUES('{bookId}','{userId}','{borrowDate}','{data_wybor_wyp.SelectedDate}');", myConnection);
            ex.ExecuteReader();
            myConnection.Close();
            myConnection.Dispose();

            imie.Text = "";
            nazwisko.Text = "";
            ksiazka.Text = "";
            data_wybor_wyp.Text = "";

            MessageBox.Show("Książka została dodana pomyślnie!", "", MessageBoxButton.OK);
            //*****

        }

        private void resetBorrowingData(object sender, RoutedEventArgs e)
        {
            imie.Text = "";
            nazwisko.Text = "";
            ksiazka.Text = "";
            data_wybor_wyp.Text = "";
        }

        private void data_wybor_wyp_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            // Cancel the event to prevent the text from being entered
            e.Handled = true;
            // Set the IsDropDownOpen property to true to open the dropdown
            data_wybor_wyp.IsDropDownOpen = true;
        }

        private void data_wybor_wyp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            data_wybor_wyp.IsDropDownOpen = false;
            Keyboard.ClearFocus();
        }

        private void usersShortcutSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void readerssShortcutSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
