using Flashcards.empty_codes.Controllers;
using Flashcards.empty_codes.Models;
using Spectre.Console;

namespace Flashcards.empty_codes.Views
{
    internal class StudySessionMenu
    {
        public StacksController StackController { get; }
        public FlashcardsController FlashcardController { get; }
        public StudySessionController StudySessionController { get; }
        public MainMenu MainMenu { get; }
        public StackMenu StackMenu { get; }
        public FlashcardMenu FlashcardMenu { get; }
        public void GetStudySessionMenu()
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an [green]option below[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal your choices)[/]")
                    .AddChoices(new[] {
                        "Start a Study Session", "View Study Session Records", 
                    }));

            switch (choice)
            {
                case "Start a Study Session":
                    StartNewStudySession();
                    break;
                case "View Study Session Records":
                    ViewStudySessions();
                    break;
                case "Return to Main Menu":
                    MainMenu.GetMainMenu();
                    break;
                default:
                    AnsiConsole.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }

        public void StartNewStudySession()
        {
            StackMenu.ViewAllStacks();
            var name = AnsiConsole.Ask<string>("Enter the name of the stack you want to study: ");
            StackDTO stack = new StackDTO();
            stack.StackName = name;
            if (StackController.CheckIfStackExists(stack) > 0)
            {
                AnsiConsole.WriteLine($"Current stack: {stack.StackName}");
                GetStudySessionQandA(stack);
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]No stack found with the provided name: {stack.StackName}[/]");
            }
        }

        public void GetStudySessionQandA(StackDTO stack)
        {
            StudySessionDTO session = new StudySessionDTO();
            int correctAnswers = 0;
            var cards = FlashcardController.ViewAllFlashcards(stack);
            if (cards.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No flashcards found![/]");
            }
            else
            {
                session.StackId = stack.StackId;
                session.StudyDate = DateTime.Now;
                foreach (var card in cards)
                {
                    AnsiConsole.WriteLine($"Question: {card.Question}");
                    var inputAnswer = AnsiConsole.Ask<string>("Enter your answer: ");
                    
                    if (inputAnswer == card.Answer)
                    {
                        correctAnswers++;
                    }
                }
                session.Score = $"You got {correctAnswers} correct out of {cards.Count} questions.";
                AnsiConsole.WriteLine(session.Score);
                FlashcardMenu.ViewAllFlashcards(stack);
            }
        }

        public void ViewStudySessions()
        {
            var sessions = StudySessionController.ViewAllSessions();
            if (sessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No sessions found![/]");
            }
            else
            {
                var table = new Table();
                table.Title = new TableTitle("All Sessions", Style.Parse("bold yellow"));
                table.AddColumn("[bold]Stack Name[/]");
                table.AddColumn("[bold]Session Id[/]");
                table.AddColumn("[bold]Score[/]");
                table.AddColumn("[bold]Study Date[/]");

                foreach (var session in sessions)
                {
                    var stack = StackController.GetStackById(session.StackId);
                    table.AddRow(
                        stack.StackName,
                        session.SessionId.ToString(),
                        session.Score,
                        session.StudyDate.ToString("yyyy-MM-dd hh:mm")
                    );
                }
                Console.Clear();
                AnsiConsole.Write(table);
            }
        }
    }
}
