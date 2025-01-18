using Spectre.Console;
using System.Data.SqlClient;
using Dapper;

using static Flashcards.Program;
using static Flashcards.UserInterface;
using static Flashcards.Helpers;
using static Flashcards.Configuration;

namespace Flashcards;

internal static class Manage
{
    internal static void ShowStacks()
    {
        if (stackNames.Any())
        {
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.BorderColor(Color.Yellow2);
            table.AddColumn(new TableColumn("[bold]Stacks:[/]").Centered());
            
            foreach (StackShowDTO stack in stackNames)
            {
                table.AddRow($"[italic]{stack.Name}[/]");
            }
            
            AnsiConsole.Write(table);
        }
        else { PrintError("No stacks found."); }
    }
    
    internal static void CreateStack()
    {
        string stackName = GetUserInput("Enter the name of the stack you want to create: ");
        while (string.IsNullOrEmpty(stackName) || stackNames.Exists(s => s.Name == stackName))
        {
            AnsiConsole.MarkupLine("Stack name is empty or already exists.");
            stackName = GetUserInput("Enter name of the stack you want to create: ");
        };
        
        try
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var query = "INSERT INTO stacks (Name) VALUES (@Name);";
                connection.Execute(query, new { Name = stackName });
                
                AnsiConsole.MarkupLine($"'{stackName}' stack has been created.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    internal static void DeleteStack()
    {
        string stackName = ShowStackMenu("Choose stack that you want to be deleted: ");

        try
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var query = "DELETE FROM stacks WHERE Name = @Name;";
                connection.Execute(query, new { Name = stackName });
                
                AnsiConsole.MarkupLine($"'{stackName}' stack has been deleted.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    internal static void ShowFlashcards()
    {
        string stackName = ShowStackMenu("Choose the stack that you want to see flashcards of: ");

        try
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                List<FlashcardShowDTO> flashcards = new List<FlashcardShowDTO>();

                var query = "SELECT * FROM flashcards WHERE stack_id = @StackId;";
                var reader = connection.ExecuteReader(query, new { StackId = GetStackIdByName(stackName) });

                while (reader.Read())
                {
                    string question = reader.GetString(reader.GetOrdinal("question"));
                    string answer = reader.GetString(reader.GetOrdinal("answer"));
                    flashcards.Add(new FlashcardShowDTO(question, answer));
                }

                if (flashcards.Any())
                {
                    int flashcardCount = 0;
                    
                    var table = new Table();
                    table.Border(TableBorder.Rounded);
                    table.BorderColor(Color.Yellow2);
                    table.AddColumn(new TableColumn("[bold]ID[/]").Centered());
                    table.AddColumn(new TableColumn("[bold]Question[/]").Centered());
                    table.AddColumn(new TableColumn("[bold]Answer[/]").Centered());
                    
                    foreach (FlashcardShowDTO flashcard in flashcards)
                    {
                        flashcardCount++; 
                        table.AddRow(flashcardCount.ToString(), flashcard.Question, flashcard.Answer);
                    }
                    
                    AnsiConsole.Write(table);
                }
                else { PrintError("No flashcards found."); }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    internal static void CreateFlashcard()
    {
        string stackName = ShowStackMenu("Choose the stack you want to create the flashcard in: ");

        string question = GetUserInput("Enter question: ");
        while (string.IsNullOrEmpty(question))
        {
            AnsiConsole.MarkupLine("Question can't be empty.");
            question = GetUserInput("Enter question: ");
        }

        string answer = GetUserInput("Enter answer: ");
        while (string.IsNullOrEmpty(answer))
        {
            AnsiConsole.MarkupLine("Answer can't be empty.");
            question = GetUserInput("Enter answer: ");
        }

        using (var connection = new SqlConnection(GetConnectionString()))
        {
            try
            {
                var query = "INSERT INTO flashcards (question, answer, stack_id) VALUES (@Question, @Answer, @Stack_id);";
                connection.Execute(query, new { Question = question, Answer = answer, Stack_id = GetStackIdByName(stackName) });

                AnsiConsole.MarkupLine($"A flashcard has been created in the '{stackName}' stack.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    

    internal static void DeleteFlashcard()
    {
        string stackName = ShowStackMenu("Choose the stack you want to delete the flashcard from: ");

        try
        {
            List<FlashcardDeleteDTO> flashcards = new List<FlashcardDeleteDTO>();
            List<FlashcardDeleteDTO> reindexedFlashcards = new List<FlashcardDeleteDTO>();
            
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var query = "SELECT * FROM flashcards WHERE stack_id = @StackId;";
                var reader = connection.ExecuteReader(query, new { StackId = GetStackIdByName(stackName) });

                int flashcardCount = 1;
                while (reader.Read())
                {
                    int id = reader.GetInt32(reader.GetOrdinal("flashcard_id"));
                    string question = reader.GetString(reader.GetOrdinal("question"));
                    string answer = reader.GetString(reader.GetOrdinal("answer"));
                    flashcards.Add(new FlashcardDeleteDTO(id, question, answer));
                    reindexedFlashcards.Add(new FlashcardDeleteDTO(flashcardCount, question, answer));
                    flashcardCount++;
                }

                if (reindexedFlashcards.Any())
                {
                    var table = new Table();
                    table.Border(TableBorder.Rounded);
                    table.BorderColor(Color.Yellow2);
                    table.AddColumn(new TableColumn("[bold]ID[/]").Centered());
                    table.AddColumn(new TableColumn("[bold]Question[/]").Centered());
                    table.AddColumn(new TableColumn("[bold]Answer[/]").Centered());
                    
                    foreach (FlashcardDeleteDTO flashcard in reindexedFlashcards)
                    {
                        table.AddRow(flashcard.Id.ToString(), flashcard.Question, flashcard.Answer);
                    }
                    
                    AnsiConsole.Write(table);
                }
                else { PrintError("No flashcards found."); PressAnyKey(); UserInterface.ShowMenu(); }
            }
            
            int reindexedFlashcardId = Convert.ToInt32(GetUserInput("\nEnter ID of the flashcard you want to delete: "));
            while (!reindexedFlashcards.Any(f => f.Id == reindexedFlashcardId))
            {
                AnsiConsole.MarkupLine("Flashcard doesn't exist.");
                reindexedFlashcardId = Convert.ToInt32(GetUserInput("\nEnter ID of the flashcard you want to delete: "));
            }
            int flashcardId = flashcards[reindexedFlashcardId - 1].Id;

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var query = "DELETE FROM flashcards WHERE flashcard_id = @FlashcardId;";
                connection.Execute(query, new { FlashcardId = flashcardId });
                
                AnsiConsole.MarkupLine($"A flashcard has been deleted from the '{stackName}' stack.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}