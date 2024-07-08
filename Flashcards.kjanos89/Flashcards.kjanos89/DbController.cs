using System.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;
using Spectre.Console;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Collections;

namespace Flashcards.kjanos89
{
    public class DbController
    {
        private readonly string _connectionString;
        Menu menu;
        Stack stack;

        public DbController(Menu _menu)
        {
            menu = _menu;
            _connectionString = ConfigurationManager.AppSettings["connectionString"] ?? "Server=.;Integrated Security=True;TrustServerCertificate=True;";
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
            menu.StackMenu();
        }
        public void AddStack()
        {
                AnsiConsole.MarkupLine("[bold]Please give me a name:[/]");
                string name = Console.ReadLine();
                AnsiConsole.MarkupLine("[bold]Please give me a short description:[/]");
                string description = Console.ReadLine();

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
        public void ChangeStack()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold]Please give me an id of the stack you want to change to:[/]");
            Int32.TryParse(Console.ReadLine(), out int id);
            string result = "no";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.ChangeDatabase("Flashcards");
                var stack = connection.QueryFirstOrDefault<Stack>("SELECT StackId, Name FROM Stack WHERE StackId = @Id", new { Id = id });
                connection.Close();
               menu.currentStack= stack?.Name??"no";
            }
            AnsiConsole.MarkupLine("[bold]Press any key to return to the Stack menu.[/]");
            Console.ReadLine();
            menu.StackMenu();
        }
        public void DeleteStack()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold]Please give me the id of the stack you want to delete:[/]");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int id))
            {
                if (DoesStackIdExist(id))
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        connection.ChangeDatabase("Flashcards");
                        string deleteCommand = "DELETE FROM Stack WHERE StackId = @Id";
                        int rowsAffected = connection.Execute(deleteCommand, new { Id = id });
                        if (rowsAffected > 0)
                        {
                            AnsiConsole.MarkupLine("[green bold]Stack deleted successfully![/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red bold]No stack found with the provided ID.[/]");
                            menu.StackMenu();
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
                menu.StackMenu();
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
                var exists = connection.ExecuteScalar<int>(
                    "SELECT COUNT(1) FROM Stack WHERE StackId = @Id",
                    new { Id = stackId }
                );
                connection.Close();
                return exists > 0;
            }
        }

        public void ManageFlashcards()
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                



                connection.Close();
            }
        }
        public void Study()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Close();
            }
        }
    }
}
