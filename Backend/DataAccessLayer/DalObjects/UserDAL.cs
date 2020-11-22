using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalObjects
{
    class UserDAL:DalObject
    {
        //const fields - the name of the columns in the database
        public const string UserEmailColumn = "UserEmail";
        public const string NicknameColumn = "Nickname";
        public const string PasswordColumn = "Password";
        public const string boardIdColumn = "BoardId";

        //fields + getters and setters
        private string email;
        public string Email { get => email;
            set
            {
                if (Id == -1) //if wasn't inserted to db
                    email = value;
                else //wasn't required but for future requirements
                {
                    dalC.Update(Id, UserEmailColumn, value);
                    email = value;
                }
            } }
        
        private string nickname;
        public string Nickname { get => nickname; set
            {
                if (Id == -1)
                    nickname = value;
                else
                {
                    dalC.Update(Id, NicknameColumn, value);
                    nickname = value;
                }
            }
        }

        private string password;
        public string Password { get => password; set
            {
                if (Id == -1)
                    password = value;
                else
                {
                    dalC.Update(Id, PasswordColumn, value);
                    password = value;
                }
            }
        }

        private long boardId=-1;
        public long BoardId { get => boardId;
            set
            {
                dalC.Update(Id, boardIdColumn, value);
                boardId = value;
            }
        }


        //constructors
        /// <summary>
        /// Simple constructor, will be used when creating a user in the business layer
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="nickname">user nickname</param>
        /// <param name="password">user password</param>
        public UserDAL(string email,string nickname, string password) : base(new UserDalController())
        {
            Email = email;
            Nickname = nickname;
            Password = password;
            Id = dalC.Insert(this);
        }

        /// <summary>
        /// Simple constructor, will be used when selecting users from database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <param name="nickname"></param>
        /// <param name="password"></param>
        /// <param name="boardid"></param>
        public UserDAL(long userId,string email, string nickname, string password, long boardid) : base(new UserDalController())
        {
            Email = email;
            Nickname = nickname;
            Password = password;
            Id = userId;
            this.boardId = boardid;
        }

    }
}
