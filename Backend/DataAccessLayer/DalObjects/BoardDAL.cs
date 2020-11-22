using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalObjects
{
    class BoardDAL:DalObject
    {
        public const string userCreatorColumn = "CreatorId";

        //fields - getters and setters
        private long creatorID;

        public virtual long CreatorID { get => creatorID; }


        public BoardDAL():base(null)
        {
            //for testing manners - mocking this object
        }

        /// <summary>
        ///Simple constructor -  Creating new board in business
        /// </summary>
        /// <param name="userid"></param>
        public BoardDAL(long creatorID) : base(new BoardDalController())
        {
            this.creatorID = creatorID;
            Id = dalC.Insert(this);
        }
        /// <summary>
        /// Simple constructor - used when loading data from db
        /// </summary>
        /// <param name="boardid"></param>
        /// <param name="userid"></param>
        public BoardDAL(long boardid, long creatorID) : base(new BoardDalController())
        {
            this.creatorID = creatorID;
            Id = boardid;
        }

    }
}
