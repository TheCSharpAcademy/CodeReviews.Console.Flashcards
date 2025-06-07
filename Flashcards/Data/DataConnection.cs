using Dapper;
using Flashcards.DTOs;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace Flashcards.Data;

public class DataConnection
{
    private IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    private static string ConnectionString;

    public DataConnection()
    {
        ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
    }

    internal void CreateDatabase()
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var createCategoryTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categories')
                        CREATE TABLE Categories (
                            Id int IDENTITY(1,1) NOT NULL,
                            Name NVARCHAR(20) NOT NULL UNIQUE,
                                PRIMARY KEY (Id)
                        );";

                connection.Execute(createCategoryTable);

                var createFlashcardTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
                        CREATE TABLE Flashcards (
                            Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                            Question NVARCHAR(70) NOT NULL,
                            Answer NVARCHAR(30) NOT NULL,
                            CategoryId int NOT NULL 
                                FOREIGN KEY 
                                REFERENCES Categories(Id) 
                                ON DELETE CASCADE 
                                ON UPDATE CASCADE
                        );";

                connection.Execute(createFlashcardTable);

                var createStudySessionTable =
                    @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessions')
                        CREATE TABLE StudySessions (
                            Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                            Questions int NOT NULL,
                            Date DateTime NOT NULL, 
                            CorrectAnswers int NOT NULL,
                            Percentage AS (CorrectAnswers * 100) / Questions PERSISTED,
                            Time TIME NOT NULL,
                            CategoryId int NOT NULL
                                 FOREIGN KEY 
                                 REFERENCES Categories(Id)
                                 ON DELETE CASCADE 
                                 ON UPDATE CASCADE
                        );";

                connection.Execute(createStudySessionTable);
            }
        }
        catch (Exception e)
        {
            AnsiConsole.WriteLine($"There was a problem creating the tables: {e.Message}");
        }
    }

    #region Category Methods

    internal void InsertCategory(Category category)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string insertQuery = "INSERT INTO Categories (Name) VALUES (@Name)";

            connection.Execute(insertQuery, new { Name = category.Name });
            ;
        }
    }

    internal IEnumerable<Category> GetAllCategories()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string insertQuery = "SELECT * FROM Categories";

            var records = connection.Query<Category>(insertQuery);

            return records;
        }
    }

    internal void UpdateCategory(Category category)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string updateQuery = "UPDATE Categories SET Name = @Name WHERE Id = @Id";

            connection.Execute(updateQuery, new { Name = category.Name, Id = category.Id });
        }
    }

    internal void DeleteCategory(int id)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string deleteQuery = "DELETE FROM Categories WHERE Id = @Id";

            connection.Execute(deleteQuery, new { Id = id });
        }
    }

    #endregion

    #region Flashcard Methods

    internal void InsertFlashcard(Flashcard flashcard)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string insertQuery = @"
                    INSERT INTO Flashcards (Question, Answer, CategoryId) 
                    VALUES (@Question, @Answer, @CategoryId)";

            connection.Execute(insertQuery,
                new { Question = flashcard.Question, Answer = flashcard.Answer, flashcard.CategoryId });
        }
    }

    internal IEnumerable<Flashcard> GetFlashcardByCategory(int categoryId)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string selectQuery = "SELECT * FROM Flashcards WHERE CategoryId = @CategoryId";

            var records = connection.Query<Flashcard>(selectQuery, new { CategoryId = categoryId });

            return records;
        }
    }

    internal IEnumerable<Flashcard> GetAllFlashcards()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string insertQuery = "SELECT * FROM Flashcards";

            var records = connection.Query<Flashcard>(insertQuery);

            return records;
        }
    }

    internal void UpdateFlashcard(int flashcardId, Dictionary<string, object> flashcardToUpdate)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string updateQuery = "UPDATE Flashcards SET ";

            var parameters = new DynamicParameters();
            foreach (var kvp in flashcardToUpdate)
            {
                updateQuery += $"{kvp.Key} = @{kvp.Key}, ";
                parameters.Add(kvp.Key, kvp.Value);
            }

            updateQuery = updateQuery.TrimEnd(',', ' ');

            updateQuery += " WHERE Id = @Id";
            parameters.Add("Id", flashcardId);

            connection.Execute(updateQuery, parameters);
        }
    }

    internal void DeleteFlashcard(int id)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string deleteQuery = "DELETE FROM Flashcards WHERE Id = @Id";

            connection.Execute(deleteQuery, new { Id = id });
        }
    }

    #endregion

    #region StudySession Methods

    internal void InsertStudySession(StudySession session)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string insertQuery =
                @"INSERT INTO StudySessions (Questions, CorrectAnswers, CategoryId, Time, Date) 
                VALUES (@Questions, @CorrectAnswers, @CategoryId, @Time, @Date)";

            connection.Execute(insertQuery,
                new { session.Questions, session.CorrectAnswers, session.CategoryId, session.Time, session.Date });
        }
    }

    internal List<StudySessionDTO> GetStudySessions()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string selectQuery = 
                @"SELECT
                    category.Name as CategoryName,
                    studySession.Date,
                    studySession.Questions,
                    studySession.CorrectAnswers,
                    studySession.Percentage,
                    studySession.Time
                FROM
                    StudySessions studySession
                INNER JOIN
                    Categories category ON studySession.CategoryId = category.Id;";

            return connection.Query<StudySessionDTO>(selectQuery).ToList();
        }
    }

    #endregion

    #region Seed Data

    internal void InsertSeedSessions(List<Category> categories, List<Flashcard> flashcards)
    {
        SqlTransaction transaction = null;

        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            transaction = connection.BeginTransaction();

            connection.Execute("INSERT INTO Categories (Name) VALUES (@Name)", categories, transaction: transaction);
            connection.Execute(
                "INSERT INTO Flashcards (Question, Answer, CategoryId) VALUES (@Question, @Answer, @CategoryId)",
                flashcards, transaction: transaction);

            transaction.Commit();
        }
    }

    internal void DeleteTables()
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string dropFlashcardsTable = "DROP TABLE Flashcards";
                connection.Execute(dropFlashcardsTable);

                string dropStudySessionsTable = "DROP TABLE StudySessions";
                connection.Execute(dropStudySessionsTable);

                string dropCategoriesTable = "DROP TABLE Categories";
                connection.Execute(dropCategoriesTable);
            }
        }
        catch (Exception e)
        {
            AnsiConsole.WriteLine($"There was a problem deleting the tables: {e.Message}");
        }
    }

    #endregion
}