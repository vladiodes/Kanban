using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalObjects
{
    class ColumnDAL:DalObject
    {
        public const string columnNameCol = "Name";
        public const string columnOrdinalCol = "ColumnOrdinal";
        public const string LimitCol = "LimitedTasks";
        public const string boardIdCol = "boardid";

        //fields + getters and setters
        private string columnName;
        public string ColumnName { get => columnName; set
            {
                if (Id == -1)
                    columnName = value;
                else
                {
                    dalC.Update(Id, columnNameCol, value);
                    columnName = value;
                }
            }
        }

        private long limit;
        public long Limit { get => limit; set
            {
                if (Id == -1)
                    limit = value;
                else
                {
                    dalC.Update(Id, LimitCol, value);
                    limit = value;
                }
            }
        }

        private long columnOrdinal;
        public long ColumnOrdinal { get => columnOrdinal;
            set
            {
                if (Id == -1)
                    columnOrdinal = value;
                else
                {
                    dalC.Update(Id, columnOrdinalCol, value);
                    limit = value;
                }
            }
        }

        private long boardId;
        public long BoardId { get => boardId; set { boardId = value; } }

        //constructors
        /// <summary>
        /// Simple constructor, will be used when creating a column in the business layer
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="colOrdinal"></param>
        /// <param name="limit"></param>
        /// <param name="boardId"></param>
        public ColumnDAL(string columnName, long columnOrdinal, long limit,long boardId):base(new ColumnDalController())
        {
            ColumnName = columnName;
            Limit = limit;
            ColumnOrdinal = columnOrdinal;
            BoardId = boardId;
            Id = dalC.Insert(this);
        }

        /// <summary>
        /// Simple constructor - Will be used when loading from db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="columnName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="limit"></param>
        /// <param name="boardId"></param>
        public ColumnDAL(long id,string columnName, long columnOrdinal, long limit, long boardId) : base(new ColumnDalController())
        {
            ColumnName = columnName;
            Limit = limit;
            ColumnOrdinal = columnOrdinal;
            BoardId = boardId;
            Id = id;
        }


        //methods

        /// <summary>
        /// Deletes this column from database
        /// </summary>
        /// <returns>True if was deleted</returns>
        public bool delete()
        {
            return dalC.Delete(this);
        }
    }
}
