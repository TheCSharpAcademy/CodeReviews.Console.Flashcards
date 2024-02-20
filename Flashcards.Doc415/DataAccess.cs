using Dapper;
using FlashCards.Doc415.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FlashCards.Doc415;

internal class DataAccess
{
    IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

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

                string createStackTableSql = @"IF NOT EXISTS (Select * FROM sys.tables WHERE name='Stacks')
                                            CREATE TABLE Stacks(
                                            ID int IDENTITY (1,1) NOT NULL,
                                            Name NVARCHAR(30) NOT NULL UNIQUE,
                                            Primary Key(Id)
                                            );";
                conn.Execute(createStackTableSql);

                string createFlashcardTableSql = @"IF NOT EXISTS (Select * FROM sys.tables WHERE name='Flashcards')
                                            CREATE TABLE Flashcards(
                                            ID int IDENTITY (1,1) NOT NULL PRIMARY KEY,
                                            Question NVARCHAR(30) NOT NULL,
                                            Answer NVARCHAR(30) NOT NULL,
                                            StackId int NOT NULL
                                                FOREIGN KEY 
                                                REFERENCES Stacks(Id)
                                                ON DELETE CASCADE
                                                ON UPDATE CASCADE
                                            );";

                conn.Execute(createFlashcardTableSql);

                string createStudyTableSql = @"IF NOT EXISTS (Select * FROM sys.tables WHERE name='Studies')
                                            CREATE TABLE Studies(
                                            ID int IDENTITY (1,1) NOT NULL PRIMARY KEY,
                                            Date DATE NOT NULL,
                                            Score int NOT NULL,
                                            StackId int NOT NULL
                                                FOREIGN KEY 
                                                REFERENCES Stacks(Id)
                                                ON DELETE CASCADE
                                                ON UPDATE CASCADE
                                            );";
                conn.Execute(createStudyTableSql);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem creating the tables: {ex.Message}");
        }
    }

    internal void AddStack(CardStack _stack)
    {
        try
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string insertQuery = @"INSERT INTO Stacks (name) Values (@Name)";
            connection.Execute(insertQuery, new { _stack.Name });

        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem inserting the stack: {ex.Message}");
        }
    }

    internal IEnumerable<CardStack> GetAllStacks()
    {
        try
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string getAllStacksQuery = @"SELECT * FROM Stacks";
            var records = connection.Query<CardStack>(getAllStacksQuery);
            return records;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem getting stack list: {ex.Message}");
            Console.ReadLine();
            return new List<CardStack>();
        }
    }

    internal void UpdateFlashcard(int Id, string Question, string Answer, int StackId)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        var updateFlashcardQuery = @"UPDATE Flashcards SET Question=@Question, Answer=@Answer, StackId=@StackId WHERE Id=@Id ";
        connection.Execute(updateFlashcardQuery, new { Question, Answer, StackId, Id });
    }
    internal void DeleteFlashcard(int Id)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        string deleteFlashcardQuery = @"DELETE FROM Flashcards Where Id=@Id";
        connection.Execute(deleteFlashcardQuery, new { Id });
    }
    internal void DeleteStack(int Id)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        string deleteStackById = @"DELETE FROM Stacks Where Id=@Id";
        connection.Execute(deleteStackById, new { Id });
    }

    internal void UpdateStack(int Id, string Name)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        string updateStackById = @"UPDATE Stacks SET Name=@Name WHERE Id=@Id";
        connection.Execute(updateStackById, new { Name, Id });
    }

    internal void AddFlashcard(Flashcard _flashcard)
    {
        try
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string addFlashCardQuery = @"INSERT INTO Flashcards (Question,Answer,StackId) VALUES (@Question,@Answer,@StackId)";
            connection.Execute(addFlashCardQuery, new
            {
                _flashcard.Question,
                _flashcard.Answer,
                _flashcard.StackId
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem inserting the flashcard: {ex.Message}");
            Console.ReadLine();
        }
    }

    internal IEnumerable<Flashcard> GetAllFlashcards()
    {
        try
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string getAllStacksQuery = @"SELECT * FROM Flashcards";
            var records = connection.Query<Flashcard>(getAllStacksQuery);
            return records;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem getting stack list: {ex.Message}");
            Console.ReadLine();
            return new List<Flashcard>();
        }
    }

    internal IEnumerable<Flashcard> GetStackFlashcards(int StackId)
    {
        try
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string getAllStacksQuery = @"SELECT * FROM Flashcards WHERE StackId=@StackId";
            var records = connection.Query<Flashcard>(getAllStacksQuery, new { StackId });
            return records;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem getting stack list: {ex.Message}");
            Console.ReadLine();
            return new List<Flashcard>();
        }
    }

    internal void AddStudy(int StackId, int Score)
    {
        var connection = new SqlConnection(ConnectionString);
        connection.Open();
        var addStudyQuery = @"INSERT INTO Studies (Date,Score,StackId) VALUES (@Date,@Score,@StackId)";
        try
        {
            string Date = DateTime.Now.ToString("yyyy-MM-dd");
            connection.Execute(addStudyQuery, new { Date, Score, StackId });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem adding the study: {ex.Message}");
            Console.ReadLine();
        }
    }

    internal IEnumerable<Study> GetAllStudies()
    {
        var connection = new SqlConnection(ConnectionString);
        connection.Open();
        var getAllStudiesQuery = @"SELECT * FROM studies";
        var records = connection.Query<Study>(getAllStudiesQuery);
        return records;
    }

    internal IEnumerable<Report> GetReport()
    {
        var connection = new SqlConnection(ConnectionString);
        connection.Open();
        var reportQuery = @"SELECT StackId,
                            MONTH(date) AS ""Month"",
	                        YEAR(date) AS ""Year"",
                            AVG(score) AS Avarage,
                            COUNT(*) AS StudyCount
                            FROM Studies GROUP BY
                            StackId,
                            Month(date),
                            YEAR(date)
                            ORDER BY YEAR(date)";
        try
        {
            var reports = connection.Query<Report>(reportQuery);
            return reports;
        }
       catch (SqlException ex)
        {
            Console.WriteLine($"There was an error getting report: {ex.Message}");
            return new List<Report>();
        }
    }
}
