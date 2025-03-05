using System.Globalization;
using Dapper;
using Spectre.Console;

namespace vcesario.Flashcards;

public class StudyArea()
{
    enum MenuOption
    {
        StartSession,
        ViewLastSessions,
        ViewAnnualReport,
        DebugAddSessions,
        Return,
    }

    private const int TOTAL_ROUNDS = 10;

    public void Open()
    {
        MenuOption chosenOption;
        do
        {
            Console.Clear();
            Console.WriteLine(ApplicationTexts.STUDYAREA_HEADER);

            Console.WriteLine();
            chosenOption = AnsiConsole.Prompt(
               new SelectionPrompt<MenuOption>()
               .Title(ApplicationTexts.PROMPT_ACTION)
               .AddChoices(Enum.GetValues<MenuOption>())
           );

            switch (chosenOption)
            {
                case MenuOption.StartSession:
                    StartSession();
                    break;
                case MenuOption.ViewLastSessions:
                    ViewLastSessions();
                    break;
                case MenuOption.ViewAnnualReport:
                    ViewAnnualReport();
                    break;
                case MenuOption.DebugAddSessions:
                    AddDebugSessions();
                    break;
                case MenuOption.Return:
                default:
                    break;
            }
        }
        while (chosenOption != MenuOption.Return);
    }

    private void StartSession()
    {
        var stacksManager = new StacksManager();

        var chosenStack = stacksManager.PromptStack();
        if (chosenStack.Id == -1)
        {
            return;
        }

        var cards = stacksManager.GetCards_FrontBack(chosenStack);
        if (cards == null || cards.Count == 0)
        {
            Console.WriteLine(ApplicationTexts.STACKSMANAGER_LOG_STACKEMPTY);
            Console.ReadLine();
            return;
        }

        var random = new Random();
        int currentRound = 1;
        int score = 0;
        DateTime now = DateTime.Now;

        do
        {
            int cardI = currentRound % cards.Count;
            if (cardI == 1)
            {
                ShuffleCards();
            }

            Console.Clear();
            AnsiConsole.MarkupLine(string.Format(ApplicationTexts.STUDYAREA_HEADER_SESSION, $"[cornflowerblue]{chosenStack.Name}[/]"));

            Console.WriteLine();
            Console.WriteLine(string.Format(ApplicationTexts.STUDYAREA_ROUND, currentRound, TOTAL_ROUNDS));
            Console.WriteLine();
            AnsiConsole.MarkupLine($"[grey]{ApplicationTexts.TOOLTIP_LEAVE}[/]");

            bool revertCard = random.NextSingle() < 0.3f;

            string question = cards[cardI].Front;
            string answer = cards[cardI].Back;

            if (revertCard)
            {
                (question, answer) = (answer, question);
            }

            string color = revertCard ? "indianred" : "gold3_1";

            Console.WriteLine();
            var userAnswer = AnsiConsole.Prompt(new TextPrompt<string>($"[{color}]{question}[/] >> "));

            if (userAnswer.Equals("."))
            {
                break;
            }

            if (userAnswer.ToLower().Equals(answer.ToLower()))
            {
                score++;
                Console.WriteLine();
                AnsiConsole.MarkupLine($"[green]{ApplicationTexts.STUDYAREA_LOG_CORRECT}[/]");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine();
                AnsiConsole.MarkupLine($"[maroon]{ApplicationTexts.STUDYAREA_LOG_INCORRECT}[/]");
                Console.ReadLine();
            }
        }
        while (currentRound++ < TOTAL_ROUNDS);

        Console.Write(string.Format(ApplicationTexts.STUDYAREA_LOG_FINALSCORE, score, TOTAL_ROUNDS));
        if (score == TOTAL_ROUNDS)
        {
            AnsiConsole.MarkupLine($" [cyan1]{ApplicationTexts.STUDYAREA_LOG_ACED}[/]");
        }
        else
        {
            Console.WriteLine();
        }

        using (var connection = DataService.OpenConnection())
        {
            try
            {
                string sql = @"INSERT INTO StudySessions(StackId, Date, Score)
                            VALUES(@StackId, @Date, @Score)";
                connection.Execute(sql, new { StackId = chosenStack.Id, Date = now, Score = score });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        Console.ReadLine();

        void ShuffleCards()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                int targetIndex = random.Next(cards.Count);
                (cards[i], cards[targetIndex]) = (cards[targetIndex], cards[i]);
            }
        }
    }

    private void ViewLastSessions()
    {
        List<StudySessionDTO_StackNameDateScore> sessions;
        using (var connection = DataService.OpenConnection())
        {
            string sql = @"SELECT Stacks.Name AS 'StackName', Date, Score FROM StudySessions
                            JOIN Stacks ON StudySessions.StackId = Stacks.Id
                            ORDER BY Date DESC";
            sessions = connection.Query<StudySessionDTO_StackNameDateScore>(sql).ToList();
        }

        Console.Clear();
        Console.WriteLine(ApplicationTexts.STUDYAREA_HEADER_VIEWLASTSESSIONS);

        Console.WriteLine();
        PrintSessionsTable(sessions);

        Console.ReadLine();

    }

