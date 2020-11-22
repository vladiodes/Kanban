using Presentation.Model;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class ColumnUpdateViewModel:NotifiableObject
    {
        //fields

        private ColumnModel _column;
        private UserModel _loggedin;
        private BoardModel _currentBoard;

        private int _colOrdinaltoAdd;


        private bool _isAddView;
        public bool IsAddView
        {
            get => _isAddView;
            set //sets the window visibilty according to action - insert or update
            {
                if (value)
                {
                    IsAdd = "Visible";
                    IsUpdate = "Hidden";
                }
                else
                {
                    IsAdd = "Hidden";
                    IsUpdate = "Visible";
                }
                _isAddView = value;
            }

        }

        private string _isAdd;
        public string IsAdd
        {
            get => _isAdd;
            set
            {
                _isAdd = value;
                RaisePropertyChanged("IsAdd");
            }
        }

        private string _isUpdate;
        public string IsUpdate
        {
            get => _isUpdate;
            set
            {
                _isUpdate = value;
                RaisePropertyChanged("IsUpdate");
            }
        }


        public string Title
        {
            get => "Change textbox where you'd like to make changes";
        }

        private string _columnName="";
        public string ColumnName
        {
            get => _columnName;
            set
            {
                if (value.Trim().Equals(""))
                {
                    Response ="Can't insert empty name";
                    ColumnNameBorder = Brushes.Red;
                }
                else
                {
                    Response = "";
                    ColumnNameBorder = Brushes.Gray;
                }
                _columnName = value;
                RaisePropertyChanged("ColumnName");


            }
        }


        private string _limit;
        public string Limit
        {
            get => _limit;
            set
            {
                int number;
                try
                {
                    number = int.Parse(value);
                    if (number < 0)
                    {
                        LimitBorder = Brushes.Red;
                        Response ="Please insert a positive number, or 0";
                    }
                    else
                    {
                        LimitBorder = Brushes.Gray;
                        Response = "";
                    }
                        
                }
                catch(Exception e)
                {
                    LimitBorder=Brushes.Red;
                    Response ="Please insert a number";
                }
                

                _limit = value;
                RaisePropertyChanged("Limit");
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

        private string _success;
        public string Success
        {
            get => _success;
            set
            {
                _success = value;
                RaisePropertyChanged("Success");
            }
        }

        private Brush _columnNameBorder = Brushes.Gray;
        public Brush ColumnNameBorder
        {
            get => _columnNameBorder;
            set
            {
                _columnNameBorder = value;
                RaisePropertyChanged("ColumnNameBorder");
            }
        }

        private Brush _limitBorder = Brushes.Gray;
        public Brush LimitBorder
        {
            get => _limitBorder;
            set
            {
                _limitBorder = value;
                RaisePropertyChanged("LimitBorder");
            }
        }

        /// <summary>
        /// Constructor - update view
        /// </summary>
        /// <param name="column">column to update</param>
        /// <param name="user">logged in user</param>
        public ColumnUpdateViewModel(ColumnModel column, UserModel user)
        {
            _column = column;
            _loggedin = user;
            IsAddView = false;
            Limit = "" + column.Limit;
            ColumnName = column.ColumnName;
        }

        /// <summary>
        /// Constructor - insertion view
        /// </summary>
        /// <param name="user">logged in user</param>
        /// <param name="colOrdinalToAdd">Where to add new column</param>
        /// <param name="board">current board working on</param>
        public ColumnUpdateViewModel(UserModel user, int colOrdinalToAdd, BoardModel board)
        {
            _colOrdinaltoAdd = colOrdinalToAdd;
            _loggedin = user;
            _currentBoard = board;
            IsAddView = true;
            ColumnName = "";
        }

        /// <summary>
        /// Adds the column to the board
        /// </summary>
        /// <returns>true if was added, otherwise false</returns>
        public bool AddColumn()
        {
            Success = "";
            if (ColumnNameBorder.Equals(Brushes.Red))
            {
                Response = "Column Name field is still wrong";
                return false;
            }
            Response = "";
            Success = "";
            Response<Column> resp=BackendService.GetService().AddColumn(_loggedin.Email, _colOrdinaltoAdd, ColumnName);
            if (resp.ErrorOccured)
                Response = resp.ErrorMessage;
            else
            {
                _currentBoard.AddColumn(new ColumnModel(resp.Value, _colOrdinaltoAdd,_loggedin.Email));
            }
            return !resp.ErrorOccured;
        }
        /// <summary>
        /// Updates the column
        /// </summary>
        public void Update()
        {
            Success = "";
            if (LimitBorder.Equals(Brushes.Red) || ColumnNameBorder.Equals(Brushes.Red))
            {
                Response = "Some fields are still wrong";
                return;
            }
                
            Response = "";
            
            if (int.Parse(Limit) != _column.Limit)
            {
                Response resp = _column.UpdateLimit(int.Parse(Limit), _loggedin);
                if (resp.ErrorOccured)
                    Response = Response + resp.ErrorMessage;
                else
                {
                    Success =Success + "Limit updated";
                }
            }

            if (ColumnName != _column.ColumnName)
            {
                    Response resp = _column.UpdateColumnName(ColumnName, _loggedin);
                    if (resp.ErrorOccured)
                        Response = Response + "\n" + resp.ErrorMessage;

                    else
                    {
                        Success = Success + "\n" + "Column name updated";
                    }
            }
            if (Success.Equals("") && Response.Equals(""))
                Response = "No changes were made";
        }
    }
}
