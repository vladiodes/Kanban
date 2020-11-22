using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class ColumnModel:NotifiableObject
    {
        //fields

            //the filtered tasks
        private ObservableCollection<TaskModel> _tasks;
        public ObservableCollection<TaskModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;     
            }
        }

        //the original tasks
        private List<TaskModel> originalTasks;
        public List<TaskModel> OriginalTasks
        {
            get => originalTasks;
        }

        private string _columnName;
        public string ColumnName { get => _columnName;
            set
            {
                _columnName = value;
                RaisePropertyChanged("ColumnName");
            }
        }

        private int _columnOrdinal;
        public int ColumnOrdinal
        {
            get => _columnOrdinal;
            set
            {
                _columnOrdinal = value;
                //changing task col ordinal fields
                foreach(TaskModel task in OriginalTasks)
                {
                    task.ColumnOrdinal = value;
                }
                RaisePropertyChanged("ColumnOrdinal");
            }
        }

        private int _limit;
        public int Limit
        {
            get => _limit;
            set
            {
                _limit = value;
                RaisePropertyChanged("Limit");
            }
        }

        /// <summary>
        /// Constructor - when columns are first created and filter isn't used by the user
        /// </summary>
        /// <param name="servCol"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="loggedinEmail"></param>
        public ColumnModel(Column servCol, int columnOrdinal, string loggedinEmail)
        {
            _tasks = new ObservableCollection<TaskModel>();
            originalTasks = new List<TaskModel>();
            Limit = servCol.Limit;
            ColumnOrdinal = columnOrdinal;
            ColumnName = servCol.Name;
            foreach(IntroSE.Kanban.Backend.ServiceLayer.Task servTask in servCol.Tasks)
            {
                AddTask(new TaskModel(servTask,columnOrdinal,loggedinEmail));
            }
        }

        /// <summary>
        /// Constructor - a new column is added as the user filters the task
        /// </summary>
        /// <param name="servCol"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="loggedinEmail"></param>
        /// <param name="filter"></param>
        public ColumnModel(Column servCol, int columnOrdinal, string loggedinEmail,string filter) : this(servCol, columnOrdinal, loggedinEmail)
        {
            FilterTasks(filter);
        }

        /// <summary>
        /// Checks wheter the task passes the filter, if not deletes it
        /// </summary>
        /// <param name="task"></param>
        /// <param name="filter"></param>
        public void CheckFilter(TaskModel task, string filter)
        {
            if (!(task.Title.ToLower().Contains(filter.ToLower()) || task.Description.ToLower().Contains(filter.ToLower())))
                Tasks.Remove(task);
        }

        /// <summary>
        /// Removes a task from the column
        /// </summary>
        /// <param name="task"></param>
        public void Remove(TaskModel task)
        {
            originalTasks.Remove(task);
                _tasks.Remove(task);
        }

        /// <summary>
        /// Adds a task to the column
        /// </summary>
        /// <param name="task"></param>
        /// <param name="filter"></param>
        public void AddTask(TaskModel task, string filter)
        {
            task.ColumnOrdinal = ColumnOrdinal;
            originalTasks.Add(task);
            if (task.Title.ToLower().Contains(filter.ToLower()) || task.Description.ToLower().Contains(filter.ToLower()))
                AddSortedFiltered(task);

        }
        /// <summary>
        /// Adds a task to the column
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(TaskModel task)
        {
            task.ColumnOrdinal = ColumnOrdinal;
            originalTasks.Add(task);
            AddSortedFiltered(task);
        }

        /// <summary>
        /// Adds the task to the filtered task list - adds is in the right order of due dates - earier first.
        /// </summary>
        /// <param name="task"></param>
        private void AddSortedFiltered(TaskModel task)
        {
            if (Tasks.Count == 0)
            {
                Tasks.Add(task);
                return;
            }

            if (Tasks[0].DueDate.CompareTo(task.DueDate) > 0)
            {
                Tasks.Insert(0, task);
                return;
            }

            for(int i = 0; i < Tasks.Count-1; i++)
            {
                if(task.DueDate.CompareTo(Tasks[i+1].DueDate) <= 0)
                {
                    Tasks.Insert(i+1, task);
                    return;
                }
            }
            Tasks.Add(task);
        }


        /// <summary>
        /// Gets a task that its due date has changed and places it back in the right place
        /// </summary>
        /// <param name="task">the task that has changed</param>
        public void SortFiltered(TaskModel task)
        {
            Tasks.Remove(task);
            AddSortedFiltered(task);
        }

        /// <summary>
        /// Filters the task according to an input filter by user - case insensitive
        /// </summary>
        /// <param name="filter"></param>
        public void FilterTasks(string filter)
        {
            foreach(TaskModel task in originalTasks)
            {
                if (task.Title.ToLower().Contains(filter.ToLower()) || task.Description.ToLower().Contains(filter.ToLower()))
                {
                    if (!Tasks.Contains(task))
                        AddSortedFiltered(task);
                }
                else
                {
                    if (Tasks.Contains(task))
                        Tasks.Remove(task);
                }
            }
        }

        /// <summary>
        /// Updates the name of the column
        /// </summary>
        /// <param name="value"></param>
        /// <param name="loggedin"></param>
        /// <returns>returns the response from the service</returns>
        public Response UpdateColumnName(string value, UserModel loggedin)
        {
            Response resp = BackendService.GetService().ChangeColumnName(loggedin.Email, ColumnOrdinal, value);
            if (!resp.ErrorOccured)
                ColumnName = value;
            return resp;
        }

        /// <summary>
        /// Updates the limited num of tasks in the column
        /// </summary>
        /// <param name="value"></param>
        /// <param name="loggedin"></param>
        /// <returns>The response from the service</returns>
        public Response UpdateLimit(int value, UserModel loggedin)
        {
            Response resp = BackendService.GetService().LimitColumnTasks(loggedin.Email, ColumnOrdinal, value);
            if (!resp.ErrorOccured)
                Limit = value;
            return resp;
        }

    }
}
