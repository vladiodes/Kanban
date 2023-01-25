using IntroSE.Kanban.Backend.ServiceLayer;
using System;

namespace KanbanWebAPI.DTO
{
    public class TaskDTO
    {
        public string title { get; set; }
        public string description { get; set; }
        public DateTime dueDate { get; set; }
        public string emailAssignee { get; set; }
        public int id { get; set; }
        public DateTime creationTime { get; set; }
        


        public TaskDTO(Task task)
        {
            this.creationTime = task.CreationTime;
            this.description = task.Description;
            this.dueDate = task.DueDate;
            this.emailAssignee = task.emailAssignee;
            this.id = task.Id;
            this.title = task.Title;
        }
    }
}