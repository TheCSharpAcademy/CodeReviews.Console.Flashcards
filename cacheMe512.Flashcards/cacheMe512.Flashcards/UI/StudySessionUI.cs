using Spectre.Console;
using cacheMe512.Flashcards.Models;
using cacheMe512.Flashcards.Controllers;
using System.Linq;

namespace cacheMe512.Flashcards.UI;

internal class StudySessionUI
{
    private static readonly StudySessionController _sessionController = new();
    private static readonly StackController _stackController = new();
    private static readonly FlashcardController _flashcardController = new();

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]=== Study Session ===[/]")
                    .PageSize(5)
                    .AddChoices(new[]
                    {
                        "Start New Study Session",
                        "Continue Existing Study Session",
                        "End Study Session",
                        "Back to Main Menu"
                    })
            );

            HandleOption(choice);
        }
    }

    private void HandleOption(string option)
    {
        switch (option)
        {
            case "Start New Study Session":
                StartNewSession();
                break;
            case "Continue Existing Study Session":
                ContinueSession();
                break;
            case "End Study Session":
                EndSession();
                break;
            case "Back to Main Menu":
                return;
        }
    }

    private void StartNewSession()
    {
        var stacks = _stackController.GetAllStacks();
        if (!stacks.Any())
        {
            AnsiConsole.MarkupLine("[red]No stacks available. Add a stack first.[/]");
            Console.ReadKey();
            return;
        }

        var selectedStack = AnsiConsole.Prompt(
            new SelectionPrompt<Stack>()
                .Title("[bold yellow]Select a Stack for Studying[/]")
                .PageSize(10)
                .UseConverter(stack => stack.Name)
                .AddChoices(stacks)
        );

        var session = new StudySession
        {
            StackId = selectedStack.Id,
            Date = DateTime.Now,
            Score = 0
        };

        int sessionId = _sessionController.InsertSession(session);
        RunStudySession(sessionId, selectedStack);
    }

    private void ContinueSession()
    {
        var sessions = _sessionController.GetActiveSessions();
        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[red]No active study sessions available.[/]");
            Console.ReadKey();
            return;
        }

        var selectedSession = AnsiConsole.Prompt(
            new SelectionPrompt<StudySession>()
                .Title("[bold yellow]Select a Study Session to Continue[/]")
                .PageSize(10)
                .UseConverter(session => $"Session {session.Id} (Stack ID: {session.StackId})")
                .AddChoices(sessions)
        );

        var stack = _stackController.GetAllStacks().FirstOrDefault(s => s.Id == selectedSession.StackId);
        if (stack != null)
        {
            RunStudySession(selectedSession.Id, stack);
        }
    }

    private void EndSession()
    {
        var sessions = _sessionController.GetActiveSessions();
        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[red]No active study sessions to end.[/]");
            Console.ReadKey();
            return;
        }

        var selectedSession = AnsiConsole.Prompt(
            new SelectionPrompt<StudySession>()
                .Title("[bold yellow]Select a Study Session to End[/]")
                .PageSize(10)
                .UseConverter(session => $"Session {session.Id} (Stack ID: {session.StackId})")
                .AddChoices(sessions)
        );

        _sessionController.EndSession(selectedSession.Id);
        AnsiConsole.MarkupLine("[green]Study session ended successfully![/]");
        Console.ReadKey();
    }

    private void RunStudySession(int sessionId, Stack stack)
    {
        var flashcards = _flashcardController.GetFlashcardsByStackId(stack.Id).OrderBy(_ => Guid.NewGuid()).ToList();
        if (!flashcards.Any())
        {
            AnsiConsole.MarkupLine("[red]No flashcards available in this stack.[/]");
            Console.ReadKey();
            return;
        }

        int correctAnswers = 0;
        foreach (var flashcard in flashcards)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"[bold cyan]Question: {flashcard.Question}[/]");
            Console.ReadKey();
            AnsiConsole.MarkupLine($"[bold green]Answer: {flashcard.Answer}[/]");

            var isCorrect = AnsiConsole.Confirm("Did you get it right?");
            if (isCorrect)
            {
                correctAnswers++;
            }
        }

        _sessionController.UpdateSessionScore(sessionId, correctAnswers);
        AnsiConsole.MarkupLine("[green]Study session completed![/]");
        Console.ReadKey();
    }
}
