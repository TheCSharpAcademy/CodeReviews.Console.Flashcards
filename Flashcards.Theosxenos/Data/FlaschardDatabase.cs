namespace Flashcards.Data;

public class FlaschardDatabase
{
    private IConfiguration? Configuration => Program.Configuration;

    public void SeedInitialData()
    {
        using var connection = GetConnection();
        var hasInitialSeed = connection.ExecuteScalar<int>("select count(*) from DataVersion") >= 1;
        if (hasInitialSeed)
            return;

        SeedStacks();
        SeedFlashcards();
        SeedSessions();

        // Set seeding flag 
        connection.Execute("INSERT INTO DataVersion (Description, AppliedOn) VALUES ('Initial seed', @Now);",
            new { DateTime.Now });
    }

    private void SeedSessions()
    {
        var sessions = new List<Session>();
        var startDate = new DateTime(2024, 1, 1, 12, 0, 0); // Starting point
        var random = new Random();
        var sessionId = 1; // Starting ID

        for (var stackId = 1; stackId <= 2; stackId++) // Assuming 2 stacks
        {
            var currentDate = startDate;
            for (var day = 0; day < 10; day++) // Spread over 10 days
            {
                for (var sessionOfDay = 0; sessionOfDay < 10; sessionOfDay++) // 10 sessions per day
                {
                    sessions.Add(new Session
                    {
                        Id = sessionId++,
                        StackId = stackId,
                        Score = random.Next(0, 101),
                        SessionDate = currentDate
                    });
                    currentDate = currentDate.AddHours(1); // Next session an hour later
                }

                currentDate = currentDate.AddDays(day + 1).AddHours(-10); // Move to next day, reset hour offset
            }
        }

        using var connection = GetConnection();
        connection.Execute(
            "insert into Sessions (StackId, Score, SessionDate) values (@StackId, @Score, @SessionDate)", sessions);
    }

    private void SeedFlashcards()
    {
        List<Flashcard> flashcards =
        [
            new Flashcard
            {
                StackId = 1,
                Title = "C# Basics",
                Question = "What is the purpose of the 'using' statement in C#?",
                Answer =
                    "The 'using' statement is used to ensure that IDisposable objects, such as file and database connections, are properly disposed of once they are no longer needed.",
                Position = 1
            },
            new Flashcard
            {
                StackId = 1,
                Title = "Object-Oriented Programming",
                Question = "What is polymorphism in C#?",
                Answer =
                    "Polymorphism is a concept in C# that allows methods to do different things based on the object that it is acting upon, enabling objects of different classes to be treated as objects of a common superclass.",
                Position = 2
            },
            new Flashcard
            {
                StackId = 1,
                Title = "C# Collections",
                Question = "What is the difference between List<T> and Array in C#?",
                Answer =
                    "The main difference is that arrays have a fixed size while List<T> can dynamically change size. List<T> also provides more methods for searching, sorting, and manipulating collections.",
                Position = 3
            },
            new Flashcard
            {
                StackId = 2,
                Title = "JavaScript Basics",
                Question = "What is the purpose of the 'typeof' operator in JavaScript?",
                Answer =
                    "The 'typeof' operator is used to determine the type of a variable or expression in JavaScript.",
                Position = 1
            },
            new Flashcard
            {
                StackId = 2,
                Title = "JavaScript Functions",
                Question = "What is a callback function in JavaScript?",
                Answer =
                    "A callback function is a function that is passed as an argument to another function and is executed after the completion of that function.",
                Position = 2
            },
            new Flashcard
            {
                StackId = 2,
                Title = "JavaScript Arrays",
                Question = "What is the difference between 'map()' and 'forEach()' methods in JavaScript arrays?",
                Answer =
                    "'map()' returns a new array based on the result of the provided callback function, while 'forEach()' executes the provided callback function for each element without returning a new array.",
                Position = 3
            }
        ];
        using var connection = GetConnection();
        connection.Execute(
            """
            insert into Flashcards (StackId, Title, Question, Answer, Position)
            values (@StackId, @Title, @Question, @Answer, @Position)
            """, flashcards);
    }

    private void SeedStacks()
    {
        List<Stack> stacks =
        [
            new Stack
            {
                Id = 1,
                Name = "C# Stack"
            },
            new Stack
            {
                Id = 2,
                Name = "JavaScript Stack"
            }
        ];

        using var connection = GetConnection();
        connection.Execute("insert into Stacks (Name) values (@Name)", stacks);
    }

    public void CreateDb()
    {
        try
        {
            var connectionString = !string.IsNullOrEmpty(Configuration?.GetConnectionString("DefaultConnection"))
                ? Configuration.GetConnectionString("DefaultConnection")
                : Configuration?.GetConnectionString("SecretConnection");

            var builder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = builder.InitialCatalog;

            builder.InitialCatalog = "master";
            var systemConnectionString = builder.ConnectionString;

            using var connection = new SqlConnection(systemConnectionString);

            var sql = $"""
                       IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = N'{databaseName}')
                           CREATE DATABASE [{databaseName}]
                       """;

            connection.Execute(sql);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"An error occurred during database creation:\n{e.Message}", e);
        }
    }

    public void CreateTables()
    {
        var sql = """
                  -- Stacks table
                  if not exists(select * from sys.tables where name = 'Stacks')
                  create table Stacks (
                    Id int identity  constraint PK_Stacks primary key ,
                    Name nvarchar(50) constraint UQ_StackName unique
                  );

                  -- Flashcards table
                  if not exists(select * from sys.tables where name = 'Flashcards')
                  CREATE TABLE Flashcards (
                    Id INT IDENTITY CONSTRAINT PK_Flashcards PRIMARY KEY,
                    StackId INT NOT NULL CONSTRAINT FK_Flashcards_Stacks_StackId REFERENCES Stacks ON DELETE CASCADE,
                    Title NVARCHAR(50) NOT NULL,
                    Question NVARCHAR(250) NOT NULL,
                    Answer NVARCHAR(250) NOT NULL,
                    Position INT NOT NULL
                  );

                  -- Sessions table
                  if not exists(select * from sys.tables where name = 'Sessions')
                  CREATE TABLE Sessions (
                      Id INT IDENTITY CONSTRAINT PK_Sessions PRIMARY KEY,
                      StackId INT NOT NULL CONSTRAINT FK_Sessions_Stacks_StackId REFERENCES Stacks ON DELETE CASCADE,
                      Score INT NOT NULL,
                      SessionDate DATETIME2 NOT NULL
                  );

                  -- Data Version table
                    if not exists (select * from sys.tables where name = 'DataVersion')
                      create table DataVersion (
                          Id int identity constraint PK_DataVersion primary key,
                          Description nvarchar(255),
                          AppliedOn datetime
                  );
                  """;

        using var connection = GetConnection();
        connection.Execute(sql);
    }

    public IDbConnection GetConnection()
    {
        var connectionString = !string.IsNullOrEmpty(Configuration?.GetConnectionString("DefaultConnection"))
            ? Configuration.GetConnectionString("DefaultConnection")
            : Configuration?.GetConnectionString("SecretConnection");

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("No connection string found.");


        return new SqlConnection(connectionString);
    }
}