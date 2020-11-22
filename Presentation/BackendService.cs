using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    public class BackendService
    {
        private static Service service=null;


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
