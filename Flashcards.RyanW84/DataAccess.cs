using Dapper;
using Flashcards.RyanW84;
using Flashcards.RyanW84.Models;
using Flashcards.RyanW84.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class DataAccess
{
    IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    private string ConnectionString;

    public bool ConfirmConnection()
    {
        try
        {
            Console.WriteLine("*_*_*_* Flashcards *_*_*_* ");
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                Console.Write(
                    $"\nConnection Status: {System.Data.ConnectionState.Open}\nPress any Key to continue:"
                );
                Console.ReadKey();
                return true;
            }
        }
        catch
        {
            return false;
        }
    } //Informs the user if the connection was Successful / Unsuccessful

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
                        Id INT IDENTITY(1,1) NOT NULL,
                        Name NVARCHAR(200) NOT NULL UNIQUE,
                        PRIMARY KEY (Id)
                    );";
                conn.Execute(createStackTableSql);

                string createFlashcardTableSql =
                    @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
                    CREATE TABLE Flashcards (
                        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        Question NVARCHAR(200) NOT NULL,
                        Answer NVARCHAR(200) NOT NULL,
                        StackId INT NOT NULL 
                            FOREIGN KEY 
                            REFERENCES Stacks(Id) 
                            ON DELETE CASCADE 
                            ON UPDATE CASCADE
                    );";
                conn.Execute(createFlashcardTableSql);

                string createStudySessionTableSql =
                    @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessions')
     CREATE TABLE StudySessions (
         Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
         Questions INT NOT NULL,
         Date DateTime NOT NULL, 
         CorrectAnswers int NOT NULL,
         Percentage AS (CorrectAnswers * 100) / Questions PERSISTED,
         Time TIME NOT NULL,
         StackId INT NOT NULL
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

    internal IEnumerable<Stack> GetAllStacks()
    {
        Console.Clear();
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

    internal void BulkInsertRecords(List<Stack> stacks, List<Flashcard> flashcards)
    {
        SqlTransaction transaction = null; // transaction variable has to be outside the try block
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                transaction = connection.BeginTransaction();

                connection.Execute(
                    "INSERT INTO Stacks (Name) VALUES (@Name)",
                    stacks,
                    transaction: transaction
                );
                connection.Execute(
                    "INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)",
                    flashcards,
                    transaction: transaction
                );

                transaction.Commit(); // Commit the transaction if everything is successful
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem bulk inserting records: {ex.Message}");

            if (transaction != null)
            {
                transaction.Rollback(); // Rollback the transaction if an exception occurs
            }
        }
    }

    //Stack Data Access Methods
    internal void InsertStack(Stack stack)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string insertQuery =
                    @"
         INSERT INTO Stacks (Name) VALUES (@Name)";

                connection.Execute(insertQuery, new { stack.Name });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem inserting the stack: {ex.Message}");
        }
    }

    internal void DeleteStack(int id)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string deleteQuery = @"DELETE FROM stacks WHERE Id = @Id;";
                int rowsAffected = connection.Execute(deleteQuery, new { Id = id });
                Console.WriteLine($"Rows Affected: {rowsAffected}");
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

            string updateQuery =
                @"
    UPDATE stacks
    SET Name = @Name
    WHERE Id = @Id";

            connection.Execute(updateQuery, new { stack.Name, stack.Id });
        }
    }

    //Flashcard Data Access Methods
    internal IEnumerable<Flashcard> GetFlashcards(int stackId)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM flashcards WHERE StackId = @stackId";

                IEnumerable<Flashcard> records = connection.Query<Flashcard>(
                    selectQuery,
                    new { stackId }
                );

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

                string insertQuery =
                    @"
         INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)";

                connection.Execute(
                    insertQuery,
                    new
                    {
                        flashcard.Question,
                        flashcard.Answer,
                        flashcard.StackId,
                    }
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem inserting the flashcard: {ex.Message}");
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

    //Study Session Data Access Methods

    internal void InsertStudySession(StudySession session)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string insertQuery =
                    @"INSERT INTO StudySessions (Questions, CorrectAnswers, StackId, Time, Date) VALUES (@Questions, @CorrectAnswers, @StackId, @Time, @Date)";

                connection.Execute(
                    insertQuery,
                    new
                    {
                        session.Questions,
                        session.CorrectAnswers,
                        session.StackId,
                        session.Time,
                        session.Date,
                    }
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was an issues with the Study Session: {ex.Message}");
        }
    }

    internal List<StudySessionDTO> GetStudySessionData()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string sql =
                @"
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
        Stacks s ON ss.StackId = s.Id;";

            return connection.Query<StudySessionDTO>(sql).ToList();
        }
    }

    internal List<StudySessionDTO> GetReportData(int year)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string sqlGetReport =
                $@"
                SELECT 
                    MONTH(Date) AS Month, 
                    COUNT(*) AS SessionCount
                FROM StudySessions
                WHERE Date BETWEEN '{year}-01-01' AND '{year}-12-31'
                GROUP BY MONTH(Date)
                ORDER BY Month";

            return connection.Query<StudySessionDTO>(sqlGetReport).ToList();
        }
    }
}
