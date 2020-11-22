using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalObjects
{
     abstract class DalObject
    {
        //fields
        public const string IDColumnName = "Id";
        private long id = -1;
        public virtual long Id { get => id; set { id = value; } }
        protected DalController dalC;

        //constructors
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="dalC"></param>
        public DalObject(DalController dalC)
        {
            this.dalC = dalC;
        }


        //methods


    }
}
