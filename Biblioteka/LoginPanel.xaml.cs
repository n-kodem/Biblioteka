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

        private void logIn(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Login(login.Text,password.Text);
            login.Text = "";
            password.Text = "";
        }
    }
}
