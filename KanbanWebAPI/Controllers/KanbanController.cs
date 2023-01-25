using IntroSE.Kanban.Backend.ServiceLayer;
using KanbanWebAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KanbanWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class KanbanController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ServerResponse<string>> Register([FromBody] LoginInfo info)
        {
            Response resp = BackendService.GetService().Register(info.email, info.password, info.nickname);
            return resp.ErrorOccured ? new ServerResponse<string>(resp.ErrorMessage, null) : new ServerResponse<string>(null, "Successfully registered!");
        }

        [HttpPost]
        public ActionResult<ServerResponse<string>> RegisterToBoard([FromBody] RegToBoardInfo info)
        {
            Response resp = BackendService.GetService().Register(info.email, info.password, info.nickname, info.emailHost);
            return resp.ErrorOccured ? new ServerResponse<string>(resp.ErrorMessage, null) : new ServerResponse<string>(null, "Successfully registered!");
        }

        [HttpPost]
        public ActionResult<ServerResponse<string>> AssignTask([FromBody] TaskAssignInfo info)
        {
            Response resp = BackendService.GetService().AssignTask(info.email, info.columnOrdinal, info.taskId, info.emailAssignee);
            return resp.ErrorOccured ? new ServerResponse<string>(resp.ErrorMessage, null) : new ServerResponse<string>(null, "Successfully assigned task!");
        }

        [HttpPost]
        public ActionResult<ServerResponse<string>> DeleteTask([FromBody] TaskDeleteInfo info)
        {
            Response resp = BackendService.GetService().DeleteTask(info.email, info.columnOrdinal, info.taskId);
            return resp.ErrorOccured ? new ServerResponse<string>(resp.ErrorMessage, null) : new ServerResponse<string>(null, "Successfully deleted task!");
        }

        [HttpPost]
        public ActionResult<ServerResponse<UserDTO>> Login([FromBody] LoginInfo info)
        {
            Response<User> resp = BackendService.GetService().Login(info.email, info.password);
            return resp.ErrorOccured ? new ServerResponse<UserDTO>(resp.ErrorMessage, null) : new ServerResponse<UserDTO>(null, UserDTO.FromUser(resp.Value));
        }

        [HttpPost]
        public ActionResult<ServerResponse<string>> Logout([FromBody] string email)
        {
            Response resp = BackendService.GetService().Logout(email);
            return resp.ErrorOccured ? new ServerResponse<string>(resp.ErrorMessage, null) : new ServerResponse<string>(null, "Successfully logged out!");
        }

        [HttpGet("{email}")]
        public ActionResult<ServerResponse<BoardDTO>> GetBoard(string email)
        {
            Response<Board> resp = BackendService.GetService().GetBoard(email);
            return resp.ErrorOccured ? new ServerResponse<BoardDTO>(resp.ErrorMessage, null) : new ServerResponse<BoardDTO>(null, new BoardDTO(resp.Value));
        }

        [HttpPost]
        public ActionResult<ServerResponse<string>> LimitColumnTasks([FromBody] LimitColumnTasksRequestInfo info)
        {
            Response resp = BackendService.GetService().LimitColumnTasks(info.email, info.columnOrdinal, info.limit);
            return resp.ErrorOccured ? new ServerResponse<string>(resp.ErrorMessage, null) : new ServerResponse<string>(null, "Successfully limited the number of tasks in the column");
        }

        [HttpPost]
        public ActionResult<ServerResponse<TaskDTO>> AddTask([FromBody] AddTaskRequestInfo info)
        {
            Response<Task> resp = BackendService.GetService().AddTask(info.email, info.title, info.description, info.date);
            return resp.ErrorOccured ? new ServerResponse<TaskDTO>(resp.ErrorMessage, null) : new ServerResponse<TaskDTO>(null, new TaskDTO(resp.Value));
        }

        public class AddTaskRequestInfo
        {
            public string email { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public DateTime date { get; set; }
        }
    }
}