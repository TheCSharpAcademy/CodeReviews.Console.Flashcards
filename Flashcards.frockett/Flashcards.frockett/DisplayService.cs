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
}
