using FlashcardsLibrary.Models;
using Spectre.Console;

namespace FlashcardsLibrary;

public class PresentationLayer
{
    public static void ShowCurrentStacks()
    {
        var table = new Table();
        string[] columns = ["Stack Id", "Stack Name"];
        table.Title = new TableTitle("Stacks", style: "underline yellow");
        table.AddColumns(columns);
        foreach (Stacks stack in Stacks.StackList)
        {
            table.AddRow(stack.StackId.ToString(), stack.StackName);
        }
        AnsiConsole.Write(table);
    }

    public static void ShowCurrentFlashcards(int stackId)
    {
        var table = new Table();
        string[] columns = ["Flashcard Id", "Front", "Back"];
        table.Title = new TableTitle (Validator.GetStackNameFromId(stackId), style: "underline yellow");
        table.AddColumns(columns);

        List<Flashcards> filtered = Flashcards.FlashcardsList.Where(f => f.StackId == stackId).ToList();
        for (int i = 0; i < filtered.Count; i++) 
        {
            table.AddRow((i+1).ToString(), filtered[i].Front, filtered[i].Back);
        }
        AnsiConsole.Write(table);
    }

    public static void ShowCurrentStudySessions()
    {
        var table = new Table();
        string[] columns = ["Study session Id", "Date", "Score", "Stack name"];
        table.Title = new TableTitle("Study sessions", style: "underline yellow");
        table.AddColumns(columns);

        foreach (StudySessions session in StudySessions.StudySessionsList)
        {
            table.AddRow(session.StudySessionId.ToString(), session.Date.ToString(), session.Score.ToString(), session.StackName);
        }
        AnsiConsole.Write(table);
        Console.ReadLine();
    }

    public static void ShowReportCountSessionsPerMonth(List<PivotReports> input, int year, string type)
    {
        var table = new Table();
        string[] columns = ["Stack Name", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        table.AddColumns(columns);
        table.Title = new TableTitle($"{type} per month for the year {year}", style: "underline yellow");
        foreach (PivotReports report in input)
        {
            table.AddRow(report.StackName, report.January.ToString(), report.February.ToString(), report.March.ToString(), report.April.ToString(), report.May.ToString(), report.June.ToString(),
                report.July.ToString(), report.August.ToString(), report.September.ToString(), report.October.ToString(), report.November.ToString(), report.December.ToString());
        }
        AnsiConsole.Write(table);
        Console.ReadLine();
    }
}

