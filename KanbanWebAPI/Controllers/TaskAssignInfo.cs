namespace KanbanWebAPI.Controllers
{
    public class TaskAssignInfo
    {
        public string email { get; set; }
        public int columnOrdinal { get; set; }
        public int taskId { get; set; }
        public string emailAssignee { get; set; }
    }
}