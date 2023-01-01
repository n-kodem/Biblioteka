using MaterialDesignExtensions.Controls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using System.Xml.Linq;

namespace Biblioteka
{
    /// <summary>
    /// Interaction logic for AddUserControl.xaml
    /// </summary>
    public partial class AddUserControl : UserControl
    {
        public AddUserControl()
        {
            InitializeComponent();
        }

        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public char GenerateChar(Random rng)
        {
            // 'Z' + 1 because the range is exclusive
            return (char)(rng.Next('A', 'Z' + 1));
        }

        public string GenerateString(Random rng, int length)
        {
            char[] letters = new char[length];
            for (int i = 0; i < length; i++)
            {
                letters[i] = GenerateChar(rng);
            }
            return new string(letters);
        }

        private void addUser(object sender, RoutedEventArgs e)
        {
            var dbPathList = System.Reflection.Assembly.GetEntryAssembly().Location.ToString().Split('\\').ToList();
            dbPathList.RemoveRange(dbPathList.Count - 4, 4);
            var dbPath = string.Join("\\", dbPathList);
            var key = GenerateString(new Random(), 16);
            var encryptedLogin = EncryptString(key,login.Text);
            var encryptedPwd = EncryptString(key, pwd.Text);
            var isAdmin = (int)Convert.ToInt16(privileges.Text == "Admin");
            
            SqlConnection myConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath}\\baza.mdf;Integrated Security=True;Connect Timeout=30");
            var exc = myConnection.CreateCommand();
            myConnection.Open();
            var ex = new SqlCommand($"INSERT INTO users(login,haslo,decoder,isAdmin) VALUES('{encryptedLogin}','{encryptedPwd}','{key}','{isAdmin}');", myConnection);
            var reader = ex.ExecuteReader();
            myConnection.Close();
            myConnection.Dispose();

            login.Text = "";
            pwd.Text = "";
            pwdConfirm.Text = "";
            privileges.SelectedIndex = -1;

            MessageBox.Show("Użytkownik został dodany pomyślnie!", "", MessageBoxButton.OK);
        }

        private void resetData(object sender, RoutedEventArgs e)
        {
            login.Text = "";
            pwd.Text = "";
            pwdConfirm.Text = "";
            privileges.SelectedIndex = -1;
        }
    }
}
