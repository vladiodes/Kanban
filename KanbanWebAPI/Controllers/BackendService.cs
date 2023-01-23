using IntroSE.Kanban.Backend.ServiceLayer;

namespace KanbanWebAPI.Controllers
{
    public class BackendService
    {
        private static Service service = null;


        public static Service GetService()
        {
            if (service is null)
            {
                service = new Service();
                service.LoadData();
            }
            return service;
        }
    }
}