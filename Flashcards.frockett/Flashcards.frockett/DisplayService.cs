using Spectre.Console;
using Library.Models;

namespace Flashcards.frockett;

internal class DisplayService
{
    public void DisplayAllStacks(List<StackDTO> stacks)
    {
        Table table = new Table();
        table.AddColumn("Stacks");

        foreach (StackDTO stack in stacks)
        {
            table.AddRow(stack.Name.ToString());
        }
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine("Press enter to continue");
        Console.ReadLine();
    }

    public void DisplayCards(List<CardDTO> cards)
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
        AnsiConsole.WriteLine("Press enter to continue...");
        Console.ReadLine();

        /*
        foreach (CardDTO card in cards)
        {
            AnsiConsole.WriteLine($"Q: {card.Question}");
            AnsiConsole.WriteLine($"A: {card.Answer}");
            AnsiConsole.WriteLine();
        }
        */
    }
}
