using Flashcards.TwilightSaw.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.TwilightSaw.Controller
{
    public class CardStackController(AppDbContext context)
    {
        public void Create(string name)
        {
            context.Add(new CardStack(name));
            AnsiConsole.Markup($"[red]{Validation.Validate(() => context.SaveChanges())}[/]");
            Console.ReadKey();
        }

        public List<CardStack> Read()
        {
            return context.CardStacks.ToList();
        }

        public void Delete(CardStack inputStackDelete)
        {
            context.CardStacks.Where(s => s.Equals(inputStackDelete)).ExecuteDelete();
            context.SaveChanges();
        }
    }
}
