using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Dtos.StudySessions;
using Flashcards.KamilKolanowski.Enums;
using Flashcards.KamilKolanowski.Handlers;
using Flashcards.KamilKolanowski.Helpers;
using Flashcards.KamilKolanowski.Models;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Controllers;

internal class StudySession
{
    private static int _points;
    private static List<CardDto> _cards = new List<CardDto>();
    private static readonly Random Rnd = new();
    private static DateTime _startTime;
    private static DateTime _endTime;

    internal static void ViewStudySessions(DatabaseManager databaseManager)
    {
        var stackChoice = StackChoice.GetStackChoice(databaseManager);

        var studySessionTable = AnsiConsole.Prompt(
            new SelectionPrompt<string>().AddChoices(Options.ViewStudySessionsDisplay.Values)
        );

        var selectedStudySessionTable = Options
            .ViewStudySessionsDisplay.FirstOrDefault(s => s.Value == studySessionTable)
            .Key;

        switch (selectedStudySessionTable)
        {
            case Options.ViewStudySessionOptions.ViewStudySessions:
                BuildStudySessionTable(databaseManager, stackChoice);
                break;
            case Options.ViewStudySessionOptions.ViewStudySessionsAggregated:
                BuildStudySessionsTablePerMonth(databaseManager, stackChoice, "count");
                break;
            case Options.ViewStudySessionOptions.ViewStudySessionsAverageScore:
                BuildStudySessionsTablePerMonth(databaseManager, stackChoice, "average");
                break;
        }

        AnsiConsole.MarkupLine("Press any key to go back to the main menu.");
        Console.ReadKey();
    }

    internal static void StartStudy(DatabaseManager databaseManager)
    {
        var selectedStackChoice = StackChoice.GetStackChoice(databaseManager);
        var stackName = databaseManager
            .ReadStacks()
            .FirstOrDefault(s => s.StackId == selectedStackChoice)
            ?.StackName;

        var cards = FlashcardsController.GetFlashcardDtosForStack(
            databaseManager,
            selectedStackChoice
        );
        var availableCards = cards.Count;

        if (availableCards == 0)
        {
            AnsiConsole.MarkupLine(
                "[red]No flashcards available.[/]\nPress any key to go back to the Main Menu."
            );
            Console.ReadKey();
            return;
        }

        var amountOfFlashcardToStudyFrom = AnsiConsole.Prompt(
            new TextPrompt<int>("Specify how many flashcards you would like to study: ").Validate(
                input =>
                {
                    if (input <= 0)
                    {
                        return ValidationResult.Error(
                            "[red]You must study at least one flashcard.[/]"
                        );
                    }
                    if (input > availableCards)
                    {
                        AnsiConsole.MarkupLine(
                            $"[yellow]You only have {availableCards} cards, studying with {availableCards} cards instead.[/]"
                        );
                        return ValidationResult.Success();
                    }
                    ;
                    return ValidationResult.Success();
                }
            )
        );

        _startTime = DateTime.Now;
        AnsiConsole.MarkupLine($"Starting study session at {_startTime}");

        var listOfRandomIndexes = GetRandomFlashcards(amountOfFlashcardToStudyFrom, cards);
        var studySession = ProcessStudySession(listOfRandomIndexes, cards);

        _endTime = DateTime.Now;

        var score = (_points * 100) / studySession;
        var session = UserInputHandler.CreateStudySession(
            selectedStackChoice,
            stackName,
            _startTime,
            _endTime,
            score
        );
        databaseManager.AddStudySession(session);
    }

