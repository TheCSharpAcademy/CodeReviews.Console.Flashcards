using FlashCards.Models.Stack;
using Spectre.Console;

namespace FlashCards.Views
{
    public static class StackView
    {
        public static void ShowStacks(IEnumerable<StackDTO> stacks)
        {
            
            foreach (var stack in stacks) 
            {
                var table = new Table();
                table.AddColumn($"{stack.Name}");
                table.Border(TableBorder.DoubleEdge);
                AnsiConsole.Write(table);
            }
            
        }

        
    }
}
