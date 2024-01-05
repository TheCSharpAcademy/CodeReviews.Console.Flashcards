using System.Configuration;
using System.Data.SqlClient;

namespace Flashcards.StevieTV.Database;

public class DatabaseManager
{
    private static string connectionstring = ConfigurationManager.AppSettings.Get("ConnectionString");
    public static void CreateDatabase(string dbName)
    {
        using (var connection = new SqlConnection(connectionstring))
        {
            using (var databaseCommand = connection.CreateCommand())
            {
                connection.Open();
                databaseCommand.CommandText = $"If(db_id(N'{dbName}') IS NULL) CREATE DATABASE [{dbName}]";
                databaseCommand.ExecuteNonQuery();
            }
        }
    }
}