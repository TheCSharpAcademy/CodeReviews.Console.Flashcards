using FlashCards.Data;
using Spectre.Console;

namespace FlashCards.Views;

public static class TableVisualisation
{
    public static void ShowStacks(List<StackDto> stacks)
    {
        var table = new Table().AddColumn("Name");
        foreach (StackDto stack in stacks)
        {
            table.AddRow(stack.name);
        }
        AnsiConsole.Write(table);
    }

    public static void ShowFlashcards(List<FlashCardDto> flashcards)
    {
        var table = new Table().AddColumns("ID", "Front", "Back");
        foreach (FlashCardDto flashcard in flashcards)
        {
            table.AddRow(
                flashcard.flashcardId.ToString(),
                flashcard.front,
                flashcard.back
            );
        }
        AnsiConsole.Write(table);
    }

    public static void ShowFlashcardToAnswer(FlashCardDto flashcard)
    {
        var table = new Table().AddColumns("Front");
        table.AddRow(flashcard.front);
        AnsiConsole.Write(table);
    }

    public static void ShowStudySessions(List<StudyDto> sessions)
    {
        var table = new Table().AddColumns("Date", "Score", "Answered");

        foreach (StudyDto study in sessions)
        {
            table.AddRow(
                study.studyDate,
                study.score.ToString(),
                study.flashcard_amount.ToString()
            );
        }
        AnsiConsole.Write(table);
    }

    public static void ShowSessionReport(IEnumerable<dynamic>? data, string year, string stack)
    {
        if (data == null || !data.Any())
        {
            AnsiConsole.MarkupLine("[red]No data available to display.[/]");
            return;
        }
        var monthOrder = new List<string>
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };
        // Create a table with dynamic columns
        var table = new Table();
        table.AddColumn("Stack Name"); // Add the stack name column

        // Prepare a dictionary to hold average scores for each month
        var processedData = new Dictionary<string, object?>();
        
        // Process the data to map months to their values
        foreach (var row in data)
        {
            if (row is IDictionary<string, object> rowDict)
            {
                foreach (var property in rowDict)
                {
                    string month = property.Key;
                    processedData[month] = property.Value ?? 0; // Replace null values with 0
                }
            }
        }
        // Add columns to the table in the correct order
        foreach (var month in monthOrder)
        {
            table.AddColumn(month);
        }
        // Add a row with the stack name and average scores in the correct month order
        var rowValues = new List<string> { stack }; // Start with the stack name
        foreach (var month in monthOrder)
        {
            rowValues.Add(processedData.ContainsKey(month) ? processedData[month]?.ToString() ?? "0" : "0");
        }
        table.AddRow(rowValues.ToArray());
        // Render the table
        AnsiConsole.Write(
            new Panel(table)
                .Header($"Total Sessions per month ({year})")
                .Expand());
    }
    
    public static void ShowAverageReport(IEnumerable<dynamic>? data, string year, string stack)
    {
        if (data == null || !data.Any())
        {
            AnsiConsole.MarkupLine("[red]No data available to display.[/]");
            return;
        }
        // Define the correct month order
        var monthOrder = new List<string>
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };
        // Create a table with dynamic columns
        var table = new Table();
        table.AddColumn("Stack Name"); // Add the stack name column
        
        // Prepare a dictionary to hold average scores for each month
        var processedData = new Dictionary<string, object?>();
        // Process the data to map months to their values
        foreach (var row in data)
        {
            if (row is IDictionary<string, object> rowDict)
            {
                foreach (var property in rowDict)
                {
                    string month = property.Key;
                    processedData[month] = property.Value ?? 0; // Replace null values with 0
                }
            }
        }
        
        // Add columns to the table in the correct order
        foreach (var month in monthOrder)
        {
            table.AddColumn(month);
        }
        // Add a row with the stack name and average scores in the correct month order
        var rowValues = new List<string> { stack }; // Start with the stack name
        foreach (var month in monthOrder)
        {
            rowValues.Add(processedData.ContainsKey(month) ? processedData[month]?.ToString() ?? "0" : "0");
        }
        table.AddRow(rowValues.ToArray());
        // Render the table
        AnsiConsole.Write(
            new Panel(table)
                .Header($"Average Score Per Month ({year})")
                .Expand());
    }
}