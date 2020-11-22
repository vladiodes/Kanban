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
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : Window
    {
        private TaskViewModel taskViewModel;
        public TaskView(TaskModel task)
        {
            InitializeComponent();
            taskViewModel = new TaskViewModel(task);
            this.DataContext = taskViewModel;
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
