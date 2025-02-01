
using Flashcards.FunRunRushFlush.Data.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;


namespace Flashcards.FunRunRushFlush.Data;

public class SqlServerConnectionFactory
{
    private readonly string _connectionString;
    private readonly ILogger<SqlServerConnectionFactory> _log;

    public SqlServerConnectionFactory(string connectionString, ILogger<SqlServerConnectionFactory> logger)
    {
        _connectionString = connectionString;
        _log = logger;
    }

    public IDbConnection CreateConnection()
    {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
    }


    public void InitializeDatabase()
    {
        _log.LogInformation("Database initialization started.");

        // Order matters due to foreign key dependencies!!!
        CreateTable(StackTable.TableCreateStatment, StackTable.TableName);
        CreateTable(FlashcardsTable.TableCreateStatment, FlashcardsTable.TableName);
        CreateTable(StudySessionTable.TableCreateStatment, StudySessionTable.TableName);

        _log.LogInformation("Database initialization completed.");
    }

    private void CreateTable(string tableCreateStatement, string tableName)
    {
        try
        {
            using var connection = CreateConnection();
            using var command = connection.CreateCommand();
            command.CommandText = tableCreateStatement;
            command.ExecuteNonQuery();
            _log.LogInformation($"Table '{tableName}' successfully created or already exists.");
        }
        catch (Exception ex)
        {
            _log.LogError(ex, $"Error while creating the table '{tableName}'.");
            throw;
        }
    }
}