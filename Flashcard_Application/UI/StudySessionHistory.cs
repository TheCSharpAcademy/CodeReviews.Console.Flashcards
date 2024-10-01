using Flashcard_Application.DataServices;
using Flashcard_Application.Models;
using Flashcards.UI;
using Spectre.Console;

namespace Flashcard_Application.UI;

public class StudySessionHistory
{
    public static void ShowStudySessionHistory()
    {
        Console.Clear();
        AnsiConsole.Markup("[red]Here is your history of Study Sessions - [/]\n\n");
        List<StudySession> sessions = StudySessionDatabaseServices.GetStudySession();

        var table = new Table();
        table.AddColumn("Session Start Time");
        table.AddColumn("Session End Time");
        table.AddColumn("Session Score");

        foreach (var session in sessions)
        {
            table.AddRow(session.SessionStartTime.ToString(), session.SessionEndTime.ToString(), session.SessionScore.ToString());
        }

        AnsiConsole.Write(table);
        AnsiConsole.Markup("\n\n\n\n\n\n");
        MainMenu.MainMenuPrompt();
    }
}
