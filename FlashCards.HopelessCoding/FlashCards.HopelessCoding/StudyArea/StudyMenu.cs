using HelperMethods.HopelessCoding;
using Spectre.Console;

namespace FlashCards.HopelessCoding.Study;

internal class StudyMenu
{
    internal static void StudyMainMenu()
    {
        Console.Clear();
        AnsiConsole.Write(new Markup("[yellow1]Study main menu\n\n[/]"));
        var selection = Helpers.MenuSelector(["Start a new session", "View previous sessions", "View number of sessions per month", "View average score per month", "Return to main menu"]);

        switch (selection)
        {
            case "Start a new session":
                StudyCommands.SelectStackToStudyMenu();
                return;
            case "View previous sessions":
                StudyCommands.ViewStudySessions();
                Console.Write("Press any key to continue.");
                Console.ReadLine();
                return;
            case "View number of sessions per month":
                StudySessionReports.StudySessionReport(false);
                return;
            case "View average score per month":
                StudySessionReports.StudySessionReport();
                return;
            case "Return to main menu":
                return;
        }
    }
}