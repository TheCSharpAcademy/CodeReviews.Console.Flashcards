using Flashcards.Repository;
using Flashcards.Views;
using Spectre.Console;
using System.Diagnostics;

namespace Flashcards.Services
{
    public class StudySessionService
    {
        private DatabaseContext _context;
        public StudySessionService(DatabaseContext context)
        {
            _context = context;
        }

        public void SelectOperation(int choice)
        {
            var studyRepo = new StudySessionRepository(_context);
            var flashcardView = new FlashcardView(_context);

            UserInput userInput = new UserInput();
            if (choice == 3)
            {
                string stackName = flashcardView.GetStackName();
                var stackRepo = new StackRepository(_context);
                int stackId = stackRepo.GetStackId(stackName);
                var cardRepo = new FlashcardRepository(_context, stackId);

                DateTime startDate = DateTime.Now;
                Stopwatch timer = new Stopwatch();
                int score = 0;
                string duration;

                //Get all flashcard from repo
                var cards = cardRepo.GetAllCards();
                if(cards.Count == 0)
                {
                    AnsiConsole.Markup("[blue]Press enter to continue[/]");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }
                Console.Clear();
                Console.WriteLine("Starting Study Session: \n\n");

                //Extract question and answer from database;
                timer.Start();
                foreach (var card in cards) { 
                    Console.Write(card.Question + ": " );
                    string ans = userInput.GetText();
                    if(ans.ToLower().Trim() == card.Answer.ToLower().Trim())
                    {
                        AnsiConsole.Markup("[blue]Correct answer[/]\n\n");
                        score++;
                    }
                    else
                    {
                        AnsiConsole.Markup("[red]Wrong answer[/]\n");
                        AnsiConsole.Markup($"[green]Correct answer: {card.Answer}[/]\n");
                    }

                    Console.Write("Press 1 to continue: ");
                    int num = userInput.GetInt();
                    if (num != 1)
                    {
                        timer.Stop();
                        duration = timer.Elapsed.ToString();
                        studyRepo.Insert(stackId, stackName, score, startDate, duration);
                        AnsiConsole.Markup("\n[red]Returning to Main Menu...[/]");
                        Thread.Sleep(1000); 
                        Console.Clear();
                        return;
                    }
                    Console.WriteLine();
                }
                timer.Stop();
                Console.WriteLine("Yayy! You completed the Stack");

                duration = timer.Elapsed.ToString();
                studyRepo.Insert(stackId, stackName, score, startDate, duration);
            }

            else if(choice == 4)
            {
                Console.Write("Enter Stack Name: ");
                string stackName = userInput.GetText();
                studyRepo.GetSession(stackName);
            }

            else if(choice == 5)
            {
                Console.Write("Enter year: ");
                int year = userInput.GetInt();
                studyRepo.GetReport(year);
            }
            AnsiConsole.Markup("\n[blue]Press enter to continue....[/]");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
