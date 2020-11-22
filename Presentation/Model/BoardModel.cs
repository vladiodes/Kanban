using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class BoardModel:NotifiableObject
    {
        //fields

        private ObservableCollection<ColumnModel> _columns;
        public ObservableCollection<ColumnModel> Columns
        {
            get => _columns;
            set
            {
                _columns = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceBoard">Service board</param>
        /// <param name="user">logged in user viewing the board</param>
        public BoardModel (Board serviceBoard, UserModel user)
        {
            _columns = new ObservableCollection<ColumnModel>();
            int i = 0;
            foreach(string columnName in serviceBoard.ColumnsNames)
            {
                ColumnModel column = new ColumnModel(BackendService.GetService().GetColumn(user.Email, columnName).Value,i, user.Email);
                _columns.Add(column);
                i++;
            }
            
        }

        /// <summary>
        /// Advances the task from col i to col j
        /// </summary>
        /// <param name="i">index of column the task is in</param>
        /// <param name="j">index of column to move to</param>
        /// <param name="task"></param>
        public void AdvanceTask(int i, int j, TaskModel task)
        {
            Columns[i].Remove(task);
            Columns[j].AddTask(task);
        }

        /// <summary>
        /// Removes a column
        /// </summary>
        /// <param name="columnOrdinal">the ordinal to remove</param>
        /// <param name="loggedEmail">current logged in email</param>
        /// <param name="filter">the current filter of tasks the user is currently using</param>
        public void RemoveColumn(int columnOrdinal, string loggedEmail, string filter)
        {
            int leftColumn = columnOrdinal - 1;
            int rightColumn = columnOrdinal + 1;

            if (leftColumn >= 0)
                Columns[leftColumn] = new ColumnModel(BackendService.GetService().GetColumn(loggedEmail, Math.Max(columnOrdinal-1,0)).Value, leftColumn, loggedEmail,filter);

            if (rightColumn<Columns.Count)
                Columns[rightColumn] = new ColumnModel(BackendService.GetService().GetColumn(loggedEmail, rightColumn-1).Value, rightColumn, loggedEmail,filter);

            Columns.RemoveAt(columnOrdinal);

            //changing ordinal values
            for(int i = columnOrdinal; i < Columns.Count; i++)
            {
                Columns[i].ColumnOrdinal = i;
            }

          
        }

        /// <summary>
        /// Swaping 2 columns, i<j
        /// </summary>
        /// <param name="i">index i</param>
        /// <param name="j">index j</param>
        /// <param name="filter">the current filter of tasks the user is currently using</param>
        public void swapColumns(int i, int j, string loggedEmail, string filter)
        {
            Columns[i] = new ColumnModel(BackendService.GetService().GetColumn(loggedEmail, i).Value, i, loggedEmail,filter);
            Columns[j] = new ColumnModel(BackendService.GetService().GetColumn(loggedEmail, j).Value, j, loggedEmail,filter);
        }

        /// <summary>
        /// Adds a new column to the board
        /// </summary>
        /// <param name="columnModel">column to add</param>
        public void AddColumn(ColumnModel columnModel)
        {
            Columns.Insert(columnModel.ColumnOrdinal, columnModel);
            for (int i = columnModel.ColumnOrdinal + 1; i < Columns.Count; i++)
                Columns[i].ColumnOrdinal = i;
        }
    }
}
