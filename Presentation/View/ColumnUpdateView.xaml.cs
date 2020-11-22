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
    /// Interaction logic for ColumnUpdateView.xaml
    /// </summary>
    public partial class ColumnUpdateView : Window
    {
        private ColumnUpdateViewModel columnUpdateViewModel;
        public ColumnUpdateView(ColumnModel column, UserModel user)
        {
            InitializeComponent();
            DataContext = new ColumnUpdateViewModel(column,user);
            columnUpdateViewModel = (ColumnUpdateViewModel)DataContext;
        }

        public ColumnUpdateView(UserModel user, int columnOrdinal, BoardModel board)
        {
            InitializeComponent();
            DataContext = new ColumnUpdateViewModel(user, columnOrdinal, board);
            columnUpdateViewModel = (ColumnUpdateViewModel)DataContext;
        }

        private void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            if (columnUpdateViewModel.AddColumn())
            {
                MessageBox.Show("Column was added");
                this.Close();
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateBTN_Click(object sender, RoutedEventArgs e)
        {
            columnUpdateViewModel.Update();
        }
    }
}
