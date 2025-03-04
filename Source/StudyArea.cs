using System.Collections.Generic;
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
                int randomScore = random.Next(TOTAL_ROUNDS + 1);

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