using Flashcards.TwilightSaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public StudySession StartSession(CardStack stack, FlashcardController flashcardController)
        {
            var points = 0;
            var flashcards = flashcardController.Read(stack.CardStackId);
            foreach (var item in flashcards)
            {
                var end = false;
                while (!end)
                {
                    AnsiConsole.MarkupLine($"[yellow]{item.Front}[/]");
                    var userAnswer = UserInput.Create("Write the answer: ");
                    end = userAnswer == item.Back;
                    if (end)
                    {
                        AnsiConsole.MarkupLine("Nice! You got [green]1[/] point!");
                        points++;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("Hell! You've lost [red]1[/] point!");
                        points--;
                    }

                    end = true;
                }
            }

            return new StudySession(DateTime.Now.ToShortDateString(), points, stack.CardStackId);
        }

        public List<StudySession> Read()
        {
           return context.StudySessions.AsNoTracking().ToList();
        }
    }
}
