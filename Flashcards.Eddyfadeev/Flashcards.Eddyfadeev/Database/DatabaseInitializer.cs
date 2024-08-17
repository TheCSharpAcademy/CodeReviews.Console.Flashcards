using Dapper;
using Flashcards.Eddyfadeev.Interfaces.Database;

namespace Flashcards.Eddyfadeev.Database;

/// <summary>
/// The DatabaseInitializer class is responsible for initializing the database.
/// </summary>
internal class DatabaseInitializer : IDatabaseInitializer
{
    private readonly IConnectionProvider _connectionProvider;

    public DatabaseInitializer(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    /// <summary>
    /// Initializes the database by creating necessary tables.
    /// </summary>
    public void Initialize()
    {
        CreateStacks();
        CreateFlashcards();
        CreateStudySessionTable();
    }

    private void CreateStacks()
    {
        try
        {
            using var conn = _connectionProvider.GetConnection();

            conn.Open();

            const string createStackTableSql =
                """
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
                    CREATE TABLE Stacks (
                        Id INT IDENTITY(1,1) NOT NULL,
                        Name NVARCHAR(60) NOT NULL UNIQUE,
                        PRIMARY KEY (Id)
                    );
                """;

            conn.Execute(createStackTableSql);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem creating the Stacks table: {ex.Message}");
        }
    }

    private void CreateFlashcards()
    {
        try
        {
            using var conn = _connectionProvider.GetConnection();

            conn.Open();

            const string createFlashcardTableSql =
                """
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
                    CREATE TABLE Flashcards (
                        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        Question NVARCHAR(60) NOT NULL,
                        Answer NVARCHAR(60) NOT NULL,
                        StackId INT NOT NULL
                            FOREIGN KEY
                            REFERENCES Stacks(Id)
                            ON DELETE CASCADE
                            ON UPDATE CASCADE
                    );
                """;

            conn.Execute(createFlashcardTableSql);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem creating the Flashcards table: {ex.Message}");
        }
    }

    private void CreateStudySessionTable()
    {
        try
        {
            using var conn = _connectionProvider.GetConnection();
            
            conn.Open();
            
            const string createStudySessionTableSql = 
                """
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessions')
                    CREATE TABLE StudySessions (
                        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        Questions INT NOT NULL,
                        Date DATETIME NOT NULL,
                        CorrectAnswers INT NOT NULL,
                        Percentage AS (CorrectAnswers * 100) / Questions PERSISTED,
                        Time TIME NOT NULL,
                        StackId INT NOT NULL
                            FOREIGN KEY
                            REFERENCES Stacks(Id)
                            ON DELETE CASCADE
                            ON UPDATE CASCADE
                    );
                """;
            
            conn.Execute(createStudySessionTableSql);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was a problem creating the StudySessions table: {ex.Message}");
        }
    }
}