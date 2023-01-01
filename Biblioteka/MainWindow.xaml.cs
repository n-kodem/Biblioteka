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
        public void Login(string email, string password)
        {
            //******
            /*SqlConnection myConnection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=.\\Baza.mdf;Integrated Security=True");
            var exc = myConnection.CreateCommand();
            
            myConnection.Open();
            var ex = new SqlCommand("SELLECT * FROM users", myConnection);

            var reader = ex.ExecuteReader();

            while (reader.Read())
            {
                var data = reader[0] + " " + reader[1] + " " + reader[2];
            }
            reader.Close();
            reader.DisposeAsync();
            ex.Dispose();
            exc.Dispose();
            myConnection.Close();
            myConnection.Dispose();
            */
            
            //*****
            var cemail = "email";
            var cpassword = "password";

            if (cemail == email && cpassword == password)
            {
                // reset TextBoxes
                SwitchPanels();
            }

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
