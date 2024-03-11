using Dapper;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Stack = Flashcards.Models.Stack;

public class DataAcess
{
    IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    private string ConnectionString;

    public DataAcess()
    {
        ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
    }

    public void CreateTables()
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string createStackTableSql =
                   @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
                    CREATE TABLE Stacks (
                        Id int IDENTITY(1,1) NOT NULL,
                        Name NVARCHAR(30) NOT NULL UNIQUE,
                        PRIMARY KEY (Id)
                    );";

                connection.Execute(createStackTableSql);

                string createFlashcardTableSql =
                    @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
                    CREATE TABLE Flashcards (
                        Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        Question NVARCHAR(30) NOT NULL,
                        Answer NVARCHAR(30) NOT NULL,
                        StackId int NOT NULL 
                            FOREIGN KEY 
                            REFERENCES Stacks(Id) 
                            ON DELETE CASCADE 
                            ON UPDATE CASCADE
                    );";

                connection.Execute(createFlashcardTableSql);

                string createStudySessionTableSql =
                    @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessions')
                    CREATE TABLE StudySessions (
                        Id int IDENTITY (1,1) NOT NULL PRIMARY KEY,
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

                connection.Execute(createStudySessionTableSql);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    internal List<StudySessionDTO> GetStudySessionData()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string getStudyQuery = @"
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


            return connection.Query<StudySessionDTO>(getStudyQuery).ToList();
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
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
            Console.WriteLine(ex.Message);
        }
    }

    internal Stack GetStackByID(int stackId)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string selectQuery = @$"SELECT * FROM stacks
                                        WHERE Id = {stackId};";


                return connection.QueryFirstOrDefault<Stack>(selectQuery);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new List<Stack>();
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

    internal IEnumerable<Flashcard> GetFlashcards(int stackId)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM flashcards WHERE StackId = @StackId";

                var records = connection.Query<Flashcard>(selectQuery, new { StackId = stackId }).ToList();
                return records;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new List<Flashcard>();
        }
    }

    internal void InsertFlashcard(Flashcard flashcards)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string inserQuery = @"
            INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)";

                connection.Execute(inserQuery, new { flashcards.Question, flashcards.Answer, flashcards.StackId });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
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

            updateQuery += " WHERE Id = @Id;";
            parameters.Add("Id", flashcardId);

            connection.Execute(updateQuery, parameters);
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

                string dropStacksTableSql = @"DROP TABLE Stacks";
                connection.Execute(dropStacksTableSql);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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

                string resetQuery = @"
                WITH CTE AS (
                    SELECT Id, ROW_NUMBER() OVER (ORDER BY Id) AS NewId
                    FROM flashcards
                )
                UPDATE CTE SET Id = NewId";

                connection.Execute(resetQuery);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    internal void DeleteStack(int id)
    {
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM stacks WHERE Id = @Id";
                int rowsAffected = connection.Execute(deleteQuery, new { Id = id });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}