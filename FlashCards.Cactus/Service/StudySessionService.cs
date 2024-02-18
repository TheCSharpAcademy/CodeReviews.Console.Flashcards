using FlashCards.Cactus.Dao;
using FlashCards.Cactus.DataModel;
using FlashCards.Cactus.Helper;
using Spectre.Console;
using System.Diagnostics;

namespace FlashCards.Cactus.Service;

public class StudySessionService
{
    public StudySessionService(StudySessionDao studySessionDao)
    {
        StudySessionDao = studySessionDao;
        StudySessions = StudySessionDao.FindAll();
    }

    public StudySessionDao StudySessionDao { get; set; }

    public List<StudySession> StudySessions { get; set; }

    public void ShowAllStudySessions()
    {
        List<List<string>> rows = new List<List<string>>();
        StudySessions.ForEach(ss => rows.Add(new List<string>() {
            ss.StackName, ss.Date.ToShortDateString(), ss.Time.ToString("F"), ss.Score.ToString()
        }));
        ServiceHelper.ShowDataRecords(Constants.STUDYSESSION, Constants.STUDYSESSIONS, rows);
    }

    public void StartNewStudySession(List<Stack> stacks, List<FlashCard> flashcards)
    {
        Console.WriteLine("Start a new study session.");

        Tuple<int, string> stackIdName = AskUserToTypeAStackName(stacks);
        if (stackIdName.Item1 == -1) return;

        var learnFlashCards = flashcards.Where(card => card.SId == stackIdName.Item1).ToList();
        var shuffleCards = learnFlashCards.OrderBy(_ => Guid.NewGuid()).ToList();

        Console.WriteLine("Press any key to start learning, or 'q' to quit.");
        string? key = Console.ReadLine();
        if (Constants.QUIT.Equals(key)) return;

        Tuple<double, int> timeScore = LearnFlashCards(stackIdName.Item2, shuffleCards);

        StudySession newStudySession = new StudySession(stackIdName.Item1, stackIdName.Item2, DateTime.Now, timeScore.Item1, timeScore.Item2);

        int res = StudySessionDao.Insert(newStudySession);
        if (res == -1)
        {
            AnsiConsole.MarkupLine($"[red]Failed to add this study session.[/]");
        }
        else
        {
            StudySessions.Add(newStudySession);
            AnsiConsole.MarkupLine($"Study Finished. You taken [green]{timeScore.Item1.ToString("F")}[/] minutes to learn, and got [green]{timeScore.Item2}[/] score.");
        }
    }

    public Tuple<int, string> AskUserToTypeAStackName(List<Stack> stacks)
    {
        // Show available stack names.
        List<string> stackNames = stacks.Select(s => s.Name).ToList();
        List<List<string>> rows = new List<List<string>>();
        stackNames.ForEach(name => rows.Add(new List<string>() { name }));
        ServiceHelper.ShowDataRecords(Constants.STACK, Constants.STACKS, rows);


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

    public Tuple<double, int> LearnFlashCards(string stackName, List<FlashCard> flashcards)
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
            string? answer = Console.ReadLine();

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
        double timeTaken = timer.Elapsed.TotalMinutes;

        return new Tuple<double, int>(timeTaken, score);
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

        int inputId = ServiceHelper.GetUserInputId(idStr, StudySessions.Count);
        if (inputId == -1) return;

        StudySession deletedSS = StudySessions[inputId - 1];

        int res = StudySessionDao.DeleteById(deletedSS.Id);
        if (res == -1)
        {
            AnsiConsole.MarkupLine($"[red]Failed to delete No.{inputId} StudySession.[/]");
        }
        else
        {
            StudySessions = StudySessions.Where(ss => ss.Id != deletedSS.Id).ToList();
            AnsiConsole.MarkupLine($"Successfully deleted [green]No.{inputId}[/] StudySession.");
        }
    }
}

