using Flashcards.TwilightSaw.Helpers;
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
            context.SaveChanges();
            AnsiConsole.Markup($"[red]{Validation.Validate(() => context.SaveChanges())}[/]");
        }

        public List<CardStack>? Read()
        {
            var stacks = context.CardStacks.AsNoTracking().ToList();
            return stacks.Count != 0 ? stacks : null;
        }

        public void Delete(CardStack inputStackDelete)
        {
            context.CardStacks.Where(s => s.Equals(inputStackDelete)).ExecuteDelete();
            context.SaveChanges();
        }

    }
}
