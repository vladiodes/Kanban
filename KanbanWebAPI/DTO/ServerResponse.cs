namespace KanbanWebAPI.DTO
{
    public class ServerResponse<T>
    {
        public string errorMsg { get; set; }
        public T data { get; set; }

        public ServerResponse(string errorMsg, T value)
        {
            this.errorMsg = errorMsg;
            this.data = value;
        }
    }

}