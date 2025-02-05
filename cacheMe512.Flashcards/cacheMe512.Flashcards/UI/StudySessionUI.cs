using Spectre.Console;
using cacheMe512.Flashcards.DTOs;
using cacheMe512.Flashcards.Controllers;
using cacheMe512.Flashcards.Models;

namespace cacheMe512.Flashcards.UI
{
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

                if (choice == "Back to Main Menu")
                    return;

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
            }
        }

        private void StartNewSession()
        {
            var stacks = _stackController.GetAllStacks().ToList();
            if (!stacks.Any())
            {
                AnsiConsole.MarkupLine("[red]No stacks available. Add a stack first.[/]");
                AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
                Console.ReadKey();
                return;
            }

            var selectedStack = AnsiConsole.Prompt(
                new SelectionPrompt<StackDto>()
                    .Title("[bold yellow]Select a Stack for Studying[/]")
                    .PageSize(10)
                    .UseConverter(stack => stack.Name)
                    .AddChoices(stacks)
            );

            var session = new StudySessionDto(0, selectedStack.Name, DateTime.Now, 0, 0);
            int sessionId = _sessionController.InsertSession(new StudySession
            {
                StackId = selectedStack.Id,
                Date = session.Date,
                Score = session.Score,
                TotalQuestions = 0
            });

            RunStudySession(sessionId, selectedStack);
        }

        private void ContinueSession()
        {
            var sessions = _sessionController.GetActiveSessions().ToList();
            if (!sessions.Any())
            {
                AnsiConsole.MarkupLine("[red]No active study sessions available.[/]");
                AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
                Console.ReadKey();
                return;
            }

            var selectedSession = AnsiConsole.Prompt(
                new SelectionPrompt<StudySessionDto>()
                    .Title("[bold yellow]Select a Study Session to Continue[/]")
                    .PageSize(10)
                    .UseConverter(session => $"{session.StackName} - {session.Date:MM/dd/yyyy} - Score: {session.Score}")
                    .AddChoices(sessions)
            );

            var activeSession = sessions.FirstOrDefault(s =>
                s.StackName == selectedSession.StackName && s.Date == selectedSession.Date);

            if (activeSession == null)
            {
                AnsiConsole.MarkupLine("[red]Error: Could not retrieve session ID.[/]");
                AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
                Console.ReadKey();
                return;
            }

            var stack = _stackController.GetAllStacks().FirstOrDefault(s => s.Name == selectedSession.StackName);
            if (stack != null)
            {
                RunStudySession(activeSession.Id, stack);
            }
        }


        private void EndSession()
        {
            var sessions = _sessionController.GetActiveSessions().ToList();
            if (!sessions.Any())
            {
                AnsiConsole.MarkupLine("[red]No active study sessions to end.[/]");
                AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
                Console.ReadKey();
                return;
            }

            var selectedSession = AnsiConsole.Prompt(
                new SelectionPrompt<StudySessionDto>()
                    .Title("[bold yellow]Select a Study Session to End[/]")
                    .PageSize(10)
                    .UseConverter(session => $"{session.StackName} - {session.Date:MM/dd/yyyy} - Score: {session.Score}")
                    .AddChoices(sessions)
            );

            var session = sessions.FirstOrDefault(s => s.StackName == selectedSession.StackName && s.Date == selectedSession.Date);

            if (session == null)
            {
                AnsiConsole.MarkupLine("[red]Error: Could not retrieve session ID from the selected session.[/]");
                AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
                Console.ReadKey();
                return;
            }


            int score = session.Score;
            int totalQuestions = session.TotalQuestions;
            _sessionController.EndSession(session.Id);

            AnsiConsole.MarkupLine("[green]Study session ended successfully![/]");
            AnsiConsole.MarkupLine($"[bold yellow]Final Session Score: {score}/{totalQuestions} correct[/]");
            AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
            Console.ReadKey();
        }

        private void RunStudySession(int sessionId, StackDto stack)
        {
            Random random = new Random();
            int correctAnswers = 0;
            int totalQuestions = 0;
            var flashcards = _flashcardController.GetFlashcardsByStackId(stack.Id).ToList();

            if (!flashcards.Any())
            {
                Utilities.DisplayMessage("No flashcards available in this stack.", "red");
                AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                Utilities.DisplayMessage("Press Enter or Space to see Answer, or 'q' to quit.");

                var flashcard = flashcards[random.Next(flashcards.Count)];

                AnsiConsole.MarkupLine($"[bold cyan]Question: {flashcard.Question}[/]");

                var key = Console.ReadKey(intercept: true).Key;
                if (key == ConsoleKey.Q)
                {
                    break;
                }

                totalQuestions++;
                AnsiConsole.MarkupLine($"[bold green]Answer: {flashcard.Answer}[/]");

                var isCorrect = AnsiConsole.Confirm("Did you get it right?");
                if (isCorrect)
                {
                    correctAnswers++;
                }
            }

            _sessionController.UpdateSessionScore(sessionId, correctAnswers, totalQuestions);

            var updatedSession = _sessionController.GetSessionById(sessionId);
            AnsiConsole.MarkupLine($"[bold yellow]Session Score: {updatedSession?.Score}/{updatedSession?.TotalQuestions} correct[/]");
            AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
            Console.ReadKey();
        }
    }
}
