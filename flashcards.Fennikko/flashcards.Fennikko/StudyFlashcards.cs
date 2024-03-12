using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
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
        if (studySessions.Count() == 0)
        {
            AnsiConsole.MarkupLine("[red]No study sessions found.[/] Press any key to return to main menu.");
            Console.ReadKey();
            UserInput.GetUserInput();
        }
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

    public static void StudySessionReport()
    {
        AnsiConsole.Clear();
        var year = AnsiConsole.Prompt(
            new TextPrompt<string>("Please enter a year in [green](Format: yyyy)[/]: ")
                .PromptStyle("blue")
                .AllowEmpty());
        while (string.IsNullOrWhiteSpace(year) || !int.TryParse(year, out _) || year.Length < 4 || year.Length > 4)
        {
            year = AnsiConsole.Prompt(
                new TextPrompt<string>("[red]Invalid entry.[/] Please enter a year in [green](Format: yyyy)[/]: ")
                    .PromptStyle("blue")
                    .AllowEmpty());
        }

        var intYear = Convert.ToInt32(year);
        using var connection = new SqlConnection(ConnectionString);
        var command = $"""
                      SELECT 
                          Year,
                          ISNULL([1], 0) AS January,
                          ISNULL([2], 0) AS February,
                          ISNULL([3], 0) AS March,
                          ISNULL([4], 0) AS April,
                          ISNULL([5], 0) AS May,
                          ISNULL([6], 0) AS June,
                          ISNULL([7], 0) AS July,
                          ISNULL([8], 0) AS August,
                          ISNULL([9], 0) AS September,
                          ISNULL([10], 0) AS October,
                          ISNULL([11], 0) AS November,
                          ISNULL([12], 0) AS December
                      FROM (
                          SELECT 
                              YEAR(SessionDate) AS Year,
                              MONTH(SessionDate) AS Month,
                              AVG(SessionScore) AS AverageScore
                          FROM 
                              study_sessions
                          WHERE
                              YEAR(SessionDate) = '{intYear}'
                          GROUP BY 
                              YEAR(SessionDate),
                              MONTH(SessionDate)
                      ) AS MonthlyAverages
                      PIVOT (
                          AVG(AverageScore)
                          FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                      ) AS PivotTable
                      ORDER BY 
                          Year;
                      """;
        var getReports = connection.Query(command);
        if (getReports.Count() == 0)
        {
            var tryAgain = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]No study sessions found.[/] Would you like to try again?")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Yes","No"
                    }));
            if (tryAgain == "Yes")
            {
                StudySessionReport();
            }

            if (tryAgain == "No")
            {
                UserInput.GetUserInput();
            }
        }
        var reportsList = getReports.ToList();
        TableReport(reportsList);
        AnsiConsole.MarkupLine("[blue]Press any key to return to main menu[/]");
        Console.ReadKey();
    }

    public static void TableReport(List<dynamic> reports)
    {
        AnsiConsole.Clear();
        var table = new Table();
        table.Title(new TableTitle($"[blue]Study Sessions[/]"));
        table.AddColumn(new TableColumn("[#1ABC9C]Year[/]").Centered());
        table.AddColumn(new TableColumn("[#16A085]January[/]").Centered());
        table.AddColumn(new TableColumn("[#27AE60]February[/]").Centered());
        table.AddColumn(new TableColumn("[#2ECC71]March[/]").Centered());
        table.AddColumn(new TableColumn("[#3498DB]April[/]").Centered());
        table.AddColumn(new TableColumn("[#2980B9]May[/]").Centered());
        table.AddColumn(new TableColumn("[#9B59B6]June[/]").Centered());
        table.AddColumn(new TableColumn("[#8E44AD]July[/]").Centered());
        table.AddColumn(new TableColumn("[#FF5733]August[/]").Centered());
        table.AddColumn(new TableColumn("[#E74C3C]September[/]").Centered());
        table.AddColumn(new TableColumn("[#D35400]October[/]").Centered());
        table.AddColumn(new TableColumn("[#F39C12]November[/]").Centered());
        table.AddColumn(new TableColumn("[#E67E22]December[/]").Centered());
        var averageScoreList = new List<int>();
        foreach (var report in reports)
        {
            table.AddRow($"[#3EB489]{report.Year}[/]", $"[#3EB489]{report.January}[/]", $"[#3EB489]{report.February}[/]", $"[#3EB489]{report.March}[/]", $"[#3EB489]{report.April}[/]", $"[#3EB489]{report.May}[/]", $"[#3EB489]{report.June}[/]", $"[#3EB489]{report.July}[/]", $"[#3EB489]{report.August}[/]", $"[#3EB489]{report.September}[/]", $"[#3EB489]{report.October}[/]", $"[#3EB489]{report.November}[/]", $"[#3EB489]{report.December}[/]");
        }
        AnsiConsole.Write(table);
    }

}