using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using flashcards.Fennikko.Models;
using Spectre.Console;

namespace flashcards.Fennikko;

public class StudyFlashcards
{
    public static readonly string? ConnectionString = ConfigurationManager.AppSettings.Get("connectionString");

    public static void NewStudySession()
    {
        using var connection = new SqlConnection(ConnectionString);
        var sessionDate = DateTime.Now;
        var stackId = DatabaseController.GetStacks("to study");
        var flashcards = DatabaseController.GetAllFlashcards(stackId);
        var score = 0;
        foreach (var flashcard in flashcards)
        {
            AnsiConsole.Clear();
            var table = new Table();
            table.Title(new TableTitle("[blue]Study Session[/]"));
            table.AddColumn(new TableColumn("[#FFA500]FlashcardId[/]").Centered());
            table.AddColumn(new TableColumn("[#104E1D]Question[/]").Centered());

            table.AddRow($"[#3EB489]{flashcard.FlashcardIndex}[/]", $"[#3EB489]{flashcard.CardFront}[/]");
            AnsiConsole.Write(table);

            var answer = flashcard.CardBack;
            var studyAnswer = AnsiConsole.Prompt(
                new TextPrompt<string>("please enter your [green]answer[/] to the above question: ")
                    .PromptStyle("blue")
                    .AllowEmpty());
            while (string.IsNullOrWhiteSpace(studyAnswer))
            {
                studyAnswer = AnsiConsole.Prompt(
                    new TextPrompt<string>("[red]Invalid entry, cannot be empty.[/] Please enter your [green]answer[/] to the above question: ")
                        .PromptStyle("blue")
                        .AllowEmpty());
            }

            if (studyAnswer.Trim().Equals(answer.Trim(), StringComparison.CurrentCultureIgnoreCase))
            {
                score++;
                AnsiConsole.MarkupLine($"Correct! your current score is [green]{score}[/]. Press any key to continue");
                Console.ReadKey();
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Incorrect.[/] Press any key to continue");
                Console.ReadKey();
            }

        }
        AnsiConsole.MarkupLine($"The final score of your study session is: [green]{score}[/] Press any key to continue.");
        Console.ReadKey();

        AnsiConsole.Clear();
        var studySessionCreationCommand =
            "INSERT INTO study_sessions (SessionDate,SessionScore,StackId) VALUES (@SessionDate,@SessionScore,@StackId)";
        var newStudySession = new StudySessions { SessionDate = sessionDate, SessionScore = score, StackId = stackId };
        var studySession = connection.Execute(studySessionCreationCommand, newStudySession);
        AnsiConsole.Write(new Markup($"[green]{studySession}[/] study session added. Press any key to continue."));
        Console.ReadKey();
    }

    public static void GetStudySessions()
    {
        using var connection = new SqlConnection(ConnectionString);
        var stackId = DatabaseController.GetStacks("to view study sessions");
        var command = $"SELECT * FROM study_sessions WHERE StackId = '{stackId}'";
        var studySessions = connection.Query<StudySessions>(command);
        var studySessionsList = studySessions.ToList();
        var stackNameQuery = $"SELECT StackName FROM stacks WHERE StackId = '{stackId}'";
        var getStackName = connection.Query<string>(stackNameQuery);
        var stackNameList = getStackName.ToList();
        var stackName = stackNameList[0];
        TableCreation(studySessionsList,stackName);

        AnsiConsole.MarkupLine("[blue]Press any key to return to main menu[/]");
        Console.ReadKey();
    }

    public static void TableCreation(IEnumerable<StudySessions> sessions, string stackName)
    {
        AnsiConsole.Clear();
        var table = new Table();
        table.Title(new TableTitle($"[green]{stackName}[/][blue] Study Sessions[/]"));
        table.AddColumn(new TableColumn("[#FFA500]Session Date[/]").Centered());
        table.AddColumn(new TableColumn("[#104E1D]Score[/]").Centered());
        var averageScoreList = new List<int>();
        foreach (var session in sessions)
        {
            table.AddRow($"[#3EB489]{session.SessionDate}[/]", $"[#3EB489]{session.SessionScore}[/]");
            averageScoreList.Add(session.SessionScore);
        }

        var averageScore = Math.Round(averageScoreList.ToArray().AsQueryable().Average(), 2);

        table.Caption(new TableTitle($"[#87CEEB]Average study session score: {averageScore}[/]"));

        AnsiConsole.Write(table);
    }
}