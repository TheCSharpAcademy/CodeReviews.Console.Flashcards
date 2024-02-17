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
        StudySessions.ForEach(ss => rows.Add(new List<string>() { ss.StackName, ss.Time.TotalMinutes.ToString(), ss.Score.ToString() }));
        ServiceHelpers.ShowDataRecords(Constants.STUDYSESSION, Constants.STUDYSESSIONS, rows);
    }

    public void StartNewStudySession()
    {
        Console.WriteLine("Start a new study session.");
        List<Stack> stacks = new List<Stack> { new Stack(1, "Word"), new Stack(2, "Algorithm") };
        List<string> stackNames = new List<string>() { "Word", "Algorithm" };

        string stackName = AnsiConsole.Ask<string>("Please input the [green]name[/] of the Stack where you want to start to learn. Type 'q' to quit.");
        if (stackName.Equals(Constants.QUIT)) return;
        while (!stackNames.Contains(stackName))
        {
            stackName = AnsiConsole.Ask<string>($"[red]{stackName}[/] Stack dose not exist. Please input a valid Stack name. Type 'q' to quit.");
            if (stackName.Equals(Constants.QUIT)) return;
        }
        int sid = stacks.Where(s => s.Name.Equals(stackName)).ToArray()[0].Id;

        // start to learn flashcards
        List<FlashCard> flashcards = new List<FlashCard>() {
            new FlashCard(1, 1, "Freedom", "ziyou"),
            new FlashCard(2, 2, "1+1=", "2"),
            new FlashCard(3, 2, "1*1=", "1"),
        };
        var shuffleCards = flashcards.OrderBy(_ => Guid.NewGuid()).ToList();

        Console.WriteLine("Type any key to start learning, or 'q' to quit.");
        string key = Console.ReadLine();
        if (key.Equals(Constants.QUIT)) return;


        var timer = new Stopwatch();
        int score = 0;
        int cnt = 0;
        timer.Start();
        foreach (var flashcard in flashcards)
        {
            Console.Clear();
            var table = new Table();
            table.Title("StackName");
            table.AddColumn("Front");
            table.AddRow(flashcard.Front);
            AnsiConsole.Write(table);

            Console.WriteLine();
            Console.WriteLine("Please input your answer to this card. Or 'q' to quit.");
            string answer = Console.ReadLine();
            if (answer.Equals(Constants.QUIT)) break;
            if (answer.Equals(flashcard.Back))
            {
                Console.WriteLine("\nYour answer was correct.");
                score++;
            }
            else
            {
                Console.WriteLine($"\nYour answer was wrong.\nThe COREECT answer was {flashcard.Back}.");
            }
            cnt++;

            Console.WriteLine();
            Console.WriteLine("Type any key to continue.");
            Console.ReadLine();
        }
        timer.Stop();
        TimeSpan timeTaken = timer.Elapsed;

        AnsiConsole.MarkupLine($"Study Finished. You taken [green]{timeTaken.Minutes}[/] minutes to learn, and got [green]{score}/{cnt}[/] score.");
    }

    public void DeleteStudySession()
    {
        Console.WriteLine("Delete a study session.");
    }
}

