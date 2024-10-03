using Dapper;
using FlashcardApp.Core.Data;
using FlashcardApp.Core.Models;
using FlashcardApp.Core.Repositories.Interfaces;

namespace FlashcardApp.Core.Repositories;

public class FlashcardRepository : IFlashcardRepository
{
    private readonly DatabaseContext _dbContext;

    public FlashcardRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddFlashcard(Flashcard flashcard)
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "INSERT INTO FlashCards (StackId, Question, Answer) " +
                "VALUES (@StackId, @Question, @Answer)";
            var parms = new { StackId = flashcard.stack.StackId, Question = flashcard.Question, Answer = flashcard.Answer };
            await db.ExecuteAsync(query, parms);
        }
        catch
        {
            throw new Exception("An error occured while connecting to the DB");
        }
    }

    public Task DeleteFlashcard(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Flashcard>> GetAllFlashcards()
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "SELECT * FROM FlashCards fc LEFT JOIN Stacks s on fc.StackId = s.stackId";
            var flashcard = await db.QueryAsync<Flashcard, Stack, Flashcard>(query, (flashcard, stack) =>
            {
                flashcard.stack = stack;
                return flashcard;
            }, splitOn: "stackId");

            return flashcard;
        }
        catch
        {
            throw new Exception("An error occured while connecting to the DB");
        }
        
    }

    public async Task<Flashcard> GetFlashcard(int id)
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "SELECT * FROM FlashCards fc LEFT JOIN Stacks s on fc.StackId = s.stackId WHERE fc.flashcardId=@id";
            var flashCard = await db.QueryAsync<Flashcard, Stack, Flashcard>(query, (flashcard, stack) =>
            {
                flashcard.stack = stack;
                return flashcard;
            },
            new { id = id },
            splitOn: "stackId");

            var flashcard = flashCard.FirstOrDefault();
            if (flashcard == null)
            {
                Console.Error.WriteLine("Notice: The flashcard cannot be found");
                return null;
            }
            return flashcard;
        }
        catch
        {
            throw new Exception("An error occured while connecting to the DB");
        }
        
    }

    public Task UpdateFlashcard(Flashcard Flashcard)
    {
        throw new NotImplementedException();
    }
}