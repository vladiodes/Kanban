using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using log4net;


namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    internal abstract class DalController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("DalController");

        public const string SqlSeq = "sqlite_sequence";
        public const string seqCol = "seq";
        public const string seqName = "name";

        protected readonly string _connectionString;
        private readonly string _tableName;
        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            if (!File.Exists(path))
                CreateDataBase();
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
        }

        public DalController() { }

        /// <summary>
        /// Updates a value in the table - value to update is a number
        /// </summary>
        /// <param name="id">Id of the value to update</param>
        /// <param name="attributeName">The attribute to update</param>
        /// <param name="attributeValue">The new value to update to</param>
        /// <returns>True if was updated otherwise false</returns>
        public bool Update(long id, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attributeName}]=@{attributeName} WHERE {DalObject.IDColumnName}={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    log.Error(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }

        /// <summary>
        /// Updates a value in the table - value to update is a string
        /// </summary>
        /// <param name="id">Id of the value to update</param>
        /// <param name="attributeName">The attribute to update</param>
        /// <param name="attributeValue">The new value to update to</param>
        /// <returns>True if was updated otherwise false</returns>
        public bool Update(long id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attributeName}]=@{attributeName} WHERE {DalObject.IDColumnName}={id}"
                };
                try
                {

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    log.Error(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        /// <summary>
        /// Loads all content of a table
        /// </summary>
        /// <returns>The list of the table contents</returns>
        protected List<DalObject> Select()
        {
            List<DalObject> results = new List<DalObject>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
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
            return results;
        }

        /// <summary>
        /// Converts SQL reader with all of its contents to a certain DalObject
        /// </summary>
        /// <param name="reader">SQL reader with contents</param>
        /// <returns>DalObject</returns>
        protected abstract DalObject ConvertReaderToObject(SQLiteDataReader reader);

        /// <summary>
        /// Deletes an object from the table
        /// </summary>
        /// <param name="dalObject">dal object to delete</param>
        /// <returns>true if was deleted otherwise false</returns>
        public bool Delete(DalObject dalObject)
        {
            int res = -1;
            long id = dalObject.Id;
            if (id == -1)
                return false;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE {DalObject.IDColumnName}={id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        /// <summary>
        /// Deletes all data from the table
        /// </summary>
        /// <returns>True if was deleted, otherwise false</returns>
        public bool DeleteAllData()
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"PRAGMA foreign_keys = ON; DELETE FROM {_tableName};"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        /// <summary>
        /// Inserts a dal object to the db
        /// </summary>
        /// <param name="dalObject">the object to insert</param>
        /// <returns>returns the id generated by the db</returns>
        public abstract long Insert(DalObject dalObject);

        /// <summary>
        /// Creates a database if wasn't created yet
        /// </summary>
        private void CreateDataBase()
        {
            SQLiteConnection.CreateFile("database.db");
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));

            SQLiteConnection m_dbConnection = new SQLiteConnection($"Data Source={path};Version=3;");
            m_dbConnection.Open();

            string UsersTbl = $"CREATE TABLE '{UserDalController.UsersTable}' " + "" +
                $"('{DalObject.IDColumnName}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, '{UserDAL.UserEmailColumn}' TEXT NOT NULL UNIQUE, '{UserDAL.NicknameColumn}' TEXT NOT NULL,'{UserDAL.PasswordColumn}'  TEXT NOT NULL, '{UserDAL.boardIdColumn}' INTEGER NOT NULL);";

            string BoardsTbl = $"CREATE TABLE '{BoardDalController.boardsTable}' " + "" +
                $"('{DalObject.IDColumnName}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, '{BoardDAL.userCreatorColumn}' INTEGER NOT NULL, " +
                $" FOREIGN KEY('{BoardDAL.userCreatorColumn}') REFERENCES '{UserDalController.UsersTable}'('{DalObject.IDColumnName}') ON DELETE CASCADE);";

            string columnstbl = $"CREATE TABLE '{ColumnDalController.columnsTable}' " + "" +
                $"( '{DalObject.IDColumnName}'    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,     '{ColumnDAL.columnNameCol}'  TEXT NOT NULL,     '{ColumnDAL.columnOrdinalCol}' INTEGER NOT NULL,     '{ColumnDAL.LimitCol}'  INTEGER NOT NULL,     '{ColumnDAL.boardIdCol}'   INTEGER NOT NULL," + "" +
                $"     FOREIGN KEY('{ColumnDAL.boardIdCol}') REFERENCES '{BoardDalController.boardsTable}'('{DalObject.IDColumnName}') ON DELETE CASCADE);";

            string TasksTbl = $"CREATE TABLE '{TaskDalController.tasksTable}' " +
                $"('{DalObject.IDColumnName}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, '{TaskDAL.TaskTitleColumn}' TEXT NOT NULL, " + "" +
                $"'{TaskDAL.TaskCreationTimeColumn}'  TEXT NOT NULL,'{TaskDAL.TaskDueDateColumn}'   TEXT NOT NULL,'{TaskDAL.TaskDescriptionColumn}'   TEXT, " + "" +
                $"'{TaskDAL.TaskColumnIdColumn}'  INTEGER NOT NULL,'{TaskDAL.TaskAssigneeColumn}'	INTEGER NOT NULL,  FOREIGN KEY('{TaskDAL.TaskColumnIdColumn}') REFERENCES '{ColumnDalController.columnsTable}'('{DalObject.IDColumnName}') ON DELETE CASCADE," +
                $"FOREIGN KEY('{TaskDAL.TaskAssigneeColumn}') REFERENCES '{UserDalController.UsersTable}'('{DalObject.IDColumnName}') ON DELETE CASCADE);";

            string sql = UsersTbl + BoardsTbl + columnstbl + TasksTbl;
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
                log.Info("A new database was created");
            }
            catch(Exception e)
            {
                log.Error(e.Message);
            }
            finally { m_dbConnection.Close(); }
        }

    }
}
