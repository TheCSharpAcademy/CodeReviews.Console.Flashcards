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
        
        var properties = typeof(T).GetProperties()
            .Where(p => p.Name != "FlashcardId" && p.Name != "DateCreated");
        
        var columns = string.Join(", ", properties.Select(c => c.Name));
        var values = string.Join(", ", properties.Select(v => $"@{v.Name}"));

        var query =  @$"INSERT INTO Flashcards.TCSA.{tableName} ({columns})
                           VALUES ({values});";
        
        Connection.Execute(query, obj);
    }

    internal void UpdateTable<T>(string tableName, T obj, string columnToUpdate, int columnId, int rowId, string newValue)
    {
        Connection.Open();
        
        var query = @$"UPDATE Flashcards.TCSA.{tableName}
                       SET {columnToUpdate} = @{newValue}
                       WHERE {columnId} = @{rowId}";
        
        Connection.Execute(query, obj);
    }
}