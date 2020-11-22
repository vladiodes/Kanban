using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Runtime.CompilerServices;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;


namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    class UserDalController : DalController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("UserDalController");

        public const string UsersTable = "UsersTable";

        /// <summary>
        /// Simple constructor
        /// </summary>
        public UserDalController() : base(UsersTable)
        {

        }

        /// <summary>
        /// Gets a list of all users in the database
        /// </summary>
        /// <returns>List of all users in the system</returns>
        public List<UserDAL> SelectAllUsers()
        {
            List<UserDAL> result = Select().Cast<UserDAL>().ToList();

            return result;
        }

        /// <summary>
        /// Insert new user to database
        /// </summary>
        /// <param name="dalObject">The user to insert</param>
        /// <returns>The id of the inserted user</returns>
        public override long Insert(DalObject dalObject)
        {
            UserDAL user = (UserDAL)dalObject;
            long output = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UsersTable} ({UserDAL.UserEmailColumn} ,{UserDAL.NicknameColumn} ,{UserDAL.PasswordColumn},{UserDAL.boardIdColumn}) " +
                        $"VALUES (@emailVal,@nickVal,@passVal,@boardVal); SELECT {DalController.seqCol} FROM {DalController.SqlSeq} WHERE {DalController.seqName}='{UsersTable}';";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    SQLiteParameter nickParam = new SQLiteParameter(@"nickVal", user.Nickname);
                    SQLiteParameter passParam = new SQLiteParameter(@"passVal", user.Password);
                    SQLiteParameter boardParam = new SQLiteParameter(@"boardVal", user.BoardId);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(nickParam);
                    command.Parameters.Add(passParam);
                    command.Parameters.Add(boardParam);

                    dataReader = command.ExecuteReader();

                    if (dataReader.Read())
                    {
                        output = (long)dataReader.GetValue(0);
                    }

                    
                }
                catch(Exception e)
                {
                    log.Error($"Failed to insert user {user.Email}", e);
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
        /// Converts SQL reader with all of its contents to a certain DalObject
        /// </summary>
        /// <param name="reader">SQL reader with contents</param>
        /// <returns>UserDAL</returns>
        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDAL user = new UserDAL((long)reader.GetValue(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),(long)reader.GetValue(4));
            return user;
        }
    }
}
