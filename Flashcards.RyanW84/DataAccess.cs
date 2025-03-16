using Dapper;
using Flashcards.RyanW84.Models;
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
    }

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
                        Name NVARCHAR(200) NOT NULL UNIQUE,
                        PRIMARY KEY (Id)
                    );";
                conn.Execute(createStackTableSql);

                string createFlashcardTableSql =
                    @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
                    CREATE TABLE Flashcards (
                        Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        Question NVARCHAR(200) NOT NULL,
                        Answer NVARCHAR(200) NOT NULL,
                        StackId int NOT NULL 
                            FOREIGN KEY 
                            REFERENCES Stacks(Id) 
                            ON DELETE CASCADE 
                            ON UPDATE CASCADE
                    );";
                conn.Execute(createFlashcardTableSql);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem creating the tables: {ex.Message}");
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

    internal void BulkInsertRecords(List<Stack> stacks, List<Flashcard> flashcards)
    {
        SqlTransaction transaction = null; // Declare the transaction variable outside the try block
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
}
