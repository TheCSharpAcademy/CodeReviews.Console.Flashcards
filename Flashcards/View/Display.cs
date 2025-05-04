using Flashcards.Model;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using Dapper;

namespace Flashcards.View
{
    public class Display
    {
        public static string PrintMainMenu()
        {
            Console.Clear();

            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("MAIN MENU")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Close Application", "Manage Stacks", "Manage Flashcards", "Study", "View Study Session Data", "Reports"
                }));

            return menuChoice;
        }

        public static string PrintStacksMenu()
        {
            Console.Clear();

            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("STACKS MENU")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Return to Main Menu", "View All Stacks", "Add Stack", "Edit Stack", "Delete Stack"
                }));

            return menuChoice;
        }

        public static string PrintFlashcardsMenu()
        {
            Console.Clear();

            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("FLASHCARDS MENU")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Return to Main Menu", "View Flashcards", "Add Flashcard", "Edit Flashcard", "Delete Flashcard"
                }));

            return menuChoice;
        }

        public static string PrintReportsMenu()
        {
            Console.Clear();

            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("REPORTS MENU")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Return to Main Menu", "View Report: Study Sessions per Month", "View Report: Average Scores per Month"
                }));

            return menuChoice;
        }

        public static (Stacks Stack, int Index) PrintStackSelectionMenu(string heading, string title)
        {
            Console.Clear();

            var repository = new StacksRepository(DatabaseUtility.GetConnectionString());
            var stacks = repository.GetAllStacks();

            if (stacks == null || stacks.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No stacks available.[/]");
                return (null, -1);
            }

            var sortedStacks = stacks.OrderBy(stack => stack.Id).ToList();

            var displayOptions = sortedStacks.Select((stack, index) => new
            {
                DisplayText = $"{index + 1}: {stack.Name}",
                Stack = stack,
                Index = index
            }).ToList();

            var rule = new Rule($"[green]{heading}[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

            var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<dynamic>()
                .Title($"\n{title}")
                .PageSize(10)
                .AddChoices(displayOptions.Select(option => option.DisplayText).ToArray()));

            var selectedStack = displayOptions.First(option => option.DisplayText.StartsWith(selectedOption.Split(':')[0]));

            return (selectedStack.Stack, selectedStack.Index);
        }

        public static (FlashcardDto Flashcard, int Index) PrintFlashcardSelectionMenu(string heading, string title, int stackId)
        {
            Console.Clear();

            var repository = new FlashcardsRepository(DatabaseUtility.GetConnectionString());
            var flashcards = repository.GetAllFlashcardsForStack(stackId);

            if (flashcards == null || flashcards.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No flashcards available.[/]");
                return (null, -1);
            }

            var sortedFlashcards = flashcards.OrderBy(flashcard => flashcard.FlashcardId).ToList();

            var displayOptions = sortedFlashcards.Select((flashcard, index) => new
            {
                DisplayText = $"{index + 1}: [[Question]] {flashcard.Question} [[Answer]] {flashcard.Answer}",
                Flashcard = flashcard,
                Index = index
            }).ToList();

            var rule = new Rule($"[green]{heading}[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

            var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<dynamic>()
                .Title($"\n{title}")
                .PageSize(10)
                .AddChoices(displayOptions.Select(option => option.DisplayText).ToArray()));

            var selectedFlashcard = displayOptions.First(option => option.DisplayText.StartsWith(selectedOption.Split(':')[0]));

            return (selectedFlashcard.Flashcard, selectedFlashcard.Index);
        }

        public static void PrintAllStacks(string heading)
        {
            var repository = new StacksRepository(DatabaseUtility.GetConnectionString());
            var stacks = repository.GetAllStacks();

            Console.Clear();

            var rule = new Rule($"[green]{heading}[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[dodgerblue1]ID[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Stack Name[/]").Centered());

            if (stacks == null || stacks.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No records found.[/]");
                return;
            }

            var sortedStacks = stacks.OrderBy(stack => stack.Id).ToList();

            int indexPlusOne = 1;

            foreach (var stack in sortedStacks)
            {
                table.AddRow(indexPlusOne.ToString(), stack.Name);
                indexPlusOne++;
            }

            AnsiConsole.Write(table);
        }

        public static void PrintAllFlashcardsForStack(string heading, int stackId)
        {
            var repository = new FlashcardsRepository(DatabaseUtility.GetConnectionString());
            var flashcards = repository.GetAllFlashcardsForStack(stackId);

            string? stackName = null;

            string stackQuery = "SELECT StackName AS Name FROM Stacks WHERE StackId = @stackId";

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                Stacks stack = connection.QuerySingleOrDefault<Stacks>(stackQuery, new { stackId });

                stackName = stack.Name.ToString();
            }

            Console.Clear();

            var rule = new Rule($"[green]{heading}[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[dodgerblue1]ID[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Question[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Answer[/]").Centered());

            if (flashcards.Count == 0)
            {
                AnsiConsole.Write(new Markup($"\n [dodgerblue1]Stack:[/] [white]{stackName}[/]\n"));
                AnsiConsole.MarkupLine("\n[red]No records found.[/]");
                return;
            }

            var sortedFlashcards = flashcards.OrderBy(flashcard => flashcard.FlashcardId).ToList();

            int indexPlusOne = 1;

            foreach (var flashcard in sortedFlashcards)
            {
                table.AddRow(
                    indexPlusOne.ToString(),
                    flashcard.Question!,
                    flashcard.Answer!
                );
                indexPlusOne++;
            }

            AnsiConsole.Write(new Markup($"\n [dodgerblue1]Stack:[/] [white]{stackName}[/]\n"));
            AnsiConsole.Write(table);
        }

        public static void PrintAllStudySessionData(string heading)
        {
            var sessionRepo = new StudySessionRepository(DatabaseUtility.GetConnectionString());
            var sessions = sessionRepo.GetAllStudySessions();

            Console.Clear();

            var rule = new Rule($"[green]{heading}[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[dodgerblue1]ID[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Stack Name[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Session Start Time[/]").Centered())
                .AddColumn(new TableColumn("[dodgerblue1]Percentage Correct[/]").Centered());

            if (sessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No records found.[/]");
                return;
            }

            int count = sessions.Count;

            foreach (var session in sessions)
            {
                table.AddRow(
                    count.ToString(),
                    session.StackName.ToString(),
                    session.SessionStartTime.ToString("MMMM dd, yyyy h:mm tt"),
                    session.PercentageCorrect.ToString("0") + "%"
                );
                count--;
            }

            AnsiConsole.Write(table);
        }

        public static void PrintSessionsPerMonthReport()
        {
            string year = UI.PromptForReportYear("\nEnter the year you would like to report on (yyyy): ");

            var connectionString = DatabaseUtility.GetConnectionString();
            var query = $@"
        DECLARE @StartDate DATE = CAST(CONCAT({year}, '-01-01') AS DATE);
        DECLARE @EndDate DATE = CAST(CONCAT({year}, '-12-31') AS DATE);

        SELECT 
            StackName,
            COALESCE([{year}-01], 0) AS [January],
            COALESCE([{year}-02], 0) AS [February],
            COALESCE([{year}-03], 0) AS [March],
            COALESCE([{year}-04], 0) AS [April],
            COALESCE([{year}-05], 0) AS [May],
            COALESCE([{year}-06], 0) AS [June],
            COALESCE([{year}-07], 0) AS [July],
            COALESCE([{year}-08], 0) AS [August],
            COALESCE([{year}-09], 0) AS [September],
            COALESCE([{year}-10], 0) AS [October],
            COALESCE([{year}-11], 0) AS [November],
            COALESCE([{year}-12], 0) AS [December]
        FROM (
            SELECT 
                S.StackId,
                S.StackName,
                FORMAT(SS.SessionStartTime, 'yyyy-MM') AS SessionMonth,
                COUNT(SS.SessionID) AS SessionCount
            FROM 
                Stacks S
            LEFT JOIN 
                StudySessionStats SS ON SS.StackID = S.StackID AND SS.SessionStartTime >= @StartDate AND SS.SessionStartTime <= @EndDate
            GROUP BY 
                S.StackId, S.StackName, FORMAT(SS.SessionStartTime, 'yyyy-MM')
        ) AS SourceTable
        PIVOT (
            SUM(SessionCount)
            FOR SessionMonth IN ([{year}-01], [{year}-02], [{year}-03], [{year}-04], [{year}-05], 
                                 [{year}-06], [{year}-07], [{year}-08], [{year}-09], [{year}-10], 
                                 [{year}-11], [{year}-12])
        ) AS PivotTable
        ORDER BY StackId ASC;";

            using (var connection = new SqlConnection(connectionString))
            {
                int desiredWidth = 135;

                int height = 30;

                int width = Math.Min(desiredWidth, Console.LargestWindowWidth);

                Console.SetWindowSize(width, height);

                var command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();

                    Console.Clear();

                    var rule = new Rule($"[green]Study Sessions per Month for {year}[/]");
                    rule.Justification = Justify.Left;
                    AnsiConsole.Write(rule);

                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn(new TableColumn("[dodgerblue1]Stack Name[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]January[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]February[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]March[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]April[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]May[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]June[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]July[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]August[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]September[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]October[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]November[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]December[/]").Centered());

                    if (!reader.HasRows)
                    {
                        AnsiConsole.MarkupLine("\n[red]No records found.[/]");
                        return;
                    }

                    while (reader.Read())
                    {
                        table.AddRow(
                            reader["StackName"].ToString(),
                            reader["January"].ToString(),
                            reader["February"].ToString(),
                            reader["March"].ToString(),
                            reader["April"].ToString(),
                            reader["May"].ToString(),
                            reader["June"].ToString(),
                            reader["July"].ToString(),
                            reader["August"].ToString(),
                            reader["September"].ToString(),
                            reader["October"].ToString(),
                            reader["November"].ToString(),
                            reader["December"].ToString()
                        );
                    }

                    AnsiConsole.Write(table);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        public static void PrintAverageScoresPerMonthReport()
        {
            string year = UI.PromptForReportYear("\nEnter the year you would like to report on (yyyy): ");

            var connectionString = DatabaseUtility.GetConnectionString();

            var query = $@"
    DECLARE @StartDate DATE = CAST(CONCAT({year}, '-01-01') AS DATE);
    DECLARE @EndDate DATE = CAST(CONCAT({year}, '-12-31') AS DATE);

    SELECT 
        StackName,
        COALESCE([{year}-01], 0) AS [January],
        COALESCE([{year}-02], 0) AS [February],
        COALESCE([{year}-03], 0) AS [March],
        COALESCE([{year}-04], 0) AS [April],
        COALESCE([{year}-05], 0) AS [May],
        COALESCE([{year}-06], 0) AS [June],
        COALESCE([{year}-07], 0) AS [July],
        COALESCE([{year}-08], 0) AS [August],
        COALESCE([{year}-09], 0) AS [September],
        COALESCE([{year}-10], 0) AS [October],
        COALESCE([{year}-11], 0) AS [November],
        COALESCE([{year}-12], 0) AS [December]
    FROM (
        SELECT 
            S.StackId,
            S.StackName,
            FORMAT(SS.SessionStartTime, 'yyyy-MM') AS SessionMonth,
            AVG(SS.PercentageCorrect) AS AverageScore
        FROM 
            Stacks S
        LEFT JOIN 
            StudySessionStats SS ON S.StackId = SS.StackID AND SS.SessionStartTime >= @StartDate AND SS.SessionStartTime <= @EndDate
        GROUP BY 
            S.StackId, S.StackName, FORMAT(SS.SessionStartTime, 'yyyy-MM')
    ) AS SourceTable
    PIVOT (
        AVG(AverageScore) FOR SessionMonth IN ([{year}-01], [{year}-02], [{year}-03], [{year}-04], [{year}-05], 
                                                [{year}-06], [{year}-07], [{year}-08], [{year}-09], 
                                                [{year}-10], [{year}-11], [{year}-12])
    ) AS PivotTable
    ORDER BY StackId ASC;";

            using (var connection = new SqlConnection(connectionString))
            {
                int desiredWidth = 135;

                int height = 30;

                int width = Math.Min(desiredWidth, Console.LargestWindowWidth);

                Console.SetWindowSize(width, height);

                var command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();

                    Console.Clear();

                    var rule = new Rule($"[green]Average Scores per Month for {year}[/]");
                    rule.Justification = Justify.Left;
                    AnsiConsole.Write(rule);

                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn(new TableColumn("[dodgerblue1]Stack Name[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]January[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]February[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]March[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]April[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]May[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]June[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]July[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]August[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]September[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]October[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]November[/]").Centered())
                        .AddColumn(new TableColumn("[dodgerblue1]December[/]").Centered());

                    if (!reader.HasRows)
                    {
                        AnsiConsole.MarkupLine("\n[red]No records found.[/]");
                        return;
                    }

                    while (reader.Read())
                    {
                        table.AddRow(
                            reader["StackName"].ToString(),
                            Math.Round(Convert.ToDecimal(reader["January"])).ToString() + "%",
                            Math.Round(Convert.ToDecimal(reader["February"])).ToString() + "%",
                            Math.Round(Convert.ToDecimal(reader["March"])).ToString() + "%",
                            Math.Round(Convert.ToDecimal(reader["April"])).ToString() + "%",
                            Math.Round(Convert.ToDecimal(reader["May"])).ToString() + "%",
                            Math.Round(Convert.ToDecimal(reader["June"])).ToString() + "%",
                            Math.Round(Convert.ToDecimal(reader["July"])).ToString() + "%",
                            Math.Round(Convert.ToDecimal(reader["August"])).ToString() + "%",
                            Math.Round(Convert.ToDecimal(reader["September"])).ToString() + "%",
                            Math.Round(Convert.ToDecimal(reader["October"])).ToString() + "%",
                            Math.Round(Convert.ToDecimal(reader["November"])).ToString() + "%",
                            Math.Round(Convert.ToDecimal(reader["December"])).ToString() + "%"
                        );
                    }

                    AnsiConsole.Write(table);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
    }
}
