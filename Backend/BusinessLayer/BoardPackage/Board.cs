using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer.ColumnPackage;
using IntroSE.Kanban.Backend.BusinessLayer.TaskPackage;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Board
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BusinessBoard");

        private const int MinimalColumns = 2;
        private const int leftmost = 0;
        private string[] defaultColumns = { "backlog", "in progress", "done" };
        //fields

        private int boardid=-1;
        public int BoardId { get => boardid; }

        private int creatorID;
        public int CreatorID { get => creatorID; }

        private List<Column> columnsByOrder;
        public List<Column> ColumnsByOrder { get => columnsByOrder; }

        private Dictionary<string, Column> columnsByNames;
        public Dictionary<string,Column> ColumnsByNames { get => columnsByNames; }

        private List<int> accessingIDs;

        

        private BoardDAL dalBoard;

        //constructors
        /// <summary>
        /// Simple constructor - for creating boards in business layer
        /// </summary>
        /// <param name="creatorID">the id of the creator of the board</param>
        public Board(int creatorID)
        {
            columnsByOrder = new List<Column>();
            columnsByNames = new Dictionary<string, Column>();
            this.creatorID = creatorID;
            dalBoard = new BoardDAL(creatorID);
            boardid = (int)dalBoard.Id;
            accessingIDs = new List<int>();
            accessingIDs.Add(creatorID);
        }

        /// <summary>
        /// Simple constructor - for creating boards when loading from db
        /// </summary>
        /// <param name="dalBoard">BoardDAL</param>
        public Board(BoardDAL dalBoard, List<int> AccessingIDs)
        {
            columnsByOrder = new List<Column>();
            columnsByNames = new Dictionary<string, Column>();
            this.creatorID = (int)dalBoard.CreatorID;
            this.dalBoard = dalBoard;
            boardid = (int)dalBoard.Id;
            accessingIDs = AccessingIDs;
        }
        //methods

        /// <summary>
        /// Gets the column names of the board
        /// </summary>
        /// <returns>A list of the names</returns>
        public List<string> getBoard()
        {
            List<string> columnsNames = new List<string>();
            foreach (Column column in columnsByOrder)
            {
                columnsNames.Add(column.ColumnName);
            }
            return columnsNames;
        }

        /// <summary>
        /// Advances the task to the next column
        /// </summary>
        /// <param name="columnOrdinal">The ordinal of the column in which the task is in</param>
        /// <param name="taskId">Task id to move</param>
        /// <param name="userid">The userid that wants to advance</param>
        public void AdvanceTask(int columnOrdinal, int taskId,int userid)
        {
            Ttask task = GetTask(taskId, columnOrdinal);
            task.isAssignee(userid);
            if (!columnsByOrder.ElementAt(columnOrdinal+1).checkLimit())
                throw new ApplicationException("You can't advance to the next column because you've reached to limit in this column");

            columnsByOrder.ElementAt(columnOrdinal).DeleteTask(task);
            columnsByOrder.ElementAt(columnOrdinal+1).AddTask(task);

            log.Info("Task was advanced successfully");
        }

        /// <summary>
        /// Limits the column
        /// </summary>
        /// <param name="columnOrdinal">The order of the column</param>
        /// <param name="limit">Number to limit to</param>
        public void limitColumnTasks(int columnOrdinal, int limit, int userid)
        {
            isCreator(userid);
            isWithinRange(columnOrdinal);
            columnsByOrder.ElementAt(columnOrdinal).LimitColumnTasks(limit);
        }

        /// <summary>
        /// Throws an exception if the user id isn't the creator
        /// </summary>
        /// <param name="userid">user id</param>
        private void isCreator(int userid)
        {
            if (userid != creatorID)
                throw new ApplicationException("You aren't the creator of this board");
        }

        /// <summary>
        /// Adding a new task to the board
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        /// <param name="dueDate">Due date</param>
        /// <param name="userid">The user id adding the task</param>
        /// <returns>The task that was added</returns>
        public Ttask AddNewTask(string title, string description, DateTime dueDate,int userid)
        {
            if (!columnsByOrder.ElementAt(leftmost).checkLimit())
                throw new ApplicationException("You can't create more tasks, you've reached the limit");
            Ttask task = new Ttask(title, description, dueDate, columnsByOrder[leftmost].ColumnID,userid);
            columnsByOrder.ElementAt(leftmost).AddTask(task);
            log.Debug($"A task of to board {BoardId} has been added and saved to database");
            return task;
        }

        /// <summary>
        /// Updates the due date of a task
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="dueDate"></param>
        /// <param name="userid">User id asking to make the update</param>
        public void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate, int userid)
        {
            GetTask(taskId, columnOrdinal).UpdateTaskDueDate(dueDate,userid);
        }

        /// <summary>
        /// Gets a column by order
        /// </summary>
        /// <param name="columnOrdinal">The order</param>
        /// <returns>The wanted column</returns>
        public Column GetColumn(int columnOrdinal)
        {
            isWithinRange(columnOrdinal);
            return columnsByOrder.ElementAt(columnOrdinal);
        }

        /// <summary>
        /// Gets a column by name
        /// </summary>
        /// <param name="columnName">The name of the column</param>
        /// <returns>The wanted column</returns>
        public Column GetColumn(string columnName)
        {
            if (!columnsByNames.ContainsKey(columnName))
                throw new ApplicationException("No such column with this name");
            return columnsByNames[columnName];
        }

        /// <summary>
        /// Gets a task to make changes/advance to
        /// </summary>
        /// <param name="taskId">The id of the task</param>
        /// <param name="columnOrdinal">Order of the column the task is in</param>
        /// <returns>The task itself</returns>
        public Ttask GetTask(int taskId, int columnOrdinal)
        {
            isWithinRange(columnOrdinal);

            if (columnOrdinal == RightMost())
                throw new ApplicationException("You can't make changes to a task in the rightmost column");

            if (columnsByOrder.ElementAt(columnOrdinal).Tasks.Count == 0)
                throw new ApplicationException("You don't have any tasks in this column");

            Ttask task = columnsByOrder.ElementAt(columnOrdinal).getTask(taskId);

            if (task == null)
                throw new ApplicationException("No such task in this column");

            return task;
        }

        /// <summary>
        /// Updates the title of a task
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        /// <param name="userid">User id asking to make the update</param>
        public void UpdateTaskTitle(int columnOrdinal, int taskId, string title, int userid)
        {
            GetTask(taskId, columnOrdinal).UpdateTaskTitle(title,userid);
        }

        /// <summary>
        /// Building the default board for a new registered user
        /// </summary>
        public void buildDefaultBoard()
        {
            for (int i = 0; i < defaultColumns.Length; i++)
            {
                Column column = new Column(i, defaultColumns[i], boardid);
                columnsByOrder.Add(column);
                columnsByNames[defaultColumns[i]] = column;
            }
        }

        /// <summary>
        /// Updates the description of a task
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="description"></param>
        /// <param name="userid">The userid asking to update</param>
        public void UpdateTaskDescription(int columnOrdinal, int taskId, string description,int userid)
        {
            GetTask(taskId, columnOrdinal).UpdateTaskDescription(description,userid);
        }

        /// <summary>
        /// Removes a column
        /// </summary>
        /// <param name="columnOrdinal">The order of the column to remove</param>
        /// <param name="userid"></param>
        public void RemoveColumn(int columnOrdinal,int userid)
        {
            isCreator(userid);
            isWithinRange(columnOrdinal);

            if (columnsByOrder.Count <= MinimalColumns)
                throw new ApplicationException($"You must have at least {MinimalColumns} columns");


            Column toRemove = columnsByOrder.ElementAt(columnOrdinal);

            if (columnOrdinal == leftmost) //need to delete the leftmost column
            {
                Column TasksToMoveTo = columnsByOrder.ElementAt(leftmost + 1);
                if (!TasksToMoveTo.checkLimit(toRemove.Tasks.Count))
                    throw new ApplicationException("Can't delete this column because can't deliever the tasks to the next column");
                foreach (Ttask task in toRemove.Tasks)
                {
                    TasksToMoveTo.AddTask(task);
                }
            }
            else //need to delete a column which has a column on its left
            {
                Column TasksToMoveTo = columnsByOrder.ElementAt(columnOrdinal - 1);
                if (!TasksToMoveTo.checkLimit(toRemove.Tasks.Count))
                    throw new ApplicationException("Can't delete this column because can't deliever the tasks to the previous column");
                foreach (Ttask task in toRemove.Tasks)
                {
                    TasksToMoveTo.AddTask(task);
                }
            }

            //updating columns ordinal field.
            for (int i = columnOrdinal + 1; i <= RightMost(); i++)
            {
                columnsByOrder.ElementAt(i).ColumnOrdinal--;
            }


            //deleting the column from data structures
            columnsByNames[toRemove.ColumnName].DalCol.delete();
            columnsByNames.Remove(toRemove.ColumnName);
            columnsByOrder.Remove(toRemove);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userIdAssigned"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="userIdToAssign"></param>
        public void AssignTask(int userIdAssigned, int columnOrdinal, int taskId, int userIdToAssign)
        {
            if (!accessingIDs.Contains(userIdToAssign))
                throw new ApplicationException("Can't assign to this user because he doesn't have access to the same board");
           GetTask(taskId,columnOrdinal).Assign(userIdAssigned, userIdToAssign);
        }

        /// <summary>
        /// Adds a new column
        /// </summary>
        /// <param name="columnOrdinal">The order where to add</param>
        /// <param name="Name">The name of the new column</param>
        /// <param name="userid">user id</param>
        /// <returns>The added column</returns>
        public Column AddColumn(int columnOrdinal, string Name,int userid)
        {
            isCreator(userid);
            if (!(columnOrdinal >= leftmost && columnOrdinal <= RightMost()+1))
                throw new ApplicationException("Can't add a column in this column ordinal");

            if (Name!=null && columnsByNames.ContainsKey(Name))
                throw new ApplicationException("You already have a column with this name");

            Column toAdd = new Column(columnOrdinal, Name, BoardId);
            if (columnOrdinal == RightMost()+1) //we want to insert the new column at the rightmost place
                columnsByOrder.Insert(columnOrdinal, toAdd);

            //if we want to insert in the middle
            else
            {
                //updating ordinal fields +1
                for (int i = columnOrdinal; i < columnsByOrder.Count; i++)
                {
                    columnsByOrder.ElementAt(i).ColumnOrdinal++;
                }
                columnsByOrder.Insert(columnOrdinal, toAdd);
            }
            //adding to the data structure by name keys
            columnsByNames[Name] = toAdd;

            return toAdd;
        }

        /// <summary>
        /// Changes the name of the column
        /// </summary>
        /// <param name="userid">User id asking to change the name</param>
        /// <param name="columnOrdinal">The order of the column</param>
        /// <param name="newName">The new name to change to</param>
        public void ChangeColumnName(int userid, int columnOrdinal, string newName)
        {
            isCreator(userid);
            isWithinRange(columnOrdinal);
            Column columnToChange = columnsByOrder[columnOrdinal];
            string oldName = columnToChange.ColumnName;
            columnsByOrder[columnOrdinal].ColumnName = newName;
            columnsByNames.Remove(oldName);
            columnsByNames[columnToChange.ColumnName] = columnToChange;
        }

        /// <summary>
        /// Deletes a task
        /// </summary>
        /// <param name="userIdAssigned">user id assinged to the task</param>
        /// <param name="columnOrdinal">The order of the column the task is in</param>
        /// <param name="taskId">Task id</param>
        public void DeleteTask(int userIdAssigned, int columnOrdinal, int taskId)
        {
            Ttask taskToDelete = GetTask(taskId, columnOrdinal);
            taskToDelete.DeleteTask(userIdAssigned);
            columnsByOrder[columnOrdinal].DeleteTask(taskToDelete);
        }

        /// <summary>
        /// Moves a column to the right
        /// </summary>
        /// <param name="columnOrdinal">The order of the column to move</param>
        /// <param name="userid">user id</param>
        /// <returns>The column shifted</returns>
        public Column MoveColumnRight(int columnOrdinal, int userid)
        {
            isCreator(userid);
            if (columnOrdinal < leftmost || columnOrdinal >= RightMost())
                throw new ApplicationException("No negative ordinal, or rightmost column can't be moved to the right");

            //swaping the columns and updating ordinal fields
            swapColumns(columnOrdinal, columnOrdinal + 1);

            return columnsByOrder.ElementAt(columnOrdinal + 1);
        }

        /// <summary>
        /// Moves a column to the left
        /// </summary>
        /// <param name="columnOrdinal">The order of the column to move</param>
        /// <param name="userid">user id</param>
        /// <returns>The column shifted</returns>
        public Column MoveColumnLeft(int columnOrdinal, int userid)
        {
            isCreator(userid);
            if (columnOrdinal <= leftmost || columnOrdinal > RightMost())
                throw new ApplicationException("Column doesn't exist or either left most or negative ordinal");

            //swaping the columns and updating ordinal fields
            swapColumns(columnOrdinal, columnOrdinal - 1);

            return columnsByOrder[columnOrdinal - 1];
        }

        /// <summary>
        /// Swaps between 2 columns
        /// </summary>
        /// <param name="i">ordinal i</param>
        /// <param name="j">ordinal j</param>
        private void swapColumns(int i, int j)
        {
            Column temp = columnsByOrder.ElementAt(i);
            columnsByOrder[i] = columnsByOrder[j];
            columnsByOrder[j] = temp;
            temp.ColumnOrdinal = j;
            columnsByOrder[i].ColumnOrdinal = i;
        }

        /// <summary>
        /// Loads all columns of the board
        /// </summary>
        public void LoadColumns()
        {
            ColumnDalController columnC = new ColumnDalController();
            //load all columns of this board
            List<ColumnDAL> columns = columnC.SelectColumnsByBoardID(BoardId);
            foreach(ColumnDAL dalColumn in columns)
            {
                Column bizcol = new Column(dalColumn);

                //load all tasks belonging to this column
                bizcol.LoadTasks();
                columnsByOrder.Add(bizcol);
                columnsByNames[bizcol.ColumnName] = bizcol;
            }
        }

        /// <summary>
        /// The ordinal of the rightmost column
        /// </summary>
        /// <returns>Returns the order of the rightmost column</returns>
        private int RightMost()
        {
            return columnsByOrder.Count - 1;
        }

        /// <summary>
        /// Throws an exception if the column isn't within the legal range
        /// </summary>
        /// <param name="columnOrdinal"></param>
        private void isWithinRange(int columnOrdinal)
        {
            if (columnOrdinal < leftmost | columnOrdinal > RightMost())
                throw new ApplicationException("This column isn't within the range");
        }

        /// <summary>
        /// Grants access to a user to the board
        /// </summary>
        /// <param name="userid">userid to grant access to</param>
        public void grantAccess(int userid)
        {
            accessingIDs.Add(userid);
        }
    }
}
