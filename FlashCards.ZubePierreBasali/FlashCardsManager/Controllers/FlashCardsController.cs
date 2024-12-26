using FlashCards.Database;
using FlashCards.FlashCardsManager.Models;
using Spectre.Console;

namespace FlashCards.FlashCardsManager.Controllers
{
    internal class FlashCardsController
    {
        internal Stacks GetStack(DataTools dataTools)
        {
                List<Stacks> stacks = dataTools.GetStacks();
                return AnsiConsole.Prompt(new SelectionPrompt<Stacks>()
                    .AddChoices(new Stacks{ Id = 0,Name = "[red]Cancel[/]",NumberOfCards = 0})
                    .AddChoices(stacks)
                    .UseConverter(x => $"Id: {x.Id} - Subject: {x.Name} - Cards: {x.NumberOfCards}"));
        }
        public FlashCard GetFlashCard(string stack,DataTools dataTools)
        {
                List<FlashCard> flashCards = dataTools.GetFlashCards(stack);
                return AnsiConsole.Prompt(new SelectionPrompt<FlashCard>()
                    .Title(stack)
                    .AddChoices(new FlashCard { Id = 0, Question = "[red]Cancel[/]", Answer = ""})
                    .AddChoices(flashCards)
                    .UseConverter(x => $"Id: {x.Id} - Question: {x.Question} - Answer: {x.Answer}")
                    );
        }
    }
}
