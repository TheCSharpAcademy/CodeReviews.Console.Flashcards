using Flashcards.Controllers;
using Flashcards.Models;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace Flashcards.Managers;

internal class StudyManager
{
    private StudyController studyController = new();
    private FlashcardManager flashcardManager = new();
    private CardStackController cardStackController = new();
    private List<Flashcard> flashcards;
    private List<StudySession> sessions;
    private (string Name, int Id) StackData;
    private int score;

    internal void DisplayStudyOptions()
    {
        bool loop = true;
        while (loop)
        {
            Console.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Welcome to [yellow]study mode[/]! Choose an option:")
                .AddChoices(["Begin a study session", "View results", "Return"]));

            switch (choice)
            {
                case "Begin a study session":
                    StackData = flashcardManager.GetStackChoice();
                    if (StackData.Id == 0)
                    {
                        AnsiConsole.MarkupLine("[red]No stacks found![/]");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        loop = false;
                    }
                    else
                    {
                        flashcards = flashcardManager.flashcardController.GetFlashcards(StackData.Id);
                        score = ConductSession(flashcards);
                        studyController.AddSession(StackData.Id, score, flashcards.Count, DateTime.Now);
                    }
                    break;
                case "View results":
                    sessions = studyController.GetAllSessions();
                    if (sessions.IsNullOrEmpty())
                        AnsiConsole.MarkupLine("[red]No sessions recorded![/]");
                    else
                    {
                        AnsiConsole.MarkupLine("[blue]Your results: [/]");
                        foreach (var session in sessions)
                        {
                            Console.WriteLine($"Stack: {cardStackController.GetStackNameById(session.StackId)} Score: {session.Score}/{session.MaxScore} Date: {session.Date}");
                        }
                    }
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    break;
                case "Return":
                    loop = false;
                    break;
            }
        }
    }

    internal int ConductSession(List<Flashcard> flashcards)
    {
        Random rnd = new Random();
        List<Flashcard> questions = flashcards.OrderBy(_ => rnd.Next()).ToList();
        int points = 0;

        for (int i = 0; i < questions.Count; i++)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"Current flashcard stack: [blue]{StackData.Name}[/]");
            Console.WriteLine($"Word: {questions[i].Term}");
            Console.WriteLine("Answer: ");
            if (Console.ReadLine().ToLower() == questions[i].Definition.ToLower())
            {
                points++;
                AnsiConsole.MarkupLine("[green]Correct![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Wrong[/]");
                Console.WriteLine($"Correct answer: {questions[i].Definition}");
            }
            Thread.Sleep(1500);
        }
        Console.WriteLine($"You answered {points} questions correctly out of {questions.Count}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadLine();
        return points;
    }
}