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
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        private BoardViewModel boardViewModel;

        public BoardView(UserModel user)
        {
            InitializeComponent();
            this.DataContext = new BoardViewModel(user);
            boardViewModel = (BoardViewModel)DataContext;
        }


        private void LogoutBTN_Click(object sender, RoutedEventArgs e)
        {
            BackendService.GetService().Logout(boardViewModel.LoggedinUser.Email);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

    
        private void ViewTask_Click(object sender, RoutedEventArgs e)
        {
            if (boardViewModel.IsViewable())
            {
                TaskView taskView = new TaskView(boardViewModel.SelectedTask);
                taskView.ShowDialog();
            }
        }

        private void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if (boardViewModel.IsViewable())
            {
                TaskInsertUpdate taskInUp = new TaskInsertUpdate(boardViewModel.SelectedTask, true, boardViewModel.LoggedinUser, boardViewModel.Board.Columns[boardViewModel.SelectedTask.ColumnOrdinal],boardViewModel.FilterText);
                taskInUp.ShowDialog();
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            TaskInsertUpdate taskInUp = new TaskInsertUpdate(null, false, boardViewModel.LoggedinUser, boardViewModel.Board.Columns.ElementAt(0),boardViewModel.FilterText);
            taskInUp.ShowDialog();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (boardViewModel.IsViewable())
            {
                boardViewModel.DeleteTask();
            }
        }

        private void AdvanceTask_Click(object sender, RoutedEventArgs e)
        {
            if (boardViewModel.IsViewable())
            {
                boardViewModel.AdvanceTask();
            }
        }

        private void UpdateColumn_Click(object sender, RoutedEventArgs e)
        {
            if (boardViewModel.IsViewableColumnFunction())
            {
                ColumnUpdateView columnUpdateView = new ColumnUpdateView(boardViewModel.SelectedColumn, boardViewModel.LoggedinUser);
                columnUpdateView.ShowDialog();
            }
        }

        private void DeleteColumn_Click(object sender, RoutedEventArgs e)
        {
            if (boardViewModel.IsViewableColumnFunction())
            {
                boardViewModel.deleteColumn();
            }
        }

        private void MoveLeft_Click(object sender, RoutedEventArgs e)
        {
            if (boardViewModel.IsViewableColumnFunction())
            {
                boardViewModel.MoveColumnLeft();
            }
        }

        private void MoveRight_Click(object sender, RoutedEventArgs e)
        {
            if (boardViewModel.IsViewableColumnFunction())
            {
                boardViewModel.MoveColumnRight();
            }
        }

        private void AddLeft_Click(object sender, RoutedEventArgs e)
        {
            if (boardViewModel.IsViewableColumnFunction())
            {
                ColumnUpdateView columnUpdateView = new ColumnUpdateView(boardViewModel.LoggedinUser, boardViewModel.SelectedColumn.ColumnOrdinal, boardViewModel.Board);
                columnUpdateView.ShowDialog();
            }
        }

        private void AddRight_Click(object sender, RoutedEventArgs e)
        {
            if (boardViewModel.IsViewableColumnFunction())
            {
                ColumnUpdateView columnUpdateView = new ColumnUpdateView(boardViewModel.LoggedinUser, boardViewModel.SelectedColumn.ColumnOrdinal + 1, boardViewModel.Board);
                columnUpdateView.ShowDialog();
            }
        }

        private void FilterBTN_Click(object sender, RoutedEventArgs e)
        {
            boardViewModel.FilterTasks();
        }

        private void ClearBTN_Click(object sender, RoutedEventArgs e)
        {
            boardViewModel.ClearFilter();
        }
    }
}
