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
                "VALUES(@StackId, @Question, @Answer)";
            Console.WriteLine(flashcard.Stack.StackId);
            var parms = new { StackId = flashcard.Stack.StackId, Question = flashcard.Question, Answer = flashcard.Answer };
            await db.ExecuteAsync(query, parms);
        }
        catch(Exception ex)
        {
            throw new Exception($"{ex.Message}");
        }
    }

    public async Task DeleteFlashcard(int id)
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "DELETE FROM flashcards WHERE flashcardId=@Id";
            await db.ExecuteAsync(query, new { Id = id });
        }
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}");
        }
    }

    public async Task DeleteFlashcardByQuestion(string question)
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "DELETE FROM flashcards WHERE Question=@Question";
            await db.ExecuteAsync(query, new { Question = question });
        }
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}");
        }
    }

    public async Task<IEnumerable<Flashcard>> GetAllFlashcards()
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "SELECT * FROM FlashCards fc LEFT JOIN Stacks s on fc.StackId = s.stackId";
            var flashcard = await db.QueryAsync<Flashcard, Stack, Flashcard>(query, (flashcard, stack) =>
            {
                flashcard.Stack = stack;
                return flashcard;
            }, splitOn: "stackId");

            return flashcard;
        }
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}");
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
                flashcard.Stack = stack;
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
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}");
        }

    }

    public async Task<Flashcard> GetFlashcardByQuestion(string question)
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "SELECT * FROM FlashCards f LEFT JOIN Stacks s on f.StackId = s.stackId WHERE f.Question=@question";
            var flashCard = await db.QueryAsync<Flashcard, Stack, Flashcard>(query, (flashcard, stack) =>
            {
                flashcard.Stack = stack;
                return flashcard;
            },
            new { question = question },
            splitOn: "stackId");

            var flashcard = flashCard.FirstOrDefault();
            if (flashcard == null)
            {
                Console.Error.WriteLine("Notice: The flashcard cannot be found");
                return null;
            }
            return flashcard;
        }
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}");
        }
    }

    public async Task<IEnumerable<Flashcard>> GetFlashcardsByStackname(string name)
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "SELECT * FROM FlashCards f LEFT JOIN Stacks s on f.StackId = s.stackId WHERE s.Name = @name";
            var flashcard = await db.QueryAsync<Flashcard, Stack, Flashcard>(query, (flashcard, stack) =>
            {
                flashcard.Stack = stack;
                return flashcard;
            }, new {name = name},
            splitOn: "stackId");
            return flashcard;
        }
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}");
        }
    }

    public Task UpdateFlashcard(Flashcard Flashcard)
    {
        throw new NotImplementedException();
    }
}
