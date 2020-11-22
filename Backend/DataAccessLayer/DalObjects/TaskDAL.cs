using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalObjects
{
    class TaskDAL:DalObject
    {
        
        public const string TaskTitleColumn = "Title";
        public const string TaskCreationTimeColumn = "CreationTime";
        public const string TaskDueDateColumn = "DueDate";
        public const string TaskDescriptionColumn = "Description";
        public const string TaskColumnIdColumn = "columnID";
        public const string TaskAssigneeColumn = "AssigneeID";

        //fields - getters and setters
        private string title;
        public string Title
        {
            get => title; set
            {
                if (Id == -1)
                    title = value;
                else
                {
                    dalC.Update(Id, TaskTitleColumn, value);
                    title = value;
                }
            }
        }

        private string creationTime;
        public string CreationTime
        {
            get => creationTime; set
            {
                if (Id == -1)
                    creationTime = value;
                else
                {
                    dalC.Update(Id, TaskCreationTimeColumn, value);
                    creationTime = value;
                }
            }
        }

        private string dueDate;
        public string DueDate
        {
            get => dueDate; set
            {
                if (Id == -1)
                    dueDate = value;
                else
                {
                    dalC.Update(Id, TaskDueDateColumn, value);
                    dueDate = value;
                }
            }
        }

        private string description;
        public string Description
        {
            get => description; set
            {
                if (Id == -1)
                {
                    if (value == null)
                        description = "";
                    else
                        description = value;
                }

                else
                {
                    if (value == null)
                    {
                        dalC.Update(Id, TaskDescriptionColumn, "");
                        description = "";
                    }
                    else
                    {
                        dalC.Update(Id, TaskDescriptionColumn, value);
                        description = value;
                    }
                }
            }
        }

        private long columnID;
        public long ColumnID { get => columnID;
            set
            {
                if (Id == -1)
                    columnID = value;
                else
                {
                    dalC.Update(Id, TaskColumnIdColumn, value);
                    columnID = value;
                }
            }
        }

        private long assigneeID;
        public long AssigneeID { get => assigneeID;
            set
            {
                if (Id == -1)
                    assigneeID = value;
                else
                {
                    dalC.Update(Id, TaskAssigneeColumn, value);
                    assigneeID = value;
                }
            }
            
        }



        //constructors
        /// <summary>
        /// Simple constructor, will be used when creating a new object in business layer
        /// </summary>
        /// <param name="title"></param>
        /// <param name="creationTime"></param>
        /// <param name="dueDate"></param>
        /// <param name="description"></param>
        /// <param name="columnID"></param>
        /// <param name="assigneeID"></param>
        public TaskDAL(string title, string creationTime, string dueDate, string description, long columnID,long assigneeID) : base(new TaskDalController())
        {
            Title = title;
            CreationTime = creationTime;
            DueDate = dueDate;
            Description = description;
            ColumnID = columnID;
            AssigneeID = assigneeID;
            Id = dalC.Insert(this);
        }

        /// <summary>
        /// Simple constructor for loading tasks from db
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="title"></param>
        /// <param name="creationTime"></param>
        /// <param name="dueDate"></param>
        /// <param name="description"></param>
        /// <param name="columnID"></param>
        /// <param name="assigneeID"></param>
        public TaskDAL(long taskid, string title, string creationTime, string dueDate, string description, long columnID, long assigneeID) : base(new TaskDalController())
        {
            Title = title;
            CreationTime = creationTime;
            DueDate = dueDate;
            Description = description;
            ColumnID = columnID;
            AssigneeID = assigneeID;
            Id = taskid;
        }

        public void Delete()
        {
            dalC.Delete(this);
        }
    }
}
