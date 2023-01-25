namespace KanbanWebAPI.Controllers
{
    public class LimitColumnTasksRequestInfo
    {
        public string email { get; set; }
        public int columnOrdinal { get; set; }
        public int limit { get; set; }
    }
}