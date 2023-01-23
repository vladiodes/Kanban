using IntroSE.Kanban.Backend.ServiceLayer;
using System.Collections.Generic;

namespace KanbanWebAPI.DTO
{
    public class BoardDTO
    {
        public List<string> ColumnsNames { get; set; }
        public string emailCreator { get; set; }
        public BoardDTO(Board board)
        {
            ColumnsNames = new List<string>();
            foreach(string name in board.ColumnsNames)
            {
                ColumnsNames.Add(name);
            }
            emailCreator = board.emailCreator;
        }
    }
}