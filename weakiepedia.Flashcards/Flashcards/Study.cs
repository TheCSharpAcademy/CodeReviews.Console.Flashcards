using Spectre.Console;
using System.Data.SqlClient;
using Dapper;

using static Flashcards.Program;
using static Flashcards.Helpers;
using static Flashcards.Configuration;

namespace Flashcards;

internal static class Study
{
    internal static void StartStudySession(string stackName)
    {
        List<FlashcardShowDTO> flashcards = new List<FlashcardShowDTO>();
        
        try
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var query = "SELECT * FROM flashcards WHERE stack_id = @StackId;";
                var reader = connection.ExecuteReader(query, new { StackId = GetStackIdByName(stackName) });

                while (reader.Read())
                {
                    string question = reader.GetString(reader.GetOrdinal("question"));
                    string answer = reader.GetString(reader.GetOrdinal("answer"));
                    flashcards.Add(new FlashcardShowDTO(question, answer));
                }

                if (!flashcards.Any()) { PrintError("No flashcards found in this stack."); PressAnyKey(); UserInterface.ShowMenu(); }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        DateTime startTime = DateTime.Now;
        int countFlashcards = 0;
        int score = 0;
        
        foreach (FlashcardShowDTO flashcard in flashcards)
        {
            Console.Clear();
            countFlashcards++;
            var questionTable = new Table();
            questionTable.Title($"[bold][yellow2]{stackName}[/][/]");
            questionTable.Border(TableBorder.Rounded);
            questionTable.BorderColor(Color.Yellow2);
            questionTable.AddColumn(new TableColumn("[bold]Front/Question[/]").Centered());
            questionTable.AddRow($"[italic]{flashcard.Question}[/]");
            
            var answerTable = new Table();
            answerTable.Title($"[bold][yellow2]{stackName}[/][/]");
            answerTable.Border(TableBorder.Rounded);
            answerTable.BorderColor(Color.Yellow2);
            answerTable.AddColumn(new TableColumn("[bold]Back/Answer[/]").Centered());
            answerTable.AddRow($"[italic]{flashcard.Answer}[/]");
            
            AnsiConsole.Write(questionTable);
            
            if (GetUserInput("Answer: ").ToLower() == flashcard.Answer.ToLower())
            {
                Console.Clear();
                AnsiConsole.Write(answerTable);
                AnsiConsole.MarkupLine("Correct!");
                score++;
            }
            else
            {
                Console.Clear();
                AnsiConsole.Write(answerTable);
                AnsiConsole.MarkupLine($"Wrong! (The correct answer was: {flashcard.Answer.ToLower()})");
            }
            
            //Logic that prevents asking user to press any key twice.
            int totalFlashcards = flashcards.Count;
            if (!(countFlashcards + 1 > totalFlashcards))
            {
                PressAnyKey();
            }
        }
        
        //Calculate the score in percents.
        score = (score * 100) / flashcards.Count;
        AnsiConsole.MarkupLine($"\nTotal score: [underline]{score}%[/]");
        
        using (var connection = new SqlConnection(GetConnectionString()))
        {
            var query = "INSERT INTO sessions (score, date, stack_id) VALUES (@Score, @Date, @StackId);";
            connection.Execute(query, new { Score = score, Date = startTime, StackId = GetStackIdByName(stackName) });
        }
        
        PressAnyKey();
    }

    internal static void ShowStudySessions()
    {
        Console.Clear();
        
        var table = new Table();
        table.Title("[bold][yellow2]Study Sessions:[/][/]");
        table.Border(TableBorder.Rounded);
        table.BorderColor(Color.Yellow2);
        table.AddColumn(new TableColumn("[bold]Session ID[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Stack Name[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Score[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Date[/]").Centered());
        
        foreach (Session session in sessions)
        {
            table.AddRow(session.Id.ToString(), GetStackNameById(session.StackId), $"{session.Score.ToString()}%", session.Date.ToShortDateString());
        }
        
        AnsiConsole.Write(table);
    }
}