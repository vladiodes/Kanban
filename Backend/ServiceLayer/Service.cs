using System;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// The service for using the Kanban board.
    /// It allows executing all of the required behaviors by the Kanban board.
    /// You are not allowed (and can't due to the interfance) to change the signatures
    /// Do not add public methods\members! Your client expects to use specifically these functions.
    /// You may add private, non static fields (if needed).
    /// You are expected to implement all of the methods.
    /// Good luck.
    /// </summary>
    public class Service : IService
    {
        //fields
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Main Service");
        private UserService userService;
        private BoardService boardService;

        /// <summary>
        /// Simple public constructor.
        /// </summary>
        public Service()
        {
            userService = new UserService();
            boardService = new BoardService();
        }

        /// <summary>        
        /// Loads the data. Intended be invoked only when the program starts
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error.</returns>
        public Response LoadData()
        {
            Response response;
            try
            {
                userService.LoadData();
                boardService.LoadData();
                log.Info("All existing data was loaded successfully");
                response = new Response();
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Failed to load data", e);
                response = new Response(e.Message);
            }
            return response;
        }


        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, string nickname)
        {
            return userService.Register(email, password, nickname, boardService.boardC);
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(string email, string password)
        {
            return userService.Login(email, password);
        }

        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string email)
        {
            return userService.Logout(email);
        }

        /// <summary>
        /// Returns the board of a user. The user must be logged in
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        public Response<Board> GetBoard(string email)
        {
            return boardService.GetBoard(userService.UserC.GetId(email), userService.UserC.currentId(), userService.UserC);
        }

        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            return boardService.LimitColumnTasks(userService.UserC.GetId(email), columnOrdinal, limit, userService.UserC.currentId());
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate)
        {
            return boardService.AddTask(userService.UserC.GetId(email), title, description, dueDate, userService.UserC.currentId(), userService.UserC);
        }

        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            return boardService.UpdateTaskDueDate(userService.UserC.GetId(email), columnOrdinal, taskId, dueDate, userService.UserC.currentId());
        }

        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            return boardService.UpdateTaskTitle(userService.UserC.GetId(email), columnOrdinal, taskId, title, userService.UserC.currentId());
        }

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            return boardService.UpdateTaskDescription(userService.UserC.GetId(email), columnOrdinal, taskId, description, userService.UserC.currentId());
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            return boardService.AdvanceTask(userService.UserC.GetId(email), columnOrdinal, taskId, userService.UserC.currentId());
        }


        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<Column> GetColumn(string email, string columnName)
        {
            return boardService.GetColumn(userService.UserC.GetId(email), columnName, userService.UserC.currentId(), userService.UserC);
        }

        /// <summary>
        /// Returns a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<Column> GetColumn(string email, int columnOrdinal)
        {
            return boardService.GetColumn(userService.UserC.GetId(email), columnOrdinal, userService.UserC.currentId(), userService.UserC);
        }

        ///<summary>Remove all persistent data.</summary>
        /// <returns>Response object with an error if occured</returns>
        public Response DeleteData()
        {
            Response resp;
            try
            {
                userService.DeleteData();
                boardService.DeleteData();
                resp = new Response();
                log.Info("All data was deleted");
            }
            catch (ApplicationException e)
            {
                resp = new Response(e.Message);
                log.Warn(e.Message);
            }
            catch (Exception e)
            {
                resp = new Response(e.Message);
                log.Error("Failed to delete data", e);
            }

            return resp;
        }

        /// <summary>
        /// Removes a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>inal"></param>
        /// 
        public Response RemoveColumn(string email, int columnOrdinal)
        {
            return boardService.RemoveColumn(userService.UserC.GetId(email), userService.UserC.currentId(), columnOrdinal);
        }

        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Location to place to column</param>
        /// <param name="Name">new Column name</param>
        /// <returns>A response object with a value set to the Column, the response should contain a error message in case of an error</returns>
        public Response<Column> AddColumn(string email, int columnOrdinal, string Name)
        {
            return boardService.AddColumn(userService.UserC.GetId(email), userService.UserC.currentId(), columnOrdinal, Name, userService.UserC);
        }

        /// <summary>
        /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the columns</param>
        /// <returns>A response object with a value set to the column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnRight(string email, int columnOrdinal)
        {
            return boardService.MoveColumnRight(userService.UserC.GetId(email), userService.UserC.currentId(), columnOrdinal,userService.UserC);
        }

        /// <summary>
        /// Moves a column to the left, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the columns</param>
        /// <returns>A response object with a value set to the column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnLeft(string email, int columnOrdinal)
        {
            return boardService.MoveColumnLeft(userService.UserC.GetId(email), userService.UserC.currentId(), columnOrdinal, userService.UserC);
        }

        /// <summary>
        /// Registers a new user and joins the user to an existing board.
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <param name="emailHost">The email address of the host user which owns the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, string nickname, string emailHost)
        {
            return userService.Register(email, password, nickname, emailHost,boardService.boardC);
        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            return boardService.AssignTask(userService.UserC.GetId(email), columnOrdinal, taskId, userService.UserC.GetId(emailAssignee), userService.UserC.currentId());
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        		
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response DeleteTask(string email, int columnOrdinal, int taskId)
        {
            return boardService.DeleteTask(userService.UserC.GetId(email), columnOrdinal, taskId, userService.UserC.currentId());
        }

        /// <summary>
        /// Change the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="newName">The new name.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            return boardService.ChangeColumnName(userService.UserC.GetId(email), columnOrdinal, newName, userService.UserC.currentId());
        }
    }
}
