using System;
using System.Collections.Generic;
using System.Linq;
using nayuta.Internal;
using SQLite;

namespace nayuta
{
    public enum QueryType
    {
        INSERT,
        SELECT
    }

    public class DatabaseManager : Manager<DatabaseManager>
    {
        private SQLiteConnection _sqliteConn;
        private SQLiteConnectionString _sqliteOptions;
        private bool _isOpen = false;

        public DatabaseManager()
        {
            var DataSource = "database.db";
            _sqliteOptions = new SQLiteConnectionString(DataSource, false);

            //GetUser(69);
            //GetUserFromDatabase(69);
            //Query("CREATE TABLE users (id INT PRIMARY KEY, discord_id INT, osu_id INT)");
        }

        /// <summary>
        /// Opens an connection to the database
        /// </summary>
        /// <returns>Returns whether the connection is successfully established</returns>
        private SQLiteConnection GetConnection()
        {
            if (_isOpen) return _sqliteConn;
            _sqliteConn = new SQLiteConnection(_sqliteOptions);
            _isOpen = true;
            return _sqliteConn;
        }

        private static T QueryFirstResult<T>(SQLiteConnection conn, string query) where T : new()
        {
            return conn.Query<T>(query).FirstOrDefault();
        }

        /// <summary>
        /// Gets all the users from the database
        /// </summary>
        public InternalUser GetUser(ulong DiscordID)
        {
            InternalUser user = InternalUserManager.Instance.GetUser(DiscordID);
            if (user != null)
                return user;

            SQLiteConnection conn = GetConnection();
            string query = $"SELECT * FROM users WHERE discord_id=" + DiscordID;
            user = QueryFirstResult<InternalUser>(conn, query);
            Close();
            if (user != null)
                return user;
            return null;
        }

        /// <summary>
        /// Saves InternalUser to database
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(InternalUser user)
        {
            if(user.IsNewUser)
                SaveUser(user);
            else
            {
                SQLiteConnection conn = GetConnection();
                conn.Update(user);
                Close();
            }
        }

        private void SaveUser(InternalUser user)
        {
            SQLiteConnection conn = GetConnection();
            conn.Insert(user);
            Close();
        }

        /// <summary>
        /// Closes the connection
        /// </summary>
        private void Close()
        {
            if (!_isOpen) return;
            _sqliteConn.Close();
            _isOpen = false;
        }
    }
}