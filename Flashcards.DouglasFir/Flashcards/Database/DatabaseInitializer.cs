using System.Data.SqlClient;
using Flashcards.Services;
using Dapper;

namespace Flashcards.Database;

public class DatabaseInitializer
{
    private readonly DatabaseContext _dbContext;

    public DatabaseInitializer(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void InitializeDatabase()
    {

        CreateDatabase();
        CreateTables();
        CreateViews();

    }

    private void CreateDatabase()
    {
        string sql = $"CREATE DATABASE [{ConfigSettings.dbName}]";

        try
        {
            using (SqlConnection connection = _dbContext.GetConnectionToMaster())
            {
                connection.Execute(sql);
            }
        }
        catch (SqlException e)
        {
            Utilities.DisplayExceptionErrorMessage("Error creating database.", e.Message);
            throw;
        }
    }

    private void CreateTables()
    {
        CreateStacksTable();
        CreateFlashCardsTable();
        CreateStudySessionsTable();
    }

    private void CreateViews()
    {
        CreateFlashCardsView();
        CreateFlashCardsRenumberedView();
        CreateStudySessionsView();
    }

    private void CreateStacksTable()
    {
        string sql = $@"
            CREATE TABLE {ConfigSettings.tbStackName} (
            StackID INT PRIMARY KEY IDENTITY(1,1),
	        StackName VARCHAR(255) UNIQUE NOT NULL
        );";

        try
        {
            ExecuteSql(sql);
        }
        catch (SqlException ex)
        {
            Utilities.DisplayExceptionErrorMessage($"Failed to create database table {ConfigSettings.tbStackName}", ex.Message);
            Utilities.DisplayInformationConsoleMessage("Application cannot start without the database. Please check the error and restart the application.");
            Environment.Exit(1); // Exit the application with an error code
        }
    }

    private void CreateFlashCardsTable()
    {
        string sql = $@"
            CREATE TABLE {ConfigSettings.tbFlashCardsName} (
                CardID INT PRIMARY KEY IDENTITY,
                Front TEXT NOT NULL,
                Back TEXT NOT NULL,
                StackID INT NOT NULL,
                FOREIGN KEY (StackID) REFERENCES {ConfigSettings.tbStackName}(StackID) ON DELETE CASCADE
            )";

        try
        { 
            ExecuteSql(sql);
        }
        catch (SqlException ex)
        {
            Utilities.DisplayExceptionErrorMessage($"Failed to create database table {ConfigSettings.tbFlashCardsName}", ex.Message);
            Utilities.DisplayInformationConsoleMessage("Application cannot start without the database. Please check the error and restart the application.");
            Environment.Exit(1); // Exit the application with an error code
        }
    }

    private void CreateStudySessionsTable()
    {
        string sql = $@"
            CREATE TABLE {ConfigSettings.tbStudySessionsName} (
                SessionID INT PRIMARY KEY IDENTITY(1,1),
                StackID INT NOT NULL,
                SessionDate DATETIME NOT NULL,
                Score INT NOT NULL,                       
                FOREIGN KEY (StackID) REFERENCES {ConfigSettings.tbStackName}(StackID) ON DELETE CASCADE
            );";

        try 
        { 
            ExecuteSql(sql);
        }
        catch (SqlException ex)
        {
            Utilities.DisplayExceptionErrorMessage($"Failed to create database table {ConfigSettings.tbStudySessionsName}", ex.Message);
            Utilities.DisplayInformationConsoleMessage("Application cannot start without the database. Please check the error and restart the application.");
            Environment.Exit(1); // Exit the application with an error code
        }
    }

    private void CreateFlashCardsView()
    {
        string sql = $@"
            CREATE VIEW {ConfigSettings.vwFlashCardsName} AS
            SELECT
                CardID,
                Front,
                Back
            FROM
                {ConfigSettings.tbFlashCardsName};";

        try
        {
            ExecuteSql(sql);
        }
        catch (SqlException ex)
        {
            Utilities.DisplayExceptionErrorMessage($"Failed to create database views {ConfigSettings.vwFlashCardsName}", ex.Message);
            Utilities.DisplayInformationConsoleMessage("Application cannot start without the database. Please check the error and restart the application.");
            Environment.Exit(1); // Exit the application with an error code
        }
    }

    private void CreateFlashCardsRenumberedView()
    {
        string sql = $@"
            CREATE VIEW {ConfigSettings.vwFlashCardsRenumberedName} AS
            SELECT
                ROW_NUMBER() OVER (PARTITION BY StackID ORDER BY CardID) AS DisplayCardID,
                Front,
                Back,
                StackID
            FROM
                {ConfigSettings.tbFlashCardsName};";

        try
        {
            ExecuteSql(sql);
        }
        catch (SqlException ex)
        {
            Utilities.DisplayExceptionErrorMessage($"Failed to create database views {ConfigSettings.vwFlashCardsRenumberedName}", ex.Message);
            Utilities.DisplayInformationConsoleMessage("Application cannot start without the database. Please check the error and restart the application.");
            Environment.Exit(1); // Exit the application with an error code
        }
    }

    private void CreateStudySessionsView()
    {
        string sql = $@"
            CREATE VIEW {ConfigSettings.vwStudySessionsName} AS
            SELECT
                s.SessionID,
                st.StackName,
                s.SessionDate,
                s.Score
            FROM
                {ConfigSettings.tbStudySessionsName} s
            JOIN
                {ConfigSettings.tbStackName} st ON s.StackID = st.StackID;";

        try
        {
            ExecuteSql(sql);
        }
        catch (SqlException ex)
        {
            Utilities.DisplayExceptionErrorMessage($"Failed to create database views {ConfigSettings.vwFlashCardsRenumberedName}", ex.Message);
            Utilities.DisplayInformationConsoleMessage("Application cannot start without the database. Please check the error and restart the application.");
            Environment.Exit(1); // Exit the application with an error code
        }
    }

    private void ExecuteSql(string sql)
    {
        try
        {
            using (SqlConnection connection = _dbContext.GetConnectionToFlashCards())
            {
                connection.Execute(sql);
            }
        }
        catch
        {
            throw;
        }
    }
}
