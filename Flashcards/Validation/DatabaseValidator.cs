using Microsoft.Data.SqlClient;

namespace Flashcards.Validation
{
    public static class DatabaseValidator
    {
        static string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");
        static string databaseName = System.Configuration.ConfigurationManager.AppSettings.Get("DatabaseName");
        static string dbConnectionString = connectionString + ";Database=" + databaseName + ";Trusted_Connection=True;";

        public static bool DoWeHaveRows(string tableName)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"SELECT TOP 1 * FROM {tableName}";

                SqlDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return true;
                }
                reader.Close();
                connection.Close();
                return false;
            }
        }

        public static bool DoesValueExist(string tableName, string IDName, int ID)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @$"SELECT TOP 1 * FROM {tableName}
                    WHERE {IDName} = {ID}";

                SqlDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return true;
                }
                reader.Close();
                connection.Close();
                return false;
            }
        }
    }
}
