using System.Configuration;
using System.Data.SqlClient;

namespace Flashcards.StevieTV.Database;

internal class DatabaseManager
{
    private readonly string _dbName;
    private readonly string _masterConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    public readonly string ConnectionString;

    public DatabaseManager(string dbName)
    {
        _dbName = dbName;
        ConnectionString = $"{_masterConnectionString}Initial Catalog={dbName};";
    }

    public void CreateDatabase()
    {
        using (var connection = new SqlConnection(_masterConnectionString))
        {
            using (var databaseCommand = connection.CreateCommand())
            {
                connection.Open();
                databaseCommand.CommandText = $"If(db_id(N'{_dbName}') IS NULL) CREATE DATABASE [{_dbName}]";
                databaseCommand.ExecuteNonQuery();
            }
        }
    }

    public void CreateTable(string tableName, string tableContents)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();

                tableCommand.CommandText =
                    @$"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{tableName}]') AND type in (N'U'))
                       BEGIN
                         CREATE TABLE {tableName} ({tableContents})
                       END";

                tableCommand.ExecuteNonQuery();
            }
        }
    }

}