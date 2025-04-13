using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Flashcards.KamilKolanowski.Data;

internal class DatabaseManager
{
    private string _connectionString;
    
    internal DatabaseManager()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        _connectionString = config.GetConnectionString("DatabaseConnection");
    } 
    
    private SqlConnection Connection => new(_connectionString);

    internal List<T> ReadTable<T>(string tableName)
    {
        Connection.Open();
        
        string query = $"SELECT * FROM Flashcards.TCSA.{tableName}";
        return Connection.Query<T>(query).ToList();
    }

    internal void WriteTable<T>(string tableName, T obj)
    {
        Connection.Open();
        string query = @$"INSERT INTO {tableName} ({obj})
                          VALUES ({string.Join(",", obj.GetType().GetProperties().Select(p => p.Name))})";
        
        Connection.Execute(query, obj);
    }
}