    private void ViewAnnualReport()
    {
        Console.Clear();
        Console.WriteLine(ApplicationTexts.STUDYAREA_HEADER_VIEWANNUALREPORT);

        var validator = new UserInputValidator();

        Console.WriteLine();
        var year = AnsiConsole.Prompt(
            new TextPrompt<string>(ApplicationTexts.STUDYAREA_PROMPT_YEAR)
            .Validate(validator.ValidatePositiveIntOrPeriod));

        if (year.Equals("."))
        {
            return;
        }

        using (var connection = DataService.OpenConnection())
        {
            try
            {
                string sql = @"SELECT StackName, [1] as 'jan', [2] as 'feb', [3] as 'mar', [4] as 'apr', [5] as 'may', [6] as 'jun',
                                                    [7] as 'jul', [8] as 'aug', [9] as 'sep', [10] as 'oct', [11] as 'nov', [12] as 'dec'
                            FROM (
                                SELECT StudySessions.Id AS 'ID', Stacks.Name AS 'StackName', MONTH(StudySessions.Date) AS 'Month'
                                FROM StudySessions
                                JOIN Stacks ON StudySessions.StackId = Stacks.Id
                                WHERE YEAR(Date) = @Year
                            ) AS SourceTable
                            PIVOT(
                                COUNT(ID)
                                FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                            ) AS PivotTable;";
                List<dynamic> studySessionsPerMonth = connection.Query(sql, new { Year = year }).ToList();

                Console.WriteLine();
                AnsiConsole.MarkupLine($"[indianred]{ApplicationTexts.STUDYAREA_HEADER_STUDYSESSIONSPERMONTH}[/]");
                PrintAnnualReportTable(studySessionsPerMonth, "F0", true);

                sql = @"SELECT StackName, [1] as 'jan', [2] as 'feb', [3] as 'mar', [4] as 'apr', [5] as 'may', [6] as 'jun',
                                                    [7] as 'jul', [8] as 'aug', [9] as 'sep', [10] as 'oct', [11] as 'nov', [12] as 'dec'
                            FROM (
                                SELECT Stacks.Name AS 'StackName', MONTH(StudySessions.Date) AS 'Month', StudySessions.Score
                                FROM StudySessions
                                JOIN Stacks ON StudySessions.StackId = Stacks.Id
                                WHERE YEAR(Date) = @Year
                            ) AS SourceTable
                            PIVOT(
                                AVG(Score)
                                FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                            ) AS PivotTable;";
                List<dynamic> averageScorePerMonth = connection.Query(sql, new { Year = year }).ToList();

                Console.WriteLine();
                AnsiConsole.MarkupLine($"[indianred]{ApplicationTexts.STUDYAREA_HEADER_AVERAGESCORE}[/]");
                PrintAnnualReportTable(averageScorePerMonth, "F", false);

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadLine();
            }
        }


    }

    private void AddDebugSessions()
    {
        List<int> stackIds;
        using (var connection = DataService.OpenConnection())
        {
            try
            {
                string sql = "SELECT Id FROM Stacks";
                stackIds = connection.Query<int>(sql).ToList();
            }
            catch (Exception ex)

            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadLine();
                return;
            }
        }

