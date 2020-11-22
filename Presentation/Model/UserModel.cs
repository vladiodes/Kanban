using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class UserModel:NotifiableObject
    {
        //fields

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                RaisePropertyChanged("Nickname");
            }
        }

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="backendUser"></param>
        public UserModel(User backendUser)
        {
            Email = backendUser.Email;
            Nickname = backendUser.Nickname;
        }
    }
}
