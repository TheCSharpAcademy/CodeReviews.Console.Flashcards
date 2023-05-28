using ConsoleTableExt;
using Flashcards.CoreyJordan.DTOs;

namespace Flashcards.CoreyJordan.Display;
internal class SessionUI
{
    private List<MenuModel> ReportMenu { get; } = new()
    {
        new MenuModel("1", "View Full Report"),
        new MenuModel("2", "View by Date"),
        new MenuModel("3", "View by Pack"),
        new MenuModel("X", "Exit Report Card")
    };

    internal void DisplayEndOfSession(SessionDto session)
    {
        List<SessionDto> sessions = new()
        {
            session
        };
        ConsoleTableBuilder
            .From(sessions)
            .WithColumn("", "USER", "PACK", "CARDS", "DATE", "SCORE")
            .WithFormat(ConsoleTableBuilderFormat.MarkDown)
            .ExportAndWriteLine(TableAligntment.Center); 
    }

    internal void DisplayUsers(List<string> users)
    {
        ConsoleTableBuilder
            .From(users)
            .WithFormat(ConsoleTableBuilderFormat.MarkDown)
            .ExportAndWriteLine(TableAligntment.Center);
    }

    internal void DisplayReportMenu()
    {
        ConsoleTableBuilder
            .From(ReportMenu)
            .WithFormat(ConsoleTableBuilderFormat.MarkDown)
            .ExportAndWriteLine(TableAligntment.Center);
    }

    internal void DisplaySessions(List<SessionDto> sessions)
    {
        ConsoleTableBuilder
            .From(sessions)
            .WithFormat(ConsoleTableBuilderFormat.MarkDown)
            .ExportAndWriteLine(TableAligntment.Center);
    }

    internal void DisplayReport(ReportDto report)
    {
        List<ReportDto> reports = new()
        {
            report
        };

        Console.WriteLine();
        ConsoleTableBuilder
            .From(reports)
            .WithFormat(ConsoleTableBuilderFormat.MarkDown)
            .ExportAndWriteLine(TableAligntment.Center);
    }
}
