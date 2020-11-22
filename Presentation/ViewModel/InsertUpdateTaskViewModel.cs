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
    class InsertUpdateTaskViewModel:NotifiableObject
    {
        //fields

        private TaskModel _task;
        private UserModel _loggedin;
        private ColumnModel _column;
        private string _currentFilter;
        
        //stands for insert/update view of the window
        private bool _isUpdate;
        public bool IsUpdate { get => _isUpdate;
            set
            { //sets the visibility of the window according to the view
                if (value)
                {
                    UpdateVisibility = "Visible";
                    AddVisibility = "Hidden";
                }


                else {
                    AddVisibility = "Visible";
                    UpdateVisibility = "Hidden";
                }
                    
                _isUpdate = value;
            }
        }

        private string _assignee;
        public string Assignee
        {
            get => _assignee;
            set
            {
                Response = "";
                if (value.Trim().Equals(""))
                {
                    Response = "Can't insert empty assignee";
                    AssigneeBorder = Brushes.Red;
                }
                else
                {
                    _assignee = value;
                    RaisePropertyChanged("Assignee");
                    AssigneeBorder = Brushes.Gray;
                }
            }
        }

        private Brush _assigneeBorder=Brushes.Gray;
        public Brush AssigneeBorder
        {
            get => _assigneeBorder;
            set
            {
                _assigneeBorder = value;
                RaisePropertyChanged("AssigneeBorder");
            }
        }

        //Visibility of some textboxes if wants to insert task
        private string _addVisibility;
        public string AddVisibility
        {
            get => _addVisibility;
            set
            {
                _addVisibility = value;
                RaisePropertyChanged("AddVisibility");
            }
        }

        //visibility of some textboxes and buttons if wants to update a task
        private string _updateVisibility;
        public string UpdateVisibility
        {
            get => _updateVisibility;
            set
            {
                _updateVisibility = value;
                RaisePropertyChanged("UpdateVisibility");
            }
        }
        private string _title;
        public string Title
        {
            get => _title;

            set
            {
                Response = "";
                if (value.Trim().Equals("") || value.Equals(""))
                {
                    Response = "Can't insert empty title";
                    TitleBorder = Brushes.Red;
                }
                else
                    TitleBorder = Brushes.Gray;
                _title = value;
                RaisePropertyChanged("Title");


            }
        }

        private Brush _titleBorder = Brushes.Gray;
        public Brush TitleBorder
        {
            get => _titleBorder;
            set
            {
                _titleBorder = value;
                RaisePropertyChanged("TitleBorder");
            }
        }

        private string _description;
        public string Description
        {
            get => _description;

            set
            {
                Response = "";
                _description = value;
                RaisePropertyChanged("Description");
                DescriptionBorder = Brushes.Gray;

            }
        }

        private Brush _descriptionBorder = Brushes.Gray;
        public Brush DescriptionBorder
        {
            get => _descriptionBorder;
            set
            {
                _descriptionBorder = value;
                RaisePropertyChanged("DescriptionBorder");
            }
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;

            set
            {
                Response = "";
                if (value.CompareTo(DateTime.Today) < 0)
                {
                    Response = "Due date has passed";
                    DueDateBorder = Brushes.Red;
                }
                else
                {
                    DueDateBorder = Brushes.Gray;
                }
                _dueDate = value;
                RaisePropertyChanged("DueDate");

            }
        }

        private Brush _dueDateBorder = Brushes.Gray;
        public Brush DueDateBorder
        {
            get => _dueDateBorder;
            set
            {
                _dueDateBorder = value;
                RaisePropertyChanged("DueDateBorder");
            }
        }


        private string _response;
        public string Response
        {
            get => _response;
            set
            {
                _response = value;
                RaisePropertyChanged("Response");
            }
        }

        private string _success = "";
        public string Success
        {
            get => _success;
            set
            {
                _success = value;
                RaisePropertyChanged("Success");
            }
        }

        //Constructor
        public InsertUpdateTaskViewModel(TaskModel task, bool isUpdate, UserModel _loggedin, ColumnModel column, string currentFilter)
        {
            _column = column;
            _task = task;
            IsUpdate = isUpdate;
            this._loggedin = _loggedin;
            _currentFilter = currentFilter;
            if (IsUpdate) //update view
            {
                Title = _task.Title;
                Assignee = _task.Assignee;
                DueDate = _task.DueDate;
                Description = _task.Description;
            }
            else //insertion view
            {
                DueDate = DateTime.Today;
                Description = "";
                Title = "";
                
            }
        }
        
        /// <summary>
        /// Adds the task to the column
        /// </summary>
        /// <returns>true if was added, otherwise false</returns>
        public bool Add()
        {

            Success = "";
            if (!checkInuput())
            { Response = "Not all fields are correct"; return false; }

            Response = "";
            Response<IntroSE.Kanban.Backend.ServiceLayer.Task> resp = BackendService.GetService().AddTask(_loggedin.Email, Title, Description, DueDate);
            if (resp.ErrorOccured)
            {
                Response = resp.ErrorMessage;
            }
            else
            {
                _task = new TaskModel(resp.Value, _column.ColumnOrdinal, _loggedin.Email);
                _column.AddTask(_task,_currentFilter);
            }
            return !resp.ErrorOccured;

        }

        public void Update()
        {
            Success = "";
            if (!checkInuput())
            { Response = "Not all fields are correct"; return; }
            Response = "";
            Success = "";
            //assignee update
            if (!_task.Assignee.Equals(Assignee))
            {
                Response resp = _task.updateAssignee(Assignee);
                if (resp.ErrorOccured)
                {
                    Response = resp.ErrorMessage;
                    AssigneeBorder = Brushes.Red;
                }
                else
                {
                    Success = "Assignee updated";
                    AssigneeBorder = Brushes.Gray;
                }
            }
            //title update
            if (!_task.Title.Equals(Title))
            {
                Response resp = _task.updateTitle(Title);
                if (resp.ErrorOccured)
                {
                    Response = Response + "\n" + resp.ErrorMessage;
                    TitleBorder = Brushes.Red;
                }
                else
                {
                    Success = Success + "\n" + "Title updated";
                    _column.CheckFilter(_task, _currentFilter);
                    TitleBorder = Brushes.Gray;
                }
            }
            //description update
            if (!_task.Description.Equals(Description))
            {
                Response resp = _task.updateDescription(Description);
                if (resp.ErrorOccured)
                { Response = Response + "\n" + resp.ErrorMessage; DescriptionBorder=Brushes.Red; }
                else
                {
                    Success = Success + "\n" + "Description updated";
                    _column.CheckFilter(_task, _currentFilter);
                    DescriptionBorder= Brushes.Gray;
                }
            }
            //due date update
            if (!_task.DueDate.Equals(DueDate))
            {
                Response resp = _task.updateDueDate(DueDate);
                if (resp.ErrorOccured)
                { Response = Response + "\n" + resp.ErrorMessage; DueDateBorder = Brushes.Red; }
                else
                {
                    Success = Success + "\n" + "Due date updated";
                    _column.SortFiltered(_task);
                    DueDateBorder= Brushes.Gray;
                }
            }
            if (Response.Equals("") && Success.Equals(""))
                Response = "Nothing was changed";
        }

        /// <summary>
        /// Checks whether all inputs are valid
        /// </summary>
        /// <returns>true for valid input otherwise false</returns>
        private bool checkInuput()
        {
            return DescriptionBorder != Brushes.Red && TitleBorder != Brushes.Red && DueDateBorder != Brushes.Red && AssigneeBorder != Brushes.Red;
        }
        
    }
}
