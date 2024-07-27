using System.Diagnostics.Eventing.Reader;
using Models;
using Spectre.Console;

namespace Flashcards;

public static class UI
{

    public static void Clear() => AnsiConsole.Clear();

    public static int MenuSelection(string title, string[] options)
    {
        var response = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                .AddChoices(options)
        );

        for (var i = 0; i < options.Length; i++)
        {
            if (response == options[i])
            {
                return i;
            }
        }
        throw new Exception("Invalid option selected in menu");
    }

    public static string StringResponse(string question) => AnsiConsole.Ask<string>(question + ":");

    public static string StringResponseWithDefault(string question, string defaultResponse)
    {
        var response = AnsiConsole.Prompt(
            new TextPrompt<string>(question + " (Press 'enter' to leave as [grey]'" + defaultResponse + "'[/]):")
                .AllowEmpty()
        );

        if (response == null || response == "")
        {
            return defaultResponse;
        }
        else
        {
            return response;
        }
    }

    public static int IntResponse(string question)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>(question + ":")
                .ValidationErrorMessage("[red]That's not a valid number[/]")
                .Validate(id =>
                {
                    return id switch
                    {
                        < 0 => ValidationResult.Error("[red]Id's must be a number greater than or equal to 0[/]"),
                        _ => ValidationResult.Success(),
                    };
                })
        );
    }

    public static string TimeResponse(string question)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>(question + " formatted like [yellow]23:59 dd-MM-yy[/]:")
                .PromptStyle("green")
                .ValidationErrorMessage("format times like [red]23:59 dd-MM-yy[/]")
                .Validate(time =>
                {
                    return DateTime.TryParseExact(time, "HH:mm dd-MM-yy", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime);
                })
        );
    }

    public static void ConfirmationMessage(string message)
    {
        AnsiConsole.Console.MarkupLine(message + " Press 'enter' to continue");
        Console.ReadLine();
        AnsiConsole.Clear();
    }

    // public static void DisplayCodingSessions(List<CodingSession> sessions)
    // {
    //     var table = new Table();

    //     string[] columns = ["ID", "Task", "Start Time", "End Time", "Duration (min)"];
    //     table.AddColumns(columns);

    //     foreach (var session in sessions)
    //     {
    //         int durationMin = session.Duration / 60;
    //         table.AddRow(session.Id.ToString(), session.Task, session.StartTime, session.EndTime, durationMin.ToString());
    //     }

    //     AnsiConsole.Write(table);
    // }

    public static void InvalidationMessage(string message) => AnsiConsole.MarkupLine("[red]" + message + "[/]");

    public static void DisplayStudySessions(IEnumerable<StudySessionInfoDto> studySessions)
    {
        var table = new Table();

        string[] columns = ["ID", "StudyTime", "Score", "Stack"];
        table.AddColumns(columns);

        foreach (var studySession in studySessions)
        {
            table.AddRow(
                studySession.Id.ToString(),
                studySession.StudyTime.ToShortDateString(),
                studySession.Score.ToString(),
                studySession.StackName
            );
        }
        AnsiConsole.Write(table);
    }
}
