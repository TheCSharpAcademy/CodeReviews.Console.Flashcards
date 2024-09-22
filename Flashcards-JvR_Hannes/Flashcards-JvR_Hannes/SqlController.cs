using System.Data.SqlClient;
using Spectre.Console;
using static Flashcards_JvR_Hannes.Dto;
namespace Flashcards_JvR_Hannes
{
    internal static class SqlController
    {
        public static class DatabaseManager
        {
            internal static void InitializeDatabase()
            {

                using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        Console.WriteLine("Database connection succesfull");

                        ConfigureBeforeLaunching.CreateStacksTable(connection);
                        ConfigureBeforeLaunching.CreateFlashcardsTable(connection);
                        ConfigureBeforeLaunching.CreateStudySession(connection);

                        ConfigureBeforeLaunching.InsertInitialData(connection);
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"An error occurred while connecting to the database: {ex.Message}");
                    }
                }
            }
        }
        public static void AddStack(string stackName)
        {
            Console.Clear();
            using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
            {
                string query = @"INSERT INTO Stacks (StackName) VALUES (@StackName)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StackName", stackName);

                    connection.Open();
                    command.ExecuteNonQuery();
                    AnsiConsole.MarkupLine($"[blue]Stack '{stackName}' added successfully[/]");
                }
            }
        }
        public static void MenuDisplay()
        {
            Console.Clear();
            bool appClose = false;

            while (!appClose)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[lime]Main Menu[/]")
                        .PageSize(10)  
                        .HighlightStyle(new Style(foreground: Color.Aqua))
                        .AddChoices(new[] {
                            "Create Stack",
                            "Update Stack",
                            "View Stacks",
                            "Delete Stack",
                            "Study Session",
                            "View Study Sessions",
                            "Close App"
                        }));

                switch (choice)
                {
                    case "Create Stack":
                        AnsiConsole.MarkupLine("[blue]You selected 'Create Stack'[/]");
                        string stackName = AnsiConsole.Ask<string>("Enter a stack name: ");
                        AddStack(stackName);
                        break;

                    case "Update Stack":
                        AnsiConsole.MarkupLine("[blue]You selected 'Update Stack'[/]");

                        int stackId = AnsiConsole.Ask<int>("Enter the stack ID to update");

                        string newStackName = AnsiConsole.Ask<string>("Enter the new name for the stack");

                        using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
                        {
                            connection.Open();

                            string updateQuery = "UPDATE Stacks SET StackName = @newStackName WHERE StackId = @stackId";

                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@newStackName", newStackName);
                                updateCommand.Parameters.AddWithValue("@stackId", stackId);

                                int rowsAffected = updateCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    AnsiConsole.MarkupLine("[green]Stack updated successfully.[/]");

                                    RenumberFlashcards(connection, stackId);
                                    AnsiConsole.MarkupLine("[green]Flashcards renumbered successfully.[/]");
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine("[red]Stack not found. Please make sure the stack ID is correct.[/]");
                                }
                            }
                        }
                        break;

                    case "View Stacks":
                        AnsiConsole.MarkupLine("[blue]You selected 'View Stacks'[/]");
                        using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
                        {
                            connection.Open();
                            var stacks = GetStacks(connection);

                            foreach( var stack in stacks)
                            {
                                AnsiConsole.MarkupLine($"[green]Stack {stack.StackId}[/] - Name: {stack.StackName}");
                            }
                        }
                        break;

                    case "Delete Stack":
                        AnsiConsole.MarkupLine("[blue]You selected 'Delete Stack'[/]");
                        stackId = AnsiConsole.Ask<int>("Enter the stack ID to delete");
                        using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
                        {
                            connection.Open();
                            DeleteStack(connection, stackId);
                        }
                        break;

                    case "Study Session":
                        AnsiConsole.MarkupLine("[blue]You selected 'Study Session'[/]");
                        stackId = AnsiConsole.Ask<int>("Enter the stack ID to start the session.");
                        int score = AnsiConsole.Ask<int>("Enter the score for this session");

                        using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
                        {
                            connection.Open();
                            DateTime sessionDate = DateTime.Now;
                            SaveStudySession(connection, stackId, sessionDate, score);
                        }
                        break;

                    case "View Study Sessions":
                        AnsiConsole.MarkupLine("[blue]You selected 'View Study Sessions'[/]");
                        using (SqlConnection connection = new SqlConnection(Program.ConnectionString))
                        {
                            connection.Open();
                            var sessions = GetStudySessions(connection);

                            foreach (var session in sessions)
                            {
                                AnsiConsole.MarkupLine($"[green]Session {session.SessionId}[/] - StackId: {session.StackId}, Date: {session.Date}, Score: {session.Score}");
                            }
                        }
                        break;

                    case "Close App":
                        AnsiConsole.MarkupLine("[red]Closing application...[/]");
                        appClose = true;
                        Environment.Exit(1);
                        break;
                }
            }
        }
        public static void RenumberFlashcards(SqlConnection connection, int stackId)
        {
            Console.Clear();
            string query = @"
                WITH CTE AS (
                    SELECT FlashcardId, ROW_NUMBER() OVER (ORDER BY FlashcardId) AS NewDisplayId
                    FROM Flashcards WHERE StackId = @StackId
                    )
                    UPDATE Flashcards SET DisplayId = CTE.NewDisplayId
                    FROM CTE WHERE Flashcards.FlashcardId = CTE.FlashcardId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StackId", stackId);
                command.ExecuteNonQuery();
            }
        }
        public static void SaveStudySession(SqlConnection connection, int stackId, DateTime date, int score)
        {
            string checkStackIdQuery = "SELECT COUNT(*) FROM Stacks WHERE StackId = @stackId";

            using (SqlCommand checkCommand = new SqlCommand(checkStackIdQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@stackId", stackId);

                int stackCount = (int)checkCommand.ExecuteScalar();

                if (stackCount == 0)
                {
                    AnsiConsole.MarkupLine("[red]Error: Stack ID does not exist.[/]");
                    return;
                }
            }

            string insertQuery = 
                @"INSERT INTO StudySessions (StackId, Date, Score)
                    VALUES 
                        (@stackId,
                        @date,
                        @score)";

            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@stackId", stackId);
                insertCommand.Parameters.AddWithValue("@date", date);
                insertCommand.Parameters.AddWithValue("@score", score);

                insertCommand.ExecuteNonQuery();

                AnsiConsole.MarkupLine("[green]Study session added successfully.[/]");
            }
        }
        public static List<StudySessionDto> GetStudySessions(SqlConnection connection)
        {
            string query = "SELECT * FROM StudySessions ORDER BY Date DESC";
            List<StudySessionDto> sessions = new List<StudySessionDto>();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sessions.Add(new StudySessionDto
                        {
                            SessionId = reader.GetInt32(0),
                            StackId = reader.GetInt32(1),
                            Date = reader.GetDateTime(2),
                            Score = reader.GetInt32(3)
                        });
                    }
                }
            }
            return sessions;
        }
        public static void DeleteStack(SqlConnection connection, int stackId)
        {
            Console.Clear();
            string query = "DELETE FROM Stacks WHERE StackId = @StackId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StackId", stackId);
                command.ExecuteNonQuery();
                AnsiConsole.MarkupLine($"[red]Stack {stackId} deleted successfully[/]");
            }
        }
        public static List<StackDto> GetStacks(SqlConnection connection)
        {
            string query = "SELECT * FROM Stacks";
            List<StackDto> stacks = new List<StackDto>();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stacks.Add(new StackDto
                        {
                            StackId = reader.GetInt32(0),
                            StackName = reader.GetString(1)
                        });
                    }
                }
            }
            return stacks;
        }
    }
}