    private static int ProcessStudySession(List<int> listOfRandomIndexes, List<CardDto> cards)
    {
        _points = 0;
        for (var i = 0; i < listOfRandomIndexes.Count; i++)
        {
            var guess = AnsiConsole.Prompt(
                new TextPrompt<string>(
                    $"Specify what's: `{cards[listOfRandomIndexes[i]].FlashcardTitle}`"
                )
            );

            if (guess == cards[listOfRandomIndexes[i]].FlashcardContent)
            {
                AnsiConsole.MarkupLine("[springgreen2_1]You guessed it![/]");
                _points++;
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]You're wrong![/]");
            }
        }
        ShowResult();
        return listOfRandomIndexes.Count;
    }

    private static List<int> GetRandomFlashcards(int amount, List<CardDto> cards)
    {
        var indexes = new HashSet<int>();

        while (indexes.Count < amount && indexes.Count < cards.Count)
        {
            indexes.Add(Rnd.Next(0, cards.Count));
        }

        return indexes.ToList();
    }

    private static List<StudySessionDto> GetStudySessionsDtosForStack(
        DatabaseManager databaseManager,
        int stackChoice
    )
    {
        var studySessions = databaseManager.ReadStudySessions(stackChoice);

        return studySessions
            .Select(studySession => new StudySessionDto
            {
                StackName = studySession.StackName,
                StartTime = studySession.StartTime,
                EndTime = studySession.EndTime,
                Score = studySession.Score,
            })
            .ToList();
    }

    private static List<StudySessionAggregatedDto> GetStudySessionsAggregatedDtosForStack(
        DatabaseManager databaseManager,
        int stackChoice,
        string aggregationType
    )
    {
        var studySessions =
            aggregationType == "count"
                ? databaseManager.ReadStudySessionsAggregated(stackChoice)
                : databaseManager.ReadStudySessionsAverage(stackChoice);

        return studySessions
            .Select(studySession => new StudySessionAggregatedDto
            {
                StackName = studySession.StackName,
                Year = studySession.Year,
                January = studySession.January,
                February = studySession.February,
                March = studySession.March,
                April = studySession.April,
                May = studySession.May,
                June = studySession.June,
                July = studySession.July,
                August = studySession.August,
                September = studySession.September,
                October = studySession.October,
                November = studySession.November,
                December = studySession.December,
            })
            .ToList();
    }

    private static void BuildStudySessionTable(DatabaseManager databaseManager, int stackChoice)
    {
        var studySessionDtos = GetStudySessionsDtosForStack(databaseManager, stackChoice);
        var studySessionsTable = new Table();

        studySessionsTable.Title("[bold yellow]Study Sessions[/]");
        studySessionsTable.Border(TableBorder.Rounded);
        studySessionsTable.BorderColor(Color.HotPink3);

        studySessionsTable.AddColumn("[darkorange3_1]Stack Name[/]");
        studySessionsTable.AddColumn("[darkorange3_1]Start Time[/]");
        studySessionsTable.AddColumn("[darkorange3_1]End Time[/]");
        studySessionsTable.AddColumn("[darkorange3_1]Score (%)[/]");

        foreach (var studySession in studySessionDtos)
        {
            studySessionsTable.AddRow(
                $"[grey69] {studySession.StackName}[/]",
                $"[grey69] {studySession.StartTime}[/]",
                $"[grey69] {studySession.EndTime}[/]",
                $"[grey69] {studySession.Score}[/]"
            );
        }

        AnsiConsole.Write(studySessionsTable);
    }

    private static void BuildStudySessionsTablePerMonth(
        DatabaseManager databaseManager,
        int stackChoice,
        string aggregationType
    )
    {
        var studySessionsAggregatedDto = GetStudySessionsAggregatedDtosForStack(
            databaseManager,
            stackChoice,
            aggregationType
        );

        var studySessionsTable = new Table();
        var title =
            aggregationType == "count"
                ? "[bold yellow]Study Sessions per Month[/]"
                : "[bold yellow]Average Score per Month[/]";

        studySessionsTable.Title(title);
        studySessionsTable.Border(TableBorder.Rounded);
        studySessionsTable.BorderColor(Color.HotPink3);

        studySessionsTable.AddColumn("[darkorange3_1]Stack Name[/]");
        studySessionsTable.AddColumn("[darkorange3_1]Year[/]");
        studySessionsTable.AddColumn("[darkorange3_1]January[/]");
        studySessionsTable.AddColumn("[darkorange3_1]February[/]");
        studySessionsTable.AddColumn("[darkorange3_1]March[/]");
        studySessionsTable.AddColumn("[darkorange3_1]April[/]");
        studySessionsTable.AddColumn("[darkorange3_1]May[/]");
        studySessionsTable.AddColumn("[darkorange3_1]June[/]");
        studySessionsTable.AddColumn("[darkorange3_1]July[/]");
        studySessionsTable.AddColumn("[darkorange3_1]August[/]");
        studySessionsTable.AddColumn("[darkorange3_1]September[/]");
        studySessionsTable.AddColumn("[darkorange3_1]October[/]");
        studySessionsTable.AddColumn("[darkorange3_1]November[/]");
        studySessionsTable.AddColumn("[darkorange3_1]December[/]");

        foreach (var studySession in studySessionsAggregatedDto)
        {
            studySessionsTable.AddRow(
                $"[grey69] {studySession.StackName}[/]",
                $"[grey69] {studySession.Year}[/]",
                $"[grey69] {Math.Round(studySession.January, 2)}[/]",
                $"[grey69] {Math.Round(studySession.February, 2)}[/]",
                $"[grey69] {Math.Round(studySession.March, 2)}[/]",
                $"[grey69] {Math.Round(studySession.April, 2)}[/]",
                $"[grey69] {Math.Round(studySession.May, 2)}[/]",
                $"[grey69] {Math.Round(studySession.June, 2)}[/]",
                $"[grey69] {Math.Round(studySession.July, 2)}[/]",
                $"[grey69] {Math.Round(studySession.August, 2)}[/]",
                $"[grey69] {Math.Round(studySession.September, 2)}[/]",
                $"[grey69] {Math.Round(studySession.October, 2)}[/]",
                $"[grey69] {Math.Round(studySession.November, 2)}[/]",
                $"[grey69] {Math.Round(studySession.December, 2)}[/]"
            );
        }

        foreach (var column in studySessionsTable.Columns)
        {
            column.Centered();
        }

        AnsiConsole.Write(studySessionsTable);
    }

    private static void ShowResult()
    {
        AnsiConsole.MarkupLine($"[blueviolet]You scored {_points} point(s)![/]");
        AnsiConsole.MarkupLine("Press any key to go back to the Main Menu.");
        Console.ReadKey();
    }
}
