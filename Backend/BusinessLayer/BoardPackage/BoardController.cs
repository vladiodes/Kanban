using IntroSE.Kanban.Backend.BusinessLayer.TaskPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer.ColumnPackage;
using System.Collections;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class BoardController
    {

        //fields
        private Dictionary<int,Board> boards;

        //constructor
        /// <summary>
        /// Simple constructor, initializing dictionary
        /// </summary>
        public BoardController()
        {
            boards = new Dictionary<int, Board>();
        }

        //methods

       

        /// <summary>
        /// Get a list of the column names in b board
        /// </summary>
        /// <param name="userId">The user id asking</param>
        /// <param name="currentLoggedinID">current logged in id</param>
        /// <returns>The list of names of columns</returns>
        public List<string> getBoard(int userId, int currentLoggedinID)
        {
            isLoggedIn(userId, currentLoggedinID);
            return boards[userId].getBoard();
        }



        /// <summary>
        /// Advances a task
        /// </summary>
        /// <param name="userId">user id asking to advance</param>
        /// <param name="columnOrdinal">Ordinal of the column the task is in</param>
        /// <param name="taskId">the id of the task to advance</param>
        /// <param name="currentId">current logged id</param>
        public void AdvanceTask(int userId, int columnOrdinal, int taskId, int currentId)
        {
            isLoggedIn(userId, currentId);
            boards[userId].AdvanceTask(columnOrdinal, taskId, userId);
        }


        /// <summary>
        /// Limits the column
        /// </summary>
        /// <param name="ColumnOrdinal">Order of the column to limit</param>
        /// <param name="limit">Number to limit</param>
        /// <param name="userId">id asking to limit</param>
        /// <param name="currentLoggedInID">current id logged in</param>
        public void LimitColumn(int ColumnOrdinal, int limit, int userId, int currentLoggedInID)
        {
            isLoggedIn(userId, currentLoggedInID);
            boards[userId].limitColumnTasks(ColumnOrdinal, limit,userId);
        }


        /// <summary>
        /// Adding a new task
        /// </summary>
        /// <param name="userid">user id</param>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        /// <param name="dueDate">Due date</param>
        /// <param name="currentuserid">current logged in id</param>
        /// <returns></returns>
        public Ttask AddNewTask(int userid, string title, string description, DateTime dueDate, int currentuserid)
        {
            isLoggedIn(userid, currentuserid);
            return boards[userid].AddNewTask(title, description, dueDate,userid); 
        }

        /// <summary>
        /// Shares the board of other user with user
        /// </summary>
        /// <param name="userid">user id that wants to share</param>
        /// <param name="otheruserid">the other user id that his board will be shared</param>
        public void ShareBoard(int userid, int otheruserid)
        {
            if (userid != -1 && otheruserid != -1)
            {
                boards[userid] = boards[otheruserid];
                boards[userid].grantAccess(userid);
            }
        }

        /// <summary>
        /// Gets a column by order
        /// </summary>
        /// <param name="userid">user id asking to get</param>
        /// <param name="currentuserid">current logged in user</param>
        /// <param name="columnOrdinal">The order of the column</param>
        /// <returns>The column</returns>
        public Column GetColumn(int userid , int currentuserid, int columnOrdinal)
        {
            isLoggedIn(userid, currentuserid);
            return boards[userid].GetColumn(columnOrdinal);
        }


        /// <summary>
        /// Gets a column by name
        /// </summary>
        /// <param name="userid">user id asking to get</param>
        /// <param name="currentuserid">current logged in user</param>
        /// <param name="columnName">The name of the column</param>
        /// <returns>The column</returns>
        public Column GetColumn(int userid, int currentuserid, string columnName)
        {
            isLoggedIn(userid, currentuserid);
            return boards[userid].GetColumn(columnName);
        }

        /// <summary>
        /// Updates the due date of a task by a given id
        /// </summary>
        /// <param name="userid">userid asking to update</param>
        /// <param name="columnOrdinal">Column in which task is in</param>
        /// <param name="taskId">Task id</param>
        /// <param name="dueDate">The due date to update</param>
        /// <param name="currentid">current id logged in</param>
        public void UpdateTaskDueDate(int userid, int columnOrdinal, int taskId, DateTime dueDate, int currentid)
        {
            isLoggedIn(userid, currentid);
            boards[userid].UpdateTaskDueDate(columnOrdinal,taskId,dueDate,userid);
        }


        /// <summary>
        /// Builds the default board - will be invoked once a new user is registered.
        /// </summary>
        /// <param name="userid">userid to build to</param>
        /// <returns>The id of the new board</returns>
        public int buildBoard(int userid)
        {
            Board built = new Board(userid);
            built.buildDefaultBoard();
            boards[userid] = built;
            return built.BoardId;
        }

        /// <summary>
        /// Remove a column
        /// </summary>
        /// <param name="userid">user id asking to remove</param>
        /// <param name="currentuserid">current id logged in</param>
        /// <param name="columnOrdinal">order of the column to remove</param>
        public void RemoveColumn(int userid, int currentuserid, int columnOrdinal)
        {
            isLoggedIn(userid, currentuserid);
            boards[userid].RemoveColumn(columnOrdinal,userid);
        }

        /// <summary>
        /// Updates The title of a task
        /// </summary>
        /// <param name="userid">user asking to update</param>
        /// <param name="columnOrdinal">column order of the task</param>
        /// <param name="taskId">task id</param>
        /// <param name="title">title to update</param>
        /// <param name="currentid">current id logged in</param>
        public void UpdateTaskTitle(int userid, int columnOrdinal, int taskId, string title, int currentid)
        {
            isLoggedIn(userid, currentid);
            boards[userid].UpdateTaskTitle(columnOrdinal, taskId, title,userid);
        }

        /// <summary>
        /// Updates the description of a given task id
        /// </summary>
        /// <param name="userid">user id asking to update</param>
        /// <param name="columnOrdinal">column order of the task</param>
        /// <param name="taskId">the task id</param>
        /// <param name="description">description to update to</param>
        /// <param name="currentid">current id logged in</param>
        public void UpdateTaskDescription(int userid, int columnOrdinal, int taskId, string description, int currentid)
        {
            isLoggedIn(userid, currentid);
            boards[userid].UpdateTaskDescription(columnOrdinal, taskId, description,userid);
        }


        /// <summary>
        /// Adding a column in the specified index
        /// </summary>
        /// <param name="userid">The id asking to add a column</param>
        /// <param name="currentuserid">The current id logged in</param>
        /// <param name="columnOrdinal">The order of the column</param>
        /// <param name="Name">The name of the column to add</param>
        /// <returns>The Column added</returns>
        public Column AddColumn(int userid, int currentuserid, int columnOrdinal, string Name)
        {
            isLoggedIn(userid, currentuserid);
            return boards[userid].AddColumn(columnOrdinal, Name,userid);
        }

        /// <summary>
        /// Moving a column to the right
        /// </summary>
        /// <param name="userid">The id asking to move</param>
        /// <param name="currentuserid">The current id logged in</param>
        /// <param name="columnOrdinal">The order of the column to move</param>
        /// <returns>The column value after the shift</returns>
        public Column MoveColumnRight(int userid, int currentuserid, int columnOrdinal)
        {
            isLoggedIn(userid, currentuserid);
            return boards[userid].MoveColumnRight(columnOrdinal,userid);
        }

        /// <summary>
        /// Moving a column to the left
        /// </summary>
        /// <param name="userid">The id asking to move</param>
        /// <param name="currentuserid">The current id logged in</param>
        /// <param name="columnOrdinal">The current order of the column</param>
        /// <returns>Column value after the movement</returns>
        public Column MoveColumnLeft(int userid, int currentuserid, int columnOrdinal)
        {
            isLoggedIn(userid, currentuserid);
            return boards[userid].MoveColumnLeft(columnOrdinal,userid);
        }

        /// <summary>
        /// Loads all the boards from the database
        /// </summary>
        public void LoadData()
        {
            BoardDalController boardC = new BoardDalController();
            List<BoardDAL> dalBoards = boardC.SelectAllBoards();
            foreach(BoardDAL board in dalBoards)
            {
                List<long> AccessingIDs = boardC.getAccessingIDs(board.Id);
                List<int> accessingIDs = new List<int>();
                foreach (long id in AccessingIDs)
                    accessingIDs.Add((int)id);
                Board bizBoard = new Board(board,accessingIDs);
                bizBoard.LoadColumns();
                foreach (int id in accessingIDs)
                    boards[id] = bizBoard;
            }        
        }

        /// <summary>
        /// Throws an exception if user id doesn't match the logged in id
        /// </summary>
        /// <param name="userid">user id</param>
        /// <param name="currentuserid">logged in id</param>
        private void isLoggedIn(int userid, int currentuserid)
        {
            if (userid == -1)
                throw new ApplicationException("No such user in the system");
            if (currentuserid == -1 || userid != currentuserid)
                throw new ApplicationException("You're not logged in");
        }

        /// <summary>
        /// Deletes all the data from data structures
        /// </summary>
        public void DeleteData()
        {
            boards = new Dictionary<int, Board>();
        }

        /// <summary>
        /// Gets the id of the board by user id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>The id of the board</returns>
        public int getBoardID(int userid)
        {
            if (userid == -1)
                return -1;
            return boards[userid].BoardId;
        }

        /// <summary>
        /// Assigns a task to other user
        /// </summary>
        /// <param name="userIdAssigned">The current user that is assigned to the task</param>
        /// <param name="columnOrdinal">The order of the column the task is in</param>
        /// <param name="taskId">The id of the task</param>
        /// <param name="userIdToAssign">User to assign to</param>
        /// <param name="currentuserid">current id of logged in user</param>
        public void AssignTask(int userIdAssigned, int columnOrdinal, int taskId, int userIdToAssign, int currentuserid)
        {
            isLoggedIn(userIdAssigned, currentuserid);
            boards[userIdAssigned].AssignTask(userIdAssigned, columnOrdinal, taskId, userIdToAssign);
        }

        /// <summary>
        /// Deletes a task
        /// </summary>
        /// <param name="userIdAssigned">user id the task is assigned to</param>
        /// <param name="columnOrdinal">The order of the column the task is in</param>
        /// <param name="taskId">The id of the task</param>
        /// <param name="currentuserid">current id of logged in user</param>
        public void DeleteTask(int userIdAssigned, int columnOrdinal, int taskId, int currentuserid)
        {
            isLoggedIn(userIdAssigned, currentuserid);
            boards[userIdAssigned].DeleteTask(userIdAssigned, columnOrdinal, taskId);
        }

        /// <summary>
        /// Gets the creator of a specific board
        /// </summary>
        /// <param name="userid">user id</param>
        /// <param name="currentid">current id logged in</param>
        /// <returns>The id of the creator</returns>
        public int getCreator(int userid, int currentid)
        {
            isLoggedIn(userid, currentid);
            return boards[userid].CreatorID;
        }

        /// <summary>
        /// Changes the name of a specific column
        /// </summary>
        /// <param name="userid">The id of the user asking to change</param>
        /// <param name="columnOrdinal">The order of the column to change</param>
        /// <param name="newName">The new name to change to</param>
        /// <param name="currentuserid">Current id of logged in user</param>
        public void ChangeColumnName(int userid, int columnOrdinal, string newName,int currentuserid)
        {
            isLoggedIn(userid, currentuserid);
            boards[userid].ChangeColumnName(userid, columnOrdinal, newName);
        }
    }
}
