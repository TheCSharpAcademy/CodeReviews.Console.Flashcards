using Dapper;
using Flashcards.FunRunRushFlush.Data.Interfaces;
using Flashcards.FunRunRushFlush.Data.Model;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Flashcards.FunRunRushFlush.Data;

public class FlashcardsDataAccess : IFlashcardsDataAccess
{
    private readonly IDbConnection _connection;

    public FlashcardsDataAccess(IDbConnection connection)
    {
        _connection = connection;
    }

    // In this approach I try to combine the best of both worlds:
    // static queries, defined at compile time for better performance, 
    // while keeping them parameterized to maintain flexibility when the model changes.
    //(I didnt like re-interpolating my strings every time the query was called in my previous Acadamy-Projects)

    private static readonly string QueryGetAllFlashcardsOfOneStack = $"""
                SELECT * FROM {FlashcardsTable.TableName}
                WHERE {FlashcardsTable.StackId} = @{FlashcardsTable.StackId}
                """;
    public List<Flashcard> GetAllFlashcardsOfOneStack(Stack stack)
    {
        List<Flashcard> flashcards = _connection.Query<Flashcard>(QueryGetAllFlashcardsOfOneStack, new { StackId = stack.Id }).ToList();
        return flashcards;
    }


    private static readonly string QueryCreateFlashcard = $"""
                INSERT INTO {FlashcardsTable.TableName} (
                        {FlashcardsTable.StackId},
                        {FlashcardsTable.Front},
                        {FlashcardsTable.Back},
                        {FlashcardsTable.Solved}
                    )
                VALUES (
                        @{FlashcardsTable.StackId},
                        @{FlashcardsTable.Front},
                        @{FlashcardsTable.Back},
                        @{FlashcardsTable.Solved}
                    );
                """;
    public void CreateFlashcard(Flashcard flashcard)
    {
        _connection.Execute(QueryCreateFlashcard,
            new
            {
                flashcard.StackId,
                flashcard.Front,
                flashcard.Back,
                flashcard.Solved
            });
    }


    private static readonly string QueryUpdateFlashcard = $"""
                UPDATE {FlashcardsTable.TableName}
                SET {FlashcardsTable.Front} = @{FlashcardsTable.Front},
                    {FlashcardsTable.Back} = @{FlashcardsTable.Back},
                    {FlashcardsTable.Solved} = @{FlashcardsTable.Solved}                    
                WHERE {FlashcardsTable.Id} = @{FlashcardsTable.Id}
                """;
    public void UpdateFlashcard(Flashcard flashcard)
    {
        _connection.Execute(QueryUpdateFlashcard, new { flashcard.Front, flashcard.Back, flashcard.Solved, flashcard.Id });
    }



    private static readonly string QueryDeleteFlashcard = $"""
                    Delete From {FlashcardsTable.TableName}
                    WHERE {FlashcardsTable.Id} = @Id;
                """;
    public void DeleteFlashcard(Flashcard flashcard)
    {
        _connection.Execute(QueryDeleteFlashcard, new { flashcard.Id });
    }
}
