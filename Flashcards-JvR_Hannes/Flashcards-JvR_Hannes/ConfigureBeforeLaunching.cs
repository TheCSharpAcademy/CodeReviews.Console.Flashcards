using Spectre.Console;
using System.Data.SqlClient;
namespace Flashcards_JvR_Hannes
{
    internal static class ConfigureBeforeLaunching
    {
        public static void EnsureDatabaseExists()
        {
            string dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "flashcardsdb.mdf");

            if (!File.Exists(dbFilePath))
            {
                AnsiConsole.MarkupLine("[yellow]Database file not found. Creating a new one...[/]");

                string connectionString = @"Server=(LocalDB)\MSSQLLocalDB;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $@"CREATE DATABASE [flashcardsdb] ON (NAME = N'flashcardsdb', FILENAME = '{dbFilePath}')";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                        AnsiConsole.MarkupLine("[green]Database created successfully at path: {dbFilePath}[/]");
                    }
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[green]Database file found at path: {dbFilePath}[/]");
            }
        }
        public static void InsertInitialData(SqlConnection connection)
        {
            InsertStacks(connection);
            InsertFlashcards(connection);
        }
        private static void InsertStacks(SqlConnection connection)
        {
            string checkQuery = @"SELECT COUNT(*) FROM Stacks";
            string insertQuery = 
                @"INSERT INTO Stacks (StackName)
                VALUES
                ('Mathematics'),
                ('History'),
                ('Programming')";
            using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
            {
                int count = (int)checkCommand.ExecuteScalar();
                if (count == 0)
                {
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.ExecuteNonQuery();
                        AnsiConsole.MarkupLine("[green]Initial stacks inserted.[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Stacks already exists. Skipping insertion...[/]");
                }
            }
        }
        private static void InsertFlashcards(SqlConnection connection)
        {
            string chechQuery = "SELECT COUNT(*) FROM Flashcards";
            string insertQuery = 
                @"INSERT INTO Flashcards (DisplayId, Question, Answer, StackId)
                VALUES
                    (1, 'What is 2 + 2?', '4', (SELECT StackId FROM Stacks WHERE StackName = 'Mathematics')),
                    (2, 'Who was the Fist President of the USA?', 'George Washington', (SELECT StackId FROM Stacks WHERE StackName = 'History')),
                    (3, 'What is a Class in Programming', 'a Blueprint for objects', (SELECT StackId FROM Stacks WHERE StackName = 'Programming'))";
            using (SqlCommand chechCommand = new SqlCommand(chechQuery, connection))
            {
                int count = (int)chechCommand.ExecuteScalar();
                if (count == 0)
                {
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.ExecuteNonQuery();
                        AnsiConsole.MarkupLine("[green]Initial flashcards inserted[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Flashcards already exists. Skipping insertion...[/]");
                }
            }
        }
        public static void CreateStacksTable(SqlConnection connection)
        {
            string query =
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
                  BEGIN
                      CREATE TABLE Stacks (
                          StackId INT PRIMARY KEY IDENTITY,
                          StackName NVARCHAR(255) UNIQUE NOT NULL
                          );
                  END";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
                AnsiConsole.MarkupLine("[green]Stacks table is ready.[/]");
            }
        }
        public static void CreateFlashcardsTable(SqlConnection connection)
        {
            string query = 
                @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
                BEGIN
                    CREATE TABLE Flashcards (
                    FlashcardId INT PRIMARY KEY IDENTITY(1,1),
                    DisplayId INT NOT NULL,
                    Question NVARCHAR(255) NOT NULL,
                    Answer NVARCHAR(255) NOT NULL,
                    StackId INT NOT NULL, -- This is the foreign key referencing Stacks
                    FOREIGN KEY (StackId) REFERENCES Stacks(StackId) ON DELETE CASCADE -- Correctly reference the StackId column in Stacks
                    );
                END";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
                AnsiConsole.MarkupLine("[green]Flashcards table is ready.[/]");
            }
        }
        public static void CreateStudySession(SqlConnection connection)
        {
            string query = 
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessions')
                BEGIN
                    CREATE TABLE StudySessions (
                        SessionId INT PRIMARY KEY IDENTITY,
                        StackId INT NOT NULL FOREIGN KEY REFERENCES Stacks(StackId) ON DELETE CASCADE,
                        Date DATETIME NOT NULL,
                        Score INT NOT NULL
                        );
                END";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
                AnsiConsole.MarkupLine("[green]StudySessions table is ready.[/]");
            }
        }
    }
}
