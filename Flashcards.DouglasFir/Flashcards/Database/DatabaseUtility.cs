using System.Data.SqlClient;
using Flashcards.Services;
using Dapper;

namespace Flashcards.Database;

public class DatabaseUtility
{
    private readonly DatabaseContext _dbContext;

    public DatabaseUtility(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool ValidateDatabaseSchema()
    {
        return CheckTablesExist() && CheckViewsExist();
    }

    public bool CheckDatabaseExists()
    {
        try
        {
            using (SqlConnection connection = _dbContext.GetConnectionToMaster())
            {
                var sql = "SELECT COUNT(*) FROM sys.databases WHERE name = @DbName";
                int result = connection.ExecuteScalar<int>(sql, new { DbName = ConfigSettings.dbName });

                return result > 0;
            }
        }
        catch (SqlException e)
        {
            Utilities.DisplayExceptionErrorMessage("Error checking if database exists.", e.Message);
            throw;
        }
    }

    public bool CheckTablesExist()
    {
        string[] tableNames = new string[] {
            ConfigSettings.tbStackName,
            ConfigSettings.tbFlashCardsName,
            ConfigSettings.tbStudySessionsName
        };

        try
        {
            using (SqlConnection connection = _dbContext.GetConnectionToFlashCards())
            {
                foreach (string tableName in tableNames)
                {
                    var sql = "SELECT COUNT(*) FROM sys.tables WHERE name = @TableName";
                    int result = connection.ExecuteScalar<int>(sql, new { TableName = tableName });

                    if (result == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        catch (SqlException e)
        {
            Utilities.DisplayExceptionErrorMessage("Error checking if tables exist.", e.Message);
            throw;
        }
    }

    public bool CheckViewsExist()
    {
        string[] viewNames = new string[] {
            ConfigSettings.vwFlashCardsName,
            ConfigSettings.vwFlashCardsRenumberedName,
            ConfigSettings.vwStudySessionsName
        };

        try
        {
            using (SqlConnection connection = _dbContext.GetConnectionToFlashCards())
            {
                foreach (string viewName in viewNames)
                {
                    var sql = "SELECT COUNT(*) FROM sys.views WHERE name = @ViewName";
                    int result = connection.ExecuteScalar<int>(sql, new { ViewName = viewName });

                    if (result == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        catch (SqlException e)
        {
            Utilities.DisplayExceptionErrorMessage("Error checking if views exist.", e.Message);
            throw;
        }
    }

    public bool DropDatabase()
    {
        try
        {
            using (SqlConnection connection = _dbContext.GetConnectionToMaster())
            {
                var sql = "DROP DATABASE FLASHCARDS";
                connection.Execute(sql);
            }

            return true;
        }
        catch (SqlException e)
        {
            Utilities.DisplayExceptionErrorMessage("Error dropping database.", e.Message);
            throw;
        }
    }
}
