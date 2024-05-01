using Flashcards.Database;
using Flashcards.DTO;
using Dapper;
using Flashcards.Services;

namespace Flashcards.DAO;

public class StackDao
{
    private readonly DatabaseContext _dbContext;

    public StackDao(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<StackDto> GetAllStacks()
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"SELECT StackID, StackName FROM {ConfigSettings.tbStackName}";
                return dbConnection.Query<StackDto>(sql);
            }
        }
        catch
        {
            throw;
        }

    }

    public void UpdateStackName(StackDto stack)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"UPDATE {ConfigSettings.tbStackName} SET StackName = @NewStackName WHERE StackID = @StackId";
                dbConnection.Execute(sql, new { NewStackName = stack.StackName, StackId = stack.StackID});
            }
        }
        catch
        {
            throw;
        }
    }

    public void DeleteStack(StackDto stack)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"DELETE FROM {ConfigSettings.tbStackName} WHERE StackID = @StackId";
                dbConnection.Execute(sql, new { StackId = stack.StackID });
            }
        }
        catch
        {
            throw;
        }
    }

    public void CreateStack(StackDto stack)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"INSERT INTO {ConfigSettings.tbStackName} (StackName) VALUES (@StackName)";
                dbConnection.Execute(sql, new { StackName = stack.StackName });
            }
        }
        catch
        {
            throw;
        }
    }
}
