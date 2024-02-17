using FlashCards.Cactus.DataModel;
using Spectre.Console;
using System.Diagnostics;

namespace FlashCards.Cactus.Service;

public class StudySessionService
{
    public StudySessionService()
    {
        StudySessions = new List<StudySession>() {
            new StudySession(1, "English", new TimeSpan(1,10,0), 10),
            new StudySession(2, "Algorithm", new TimeSpan(2,0,0), 40)
        };
    }

    public List<StudySession> StudySessions { get; set; }

    public void ShowAllStudySessions()
    {
        List<List<string>> rows = new List<List<string>>();
        StudySessions.ForEach(ss => rows.Add(new List<string>() { ss.StackName, ss.Time.TotalMinutes.ToString("F"), ss.Score.ToString() }));
        ServiceHelpers.ShowDataRecords(Constants.STUDYSESSION, Constants.STUDYSESSIONS, rows);
    }

    public void StartNewStudySession()
    {
        Console.WriteLine("Start a new study session.");

        Tuple<int, string> stackIdName = AskUserToTypeAStackName();
        if (stackIdName.Item1 == -1) return;

        // start to learn flashcards
        List<FlashCard> flashcards = new List<FlashCard>() {
            new FlashCard(1, 1, "Freedom", "ziyou"),
            new FlashCard(2, 2, "1+1=", "2"),
            new FlashCard(3, 2, "1*1=", "1"),
        };
        var learnFlashCards = flashcards.Where(card => card.SId == stackIdName.Item1).ToList();
        var shuffleCards = learnFlashCards.OrderBy(_ => Guid.NewGuid()).ToList();

        Console.WriteLine("Type any key to start learning, or 'q' to quit.");
        string key = Console.ReadLine();
        if (key.Equals(Constants.QUIT)) return;

        Tuple<TimeSpan, int> timeScore = LearnFlashCards(stackIdName.Item2, shuffleCards);

        StudySessions.Add(new StudySession(StudySessions.Count, stackIdName.Item2, timeScore.Item1, timeScore.Item2));

        AnsiConsole.MarkupLine($"Study Finished. You taken [green]{timeScore.Item1.TotalMinutes.ToString("F")}[/] minutes to learn, and got [green]{timeScore.Item2}[/] score.");
    }

    public Tuple<int, string> AskUserToTypeAStackName()
    {
        List<Stack> stacks = new List<Stack> { new Stack(1, "Word"), new Stack(2, "Algorithm") };
        List<string> stackNames = new List<string>() { "Word", "Algorithm" };

        string stackName = AnsiConsole.Ask<string>("Please input the [green]name[/] of the Stack where you want to start. Type 'q' to quit.");
        if (stackName.Equals(Constants.QUIT)) return new Tuple<int, string>(-1, "");
        while (!stackNames.Contains(stackName))
        {
            stackName = AnsiConsole.Ask<string>($"[red]{stackName}[/] Stack dose not exist. Please input a valid Stack name. Type 'q' to quit.");
            if (stackName.Equals(Constants.QUIT)) return new Tuple<int, string>(-1, "");
        }
        int sid = stacks.Where(s => s.Name.Equals(stackName)).ToArray()[0].Id;

        return new Tuple<int, string>(sid, stackName);
    }

    public Tuple<TimeSpan, int> LearnFlashCards(string stackName, List<FlashCard> flashcards)
    {
        int score = 0;
        var timer = new Stopwatch();
        timer.Start();
        foreach (var flashcard in flashcards)
        {
            Console.Clear();

            var table = new Table();
            table.Title($"{stackName}");
            table.AddColumn("Front");
            table.AddRow(flashcard.Front);
            AnsiConsole.Write(table);

            Console.WriteLine();
            Console.WriteLine("Please input your answer to this card. Or 'q' to quit.");
            string answer = Console.ReadLine();

            if (Constants.QUIT.Equals(answer)) break;
            if (flashcard.Back.Equals(answer))
            {
                Console.WriteLine("\nYour answer was correct.");
                score++;
            }
            else
            {
                Console.WriteLine($"\nYour answer was wrong.\nThe COREECT answer was {flashcard.Back}.");
            }

            Console.WriteLine();
            Console.WriteLine("Type any key to continue.");
            Console.ReadLine();
        }
        timer.Stop();
        TimeSpan timeTaken = timer.Elapsed;

        return new Tuple<TimeSpan, int>(timeTaken, score);
    }

    public void DeleteStudySession()
    {
        ShowAllStudySessions();

        if (StudySessions.Count == 0)
        {
            return;
        }

        string idStr = AnsiConsole.Ask<string>("Please input the [green]id[/] of the StudySession you want to delete. Type 'q' to quit.");
        if (idStr.Equals(Constants.QUIT)) return;

        int inputId = -1;
        while (!int.TryParse(idStr, out inputId) || inputId < 1 || inputId > StudySessions.Count)
        {
            idStr = AnsiConsole.Ask<string>($"Please input a valid id.");
        }
        StudySession deletedSS = StudySessions[inputId - 1];
        StudySessions = StudySessions.Where(ss => ss.Id != deletedSS.Id).ToList();

        AnsiConsole.MarkupLine($"Successfully deleted [green]No.{inputId}[/] StudySession.");
    }
}

