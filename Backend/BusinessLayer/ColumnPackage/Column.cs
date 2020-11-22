using IntroSE.Kanban.Backend.BusinessLayer.TaskPackage;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace IntroSE.Kanban.Backend.BusinessLayer.ColumnPackage
{
    class Column
    {
        //fields
        //constant fields
        private const int ColumnNameMaxLength = 15;
        private const int defaultLimit = 100;

        private ColumnDAL dalCol;
        public ColumnDAL DalCol { get => dalCol; }

        private string columnName;
        public string ColumnName { get => columnName;
             set
            {
                if (!VerifyColumnName(value))
                    throw new ApplicationException("Bad column name");
                columnName = value;
                if (columnID != -1)
                    dalCol.ColumnName = value;
            }
        }

        private int columnID=-1;
        public int ColumnID { get => columnID;}

        private int columnOrdinal;
        public virtual int ColumnOrdinal { get => columnOrdinal; set
            {
                columnOrdinal = value;
                if (columnID != -1)
                    dalCol.ColumnOrdinal = value;
            }
        } 

        private int Limit;
        public int limit { get => Limit;
            private set
            {
                if (value < 0)
                    throw new ApplicationException("Can't limit the number of tasks to a negative number");
                else if (value < Tasks.Count) throw new ApplicationException("You have more than " + value + " tasks and therefore can't limit");
                Limit = value;
                if (columnID != -1)
                    dalCol.Limit = value;
            }
        }

        private List<Ttask> tasks;
        public virtual List<Ttask> Tasks { get => tasks; }

        public Column()
        {
            //for testing manners - mocking this object
        }
        /// <summary>
        /// Simple constructor - when creating a column in the business layer
        /// </summary>
        /// <param name="columnOrdinal">the order id of the column</param>
        /// <param name="columnName">the name of the column</param>
        /// <param name="boardid">The id of the board the column beints to</param>
        public Column(int columnOrdinal, string columnName,int boardid)
        {
            ColumnName = columnName;
            tasks = new List<Ttask>();
            ColumnOrdinal = columnOrdinal;
            limit = defaultLimit;
            dalCol = new ColumnDAL(columnName, columnOrdinal, defaultLimit, boardid);
            columnID = (int)dalCol.Id;
        }

        /// <summary>
        /// Simple constructor-will be used when loading columns from db
        /// </summary>
        /// <param name="dalColumn"></param>
        public Column(ColumnDAL dalColumn)
        {
            ColumnName = dalColumn.ColumnName;
            tasks = new List<Ttask>();
            limit = (int)dalColumn.Limit;
            ColumnOrdinal = (int)dalColumn.ColumnOrdinal;
            dalCol = dalColumn;
            columnID = (int)dalColumn.Id;

        }
        //methods

        /// <summary>
        /// Verifies the name of the column inserted
        /// </summary>
        /// <param name="columnName">The name of the column to check</param>
        /// <returns>True - if a valid name, else returns false</returns>
        private bool VerifyColumnName(string columnName)
        {
            if (columnName == null || columnName.Trim() == "")
                return false;
            return columnName.Length <= ColumnNameMaxLength;
        }

        /// <summary>
        ///Limits the number of tasks in the column - if given a negative number throws an exception 
        /// </summary>
        /// <param name="Limit">The new limit number</param>
        public void LimitColumnTasks(int Limit)
        {
            limit = Limit;
        }

        /// <summary>
        /// Adds the specifeid task to the tasks lists of the column
        /// </summary>
        /// <param name="task">The task to add</param>
        public virtual void AddTask(Ttask task)
        {
            tasks.Add(task);
            task.DALTask.ColumnID = ColumnID;
        }

        /// <summary>
        /// Removes the specified task from the tasks list
        /// </summary>
        /// <param name="task">The task to delete</param>
        public virtual void DeleteTask(Ttask task)
        {
            tasks.Remove(task);
        }

        /// <summary>
        /// Returns the task with the id provided
        /// </summary>
        /// <param name="taskId">Task id</param>
        /// <returns>The task if exists, if not returns null</returns>
        public virtual Ttask getTask(int taskId)
        {
            foreach(Ttask task in Tasks)
            {
                if (task.TaskID == taskId)
                    return task;
            }
            return null;
        }

        /// <summary>
        /// Checks if a task can be added to the column
        /// </summary>
        /// <returns>True - can be added, else false</returns>
        public virtual bool checkLimit()
        {
            if (Tasks.Count+1 > limit)
                return false;
            return true;
        }

        /// <summary>
        /// Checks if a certain number of tasks can be added to the column
        /// </summary>
        /// <param name="tasksToDeliver">Num of tasks to add</param>
        /// <returns>True if this is possible</returns>
        public bool checkLimit(int tasksToDeliver)
        {
            if (Tasks.Count + tasksToDeliver > limit)
                return false;
            return true;
        }

        /// <summary>
        /// Changes the column ordinal of the task (in the DAL)
        /// </summary>
        /// <param name="value">the column to change to</param>
        private void changeTasksColOrdinal(int value)
        {
            foreach(Ttask task in Tasks)
            {
                task.DALTask.ColumnID = value;
            }
        }

        /// <summary>
        /// Loads the tasks to the column
        /// </summary>
        public void LoadTasks()
        {
            TaskDalController taskC = new TaskDalController();
            List<TaskDAL> dalTasks = taskC.SelectTasksByColID(ColumnID);
            foreach (TaskDAL dalTask in dalTasks)
            {
                Ttask bizTask = new Ttask(dalTask);
                AddTask(bizTask);
            }

        }
    }
}
