using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Data.SqlClient;
using Dapper;
using Flashcards.KamilKolanowski.Models;

namespace Flashcards.KamilKolanowski.Data;

internal class DatabaseManager
{
    private string _connectionString;
    
    internal DatabaseManager()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        _connectionString = config.GetConnectionString("DatabaseConnection");
    } 
    
    private SqlConnection Connection => new(_connectionString);


    internal List<Cards> ReadCards(string stackName)
    {
        Connection.Open();
        
        string query = $@"SELECT 
                            c.StackId,
                            c.FlashcardTitle,
                            c.FlashcardContent
                        FROM
                            Flashcards.TCSA.Cards AS c
                            INNER JOIN Flashcards.TCSA.Stacks AS s
                                ON s.StackId = c.StackId
                        WHERE
                            s.StackName = '{stackName}'";

        return Connection.Query<Cards>(query).ToList();
    }

    internal void UpdateCards(string stackName, string flashcardTitle, string columnToUpdate, string newValue)
    {
        Connection.Open();

        string query;
        if (columnToUpdate == "StackName")
        {
            query = $@"
            UPDATE c
                SET c.StackId = (
                    SELECT s2.StackId
                    FROM Flashcards.TCSA.Stacks s2
                    WHERE s2.StackName = '{newValue}'
                )
                FROM Flashcards.TCSA.Cards c
                INNER JOIN Flashcards.TCSA.Stacks s
                    ON s.StackId = c.StackId
                WHERE s.StackName = '{stackName}'
                    AND c.FlashcardTitle = '{flashcardTitle}'";
        }
        else
        {
            query = $@"UPDATE c   
                          SET {columnToUpdate} = '{newValue}'
                          FROM Flashcards.TCSA.Cards c
                            INNER JOIN Flashcards.TCSA.Stacks s
                                    ON s.StackId = c.StackId 
                          WHERE s.StackName = '{stackName}'
                            AND c.FlashcardTitle = '{flashcardTitle}'";
        }
        
        
        Connection.Execute(query);
    }

    internal void DeleteCards(string stackName, string flashcardTitle)
    {
        Connection.Open();
        string query;
        
        query = @$"DELETE c 
                    FROM Flashcards.TCSA.Cards c 
                    INNER JOIN Flashcards.TCSA.Stacks s ON s.StackId = c.StackId    
                    WHERE s.StackName = '{stackName}'
                      AND c.FlashcardTitle = '{flashcardTitle}'";
        
        Connection.Execute(query);
    }
    internal List<Stacks> ReadStacks()
    {
        Connection.Open();
        
        string query = $"SELECT StackId, StackName FROM Flashcards.TCSA.Stacks";
        return Connection.Query<Stacks>(query).ToList();
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