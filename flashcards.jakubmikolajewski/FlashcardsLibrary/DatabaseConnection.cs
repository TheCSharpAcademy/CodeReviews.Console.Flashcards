using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace FlashcardsLibrary;

public class DatabaseConnection : RunCommand<DatabaseConnection>
{
    private string _connectionString;

    public DatabaseConnection()
    {
        _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    }

    public List<T> Query<T>(string query, object[]? parameters = null)
    {
        using (var connection = CreateConnection())
        {
            return connection.Query<T>(query, parameters).ToList();
        }
    }

    public int Execute(string query, object[]? parameters = null)
    {
        using (var connection = CreateConnection())
        {
            return connection.Execute(query, parameters);
        }
    }

    private IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }     
}

