using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace FlashCards
{
    internal class CardStackServiceUi : UserInterface, ICardStackServiceUi
    {

        public void PrintStacks(List<CardStack> stacks)
        {
            var table = new Table();
            table.AddColumn("Stacks");

            foreach (var stack in stacks)
            {
                table.AddRow(stack.StackName);
            }

            AnsiConsole.Write(table);

        }
    }
}
