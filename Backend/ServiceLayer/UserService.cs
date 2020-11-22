using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {

        //fields
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("User Service");
        private UserController userController;
        public UserController UserC { get => userController; }

        //constructor
        public UserService() { userController = new UserController(); }


        /// <summary>        
        /// Loads the data. Intended be invoked only when the program starts
        /// </summary>
        public void LoadData()
        {
            userController.LoadData();
        }


        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <param name="boardC">BoardController</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, string nickname, BoardController boardC)
        {
            Response response;
            try
            {
                userController.Register(email, nickname, password, boardC);
                response = new Response();
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to register " + email + " " + nickname + " " + password, e);
                response = new Response(e.Message);
            }

            return response;
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(string email, string password)
        {
            Response<User> response;
            try
            {
                BusinessLayer.UserPackage.User registeredUser = userController.Login(email, password);
                User user = new User(registeredUser.Email, registeredUser.Nickname);
                response = new Response<User>(user);
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response<User>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to login with " + email + " " + password, e);
                response = new Response<User>(e.Message);
            }

            return response;
        }

        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string email)
        {
            Response response;
            try
            {
                userController.Logout(email);
                response = new Response();
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when inserting " + email + " to logout", e);
                response = new Response(e.Message);
            }

            return response;
        }

        /// <summary>
        /// Deletes all users data
        /// </summary>
        /// <returns>Response object</returns>
        public void DeleteData()
        {
            UserC.DeleteData();
            log.Info("All users data was deleted successfully");
        }

        /// <summary>
        /// Registers a new user and joins the user to an existing board.
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <param name="emailHost">The email address of the host user which owns the board</param>
        /// <param name="boardC">BoardController</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, string nickname, string emailHost, BoardController boardC)
        {
            Response response;
            try
            {
                userController.Register(email, password, nickname,emailHost, boardC);
                response = new Response();
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to register " + email + " " + nickname + " " + password, e);
                response = new Response(e.Message);
            }

            return response;
        }
    }
}
