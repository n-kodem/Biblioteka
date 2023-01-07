using System.Windows;
using System.Windows.Controls;

namespace Biblioteka
{
    /// <summary>
    /// Logika interakcji dla klasy LoginPanel.xaml
    /// </summary>
    public partial class LoginPanel : UserControl
    {
        public LoginPanel()
        {
            InitializeComponent();
        }
        ///<summary>
        /// Metoda <c>LogIn</c> loguje użytkownika na podstawie wprowadzonych przez niego danych.
        /// </summary> 
        private void logIn(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Login(login.Text,password.Password);
            login.Text = "";
            password.Password = "";
        }
    }
}
