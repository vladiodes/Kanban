using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.BusinessLayer.TaskPackage;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardService
    {

        //fields
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Board Service");
        private BoardController boardController;
        public BoardController boardC { get => boardController; }

        //constructor

        /// <summary>
        /// Simple constructor
        /// </summary>
        public BoardService()
        {
            boardController = new BoardController();
        }

        //nethods
        /// <summary>
        /// Loads data to boards
        /// </summary>
        public void LoadData()
        {
            boardC.LoadData();
            log.Info("All existing boards were loaded from database");
        }

        /// <summary>
        /// Limits the column
        /// </summary>
        /// <param name="userid">the userid asking to do so</param>
        /// <param name="columnOrdinal">the column to limit</param>
        /// <param name="limit">the number to limit</param>
        /// <param name="currentid">the current id logged in</param>
        /// <returns>a response object</returns>
        public Response LimitColumnTasks(int userid, int columnOrdinal, int limit, int currentid)
        {
            Response response;
            try
            {
                boardController.LimitColumn(columnOrdinal, limit, userid, currentid);
                response = new Response();
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to limit column");
                response = new Response(e.Message);
            }

            return response;
        }


        /// <summary>
        /// Adding a new task
        /// </summary>
        /// <param name="userid">The userid asking to do so</param>
        /// <param name="title">The title of the task</param>
        /// <param name="description">The description</param>
        /// <param name="dueDate">Due date</param>
        /// <param name="currentid">current id logged in</param>
        /// <returns>Response with the added task if succeeded</returns>
        public Response<Task> AddTask(int userid, string title, string description, DateTime dueDate, int currentid,UserController userC)
        {
            Response<Task> response;
            try
            {
                Ttask bizTask = boardC.AddNewTask(userid, title, description, dueDate, currentid);
                Task servTask = new Task((int)bizTask.TaskID, bizTask.creationTime, bizTask.dueDate, bizTask.Title, bizTask.description,userC.getEmail(bizTask.AssigneeID));
                response = new Response<Task>(servTask);
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response<Task>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to add a task");
                response = new Response<Task>(e.Message);
            }
            return response;
        }

        /// <summary>
        /// Updating task's due date
        /// </summary>
        /// <param name="userid">Id of the user asking to update</param>
        /// <param name="columnOrdinal">Column ordinal</param>
        /// <param name="taskId">The id of the task</param>
        /// <param name="dueDate">The due date to update</param>
        /// <param name="currentid">Current id logged in</param>
        /// <returns>Response object. should contain an error message if an error occured</returns>
        public Response UpdateTaskDueDate(int userid, int columnOrdinal, int taskId, DateTime dueDate, int currentid)
        {
            Response response;
            try
            {
                boardC.UpdateTaskDueDate(userid, columnOrdinal, taskId, dueDate, currentid);
                response = new Response();
            }
            catch (ApplicationException e)
            {

                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to update task due date", e);
                response = new Response(e.Message);
            }
            return response;
        }

        /// <summary>
        /// Updating task title
        /// </summary>
        /// <param name="userid">userid asking to update</param>
        /// <param name="columnOrdinal">in which column the task is in</param>
        /// <param name="taskId">the id of the task</param>
        /// <param name="title">the new title</param>
        /// <param name="currentid">current id logged in</param>
        /// <returns></returns>
        public Response UpdateTaskTitle(int userid, int columnOrdinal, int taskId, string title, int currentid)
        {
            Response response;
            try
            {
                boardC.UpdateTaskTitle(userid, columnOrdinal, taskId, title, currentid);
                response = new Response();
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when updating task title", e);
                response = new Response(e.Message);
            }
            return response;
        }

        /// <summary>
        /// Updating task description
        /// </summary>
        /// <param name="userid">id of the user asking to update</param>
        /// <param name="columnOrdinal">Column ordinal</param>
        /// <param name="taskId">The id of the task</param>
        /// <param name="description">The description to update to</param>
        /// <param name="currentid">Current id logged in</param>
        /// <returns>Response object. should contain an error message if an error occured</returns>
        public Response UpdateTaskDescription(int userid, int columnOrdinal, int taskId, string description, int currentid)
        {
            Response response;
            try
            {
                boardC.UpdateTaskDescription(userid, columnOrdinal, taskId, description, currentid);
                response = new Response();
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when updating task description", e);
                response = new Response(e.Message);
            }
            return response;
        }
        /// <summary>
        /// Advancing a task
        /// </summary>
        /// <param name="userid">The id asking to make the advance</param>
        /// <param name="columnOrdinal">The column in which the task is in</param>
        /// <param name="taskId">The ID of the task</param>
        /// <param name="currentid">current id logged in</param>
        /// <returns>Returns a response object that should contain an error message if an error has occured</returns>
        public Response AdvanceTask(int userid, int columnOrdinal, int taskId, int currentid)
        {
            Response response;
            try
            {
                boardC.AdvanceTask(userid, columnOrdinal, taskId, currentid);
                response = new Response();
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to advance a task", e);
                response = new Response(e.Message);
            }
            return response;

        }

        /// <summary>
        /// Get a spcified column
        /// </summary>
        /// <param name="userid">The id of the user asking to do so</param>
        /// <param name="columnName">The name of the column represented in string</param>
        /// <param name="currentid">Current id logged in</param>
        /// <returns>Response with the specified column value</returns>
        public Response<Column> GetColumn(int userid, string columnName, int currentid, UserController userC)
        {
            Response<Column> response;
            try

            {
                BusinessLayer.ColumnPackage.Column bizColumn = boardC.GetColumn(userid, currentid, columnName);
                List<Task> servTasks = new List<Task>();
                foreach (Ttask bizTask in bizColumn.Tasks)
                {
                    Task servTask = new Task((int)bizTask.TaskID, bizTask.creationTime, bizTask.dueDate, bizTask.Title, bizTask.description, userC.getEmail(bizTask.AssigneeID));
                    servTasks.Add(servTask);
                }
                Column servColumn = new Column(servTasks.AsReadOnly(), columnName, bizColumn.limit);
                response = new Response<Column>(servColumn);

            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response<Column>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to get a column");
                response = new Response<Column>(e.Message);
            }
            return response;
        }

        /// <summary>
        /// Deletes the data from the controller
        /// </summary>
        public void DeleteData()
        {
            boardC.DeleteData();
            log.Info("Deleted all data from boards stored in RAM");
        }

        /// <summary>
        /// Get a spcified column
        /// </summary>
        /// <param name="userid">The id of the user asking to do so</param>
        /// <param name="columnOrdinal">The ordinal of the column</param>
        /// <param name="currentid">Current id logged in</param>
        /// <returns>Respons with the specified column value</returns>
        public Response<Column> GetColumn(int userid, int columnOrdinal, int currentid, UserController userC)
        {
            Response<Column> response;
            try
            {
                BusinessLayer.ColumnPackage.Column bizColumn = boardC.GetColumn(userid, currentid, columnOrdinal);
                List<Task> servTasks = new List<Task>();
                foreach (Ttask bizTask in bizColumn.Tasks)
                {
                    Task servTask = new Task((int)bizTask.TaskID, bizTask.creationTime, bizTask.dueDate, bizTask.Title, bizTask.description, userC.getEmail(bizTask.AssigneeID));
                    servTasks.Add(servTask);
                }
                Column servColumn = new Column(servTasks.AsReadOnly(), bizColumn.ColumnName, bizColumn.limit);
                response = new Response<Column>(servColumn);

            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response<Column>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to get a column");
                response = new Response<Column>(e.Message);
            }
            return response;
        }

        /// <summary>
        /// Gets the board of a user
        /// </summary>
        /// <param name="userid">user id asking to get</param>
        /// <param name="currentid">current id logged in</param>
        /// <returns>Response object with the board value if succeeded</returns>
        public Response<Board> GetBoard(int userid, int currentid, UserController userC)
        {
            Response<Board> response;
            try
            {
                List<string> columnNames = boardController.getBoard(userid, currentid);
                int creator = boardController.getCreator(userid, currentid);
                Board board = new Board(columnNames.AsReadOnly(), userC.getEmail(creator));
                response = new Response<Board>(board);
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response<Board>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to get board");
                response = new Response<Board>(e.Message);
            }
            return response;
        }


        /// <summary>
        /// Removes a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="userid">id of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <param name="currentid">Current id logged in</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>inal"></param>
        public Response RemoveColumn(int userid, int currentid, int columnOrdinal)
        {
            Response response;
            try
            {
                boardController.RemoveColumn(userid, currentid, columnOrdinal);
                response = new Response();
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to remove column");
                response = new Response(e.Message);
            }
            return response;
        }

        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="userid">id of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Location to place to column</param>
        /// <param name="Name">new Column name</param>
        /// <param name="currentid">Current id logged in</param>
        /// <returns>A response object with a value set to the Column, the response should contain a error message in case of an error</returns>
        public Response<Column> AddColumn(int userid, int currentid, int columnOrdinal, string Name, UserController userC)
        {
            Response<Column> response;
            try
            {
                BusinessLayer.ColumnPackage.Column bizColumn = boardController.AddColumn(userid, currentid, columnOrdinal, Name);
                List<Task> servTasks = new List<Task>();
                foreach (Ttask bizTask in bizColumn.Tasks)
                {
                    Task servTask = new Task((int)bizTask.TaskID, bizTask.creationTime, bizTask.dueDate, bizTask.Title, bizTask.description,userC.getEmail(bizTask.AssigneeID));
                    servTasks.Add(servTask);
                }
                Column servColumn = new Column(servTasks.AsReadOnly(), bizColumn.ColumnName, bizColumn.limit);
                response = new Response<Column>(servColumn);
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response<Column>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to add column " + Name, e);
                response = new Response<Column>(e.Message);
            }
            return response;
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>  
        /// <param name="CurrentIdLoggedIn">Current user logged in id</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response DeleteTask(int assignedId, int columnOrdinal, int taskId, int CurrentIdLoggedIn)
        {
            Response response;
            try
            {
                boardC.DeleteTask(assignedId, columnOrdinal, taskId, CurrentIdLoggedIn);
                response = new Response();
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to delete a task", e);
                response = new Response(e.Message);
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="newName"></param>
        /// <param name="CurrentIdLoggedIn"></param>
        /// <returns></returns>
        public Response ChangeColumnName(int userId, int columnOrdinal, string newName, int CurrentIdLoggedIn)
        {
            Response response;
            try
            {
                boardC.ChangeColumnName(userId, columnOrdinal, newName, CurrentIdLoggedIn);
                response = new Response();
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to change column name", e);
                response = new Response(e.Message);
            }
            return response;
        }

        /// <summary>
        /// Assigns a task to other user
        /// </summary>
        /// <param name="assignedId">Current id assigned</param>
        /// <param name="columnOrdinal">The order of the task it is in</param>
        /// <param name="taskId">The id of the task</param>
        /// <param name="ToAssignId">Id to assign to</param>
        /// <param name="CurrentIdLoggedIn">Current user logged in id</param>
        /// <returns></returns>
        public Response AssignTask(int assignedId, int columnOrdinal, int taskId, int ToAssignId, int CurrentIdLoggedIn)
        {
            Response response;
            try
            {
                boardC.AssignTask(assignedId, columnOrdinal, taskId, ToAssignId, CurrentIdLoggedIn);
                response = new Response();
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to assign a task", e);
                response = new Response(e.Message);
            }
            return response;
        }

        /// <summary>
        /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="userid">id of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the columns</param>
        /// <param name="currentid">Current id logged in</param>
        /// <returns>A response object with a value set to the column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnRight(int userid, int currentid, int columnOrdinal, UserController userC)
        {
            Response<Column> response;
            try
            {
                BusinessLayer.ColumnPackage.Column bizColumn = boardController.MoveColumnRight(userid, currentid, columnOrdinal);
                List<Task> servTasks = new List<Task>();
                foreach (Ttask bizTask in bizColumn.Tasks)
                {
                    Task servTask = new Task((int)bizTask.TaskID, bizTask.creationTime, bizTask.dueDate, bizTask.Title, bizTask.description,userC.getEmail(bizTask.AssigneeID));
                    servTasks.Add(servTask);
                }
                Column servColumn = new Column(servTasks.AsReadOnly(), bizColumn.ColumnName, bizColumn.limit);
                response = new Response<Column>(servColumn);
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response<Column>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to move column to right");
                response = new Response<Column>(e.Message);
            }
            return response;
        }

        /// <summary>
        /// Moves a column to the left, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column.
        /// </summary>
        /// <param name="userid">id of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the columns</param>
        /// <param name="currentid">Current id logged in</param>
        /// <returns>A response object with a value set to the column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnLeft(int userid, int currentid, int columnOrdinal,UserController userC)
        {
            Response<Column> response;
            try
            {
                BusinessLayer.ColumnPackage.Column bizColumn = boardController.MoveColumnLeft(userid, currentid, columnOrdinal);
                List<Task> servTasks = new List<Task>();
                foreach (Ttask bizTask in bizColumn.Tasks)
                {
                    Task servTask = new Task((int)bizTask.TaskID, bizTask.creationTime, bizTask.dueDate, bizTask.Title, bizTask.description,userC.getEmail(bizTask.AssigneeID));
                    servTasks.Add(servTask);
                }
                Column servColumn = new Column(servTasks.AsReadOnly(), bizColumn.ColumnName, bizColumn.limit);
                response = new Response<Column>(servColumn);
            }
            catch (ApplicationException e)
            {
                log.Warn(e.Message);
                response = new Response<Column>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Error when trying to move column to left");
                response = new Response<Column>(e.Message);
            }
            return response;
        }
    }
}
