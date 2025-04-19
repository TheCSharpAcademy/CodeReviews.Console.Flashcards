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


    internal List<CardDto> ReadCards(int stackChoice)
    {
        Connection.Open();
        
        string query = $@"SELECT 
                            c.FlashcardId,
                            c.FlashcardTitle,
                            c.FlashcardContent
                        FROM
                            Flashcards.TCSA.Cards AS c
                        WHERE
                            c.StackId = @StackId";

        return Connection.Query<CardDto>(query, new { StackId = stackChoice }).ToList();
    }
    
    internal void UpdateCards(UpdateCardDto updateCardDto)
    {
        Connection.Open();

        string query;
        if (updateCardDto.ColumnToUpdate == "StackName")
        {
            query = $@"
            UPDATE c
                SET c.StackId = (
                    SELECT s2.StackId
                    FROM Flashcards.TCSA.Stacks s2
                    WHERE s2.StackId = @NewValue
                )
                FROM Flashcards.TCSA.Cards c
                INNER JOIN Flashcards.TCSA.Stacks s
                    ON s.StackId = c.StackId
                WHERE s.StackId = @StackId
                    AND c.FlashcardId = @FlashcardId";
        }
        else
        {
            query = $@"UPDATE c   
                          SET {updateCardDto.ColumnToUpdate} = @NewValue
                          FROM Flashcards.TCSA.Cards c
                            INNER JOIN Flashcards.TCSA.Stacks s
                                    ON s.StackId = c.StackId 
                          WHERE s.StackId = @StackId
                            AND c.FlashcardId = @FlashcardId";
        }
        
        Connection.Execute(query, new
        {
            updateCardDto.StackId,
            updateCardDto.FlashcardId, 
            updateCardDto.NewValue
        });
    }

    internal void DeleteCards(int stackId, int flashcardId)
    {
        Connection.Open();
        
        string query = @$"DELETE c 
                    FROM Flashcards.TCSA.Cards c 
                    INNER JOIN Flashcards.TCSA.Stacks s ON s.StackId = c.StackId    
                    WHERE s.StackId = {stackId}
                      AND c.FlashcardId = '{flashcardId}'";
        
        Connection.Execute(query);
    }
    
    internal void AddCard(CreateCardDto createCardDto)
    {
        Connection.Open();
        
        string query =  @$"INSERT INTO Flashcards.TCSA.Cards (StackId, FlashcardTitle, FlashcardContent)
                           VALUES (@StackId, @FlashcardTitle, @FlashcardContent);";
        
        Connection.Execute(query, new { createCardDto.StackId, createCardDto.FlashcardTitle, createCardDto.FlashcardContent });
    }
    
    internal List<StacksDto> ReadStacks()
    {
        Connection.Open();
        
        string query = $"SELECT StackId, StackName, Description FROM Flashcards.TCSA.Stacks";
        return Connection.Query<StacksDto>(query).ToList();
    }

    internal void AddStack(CreateStackDto createStackDto)
    {
        Connection.Open();

        string query = @$"INSERT INTO Flashcards.TCSA.Stacks (StackName, Description) 
                            VALUES (@StackName, @Description);";
        
        Connection.Execute(query, new { createStackDto.StackName, createStackDto.Description });
    }

    internal void UpdateStack(UpdateStackDto updateStackDto)
    {
        Connection.Open();

        string query = $@"UPDATE Flashcards.TCSA.Stacks
                          SET {updateStackDto.ColumnToUpdate} = @NewValue
                          WHERE StackId = @StackId";
        
        Connection.Execute(query, new { updateStackDto.StackId, updateStackDto.NewValue });
    }
}