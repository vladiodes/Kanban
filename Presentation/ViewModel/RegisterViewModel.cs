using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class RegisterViewModel:NotifiableObject
    {
        //fields
        
        private bool _isHostRegistration;
        public bool isHostRegistration
        { //defines visibility of some textboxes and labels according to the view of the window
            get => _isHostRegistration;
            set
            {
                if (value)
                    HostRegVisibility = "Visible";
                else
                    HostRegVisibility = "Hidden";
                _isHostRegistration = value;

            }
        }

        private string _HostRegVisibility;
        public string HostRegVisibility
        {
            get => _HostRegVisibility;
            set
            {
                _HostRegVisibility = value;
                RaisePropertyChanged("HostRegVisibility");
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                Response = "";
                if (value.Trim().Equals(""))
                {
                    Response = "Please enter email";
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

        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                Response = "";
                if (value.Trim().Equals(""))
                { Response = "Please enter nickname"; NickNameBorder = Brushes.Red; }
                else
                {
                    NickNameBorder = Brushes.Gray;

                }
                _nickname = value;
                RaisePropertyChanged("Nickname");
            }
        }

        private Brush _nickNameBorder = Brushes.Gray;
        public Brush NickNameBorder
        {
            get => _nickNameBorder;
            set
            {
                _nickNameBorder = value;
                RaisePropertyChanged("NickNameBorder");
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
                { Response = "Please enter password"; _passwordBorder = Brushes.Red; }
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

        private string _hostEmail;
        public string HostEmail
        {
            get => _hostEmail;
            set
            {
                Response = "";
                if (value.Trim().Equals(""))
                { Response = "Please enter host email"; HostBorder = Brushes.Red; }
                else
                {
                    HostBorder = Brushes.Gray;
    
                }
                _hostEmail = value;
                RaisePropertyChanged("HostEmail");
            }
        }
        private Brush _hostBorder = Brushes.Gray;
        public Brush HostBorder
        {
            get => _hostBorder;
            set
            {
                _hostBorder = value;
                RaisePropertyChanged("HostBorder");
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

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="isHost">true - host registration, false - regular registration</param>
        public RegisterViewModel(bool isHost)
        {
            isHostRegistration = isHost;
            if (isHost)
                HostEmail = "";
            Email = "";
            Password = "";
            Nickname = "";
            Response = "";
        }


        /// <summary>
        /// Registers the user to the system
        /// </summary>
        /// <returns> true if registeration succeeded otherwise false</returns>
        public bool Register()
        {
            if (!checkInput())
            { Response = "Not all fields were filled"; Success = ""; return false; }
            Response = "";
            Success = "";
            //host registration
            if(!isHostRegistration)
            {
                Response servResp = BackendService.GetService().Register(Email, Password, Nickname);
                if (servResp.ErrorOccured)
                {
                    Response = servResp.ErrorMessage;
                    return false;
                }
                else
                {
                    Success = "Registration succeeded";
                    return true;
                }
            }
            else //regular registration
            {
                Response servResp = BackendService.GetService().Register(Email, Password, Nickname,HostEmail);
                if (servResp.ErrorOccured)
                {
                    Response = servResp.ErrorMessage;
                    return false;
                }
                else
                {
                    Success = "Registration succeeded";
                    return true;
                }
            }
        }

        /// <summary>
        /// Checks the input - mostly checks that it isn't empty.
        /// </summary>
        /// <returns>returns true if input's valid</returns>
        private bool checkInput()
        {
            return EmailBorder != Brushes.Red && HostBorder != Brushes.Red && NickNameBorder != Brushes.Red && PasswordBorder != Brushes.Red;
        }


    }
}
