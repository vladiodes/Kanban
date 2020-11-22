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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {

        private RegisterViewModel registerView;

        public RegisterWindow(bool isHostRegistration)
        {
            InitializeComponent();
            this.DataContext = new RegisterViewModel(isHostRegistration);
            this.registerView = (RegisterViewModel)DataContext;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (registerView.Register())
            {
                MessageBox.Show(registerView.Success);
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
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
