using Flashcards.empty_codes.Controllers;
using Flashcards.empty_codes.Models;
using Spectre.Console;

namespace Flashcards.empty_codes.Views
{
    internal class StudySessionMenu
    {
        public void GetStudySessionMenu()
        {
            MainMenu menu = new MainMenu();
            Console.Clear();
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
                    menu.GetMainMenu();
                    break;
                case "View Study Session Records":
                    ViewStudySessions();
                    menu.GetMainMenu();
                    break;
                case "Return to Main Menu":
                    menu.GetMainMenu();
                    break;
                default:
                    AnsiConsole.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }

        public void StartNewStudySession()
        {
            StacksController stackController = new StacksController();
            StackMenu stackMenu = new StackMenu();
            stackMenu.ViewAllStacks();
            var name = AnsiConsole.Ask<string>("Enter the name of the stack you want to study: ");
            StackDTO stack = new StackDTO();
            stack.StackName = name;
            if (stackController.CheckIfStackExists(stack) > 0)
            {
                Console.Clear();
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
            FlashcardsController flashcardController = new FlashcardsController();
            StudySessionDTO session = new StudySessionDTO();
            int correctAnswers = 0;
            var cards = flashcardController.ViewAllFlashcards(stack);
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

                StudySessionController studySessionController = new StudySessionController();
                studySessionController.InsertSession(session);

                AnsiConsole.WriteLine(session.Score);
                AnsiConsole.WriteLine("Press any key to show the correct answers");
                Console.ReadKey();
                FlashcardMenu flashcardMenu = new FlashcardMenu();
                flashcardMenu.ViewAllFlashcards(stack);
                Console.ReadKey(); 
            }
        }

        public void ViewStudySessions()
        {
            StudySessionController studySessionController = new StudySessionController();
            var sessions = studySessionController.ViewAllSessions();
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
                    StacksController stackController = new StacksController();
                    var stack = stackController.GetStackById(session.StackId);
                    table.AddRow(
                        stack.StackName,
                        session.SessionId.ToString(),
                        session.Score,
                        session.StudyDate.ToString("yyyy-MM-dd hh:mm")
                    );
                }
                Console.Clear();
                AnsiConsole.Write(table);
                Console.ReadKey();
            }
        }
    }
}
