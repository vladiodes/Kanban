namespace KanbanWebAPI.Controllers
{
    public class TaskDeleteInfo
    {
        public string email { get; set; }
        public int columnOrdinal { get; set; }
        public int taskId { get; set; }
    }
}