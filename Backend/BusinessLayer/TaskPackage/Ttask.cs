using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;

namespace IntroSE.Kanban.Backend.BusinessLayer.TaskPackage
{
    class Ttask
    {
        private const int DescMaxLength = 300;
        private const int TitleMaxLength = 50;

        private TaskDAL dalTask;
        public TaskDAL DALTask { get => dalTask; }

        private int assigneeID=-1;
        public int AssigneeID { get => assigneeID;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("No negative ids");
                assigneeID = value;
                if (TaskID != -1)
                {
                    DALTask.AssigneeID = value;
                }
            }
        }

        private string title;
        public string Title { get => title;
            private set
            {
                if (value == null || value.Length == 0) throw new ApplicationException("the title cannot be empty");
                if (value.Length > TitleMaxLength) throw new ApplicationException("Title must be under 50 characters");
                title = value;
                if(TaskID!=-1)
                {
                    dalTask.Title = value;
                }
            }
        }

        private int taskID=-1;
        public int TaskID { get => taskID;}

        private readonly DateTime CreationTime;
        public DateTime creationTime { get => CreationTime; }

        private DateTime DueDate;
        public DateTime dueDate { get => DueDate;
            private set
            {
                if (value == null || value.CompareTo(DateTime.Today) < 0) throw new ApplicationException("Wrong due date");
                DueDate = value;
                if(TaskID!=-1)
                {
                    dalTask.DueDate = value.ToString();
                }
            }
        }

        private string Description;
        public string description { get => Description;
            private set
            {
                if (value != null && value.Length > DescMaxLength) throw new ApplicationException("the description must be under 300 characters");
                Description = value;
                if(TaskID!=-1)
                {
                    dalTask.Description = value;
                }
            }
        }


        //constructors

        /// <summary>
        /// Simple constructor - will be used when creating a task in the business layer
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <param name="columnID"></param>
        /// <param name="assigneeID"></param>
        public Ttask(string title, string description, DateTime dueDate,int columnID, int assigneeID)
        {
            Title = title;
            this.description = description;
            CreationTime = DateTime.Today;
            this.dueDate = dueDate;
            AssigneeID = assigneeID;
            dalTask = new TaskDAL(title, creationTime.ToString(), dueDate.ToString(), description, columnID,assigneeID);
            taskID = (int)dalTask.Id;
        }

        /// <summary>
        /// Same constructor as above, but this is to convert from dal object to biz object
        /// </summary>
        public Ttask(TaskDAL dalTask)
        {
            Title = dalTask.Title;
            description = dalTask.Description;
            DueDate = DateTime.Parse(dalTask.DueDate);
            CreationTime = DateTime.Parse(dalTask.CreationTime);
            AssigneeID = (int)dalTask.AssigneeID;
            this.dalTask = dalTask;
            this.taskID = (int)dalTask.Id;
        }

		public Ttask()
        {
            //for testing manners - mocking this object
        }

        /// <summary>
        /// Updating task's due date - only if not in the done column
        /// </summary>
        /// <param name="DueDate">The new Due Date</param>
        /// <param name="userId">The user id asking to update</param>
        public void UpdateTaskDueDate(DateTime DueDate, int userId)
        {
            isAssignee(userId);
            dueDate = DueDate;
        }


        /// <summary>
        /// Updating task's title - only if not in the done column
        /// </summary>
        /// <param name="title">The new title</param>
        /// <param name="userId">The user id asking to update</param>
        public void UpdateTaskTitle(string title,int userId)
        {
            isAssignee(userId);
            Title = title;
        }

        /// <summary>
        /// Updating description
        /// </summary>
        /// <param name="description">new description</param>
        /// <param name="userId">The user id asking to update</param>
        public void UpdateTaskDescription(string description, int userId)
        {
            isAssignee(userId);
            this.description = description;
        }

        /// <summary>
        /// Throws an exception if not the assignee
        /// </summary>
        /// <param name="userid">user id</param>
        public virtual void isAssignee(int userid)
        {
            if (userid != assigneeID)
                throw new ApplicationException("Only the assignee user can edit/move the task");
        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="userIdAssigned">Current user the task is assigned to</param>
        /// <param name="userIdToAssign">The user to assign to</param>
        public void Assign(int userIdAssigned, int userIdToAssign)
        {
            if (userIdAssigned == userIdToAssign)
                throw new ApplicationException("Can't assign a task to yourself");
            if (userIdAssigned != AssigneeID)
                throw new ApplicationException("You're not the assignee of this task and therefore can't assign it to other user");
            AssigneeID = userIdToAssign;
        }

        /// <summary>
        /// Deletes a task
        /// </summary>
        /// <param name="userIdAssigned">UserID assigned to the task</param>
        public void DeleteTask(int userIdAssigned)
        {
            if (userIdAssigned != AssigneeID)
                throw new ApplicationException("You're not assigned to this task and can't delete it");
            DALTask.Delete();
        }
    }
}