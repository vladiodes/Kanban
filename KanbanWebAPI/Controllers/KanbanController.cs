using IntroSE.Kanban.Backend.ServiceLayer;
using KanbanWebAPI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace KanbanWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class KanbanController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> Register([FromBody] LoginInfo info)
        {
            Response resp = BackendService.GetService().Register(info.email, info.password, info.nickname);
            return resp.ErrorOccured ? resp.ErrorMessage : "Successfully registered!";
        }

        [HttpPost]
        public ActionResult<string> RegisterToBoard([FromBody] RegToBoardInfo info)
        {
            Response resp = BackendService.GetService().Register(info.email, info.password, info.nickname, info.emailHost);
            return resp.ErrorOccured ? resp.ErrorMessage : "Successfully registered!";
        }

        [HttpPost]
        public ActionResult<string> AssignTask([FromBody] TaskAssignInfo info)
        {
            Response resp = BackendService.GetService().AssignTask(info.email, info.columnOrdinal, info.taskId, info.emailAssignee);
            return resp.ErrorOccured ? resp.ErrorMessage : "Successfully assigned task!";
        }

        [HttpPost]
        public ActionResult<string> DeleteTask([FromBody] TaskDeleteInfo info)
        {
            Response resp = BackendService.GetService().DeleteTask(info.email, info.columnOrdinal, info.taskId);
            return resp.ErrorOccured ? resp.ErrorMessage : "Successfully deleted task!";
        }

        [HttpPost]
        public ActionResult<UserDTO> Login([FromBody] LoginInfo info)
        {
            Response<User> resp = BackendService.GetService().Login(info.email, info.password);
            return resp.ErrorOccured ? UserDTO.FromBadResponse(resp) : UserDTO.FromUser(resp.Value);
        }

        [HttpPost]
        public ActionResult<string> Logout([FromBody] string email)
        {
            Response resp = BackendService.GetService().Logout(email);
            return resp.ErrorOccured ? resp.ErrorMessage : "Successfully logged out!";
        }

        [HttpGet("{email}")]
        public ActionResult<BoardDTO> GetBoard(string email)
        {
            Response<Board> resp = BackendService.GetService().GetBoard(email);
            return resp.ErrorOccured ? null : new BoardDTO(resp.Value);
        }
    }
}