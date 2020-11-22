using Presentation.Model;
using Presentation.ViewModel;
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
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginViewModel loginViewModel;
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
            this.loginViewModel = (LoginViewModel)DataContext;
        }

        private void LoginBTN_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = loginViewModel.Login();
            if (user != null)
            {
                BoardView bv = new BoardView(user);
                bv.Show();
                this.Close();
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
