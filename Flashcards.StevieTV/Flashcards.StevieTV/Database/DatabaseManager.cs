using System.Configuration;
using System.Data.SqlClient;
using Flashcards.StevieTV.Models;

namespace Flashcards.StevieTV.Database;

public class DatabaseManager
{
    private string dbName;
    private string masterConnectionstring = ConfigurationManager.AppSettings.Get("ConnectionString");
    public string connectionString;

    public DatabaseManager(string _dbName)
    {
        dbName = _dbName;
        connectionString = $"{masterConnectionstring}Initial Catalog={_dbName};";
    }

    public void CreateDatabase()
    {
        using (var connection = new SqlConnection(masterConnectionstring))
        {
            using (var databaseCommand = connection.CreateCommand())
            {
                connection.Open();
                databaseCommand.CommandText = $"If(db_id(N'{dbName}') IS NULL) CREATE DATABASE [{dbName}]";
                databaseCommand.ExecuteNonQuery();
            }
        }
    }

    public void CreateTable(string tableName, string tableContents)
    {
        using (var connection = new SqlConnection(connectionString))
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