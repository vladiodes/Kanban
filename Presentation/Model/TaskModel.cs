using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Windows.Media;

namespace Presentation.Model
{
    public class TaskModel:NotifiableObject
    {
        //fields 
        private const double ratioOfTimeColoredOrange = 0.75;
        private const int maxLengthRepresented= 15;

        private int _columnOrdinal;
        public int ColumnOrdinal
        {
            get => _columnOrdinal;
            set
            {
                _columnOrdinal = value;
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged("Represented");
            }
        }

        //Derermines the color of the task
        public SolidColorBrush TaskColor
        {
            get
            {
                if (DueDate.CompareTo(DateTime.Today) < 0)
                {
                    return Brushes.Red;
                }
                else
                {
                    TimeSpan timePassed = DateTime.Today.Subtract(CreationTime);
                    TimeSpan totalTime = DueDate.Subtract(CreationTime);
                    double total = totalTime.TotalDays;
                    double passed = timePassed.TotalDays;
                    if (passed / total > ratioOfTimeColoredOrange)
                        return Brushes.Orange;
                }
                return Brushes.Black;
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged("Represented");
            }
        }

        private string _assignee;
        public string Assignee
        {
            get => _assignee;
            set
            {
                _assignee = value;
                RaisePropertyChanged("TaskBorder");
            }
        }

        public int Id;

        public readonly DateTime CreationTime;

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                RaisePropertyChanged("DueDate");
                RaisePropertyChanged("TaskColor");
            }
        }

        private string _loggedInEmail;
        public string LoggedInEmail
        {
            get => _loggedInEmail;
            set
            {
                _loggedInEmail = value;
                RaisePropertyChanged("TaskBorder");
            }
        }

        //determines the border of the task
        public SolidColorBrush TaskBorder
        {
            get
            {
                return Assignee.Equals(LoggedInEmail) ? Brushes.Blue : Brushes.Transparent;
            }
        }

        //returns a string representing the task in the board
        public string Represented
        {
            get
            {
                string title = Title;
                string desc;
                if (Description is null)
                    desc = "";
                else
                {
                    if (Description.Length > maxLengthRepresented)
                        desc = Description.Substring(0, maxLengthRepresented) + "...";
                    else
                        desc = Description;
                }
                return title + "\n" + desc;
            }
        }

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="servTask"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="loggedInEmail"></param>
        public TaskModel(Task servTask, int columnOrdinal,string loggedInEmail)
        {
            Description = servTask.Description;
            Title = servTask.Title;
            Assignee = servTask.emailAssignee;
            Id = servTask.Id;
            DueDate = servTask.DueDate;
            CreationTime = servTask.CreationTime;
            ColumnOrdinal = columnOrdinal;
            LoggedInEmail = loggedInEmail;
        }

        /// <summary>
        /// Updates the description
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Response of the service</returns>
        public Response updateDescription(string value)
        {
            Response response = BackendService.GetService().UpdateTaskDescription(Assignee, ColumnOrdinal, Id, value);
            if (!response.ErrorOccured)
                Description = value;
            return response;
        }

        /// <summary>
        /// Updates the title
        /// </summary>
        /// <param name="value"></param>
        /// <returns>response of the service</returns>
        public Response updateTitle(string value)
        {
            Response response = BackendService.GetService().UpdateTaskTitle(Assignee, ColumnOrdinal, Id, value);
            if (!response.ErrorOccured)
                Title = value;
            return response;
        }

        /// <summary>
        /// Updates the due date
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Response of the service</returns>
        public Response updateDueDate(DateTime value)
        {
           
            Response response = BackendService.GetService().UpdateTaskDueDate(Assignee, ColumnOrdinal, Id, value);
            if (!response.ErrorOccured)
                DueDate = value;
            return response;
        }

        /// <summary>
        /// Updates the assignee
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Response of the service</returns>
        public Response updateAssignee(string value)
        {
            Response response = BackendService.GetService().AssignTask(Assignee, ColumnOrdinal, Id, value);
            if (!response.ErrorOccured)
                Assignee = value;
            return response;
        }
    }
}