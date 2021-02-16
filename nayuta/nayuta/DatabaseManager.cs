using MySql.Data.MySqlClient;

namespace nayuta
{
    public class DatabaseManager : Manager<DatabaseManager>
    {
        private const string DB_NAME = "dbs1438021";
        private const string DB_USER = "dbs1438021";
        private const string DB_PASSWORD = "$%4A4kwSrZ#JfK.";
        private const string DB_SERVER = "rdbms.strato.de";

        private MySqlConnection dbConnection;

        public DatabaseManager()
        {
            string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", DB_SERVER, DB_NAME, DB_USER, DB_PASSWORD);
            dbConnection = new MySqlConnection(connstring);
            dbConnection.Open();
        }

        public void Close()
        {
            dbConnection.Close();
        }
    }
}