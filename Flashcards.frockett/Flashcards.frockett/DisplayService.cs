using Spectre.Console;
using Library.Models;

namespace Flashcards.frockett;

internal class DisplayService
{
    public void DisplayAllStacks(List<StackDTO> stacks, bool shouldWait = true)
    {
        Table table = new Table();
        table.AddColumn("Stacks");

        foreach (StackDTO stack in stacks)
        {
            table.AddRow(stack.Name.ToString());
        }
        AnsiConsole.Write(table);

        if (shouldWait)
        {
            AnsiConsole.WriteLine("Press enter to continue");
            Console.ReadLine();
        }

    }

    public void DisplayCards(List<CardDTO> cards, bool shouldWait = true)
    {
        AnsiConsole.Clear();
        Table table = new Table();

        table.AddColumns(new[]
        {
            "Number", "Question", "Answer"
        });

        foreach (CardDTO card in cards)
        {
            int index = cards.IndexOf(card) + 1;
            table.AddRow(index.ToString(), card.Question.ToString(), card.Answer.ToString());
        }
        AnsiConsole.Write(table);

        if (shouldWait) 
        {
            AnsiConsole.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }


    }
}
