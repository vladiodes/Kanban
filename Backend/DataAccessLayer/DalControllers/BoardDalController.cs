using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    class BoardDalController:DalController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BoardDalController");

        public const string boardsTable = "BoardsTable";

        /// <summary>
        /// Simple constructor
        /// </summary>
        public BoardDalController(): base(boardsTable)
        {

        }

        /// <summary>
        /// Selects all boards from the table
        /// </summary>
        /// <returns>A list of all boards</returns>
        public List<BoardDAL> SelectAllBoards()
        {
            List<BoardDAL> boards = Select().Cast<BoardDAL>().ToList();

            return boards;
        }

        /// <summary>
        /// Inserts a new board to the table
        /// </summary>
        /// <param name="dalObject">Board to insert</param>
        /// <returns>The generated id of the board inserted in the database</returns>
        public override long Insert(DalObject dalObject)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                BoardDAL board = (BoardDAL)dalObject;
                long output = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {boardsTable} ({BoardDAL.userCreatorColumn}) " +
                        $"VALUES (@creatorIdVal); SELECT {DalController.seqCol} FROM {DalController.SqlSeq} WHERE {DalController.seqName}='{boardsTable}';";

                    SQLiteParameter userIdParam = new SQLiteParameter(@"creatorIdVal", board.CreatorID);


                    command.Parameters.Add(userIdParam);
                    command.Prepare();

                    dataReader = command.ExecuteReader();

                    if (dataReader.Read())
                    {
                        output = (long)dataReader.GetValue(0);
                    }

                }
                catch (Exception e)
                {
                    log.Error($"Failed to insert board to database{board.Id}", e);
                }
                finally
                {
                    if (dataReader != null)
                        dataReader.Close();
                    command.Dispose();
                    connection.Close();
                }
                return output;
            }
        }

        /// <summary>
        /// Converts reader to board dal
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>BoardDAL</returns>
        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDAL board = new BoardDAL((long)reader.GetValue(0), (long)reader.GetValue(1));
            return board;
        }

        /// <summary>
        /// Returns all the accessing ids of a specifid board
        /// </summary>
        /// <param name="boardid">The id of the specific board</param>
        /// <returns>A list of all IDs that can access the board</returns>
        public List<long> getAccessingIDs(long boardid)
        {
                List<long> accessingIDs = new List<long>();
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    command.CommandText = $"SELECT {UserDAL.IDColumnName} " +
                        $"FROM  { UserDalController.UsersTable} " +
                        $"WHERE {UserDAL.boardIdColumn}={boardid};";
                    SQLiteDataReader dataReader = null;
                    try
                    {
                        connection.Open();
                        dataReader = command.ExecuteReader();

                        while (dataReader.Read())
                        {
                            accessingIDs.Add((long)dataReader.GetValue(0));
                        }
                    }
                    catch (Exception e)
                    {
                        log.Error($"Error when trying to get accessing ids of boardid {boardid}");
                    }
                    finally
                    {
                        if (dataReader != null)
                        {
                            dataReader.Close();
                        }

                        command.Dispose();
                        connection.Close();
                    }

                }
                return accessingIDs;
        }
    }
}
