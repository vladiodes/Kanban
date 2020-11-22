using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    class UserController
    {
        //fields
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("User Controller");
        
        private Dictionary<string, User> usersByEmail;
        private Dictionary<int, User> usersById;
        private User LoggedIn;

        /// <summary>
        /// Simple constructor, setting the loggedin user to be null, and creating a new dictionary
        /// </summary>
        public UserController()
        {
            LoggedIn = null;
            usersByEmail = new Dictionary<string, User>();
            usersById = new Dictionary<int, User>();
        }

        //methods


        /// <summary>
        /// Registering a new user if passed all tests and saving to DB the new user
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="nickname">user's nickname</param>
        /// <param name="password">user's password</param>
        /// <param name="boardC">BoardController for building a board for the new user</param>
        public void Register(string email, string nickname, string password, BoardController boardC)
        {
            if (email == null)
                throw new ApplicationException("Can't insert null email");
            email = email.ToLower();
            if (usersByEmail.ContainsKey(email))
            {
                throw new ApplicationException("This email is already being used by another user");
            }

            
            User toRegister = new User(email, nickname, password);
            usersByEmail[email] = toRegister;
            usersById[toRegister.ID] = toRegister;
            toRegister.setBoardId(boardC.buildBoard(toRegister.ID));
            log.Debug($"New user {email} , {nickname} , {password} was registered");

        }

        /// <summary>
        /// Registers a user that shares a board
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="nickname">user's nickname</param>
        /// <param name="password">user's password</param>
        /// <param name="emailHost">Host email to share the board with</param>
        /// <param name="boardC">BoardController for sharing a board for the new user</param>
        public void Register(string email, string password, string nickname, string emailHost, BoardController boardC)
        {
            if (emailHost == null || emailHost == "")
                throw new ApplicationException("The host email can't be empty");
            emailHost = emailHost.ToLower();
            if (!usersByEmail.ContainsKey(emailHost))
                throw new ApplicationException("There's no email matching the host email");

            if (email == null)
                throw new ApplicationException("Can't insert null email");
            email = email.ToLower();
            if (usersByEmail.ContainsKey(email))
            {
                throw new ApplicationException("This email is already being used by another user");
            }

            User toRegister = new User(email, nickname, password);
            usersByEmail[email] = toRegister;
            usersById[toRegister.ID] = toRegister;
            boardC.ShareBoard(toRegister.ID, GetId(emailHost));
            toRegister.setBoardId(boardC.getBoardID(GetId(emailHost)));
            log.Debug($"New user {email} , {nickname} , {password} was registered, sharing {emailHost}'s board");
        }

        /// <summary>
        /// Returns the email of the user identified by id
        /// </summary>
        /// <param name="userId">The user id to get the email</param>
        /// <returns>The email of the user id</returns>
        public string getEmail(int userId)
        {
            if (!usersById.ContainsKey(userId))
                return null;
            return usersById[userId].Email;
        }

        /// <summary>
        /// The function puts the loggedin user in the field if the email and the password provided match the user's email AND it prevents the option of 2 users being logged in at the same time.
        /// </summary>
        /// <param name="email">Email of the user provided</param>
        /// <param name="password">Password of the user provided</param>
        /// <returns>returns the User that logged in</returns>
        public User Login(string email, string password)
        {
            if (email == null) throw new ApplicationException("Can't insert null email");
            email = email.ToLower();
            if (!usersByEmail.ContainsKey(email)) throw new ApplicationException("You have inserted a wrong email");
            if (LoggedIn != null) throw new ApplicationException("Can't login 2 users at the same time");
            if (usersByEmail[email].Login(password)) LoggedIn = usersByEmail[email];
            else throw new ApplicationException("You have inserted a wrong password");
            log.Debug($"User {email} has logged in");
            return LoggedIn;
        }

        /// <summary>
        /// The functions logs out the logged in user if there is. If not - throws exception.
        /// </summary>
        /// <param name="email">The email of the logged in user provided</param>
        public void Logout(string email)
        {
            if (LoggedIn == null) throw new ApplicationException("No user is logged in");
            if (email == null) throw new ApplicationException("Can't insert null email");
            email = email.ToLower();
            if (LoggedIn.Email.Equals(email)) LoggedIn = null;
            else throw new ApplicationException("This user isn't logged in");
            log.Debug($"User {email} has logged out");
        }


        /// <summary>
        /// Returns the currentId logged in
        /// </summary>
        /// <returns>The id logged in, -1 if nobody is logged in</returns>
        public int currentId()
        {
            if (LoggedIn == null)
                return -1;
            return LoggedIn.ID;
        }

        /// <summary>
        /// Returns the id of a user if the user exists in the system
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>The id if exists, otherwise -1</returns>
        public int GetId(string email)
        {
            if (email == null)
                return -1;
            email = email.ToLower();
            if (!usersByEmail.ContainsKey(email))
                return -1;
            return usersByEmail[email].ID;
        }

        /// <summary>
        /// Loads the users from the DB
        /// </summary>
        public void LoadData()
        {
            UserDalController dalC = new UserDalController();
            List<UserDAL> dalusers = dalC.SelectAllUsers();
            foreach(UserDAL daluser in dalusers)
            {
                User bizUser = new User(daluser);
                usersByEmail[bizUser.Email] = bizUser;
                usersById[bizUser.ID] = bizUser;
            }
        }

        /// <summary>
        /// Deletes all users from database and from ram
        /// </summary>
        public void DeleteData()
        {
            UserDalController userC = new UserDalController();
            userC.DeleteAllData();
            LoggedIn = null;
            usersByEmail = new Dictionary<string, User>();
            usersById = new Dictionary<int, User>();
            log.Info("Deleted all users data stored in ram");
        }
    }
}
