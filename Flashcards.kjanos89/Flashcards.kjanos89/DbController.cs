using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace Flashcards.kjanos89
{
    public class DbController
    {
        private readonly string _connectionString;
        Menu menu;
        Stack stack;
        List<string> stackNames;
        int points;

        public DbController(Menu _menu)
        {
            menu = _menu;
            _connectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"] ?? "Server=.;Integrated Security=True;TrustServerCertificate=True;";
            InitializeDatabase();
        }

        public void InitializeDatabase()
        {
            try
            {
                AnsiConsole.MarkupLine("[yellow bold]Starting database initialization...[/]");
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    AnsiConsole.MarkupLine("[yellow bold]Connected to SQL Server.[/]");

                    string createDBCommand = "IF (NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Flashcards')) " +
                                                "BEGIN " +
                                                "CREATE DATABASE Flashcards;" +
                                                "END";

                    using (var command = new SqlCommand(createDBCommand, connection))
                    {
                        command.ExecuteNonQuery();
                        AnsiConsole.MarkupLine("[yellow bold]Checked for database existence and created if not present.[/]");
                    }

                    connection.ChangeDatabase("Flashcards");
                    AnsiConsole.MarkupLine("[yellow bold]Switched to Flashcards database.[/]");

                    string checkStackTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Stack]') AND type in (N'U'))
                        BEGIN
                            CREATE TABLE Stack (
                                StackId INT IDENTITY(1,1) PRIMARY KEY,
                                Name NVARCHAR(100) NOT NULL,
                                Description NVARCHAR(500)
                            )
                        END";

                    connection.Execute(checkStackTable);
                    AnsiConsole.MarkupLine("[yellow bold]Checked for Stack table existence and created if not present.[/]");

                    string checkFlashcardTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Flashcard]') AND type in (N'U'))
                        BEGIN
                            CREATE TABLE Flashcard (
                                FlashcardId INT IDENTITY(1,1) PRIMARY KEY,
                                StackId INT NOT NULL,
                                Question NVARCHAR(500) NOT NULL,
                                Answer NVARCHAR(500) NOT NULL,
                                FOREIGN KEY (StackId) REFERENCES Stack(StackId)
                            )
                        END";

                    connection.Execute(checkFlashcardTable);
                    string checkStudyTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND type in (N'U'))
                        BEGIN
                            CREATE TABLE Study (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                StackId INT NOT NULL,
                                Date DATETIME NOT NULL,
                                Points INT
                            )
                        END";

                    connection.Execute(checkStudyTable);
                    AnsiConsole.MarkupLine("[yellow bold]Checked for Study table existence and created if not present.[/]");
                    stackNames = connection.Query<string>("SELECT Name FROM Stack").ToList();
                    AnsiConsole.MarkupLine("[yellow bold]Checked for Flashcard table existence and created if not present.[/]");
                    AnsiConsole.MarkupLine("[yellow bold]Database and tables initialization complete.[/]");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red bold]Error initializing database: {ex.Message}[/]");
            }
        }
        public void ViewStacks()
        {
            AnsiConsole.Clear();
            ShowStacksForMenu();
            using( var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.ChangeDatabase("Flashcards");
                var records = connection.Query<Stack>("SELECT * FROM Stack").ToList();
                if (records.Any())
                {
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine("[red]__________________________________________________________________________[/]");
                    foreach (var record in records)
                    {
                        AnsiConsole.MarkupLine($"[bold]Id: {record.StackId}, Name: {record.Name}, Description: {record.Description}[/]");
                        AnsiConsole.MarkupLine("[red]__________________________________________________________________________[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold]No records found.[/]");
                    Thread.Sleep(1000);
                    menu.DisplayMenu();
                }
                connection.Close();
            }
            AnsiConsole.MarkupLine("[bold]Press any key to return to the Stack menu.[/]");
            Console.ReadLine();
            menu.DisplayMenu();
        }
        public void AddStack()
        {
            string name = "";
            string description = "";
            AnsiConsole.Clear();
            ShowStacksForMenu();

            while (true)
            {
                AnsiConsole.MarkupLine("[bold]Please give me a name:[/]");
                string helper = Console.ReadLine();

                if (!stackNames.Contains(helper) && !string.IsNullOrEmpty(helper))
                {
                    name = helper;
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("Name is taken or invalid! Try another one.");
                }
            }

            AnsiConsole.MarkupLine("[bold]Please give me a short description:[/]");
            description = Console.ReadLine();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.ChangeDatabase("Flashcards");
                string insertCommand = "INSERT INTO Stack (Name, Description) VALUES (@Name, @Description)";
                var parameters = new { Name = name, Description = description };
                connection.Execute(insertCommand, parameters);
                AnsiConsole.MarkupLine("[green bold]Stack added successfully![/]");
                connection.Close();
            }

            AnsiConsole.MarkupLine("[bold]Press any key to return to the Stack menu.[/]");
            Console.ReadLine();
            menu.StackMenu();
        }
        public void DeleteStack()
        {
            AnsiConsole.Clear();
            ShowStacksForMenu();
            AnsiConsole.MarkupLine("[bold]Please give me the id of the stack you want to delete:[/]");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int id))
            {
                if (DoesStackIdExist(id))
                {
                    try
                    {
                        using (var connection = new SqlConnection(_connectionString))
                        {
                            connection.Open();
                            connection.ChangeDatabase("Flashcards");

                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    string deleteFlashcardsCommand = "DELETE FROM Flashcard WHERE StackId = @StackId";
                                    connection.Execute(deleteFlashcardsCommand, new { StackId = id }, transaction);
                                    string deleteStudyCommand = "DELETE FROM Study WHERE StackId = @StackId";
                                    connection.Execute(deleteStudyCommand, new { StackId = id }, transaction);
                                    string deleteStackCommand = "DELETE FROM Stack WHERE StackId = @Id";
                                    int rowsAffected = connection.Execute(deleteStackCommand, new { Id = id }, transaction);
                                    if (rowsAffected > 0)
                                    {
                                        transaction.Commit();
                                        AnsiConsole.MarkupLine("[green bold]Stack, studies and associated flashcards deleted successfully![/]");
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        AnsiConsole.MarkupLine("[red bold]No stack found with the provided ID.[/]");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    AnsiConsole.MarkupLine($"[red bold]An error occurred: {ex.Message}[/]");
                                }
                            }
                            connection.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red bold]An error occurred while connecting to the database: {ex.Message}[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red bold]No stack found with the provided ID.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red bold]Invalid input. Please enter a valid integer ID.[/]");
            }
            AnsiConsole.MarkupLine("[bold]Press any key to return to the Stack menu.[/]");
            Console.ReadLine();
            menu.StackMenu();
        }


        private bool DoesStackIdExist(int stackId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.ChangeDatabase("Flashcards");
                var exists = connection.ExecuteScalar<int>("SELECT COUNT(1) FROM Stack WHERE StackId = @Id",new { Id = stackId });
                connection.Close();
                return exists > 0;
            }
        }
        private bool DoesFlashcardIdExist(int stackId, int flashcardId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.ChangeDatabase("Flashcards");
                var flashcard = connection.QueryFirstOrDefault<Flashcard>("SELECT TOP 1 * FROM Flashcard WHERE StackId = @StackId AND FlashcardId = @FlashcardId",new { StackId = stackId, FlashcardId = flashcardId });
                connection.Close();
                return flashcard != null;
            }
        }
        public void ViewFlashcards()
        {
            AnsiConsole.Clear();
            ShowStacksForMenu();
            AnsiConsole.MarkupLine("[yellow bold]Enter the ID of the stack where you want to check the flashcards:[/]");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int id))
            {
                if (DoesStackIdExist(id))
                {
                    using(var connection = new SqlConnection(_connectionString))
                    { 
                        connection.Open();
                        connection.ChangeDatabase("Flashcards");
                        var records = connection.Query<Flashcard>("SELECT * FROM Flashcard WHERE StackId=@StackId", new { StackId=id }).ToList();
                        if(records.Any())
                        {
                            AnsiConsole.Clear();
                            AnsiConsole.MarkupLine("[red]__________________________________________________________________________[/]");
                            foreach (var record in records)
                            {
                                AnsiConsole.MarkupLine($"[bold]Id: {record.FlashcardId}, Question: {record.Question}, Answer: {record.Answer}[/]");
                                AnsiConsole.MarkupLine("[red]__________________________________________________________________________[/]");
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[bold]No records found.[/]");
                        }
                        connection.Close();
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold]Invalid id.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold]Invalid id.[/]");
            }
            AnsiConsole.MarkupLine("[bold]Press any key to return to the menu.[/]");
            Console.ReadLine();
            menu.FlashcardMenu();
        }
        public void AddFlashcard()
        {
            AnsiConsole.Clear();
            ShowStacksForMenu();
            AnsiConsole.MarkupLine("[yellow bold]Enter the ID of the stack you want the flashcard to belong to:[/]");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int id))
            {
                if (DoesStackIdExist(id))
                {
                    Flashcard flashcard = new Flashcard();
                    flashcard.StackId = id; 

                    AnsiConsole.MarkupLine("[bold]The ID exists! Now please enter the question of the flashcard:[/]");
                    flashcard.Question = Console.ReadLine();

                    AnsiConsole.MarkupLine("[bold]And now please enter the answer to that question.[/]");
                    AnsiConsole.MarkupLine($"[bold]The question was: {flashcard.Question}[/]");
                    flashcard.Answer = Console.ReadLine();

                    using (var connection = new SqlConnection(_connectionString))
                    {
                        try
                        {
                            connection.Open();
                            connection.ChangeDatabase("Flashcards");

                            string query = "INSERT INTO Flashcard (StackId, Question, Answer) VALUES (@StackId, @Question, @Answer)";
                            connection.Execute(query, new { flashcard.StackId, flashcard.Question, flashcard.Answer });
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.MarkupLine($"[red bold]An error occurred: {ex.Message}[/]");
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red bold]No stack found with the provided ID.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red bold]Invalid input. Please enter a valid integer ID.[/]");
            }

            AnsiConsole.MarkupLine("[bold]Press any key to return to the Flashcard menu.[/]");
            Console.ReadLine();
            menu.FlashcardMenu();
        }
        public void DeleteFlashcard()
        {
            AnsiConsole.Clear();
            ShowStacksForMenu();
            AnsiConsole.MarkupLine("[yellow bold]Enter the ID of the stack where you want to delete the flashcard:[/]");
            string input = Console.ReadLine();
            if (Int32.TryParse(input, out var inputInt))
            {
                ShowFlashcardsForMenu(inputInt);
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Wrong input, try again![/]");
                Thread.Sleep(1000);
                DeleteFlashcard();
            }
            AnsiConsole.MarkupLine("[yellow bold]Enter the ID of the flashcard which you want to delete:[/]");
            string input2 = Console.ReadLine();
            if (int.TryParse(input2, out int fcId))
            {
                if (int.TryParse(input, out int id))
                {
                    if (DoesStackIdExist(id) && DoesFlashcardIdExist(id, fcId))
                    {
                        using (var connection = new SqlConnection(_connectionString))
                        {
                            connection.Open();
                            connection.ChangeDatabase("Flashcards");
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    connection.Execute("DELETE FROM Flashcard WHERE StackId=@StackId AND FlashcardId=@FlashcardId",
                                        new { StackId = id, FlashcardId = fcId }, transaction);
                                    string createTempTable = @"
                                    CREATE TABLE TempFlashcard (
                                        FlashcardId INT IDENTITY(1,1) NOT NULL,
                                        StackId INT NOT NULL,
                                        Question NVARCHAR(500) NOT NULL,
                                        Answer NVARCHAR(500) NOT NULL
                                    )";
                                    connection.Execute(createTempTable, transaction: transaction);
                                    string insertTempTable = @"
                                    SET IDENTITY_INSERT TempFlashcard ON;
                                    INSERT INTO TempFlashcard (FlashcardId, StackId, Question, Answer)
                                    SELECT ROW_NUMBER() OVER (ORDER BY FlashcardId) AS FlashcardId, StackId, Question, Answer
                                    FROM Flashcard
                                    WHERE StackId = @StackId;
                                    SET IDENTITY_INSERT TempFlashcard OFF;";
                                    connection.Execute(insertTempTable, new { StackId = id }, transaction);
                                    connection.Execute("DROP TABLE Flashcard", transaction: transaction);
                                    connection.Execute("EXEC sp_rename 'TempFlashcard', 'Flashcard'", transaction: transaction);
                                    transaction.Commit();
                                    AnsiConsole.MarkupLine("[green bold]Flashcard deleted and IDs rearranged successfully![/]");
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    AnsiConsole.MarkupLine($"[red bold]An error occurred: {ex.Message}[/]");
                                }
                            }
                            connection.Close();
                            Thread.Sleep(1000);
                            menu.FlashcardMenu();
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold]Either the stack or the flashcard does not exist.[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold]Invalid ID for stack.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold]Invalid ID for flashcard.[/]");
            }
        }
        public void Study()
        {
            AnsiConsole.Clear();
            ShowStacksForMenu();
            AnsiConsole.MarkupLine("[yellow bold]Enter the ID of the stack you want to study:[/]");
            string input = Console.ReadLine();
            points = 0;
            if (int.TryParse(input, out int id))
            {
                if (DoesStackIdExist(id))
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        connection.ChangeDatabase("Flashcards");
                        List<Flashcard> flashcards = connection.Query<Flashcard>("SELECT * FROM Flashcard WHERE StackId=@StackId", new { StackId = id }).ToList();
                        StudySession(flashcards, 0, 0);
                        DateTime date = DateTime.Now;
                        string insertCommand = "INSERT INTO Study (Date, StackId, Points) VALUES (@Date, @StackId, @Points)";
                        var parameters = new { Date = date, StackId=id, Points=points };
                        connection.Execute(insertCommand, parameters);
                        connection.Close();
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold]Either the stack or the flashcard does not exist.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold]Invalid id for stack.[/]");
            }
            AnsiConsole.MarkupLine("[bold]Press any key to return to the menu.[/]");
            Console.ReadLine();
            menu.StudyMenu();
        }
        public void StudySession(List<Flashcard> fc, int id, int points)
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup($"You've earned {points} points so far this session.");
            if (id >= fc.Count)
            {
                this.points = points;
                return; 
            }

            string question = fc[id].Question;
            string answer = fc[id].Answer;
            AnsiConsole.MarkupLine("Type 'Exit' to return to the Study menu.\n");
            AnsiConsole.MarkupLine(question);
            string input = Console.ReadLine();

            if (answer.ToLower() == input.ToLower())
            {
                AnsiConsole.MarkupLine("[bold green]Good Answer! +1 point earned![/]");
                AnsiConsole.MarkupLine("[bold green]Next question incoming (if there's any left...)[/]");
                Thread.Sleep(1500);
                points++;
                id++;
                StudySession(fc, id, points);
            }
            else if (input.ToLower() == "exit")
            {
                this.points = points; 
                menu.StudyMenu(); 
            }
            else
            {
                AnsiConsole.MarkupLine($"[red bold]Wrong answer! The right answer was {fc[id].Answer}.[/]");
                Thread.Sleep(1500);
                id++;
                StudySession(fc, id, points);
            }
        }

        public void CheckSessions()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.ChangeDatabase("Flashcards");
                var records=connection.Query<Study>("SELECT * FROM Study").ToList();
                if(records.Any())
                {
                    foreach (var record in records)
                    {
                        AnsiConsole.MarkupLine($"Date: {record.Date}, Points: {record.Points}");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("No sessions saved so far!");
                    Thread.Sleep(1500);
                }
                connection.Close();
                AnsiConsole.MarkupLine("Press any key to return to the Study menu...");
                Console.ReadLine();
                menu.StudyMenu();
            }
        }
        public void ShowStacksForMenu ()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[bold yellow]Stacks:[/]");
                connection.Open();
                connection.ChangeDatabase("Flashcards");
                var records = connection.Query<Stack>("SELECT * FROM Stack").ToList();
                if (records.Any())
                {
                    AnsiConsole.MarkupLine("[red]__________________________________________________________________________[/]");
                    foreach (var record in records)
                    {
                        AnsiConsole.MarkupLine($"[bold]Id: {record.StackId}, Name: {record.Name}, Description: {record.Description}[/]");
                        AnsiConsole.MarkupLine("[red]__________________________________________________________________________[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold]No records found.[/]");
                    Thread.Sleep(500);
                    connection.Close();
                    menu.DisplayMenu();
                }
                connection.Close();
            }
        }
        public void ShowFlashcardsForMenu(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[bold yellow]Flashcards:[/]");
                connection.Open();
                connection.ChangeDatabase("Flashcards");
                var records = connection.Query<Flashcard>("SELECT * FROM Flashcard WHERE StackId=@StackId", new {StackId = id}).ToList();
                if (records.Any())
                {
                    AnsiConsole.MarkupLine("[red]__________________________________________________________________________[/]");
                    foreach (var record in records)
                    {
                        AnsiConsole.MarkupLine($"[bold]Id: {record.FlashcardId}, Question: {record.Question}[/]");
                        AnsiConsole.MarkupLine("[red]__________________________________________________________________________[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold]No records found.[/]");
                    Thread.Sleep(500);
                    connection.Close();
                    menu.DisplayMenu();
                }
                connection.Close();
            }
        }
    }
}
