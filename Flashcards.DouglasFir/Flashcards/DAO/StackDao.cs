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

    public IEnumerable<StackDto>? GetAllStacks()
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"SELECT StackID, StackName FROM {ConfigSettings.TableNameStack}";
                return dbConnection.Query<StackDto>(sql);
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Unable to retrieve stacks.", ex.Message);
            return null;
        }

    }

    public void UpdateStackName(StackDto stack)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"UPDATE {ConfigSettings.TableNameStack} SET StackName = @NewStackName WHERE StackID = @StackId";
                dbConnection.Execute(sql, new { NewStackName = stack.StackName, StackId = stack.StackID});
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Unable to update stack name.", ex.Message);
            throw;
        }
    }

    public void DeleteStack(StackDto stack)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"DELETE FROM {ConfigSettings.TableNameStack} WHERE StackID = @StackId";
                dbConnection.Execute(sql, new { StackId = stack.StackID });
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Unable to delete stack.", ex.Message);
            throw;
        }
    }

    public void CreateStack(StackDto stack)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"INSERT INTO {ConfigSettings.TableNameStack} (StackName) VALUES (@StackName)";
                dbConnection.Execute(sql, new { StackName = stack.StackName });
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Unable to create stack.", ex.Message);
            throw;
        }
    }
}
