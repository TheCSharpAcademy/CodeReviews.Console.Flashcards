namespace DotNETConsole.Flashcards.Database;
using Microsoft.Data.SqlClient;

public class Migration
{
    private DbContext _dbContext = new DbContext();

    public void Up()
    {
        // Stack Table.
        string categoryTableQuery = @"IF OBJECT_ID('dbo.stacks', 'U') IS NULL
           CREATE TABLE dbo.stacks (
               ID INT IDENTITY(1,1) PRIMARY KEY,
               NAME VARCHAR(50) UNIQUE,
               CreatedAt DATETIME DEFAULT GETDATE(),
           );";

        using (var connection = _dbContext.DBConnection())
        {
            SqlCommand command = new SqlCommand(categoryTableQuery, connection);
            connection.Open();
            command.ExecuteNonQuery();
            Console.WriteLine("stacks table created");
        }

        // FlashCard Table.
        string flashCardTableQuery = @"IF OBJECT_ID('dbo.flashcards', 'U') IS NULL
                CREATE TABLE flashcards(
                    ID INT IDENTITY(1,1) PRIMARY KEY,
                    Question TEXT,
                    Answer VARCHAR(300),
                    CreatedAt DATETIME DEFAULT GETDATE(),
                    STACK_ID INT,
                    FOREIGN KEY (STACK_ID) REFERENCES stacks(ID) ON DELETE CASCADE
                );";

        using (var connection = _dbContext.DBConnection())
        {
            SqlCommand command = new SqlCommand(flashCardTableQuery, connection);
            connection.Open();
            command.ExecuteNonQuery();
            Console.WriteLine("flashcards table created");
        }

        // Study Session Table.
        string studySessionQuery = @"IF OBJECT_ID('dbo.studylogs', 'U') IS NULL
                        CREATE TABLE studylogs(
                            ID INT IDENTITY(1,1) PRIMARY KEY,
                            LogDate DATETIME DEFAULT GETDATE(),
                            Score INT DEFAULT 0,
                            STACK_ID INT,
                            FOREIGN KEY (STACK_ID) REFERENCES stacks(ID) ON DELETE CASCADE
                        );";

        using (var connection = _dbContext.DBConnection())
        {
            SqlCommand command = new SqlCommand(studySessionQuery, connection);
            connection.Open();
            command.ExecuteNonQuery();
            Console.WriteLine("studylogs table created");
        }
    }

    public void Down()
    {
        // Studylogs Table
        string studylogsTableQuery = @"IF OBJECT_ID('dbo.studylogs', 'U') IS NOT NULL
                                                            DROP TABLE studylogs;";

        using (var connection = _dbContext.DBConnection())
        {
            SqlCommand command = new SqlCommand(studylogsTableQuery, connection);
            connection.Open();
            command.ExecuteNonQuery();
            Console.WriteLine("studylogs table dropped");
        }

        // Flashcards Table
        string flashcardsTableQuery = @"IF OBJECT_ID('dbo.flashcards', 'U') IS NOT NULL
                                            DROP TABLE flashcards;";

        using (var connection = _dbContext.DBConnection())
        {
            SqlCommand command = new SqlCommand(flashcardsTableQuery, connection);
            connection.Open();
            command.ExecuteNonQuery();
            Console.WriteLine("flashcards table dropped");
        }

        // Stack Table
        string stackTableQuery = @"IF OBJECT_ID('dbo.stacks', 'U') IS NOT NULL
                                            DROP TABLE stacks;";

        using (var connection = _dbContext.DBConnection())
        {
            SqlCommand command = new SqlCommand(stackTableQuery, connection);
            connection.Open();
            command.ExecuteNonQuery();
            Console.WriteLine("stacks table dropped");
        }
    }

    public void Reset()
    {
        this.Down();
        this.Up();
        Console.WriteLine("Database Reseted");
    }
}
