using System.Data;
using Flashcards.wkktoria.UserInteractions;
using Microsoft.Data.SqlClient;

namespace Flashcards.wkktoria.Database;

internal class DatabaseManager
{
    private readonly SqlConnection _connection;
    private readonly string _databaseName;

    internal DatabaseManager(string connectionString, string databaseName)
    {
        _connection = new SqlConnection(connectionString);
        _databaseName = databaseName;
    }

    internal void Initialize()
    {
        try
        {
            _connection.Open();

            var query = $"""
                         IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'{_databaseName}')
                         BEGIN
                            CREATE DATABASE {_databaseName};
                         END;
                         """;
            var command = new SqlCommand(query, _connection);

            command.ExecuteNonQuery();
        }
        catch (Exception)
        {
            UserOutput.ErrorMessage("Failed to initialize database.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }
    }

    internal void CreateTables()
    {
        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         IF OBJECT_ID('Stacks', 'U') IS NULL
                         BEGIN
                            CREATE TABLE Stacks(
                                Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
                                Name NVARCHAR(25) NOT NULL
                            );
                         END;

                         IF OBJECT_ID('Cards', 'U') IS NULL
                         BEGIN
                            CREATE TABLE Cards(
                                Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
                                StackId INT FOREIGN KEY REFERENCES Stacks(Id) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL,
                                Front NVARCHAR(50) NOT NULL,
                                Back NVARCHAR(50) NOT NULL
                             );
                         END;

                         IF OBJECT_ID('Sessions', 'U') IS NULL
                         BEGIN
                            CREATE TABLE Sessions(
                                Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
                                StackId INT FOREIGN KEY REFERENCES Stacks(Id) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL,
                                Date TEXT NOT NULL,
                                Score INT NOT NULL
                            );
                         END;
                         """;
            var command = new SqlCommand(query, _connection);

            command.ExecuteNonQuery();
        }
        catch (Exception)
        {
            UserOutput.ErrorMessage("Failed to create tables in database.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }
    }
}