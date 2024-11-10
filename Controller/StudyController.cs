using Flashcards.TwilightSaw.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.TwilightSaw.Controller
{
    internal class StudyController(AppDbContext context)
    {
        public void Create(StudySession session)
        {
            context.Add(session);
            context.SaveChanges();
        }

        public StudySession? StartSession(CardStack stack, FlashcardController flashcardController)
        {
            var points = 0;
            var flashcards = flashcardController.Read(stack.CardStackId);
            if (flashcards.Count == 0)
            {
                AnsiConsole.MarkupLine($"[red]No flashcards in the Stack![/]");
                return null;
            }
            foreach (var item in flashcards)
            {
                    AnsiConsole.MarkupLine($"[yellow]{item.Front}[/]");
                    var userAnswer = UserInput.Create("Write the answer");
                    if (userAnswer == "0") break;
                    var isEqual = string.Equals(userAnswer, item.Back, StringComparison.CurrentCultureIgnoreCase);
                    if (isEqual)
                    {
                        AnsiConsole.MarkupLine("Nice! You got [green]1[/] point!");
                        points++;
                    }
                    else
                        AnsiConsole.MarkupLine($"Hell! Bad answer!\n" +
                                               $"Correct answer is - {item.Back}.");
            }
            AnsiConsole.MarkupLine($"You got {points} right answers out of {flashcards.Count}");
            return new StudySession(DateTime.Now.ToShortDateString(), points, stack.CardStackId);
        }

        public List<StudySession> Read()
        {
           return context.StudySessions.AsNoTracking().ToList();
        }
    }
}
