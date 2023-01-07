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
using System.Security.Cryptography;
using System.IO;

namespace Biblioteka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public void SwitchPanels()
        {
            loginPanel.Visibility = loginPanel.Visibility ^ dashPanel.Visibility ^ (dashPanel.Visibility = loginPanel.Visibility);
        }
        public void Login(string login, string password)
        {
            var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location.ToString().Split('\\').ToList();
            dbPathList.RemoveRange(dbPathList.Count - 4, 4);
            var dbPath = string.Join("\\", dbPathList);

            SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");

            var exc = myConnection.CreateCommand();
            
            myConnection.Open();
            var ex = new SqlCommand("SELECT * FROM users", myConnection);

            var reader = ex.ExecuteReader();

            var log = "";
            var pass = "";

            while (reader.Read())
            {
                log = DecryptString(reader[3].ToString(), reader[1].ToString());
                pass = DecryptString(reader[3].ToString(), reader[2].ToString());

                if(log == login && pass == password)
                {
                    SwitchPanels();
                    return;
                }
            }
            MessageBox.Show("Login lub Hasło są nieprawidłowe");
            reader.Close();
            reader.DisposeAsync();
            ex.Dispose();
            exc.Dispose();
            myConnection.Close();
            myConnection.Dispose();

        }
        public void logOut()
        {
            // reset Data
            SwitchPanels();
        }
        public MainWindow()
        {
            InitializeComponent();
        }
        private void loseFocus(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}
