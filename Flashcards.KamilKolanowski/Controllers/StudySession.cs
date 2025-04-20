using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Dtos.StudySessions;
using Flashcards.KamilKolanowski.Helpers;
using Flashcards.KamilKolanowski.Models;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Controllers;

internal class StudySession
{
    private static int _points = 0;
    private static List<CardDto> _cards = new List<CardDto>();
    private static readonly Random Rnd = new();

    internal static void Study(DatabaseManager databaseManager)
    {
        var selectedStackChoice = StackChoice.GetStackChoice(databaseManager);
        var currentTime = DateTime.Now;
        
        Console.WriteLine($"Starting study session at {currentTime}");
        
        StartStudy(databaseManager, selectedStackChoice);
    }
    
    internal static void ViewStudySessions(DatabaseManager databaseManager)
    {
        var stackChoice = StackChoice.GetStackChoice(databaseManager);
        var studySessionDtos = GetStudySessionsDtosForStack(databaseManager, stackChoice);
        var table = BuildStudySessionTable(databaseManager, studySessionDtos);
        
        AnsiConsole.Write(table);
        
        AnsiConsole.MarkupLine("Press any key to go back to the main menu.");
        Console.ReadKey();
    }
    
    private static void StartStudy(DatabaseManager databaseManager, int stackChoice)
    {
        var amountOfFlashcardToStudyFrom = AnsiConsole.Prompt(
            new TextPrompt<int>("Specify how many flashcards you would like to study: "));

        var cards = FlashcardsController.GetFlashcardDtosForStack(databaseManager, stackChoice);
        
        var listOfRandomIndexes = GetRandomFlashcards(amountOfFlashcardToStudyFrom, cards);
        ProcessStudySession(listOfRandomIndexes, cards);
    }

    private static void ProcessStudySession(List<int> listOfRandomIndexes, List<CardDto> cards)
    {
        for (var i = 0; i < listOfRandomIndexes.Count; i++)
        {
            var guess = AnsiConsole.Prompt(
                new TextPrompt<string>($"Specify what's: `{cards[listOfRandomIndexes[i]].FlashcardTitle}`"));

            if (guess == cards[listOfRandomIndexes[i]].FlashcardContent)
            {
                AnsiConsole.MarkupLine("[springgreen2_1]You guessed it![/]");
                _points++;
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]You're wrong![/]");
                continue;
            }
            ShowResult();
        }
    }

    private static List<int> GetRandomFlashcards(int amountOfFlashcardToStudyFrom, List<CardDto> cards)
    {
        List<int> listOfRandomIndexes = [];

        for (var i = 0; i < amountOfFlashcardToStudyFrom; i++)
        {
            var idx = Rnd.Next(0, cards.Count);

            if (listOfRandomIndexes.Contains(idx))
            {
                continue;
            }
            
            listOfRandomIndexes.Add(idx);
        }

        return listOfRandomIndexes;
    }
    
    private static List<StudySessionDto> GetStudySessionsDtosForStack(DatabaseManager databaseManager, int stackChoice)
    {
        var studySessions = databaseManager.ReadStudySessions(stackChoice);

        return studySessions.Select(studySession => new StudySessionDto    
        {
            StackName = studySession.StackName,
            StartTime = studySession.StartTime,
            EndTime = studySession.EndTime,
            Score = studySession.Score
        }).ToList();
    }

    private static Table BuildStudySessionTable(DatabaseManager databaseManager, List<StudySessionDto> studySessionsDto)
    {
        var studySessionsTable = new Table();
        
        studySessionsTable.Title("[bold yellow]Study Sessions[/]");
        studySessionsTable.Border(TableBorder.Rounded);
        studySessionsTable.BorderColor(Color.HotPink3);
        
        studySessionsTable.AddColumn("[darkorange3_1]Stack Name[/]");
        studySessionsTable.AddColumn("[darkorange3_1]Start Time[/]");
        studySessionsTable.AddColumn("[darkorange3_1]End Time[/]");
        studySessionsTable.AddColumn("[darkorange3_1]Score (%)[/]");

        foreach (var studySession in studySessionsDto)
        {
            studySessionsTable.AddRow(
                $"[grey69] {studySession.StackName}[/]",
                $"[grey69] {studySession.StartTime}[/]",
                $"[grey69] {studySession.EndTime}[/]",
                $"[grey69] {studySession.Score}[/]"
            );
        }
        
        return studySessionsTable;
    }
    
    private static Table BuildStudySessionsTablePerMonth(DatabaseManager databaseManager, List<StudySessionAggregatedDto> studySessionsAggregatedDto)
    {
        var studySessionsTable = new Table();
        
        studySessionsTable.Title("[bold yellow]Study Sessions Per Month[/]");
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
                $"[grey69] {studySession.January}[/]",
                $"[grey69] {studySession.February}[/]",
                $"[grey69] {studySession.March}[/]",
                $"[grey69] {studySession.April}[/]",
                $"[grey69] {studySession.May}[/]",
                $"[grey69] {studySession.June}[/]",
                $"[grey69] {studySession.July}[/]",
                $"[grey69] {studySession.August}[/]",
                $"[grey69] {studySession.September}[/]",
                $"[grey69] {studySession.October}[/]",
                $"[grey69] {studySession.November}[/]",
                $"[grey69] {studySession.December}[/]"
            );
        }

        foreach (var column in studySessionsTable.Columns)
        {
            column.Centered();
        }
        
        return studySessionsTable;
    }
    private static void ShowResult()
    {
        AnsiConsole.MarkupLine("[blueviolet]---[/]");
        AnsiConsole.MarkupLine($"[blueviolet]You scored {_points} points![/]");
        AnsiConsole.MarkupLine("Press any key to go back to the Main Menu.");
        Console.ReadKey();
    }
}