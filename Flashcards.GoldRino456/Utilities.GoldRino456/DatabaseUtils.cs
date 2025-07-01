using Dapper;
using Microsoft.Data.SqlClient;

namespace Utilities.GoldRino456;
public static class DatabaseUtils
{
    public static void ExecuteNonQueryCommand(string connectionString, string query, DynamicParameters? parameters = null)
    {
        int result;

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            result = connection.Execute(query, parameters);
            connection.Close();
        }
    }

    public static List<T> ExecuteQueryCommand<T>(string connectionString, string query, DynamicParameters? parameters = null)
    {
        List<T> result;

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            result = connection.Query<T>(query, parameters).ToList();
            connection.Close();
        }

        return result;
    }
}
