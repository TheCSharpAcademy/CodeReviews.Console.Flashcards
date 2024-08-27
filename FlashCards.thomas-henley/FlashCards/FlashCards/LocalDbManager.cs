using Dapper;
using Microsoft.Data.SqlClient;

namespace FlashCards;

public static class LocalDbManager
{
    private const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true";

    public static void CreateDatabase(string name)
    {
        var sql = $"""
                   IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{name}')
                   BEGIN 
                      CREATE DATABASE {name}
                   END
                   """;

        var cx = new SqlConnection(ConnectionString);
        cx.Execute(sql);
    }

    public static void DropDatabase(string name)
    {
        var sql = $"DROP DATABASE {name}";

        var cx = new SqlConnection(ConnectionString);
        cx.Execute(sql);
    }
}