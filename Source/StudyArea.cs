using System.Security.Cryptography;
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
                int randomScore = random.Next(11);

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
}