using Dapper;
using Flashcards.alexgit55.Models;
using Flashcards.alexgit55.Models.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Flashcards.alexgit55;

internal class DataAccess
{
    IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

    private string ConnectionString;

    public DataAccess()
    {
        ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
    }

    internal void CreateTables()
    {
        try
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                string createStackTableSql =
                    @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
                    CREATE TABLE Stacks (
                        Id int IDENTITY(1,1) NOT NULL,
                        Name NVARCHAR(30) NOT NULL UNIQUE,
                        PRIMARY KEY (Id)
                    );";
                conn.Execute(createStackTableSql);

                string createFlashcardTableSql =
                    @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
                    CREATE TABLE Flashcards (
                        Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        Question NVARCHAR(40) NOT NULL,
                        Answer NVARCHAR(30) NOT NULL,
                        StackId int NOT NULL 
                            FOREIGN KEY 
                            REFERENCES Stacks(Id) 
                            ON DELETE CASCADE 
                            ON UPDATE CASCADE
                    );";
                conn.Execute(createFlashcardTableSql);

                string createStudySessionTableSql =
     @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessions')
     CREATE TABLE StudySessions (
         Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
         Questions int NOT NULL,
         Date DateTime NOT NULL, 
         CorrectAnswers int NOT NULL,
         Percentage AS (CorrectAnswers * 100) / Questions PERSISTED,
         Time TIME NOT NULL,
         StackId int NOT NULL
             FOREIGN KEY 
             REFERENCES Stacks(Id)
             ON DELETE CASCADE 
             ON UPDATE CASCADE
 );";
                conn.Execute(createStudySessionTableSql);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem creating the tables: {ex.Message}");
        }
    }

    internal void InsertStack(Stack stack)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string insertQuery = @"
         INSERT INTO Stacks (Name) VALUES (@Name)";

                connection.Execute(insertQuery, new { stack.Name });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem inserting the stack: {ex.Message}");
        }
    }

    internal IEnumerable<Stack> GetAllStacks()
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM stacks";

                var records = connection.Query<Stack>(selectQuery);

                return records;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem retrieving stacks: {ex.Message}");
            return new List<Stack>();
        }
    }

    internal IEnumerable<Flashcard> GetFlashcards(int stackid)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string selectQuery = $"SELECT * FROM flashcards WHERE StackId={stackid}";
                var records = connection.Query<Flashcard>(selectQuery);
                return records;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem retrieving flashcards: {ex.Message}");
            return new List<Flashcard>();
        }
    }

    internal void InsertFlashcard(Flashcard flashcard)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string insertQuery = @"
         INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)";

                connection.Execute(insertQuery, new { flashcard.Question, flashcard.Answer, flashcard.StackId });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem inserting the flashcard: {ex.Message}");
        }
    }

    internal void BulkInsertRecords(List<Stack> stacks, List<Flashcard> flashcards)
    {
        SqlTransaction transaction = null;
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                connection.Execute("INSERT INTO Stacks (Name) VALUES (@Name)", stacks, transaction: transaction);
                connection.Execute("INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)", flashcards, transaction: transaction);

                transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem bulk inserting records: {ex.Message}");

            if (transaction != null && transaction.Connection != null)
            {
                transaction.Rollback();
            }
        }
    }

    internal void DeleteTables()
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string dropFlashcardsTableSql = @"DROP TABLE Flashcards";
                connection.Execute(dropFlashcardsTableSql);

                string dropStudySessionsTableSql = @"DROP TABLE StudySessions";
                connection.Execute(dropStudySessionsTableSql);

                string dropStacksTableSql = @"DROP TABLE Stacks";
                connection.Execute(dropStacksTableSql);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem deleting tables: {ex.Message}");
        }
    }

    internal void DeleteFlashcard(int id)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM flashcards WHERE Id = @Id";

                int rowsAffected = connection.Execute(deleteQuery, new { Id = id });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem deleting the flashcard: {ex.Message}");

        }
    }

    internal void DeleteStack(int id)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM stack WHERE Id = @Id";

                int rowsAffected = connection.Execute(deleteQuery, new { Id = id });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem deleting the stack: {ex.Message}");

        }
    }

    internal void UpdateStack(Stack stack)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string updateQuery = @"
    UPDATE stacks
    SET Name = @Name
    WHERE Id = @Id";

            connection.Execute(updateQuery, new { stack.Name, stack.Id });
        }
    }

    internal void UpdateFlashcard(int flashcardId, Dictionary<string, object> propertiesToUpdate)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string updateQuery = "UPDATE flashcards SET ";
            var parameters = new DynamicParameters();

            foreach (var kvp in propertiesToUpdate)
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

    internal void InsertStudySession(StudySession session)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string insertQuery = @"
        INSERT INTO StudySessions (Questions, CorrectAnswers, StackId, Time, Date) VALUES (@Questions, @CorrectAnswers, @StackId, @Time, @Date)";

                connection.Execute(insertQuery, new { session.Questions, session.CorrectAnswers, session.StackId, session.Time, session.Date });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem with the study session: {ex.Message}");
        }
    }

    internal List<StudySessionDTO> GetStudySessionData()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string sql = @"
     SELECT
         s.Name as StackName,
         ss.Date,
         ss.Questions,
         ss.CorrectAnswers,
         ss.Percentage,
         ss.Time
     FROM
         StudySessions ss
     INNER JOIN
         Stacks s ON ss.StackId = s.Id;
 ";

            return connection.Query<StudySessionDTO>(sql).ToList();
        }
    }
}
