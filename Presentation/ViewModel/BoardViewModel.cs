using Presentation.Model;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class BoardViewModel : NotifiableObject
    {
        //fields

        private UserModel _loggedinUser;
        public UserModel LoggedinUser { get => _loggedinUser; }
        private BoardModel _board;
        public BoardModel Board
        {
            get => _board;
            set
            {
                _board = value;
                RaisePropertyChanged("Board");
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            private set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private ColumnModel _selectedColumn;
        public ColumnModel SelectedColumn
        {
            get => _selectedColumn;
            set
            {
                _selectedColumn = value;
                RaisePropertyChanged("SelectedColumn");
                Response = "";
            }
        }

        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (SelectedTask != null)
                {
                    _selectedTask = null;
                    RaisePropertyChanged("SelectedTask");
                }
                _selectedTask = value;
                RaisePropertyChanged("SelectedTask");
                Response = "";
                if(value!=null)
                SelectedColumn = Board.Columns[value.ColumnOrdinal];
            }
        }

        private string _filterText = "";
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                RaisePropertyChanged("FilterText");
            }
        }

        public string FilterDescription
        {
            get
            {
                if (FilterText == null || FilterText.Trim().Equals(""))
                    return "";
                else
                    return $"You're viewing tasks that contain '{FilterText}' in their description or title";
            }
        }

        private string _response = "";
        public string Response
        {
            get => _response;
            set
            {
                _response = value;
                RaisePropertyChanged("Response");
            }
        }

        //constructor receiving logged in user
        public BoardViewModel(UserModel u)
        {
            Board = new BoardModel(BackendService.GetService().GetBoard(u.Email).Value, u);
            Title = "Welcome, " + u.Nickname + "\n" + "please select a column or a task to make changes" + "\n" + "All tasks are sorted according to their due dates";
            _loggedinUser = u;
        }

        /// <summary>
        /// Deletes the selected task from the selected column
        /// </summary>
        public void DeleteTask()
        {
            Response resp = BackendService.GetService().DeleteTask(LoggedinUser.Email, SelectedTask.ColumnOrdinal, SelectedTask.Id);
            if (resp.ErrorOccured)
                Response = resp.ErrorMessage;
            else
            {
                Board.Columns[SelectedTask.ColumnOrdinal].Remove(SelectedTask);
            }

        }

        /// <summary>
        /// Advances the selected task
        /// </summary>
        public void AdvanceTask()
        {
            Response = "";
            Response resp = BackendService.GetService().AdvanceTask(LoggedinUser.Email, SelectedTask.ColumnOrdinal, SelectedTask.Id);
            if (resp.ErrorOccured)
                Response = resp.ErrorMessage;
            else
                Board.AdvanceTask(SelectedTask.ColumnOrdinal, SelectedTask.ColumnOrdinal + 1, SelectedTask);

        }

        /// <summary>
        /// Deletes the selected column
        /// </summary>
        public void deleteColumn()
        {
            Response = "";
            Response resp = BackendService.GetService().RemoveColumn(LoggedinUser.Email, SelectedColumn.ColumnOrdinal);
            if (resp.ErrorOccured)
                Response = resp.ErrorMessage;
            else
            {
                Board.RemoveColumn(SelectedColumn.ColumnOrdinal, LoggedinUser.Email, FilterText);
            }
        }

        /// <summary>
        /// Moves right the selected column
        /// </summary>
        public void MoveColumnRight()
        {
            Response = "";
            Response resp = BackendService.GetService().MoveColumnRight(LoggedinUser.Email, SelectedColumn.ColumnOrdinal);
            if (resp.ErrorOccured)
                Response = resp.ErrorMessage;
            else
            {
                Board.swapColumns(SelectedColumn.ColumnOrdinal, SelectedColumn.ColumnOrdinal + 1, LoggedinUser.Email, FilterText);
            }
        }

        /// <summary>
        /// Moves left the selected column
        /// </summary>
        public void MoveColumnLeft()
        {
            Response = "";
            Response resp = BackendService.GetService().MoveColumnLeft(LoggedinUser.Email, SelectedColumn.ColumnOrdinal);
            if (resp.ErrorOccured)
                Response = resp.ErrorMessage;
            else
            {
                Board.swapColumns(SelectedColumn.ColumnOrdinal - 1, SelectedColumn.ColumnOrdinal, LoggedinUser.Email, FilterText);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns true if button invoking task functions should be invoked, otherwise false and generates an apropriate message</returns>
        public bool IsViewable()
        {
            Response = "";
            bool output = SelectedTask != null;
            if (!output)
            {
                Response = "Please select a task";
                return false;
            }
           
            return output;
        }

        /// <summary>
        /// Returns true if button invoking column functions should be invoked, otherwise false and generates an apropriate message
        /// </summary>
        /// <returns></returns>
        public bool IsViewableColumnFunction()
        {
            Response = "";
            if (SelectedColumn is null)
            {
                Response = "Select a column";
                return false;
            }
            return true;
                
        }

        /// <summary>
        /// Filtering the tasks
        /// </summary>
        public void FilterTasks()
        {
            if (FilterText.Trim().Equals(""))
            {
                ClearFilter();
                return;
            }
            foreach (ColumnModel column in Board.Columns)
            {
                column.FilterTasks(FilterText);
            }
            RaisePropertyChanged("FilterDescription");
        }

        /// <summary>
        /// Clearing the filter of the tasks
        /// </summary>
        public void ClearFilter()
        {
            FilterText = "";
            foreach (ColumnModel column in Board.Columns)
            {
                column.FilterTasks(FilterText);
            }
            RaisePropertyChanged("FilterDescription");
        }
    }


}

