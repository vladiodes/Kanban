using IntroSE.Kanban.Backend.ServiceLayer;

namespace KanbanWebAPI.DTO
{
    public class UserDTO
    {
        public string email { get; set; }
        public string nickname { get; set; }
        public string errorMsg { get; set; }


        public UserDTO(string email, string nickname)
        {
            this.email = email;
            this.nickname = nickname;
        }
        public UserDTO(string errorMsg)
        {
            this.errorMsg = errorMsg;
        }
        public static UserDTO FromUser(User user)
        {
            return new UserDTO(user.Email, user.Nickname);
        }

        public static UserDTO FromBadResponse(Response<User> resp)
        {
            return new UserDTO(resp.ErrorMessage);
        }
    }
}