        var random = new Random();
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);

        foreach (var id in stackIds)
        {
            for (int i = 365 * 3; i >= 1; i--)
            {
                if (random.NextSingle() < 0.05f)
                {
                    continue;
                }

                TimeOnly randomStartTime = new(random.Next(24), random.Next(60), random.Next(60));
                DateTime date = new DateTime(today.AddDays(-i), randomStartTime);
                float randomScore = random.Next(TOTAL_ROUNDS + 1);

                using (var connection = DataService.OpenConnection())
                {
                    try
                    {
                        string sql = @"INSERT INTO StudySessions(StackId, Date, Score)
                                    VALUES(@StackId, @Date, @Score)";
                        connection.Execute(sql, new { StackId = id, Date = date, Score = randomScore });
                        Console.WriteLine($"{id} {date} {randomScore}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        Console.ReadLine();
                    }
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine(ApplicationTexts.STUDYAREA_LOG_DEBUGCREATED);
        Console.ReadLine();
    }

    private void PrintAnnualReportTable(List<dynamic> results, string numberFormat, bool totalRow)
    {
        var table = new Table();

        table.AddColumn(new TableColumn($"[yellow]Stack[/]"));
        table.AddColumn(new TableColumn($"[yellow]Jan[/]"));
        table.AddColumn(new TableColumn($"[yellow]Feb[/]"));
        table.AddColumn(new TableColumn($"[yellow]Mar[/]"));
        table.AddColumn(new TableColumn($"[yellow]Apr[/]"));
        table.AddColumn(new TableColumn($"[yellow]May[/]"));
        table.AddColumn(new TableColumn($"[yellow]Jun[/]"));
        table.AddColumn(new TableColumn($"[yellow]Jul[/]"));
        table.AddColumn(new TableColumn($"[yellow]Aug[/]"));
        table.AddColumn(new TableColumn($"[yellow]Sep[/]"));
        table.AddColumn(new TableColumn($"[yellow]Oct[/]"));
        table.AddColumn(new TableColumn($"[yellow]Nov[/]"));
        table.AddColumn(new TableColumn($"[yellow]Dec[/]"));

        if (results.Count == 0)
        {
            table.AddRow("---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---");
        }
        else
        {
            float[] totalPerMonth = new float[12];

            foreach (var result in results)
            {
                string stackName = result.StackName;
                string jan = result.jan != null ? result.jan.ToString(numberFormat, CultureInfo.InvariantCulture) : "0";
                string feb = result.feb != null ? result.feb.ToString(numberFormat, CultureInfo.InvariantCulture) : "0";
                string mar = result.mar != null ? result.mar.ToString(numberFormat, CultureInfo.InvariantCulture) : "0";
                string apr = result.apr != null ? result.apr.ToString(numberFormat, CultureInfo.InvariantCulture) : "0";
                string may = result.may != null ? result.may.ToString(numberFormat, CultureInfo.InvariantCulture) : "0";
                string jun = result.jun != null ? result.jun.ToString(numberFormat, CultureInfo.InvariantCulture) : "0";
                string jul = result.jul != null ? result.jul.ToString(numberFormat, CultureInfo.InvariantCulture) : "0";
                string aug = result.aug != null ? result.aug.ToString(numberFormat, CultureInfo.InvariantCulture) : "0";
                string sep = result.sep != null ? result.sep.ToString(numberFormat, CultureInfo.InvariantCulture) : "0";
                string oct = result.oct != null ? result.oct.ToString(numberFormat, CultureInfo.InvariantCulture) : "0";
                string nov = result.nov != null ? result.nov.ToString(numberFormat, CultureInfo.InvariantCulture) : "0";
                string dec = result.dec != null ? result.dec.ToString(numberFormat, CultureInfo.InvariantCulture) : "0";

                table.AddRow(stackName, jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec);

                if (totalRow)
                {
                    totalPerMonth[0] += result.jan ?? 0;
                    totalPerMonth[1] += result.feb ?? 0;
                    totalPerMonth[2] += result.mar ?? 0;
                    totalPerMonth[3] += result.apr ?? 0;
                    totalPerMonth[4] += result.may ?? 0;
                    totalPerMonth[5] += result.jun ?? 0;
                    totalPerMonth[6] += result.jul ?? 0;
                    totalPerMonth[7] += result.aug ?? 0;
                    totalPerMonth[8] += result.sep ?? 0;
                    totalPerMonth[9] += result.oct ?? 0;
                    totalPerMonth[10] += result.nov ?? 0;
                    totalPerMonth[11] += result.dec ?? 0;
                }
            }

            if (totalRow)
            {
                table.AddEmptyRow();
                table.AddRow("Total",
                    totalPerMonth[0].ToString(numberFormat, CultureInfo.InvariantCulture),
                    totalPerMonth[1].ToString(numberFormat, CultureInfo.InvariantCulture),
                    totalPerMonth[2].ToString(numberFormat, CultureInfo.InvariantCulture),
                    totalPerMonth[3].ToString(numberFormat, CultureInfo.InvariantCulture),
                    totalPerMonth[4].ToString(numberFormat, CultureInfo.InvariantCulture),
                    totalPerMonth[5].ToString(numberFormat, CultureInfo.InvariantCulture),
                    totalPerMonth[6].ToString(numberFormat, CultureInfo.InvariantCulture),
                    totalPerMonth[7].ToString(numberFormat, CultureInfo.InvariantCulture),
                    totalPerMonth[8].ToString(numberFormat, CultureInfo.InvariantCulture),
                    totalPerMonth[9].ToString(numberFormat, CultureInfo.InvariantCulture),
                    totalPerMonth[10].ToString(numberFormat, CultureInfo.InvariantCulture),
                    totalPerMonth[11].ToString(numberFormat, CultureInfo.InvariantCulture));
            }
        }

        table.Border = TableBorder.Rounded;

        AnsiConsole.Write(table);
    }

    private void PrintSessionsTable(List<StudySessionDTO_StackNameDateScore> sessions)
    {
        var table = new Table();

        table.AddColumn(new TableColumn($"[yellow]{ApplicationTexts.TABLE_DATE}[/]").Centered());
        table.AddColumn(new TableColumn($"[yellow]{ApplicationTexts.TABLE_STACKNAME}[/]").Centered());
        table.AddColumn(new TableColumn($"[yellow]{ApplicationTexts.TABLE_SCORE}[/]").Centered());

        if (sessions == null || sessions.Count == 0)
        {
            table.AddRow("---", "----------", "----------");
        }
        else
        {
            foreach (var session in sessions)
            {
                string scoreText = $"{session.Score} / {TOTAL_ROUNDS}";
                if (session.Score == TOTAL_ROUNDS)
                {
                    scoreText += " [cyan]![/]";
                }

                table.AddRow(session.Date.ToString(), session.StackName, scoreText);
            }
        }

        table.Border = TableBorder.Rounded;

        AnsiConsole.Write(table);
    }
}