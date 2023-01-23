using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebApiController : ControllerBase
    {
        private readonly IService _service;

        public WebApiController(IService service)
        {
            _service = service;
        }

        [HttpPost]
        public Response Register(string email, string password, string nickname)
        {
            return _service.Register(email, password, nickname);
        }
    }
}
