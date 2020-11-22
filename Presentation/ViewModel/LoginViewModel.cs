using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Presentation.Model;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation.ViewModel
{
    class LoginViewModel : NotifiableObject
    {
        //fields
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                Response = "";
                if (value.Trim().Equals(""))
                {
                    Response = "Please insert email";
                    EmailBorder = Brushes.Red;
                }
                else
                {
                    EmailBorder = Brushes.Gray;
                }
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        private Brush _emailBorder = Brushes.Gray;
        public Brush EmailBorder
        {
            get => _emailBorder;
            set
            {
                _emailBorder = value;
                RaisePropertyChanged("EmailBorder");
            }
        }


        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                Response = "";
                if (value.Trim().Equals(""))
                {
                    Response = "Please insert password";
                    PasswordBorder = Brushes.Red;
                }
                else
                {
                    PasswordBorder = Brushes.Gray;
                }
                _password = value;
                RaisePropertyChanged("Password");
            }
        }

        private Brush _passwordBorder = Brushes.Gray;
        public Brush PasswordBorder
        {
            get => _passwordBorder;
            set
            {
                _passwordBorder = value;
                RaisePropertyChanged("PasswordBorder");
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

        public LoginViewModel()
        {
            Email = "";
            Password = "";
            Response = "";
        }

        /// <summary>
        /// Logs the user in
        /// </summary>
        /// <returns>User if login succeeded, otherwise null</returns>
        public UserModel Login()
        {
            if (!checkInput())
            {
                Response = "Not all fields were filled";
                return null;
            }
            Response = "";
            Response<User> backUser = BackendService.GetService().Login(Email, Password);
            if (backUser.ErrorOccured)
            {
                Response = backUser.ErrorMessage;
                return null;
            }
            return new UserModel(backUser.Value);
        }

        /// <summary>
        /// Checks wheter input was inserted
        /// </summary>
        /// <returns>True if was, otherwise false</returns>
        private bool checkInput()
        {
            return PasswordBorder != Brushes.Red && EmailBorder != Brushes.Red;
        }
    }
}
