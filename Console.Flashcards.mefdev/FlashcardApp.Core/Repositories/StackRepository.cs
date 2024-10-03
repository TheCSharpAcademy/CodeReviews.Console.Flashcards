using FlashcardApp.Core.Models;
using FlashcardApp.Core.Repositories.Interfaces;
using FlashcardApp.Core.Data;
using Dapper;

namespace FlashcardApp.Core.Repositories;

public class StackRepository : IStackRepository
{
    private readonly DatabaseContext _dbContext;

    public StackRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddStack(Stack stack)
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "INSERT INTO Stacks (Name) VALUES (@Name)";
            await db.ExecuteAsync(query, new { stack.Name });
        }
        catch
        {
            throw new Exception("An error occured while connecting to the DB");
        }
    }

    public async Task DeleteStack(int id)
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "DELETE FROM Stacks WHERE stackId=@Id";
            await db.ExecuteAsync(query, new { Id = id });
        }
        catch
        {
            throw new Exception("An error occured while connecting to the DB");
        }
    }

    public async Task<IEnumerable<Stack>> GetAllStacks()
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "SELECT * FROM Stacks";
            return await db.QueryAsync<Stack>(query);
        }
        catch
        {
            throw new Exception("An error occured while connecting to the DB"); 
        }
    }

    public async Task<Stack> GetStackByName(string name)
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "SELECT * from stacks WHERE Name=@Name";
            var stack = await db.QueryFirstOrDefaultAsync<Stack>(query, new { Name = name });
            if (stack == null)
            {
                Console.Error.WriteLine("Notice: The Stack cannot be found");
                return stack;
            }
            return stack;
        }
        catch
        {
            throw new Exception("Invalid Operation: An error has occured while retreiving a stack");
        }
    }

    public Task UpdateStack(Stack stack)
    {
        throw new NotImplementedException();
    }
}