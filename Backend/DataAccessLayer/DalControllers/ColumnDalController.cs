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
    class ColumnDalController : DalController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ColumnDalController");

        public const string columnsTable = "ColumnsTable";

        //constructor
        /// <summary>
        /// Simple constructor
        /// </summary>
        public ColumnDalController() : base(columnsTable)
        {

        }

        /// <summary>
        /// Inserting new column to database
        /// </summary>
        /// <param name="column">column to insert</param>
        /// <returns>the id of the new column</returns>
        public override long Insert(DalObject dalObject)
        {
            ColumnDAL column = (ColumnDAL)dalObject;
            long output = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {columnsTable} ({ColumnDAL.columnNameCol} ,{ColumnDAL.columnOrdinalCol} ,{ColumnDAL.LimitCol},{ColumnDAL.boardIdCol}) " +
                        $"VALUES (@NameVal,@OrdinalVal,@LimitVal,@boardIdVal); SELECT {DalController.seqCol} FROM {DalController.SqlSeq} WHERE {DalController.seqName}='{columnsTable}';";

                    SQLiteParameter NameParam = new SQLiteParameter(@"NameVal", column.ColumnName);
                    SQLiteParameter OrdinalParam = new SQLiteParameter(@"OrdinalVal", column.ColumnOrdinal);
                    SQLiteParameter LimitParam = new SQLiteParameter(@"LimitVal", column.Limit);
                    SQLiteParameter boardIDParam = new SQLiteParameter(@"boardIdVal", column.BoardId);


                    command.Parameters.Add(NameParam);
                    command.Parameters.Add(OrdinalParam);
                    command.Parameters.Add(LimitParam);
                    command.Parameters.Add(boardIDParam);

                    dataReader = command.ExecuteReader();

                    if (dataReader.Read())
                    {
                        output = (long)dataReader.GetValue(0);
                    }

                }
                catch (Exception e)
                {
                    log.Error($"Failed to add column{column.ColumnName}", e);
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
        /// Converts reader to column DAL
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>ColumnDAL</returns>
        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            ColumnDAL column = new ColumnDAL((long)reader.GetValue(0), reader.GetString(1), (long)reader.GetValue(2), (long)reader.GetValue(3), (long)reader.GetValue(4));
            return column;
        }
        /// <summary>
        /// Selects all columns that belong to a certain board
        /// </summary>
        /// <param name="boardid">The board id we want to select columns from</param>
        /// <returns>List of all columns of the specified board</returns>
        public List<ColumnDAL> SelectColumnsByBoardID(long boardid)
        {
            List<ColumnDAL> columns = new List<ColumnDAL>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * " +
                    $"FROM  { columnsTable} " +
                    $"WHERE {ColumnDAL.boardIdCol}={boardid} " +
                    $"ORDER BY {ColumnDAL.columnOrdinalCol} ASC;";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        columns.Add((ColumnDAL)ConvertReaderToObject(dataReader));

                    }
                }
                catch(Exception e)
                {
                    log.Error($"Error when trying to get columns by boardid {boardid}");
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
            return columns;
        }

    }
}
