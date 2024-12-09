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
            var cardStack = new CardStack(name);
            context.Add(cardStack);
            try
            {
                context.SaveChanges();
                Validation.EndMessage("Created successfully.");
            }
            catch(DbUpdateException e)
            {
                AnsiConsole.MarkupLine("[red]This CardStack has been already created.[/]");
                context.Entry(cardStack).State = EntityState.Detached;
            }
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
