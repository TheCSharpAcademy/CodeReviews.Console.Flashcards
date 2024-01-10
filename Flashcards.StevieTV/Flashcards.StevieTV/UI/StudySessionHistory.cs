using Flashcards.StevieTV.Database;
using Spectre.Console;

namespace Flashcards.StevieTV.UI;

internal static class StudySessionHistory
{
    internal static void ShowStudySessions()
    {
        var studySessions = StudySessionsDatabaseManager.GetStudySessions();
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Study Sessions"));

        var table = new Table
        {
            Border = TableBorder.Rounded,
            Title = new TableTitle("Study Session History")
        };

        table.AddColumn("Date");
        table.AddColumn("Stack Name");
        table.AddColumn("Score");

        foreach (var studySession in studySessions)
        {
            var formattedDate = studySession.DateTime.ToString("dd-MMM-yy");
            var stackName = studySession.Stack.Name;
            var percentScore = $"{(studySession.Score * 100 / studySession.QuantityTested)}%";
            table.AddRow(formattedDate, stackName, percentScore);
        }
        
        AnsiConsole.Write(table);
        
        AnsiConsole.Prompt(new ConfirmationPrompt("Press enter to return to the main menu"));
    }
}