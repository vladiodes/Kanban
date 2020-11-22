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
    /// Interaction logic for TaskUpdateView.xaml
    /// </summary>
    public partial class TaskInsertUpdate : Window
    {
        private InsertUpdateTaskViewModel InsertUpdate;
        public TaskInsertUpdate(TaskModel task, bool isUpdate,UserModel loggedin, ColumnModel column, string currentFilter)
        {
            InitializeComponent();
            DataContext = new InsertUpdateTaskViewModel(task, isUpdate,loggedin,column,currentFilter);
            InsertUpdate = (InsertUpdateTaskViewModel)DataContext;
        }

        private void BTN_Click(object sender, RoutedEventArgs e)
        {
            if (InsertUpdate.Add())
            {
                MessageBox.Show("Task was added");
                this.Close();
            }
        }

        private void Exit_BTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateBTN_Click(object sender, RoutedEventArgs e)
        {
            InsertUpdate.Update();
        }
    }
}
