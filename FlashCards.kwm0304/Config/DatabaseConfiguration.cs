using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace FlashCards.kwm0304.Config;

public class DatabaseConfiguration
{
  private readonly string _connString = AppConfiguration.GetConnectionString("DefaultConnection");
  public DatabaseConfiguration(string connString)
  {
    _connString = connString;
  }

  public bool DatabaseExists()
  {
    var sql = "SELECT database_id FROM sys.databases WHERE Name = localdb";
    using var connection = new SqlConnection(_connString);
    using var command = new SqlCommand(sql, connection);
    try
    {
      connection.Open();
      var result = command.ExecuteScalar();
      return result != null;
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
      return false;
    }
  }

  public void CreateDatabaseOnStart()
  {
    if (!DatabaseExists())
    {
      var sql = "CREATE DATABASE localdb";
      using var connection = new SqlConnection(_connString);
      using var command = new SqlCommand(sql, connection);
      try
      {
        connection.Open();
        command.ExecuteNonQuery();
        AnsiConsole.WriteLine("Database created successfully");
      }
      catch (Exception e)
      {
        AnsiConsole.WriteException(e);
      }
    }
    else
    {
      AnsiConsole.WriteLine("Database already exists");
    }
  }

  public void CreateStackTableIfNotExists()
  {
    var sql = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Stacks')
            CREATE TABLE Stacks (
                StackId INT PRIMARY KEY IDENTITY,
                StackName NVARCHAR(100) NOT NULL
            )";

    Create(sql, "Stacks table created successfully");
  }
  public void CreateStudySessionTableIfNotExists()
  {
    var sql = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StudySessions')
            CREATE TABLE StudySessions (
                StudySessionId INT PRIMARY KEY IDENTITY,
                StackId INT NOT NULL,
                StudiedAt DATETIME NOT NULL,
                Score INT NOT NULL,
                FOREIGN KEY (StackId) REFERENCES Stacks(StackId)
            )";
    Create(sql, "StudySessions table created successfully");
  }
  public void CreateFlashcardTableIfNotExists()
  {
    var sql = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Flashcards')
            CREATE TABLE Flashcards (
                FlashCardId INT PRIMARY KEY IDENTITY,
                StackId INT NOT NULL,
                Question NVARCHAR(255) NOT NULL,
                Answer NVARCHAR(255) NOT NULL,
                FOREIGN KEY (StackId) REFERENCES Stacks(StackId)
            )";

    Create(sql, "Flashcards table created successfully");
  }

  private void Create(string sql, string msg)
  {
    using var connection = new SqlConnection(_connString);
    using var command = new SqlCommand(sql, connection);
    try
    {
      connection.Open();
      command.ExecuteNonQuery();
      AnsiConsole.WriteLine(msg);
    }
    catch (Exception e)
    {
      AnsiConsole.WriteException(e);
    }
  }

  public void DatabaseActionsOnStart()
  {
    CreateDatabaseOnStart();
    CreateStackTableIfNotExists();
    CreateStudySessionTableIfNotExists();
    CreateFlashcardTableIfNotExists();
  }
}