using Spectre.Console;

namespace FlashCards
{
    /// <summary>
    /// Represents a user interface for CardStackService.
    /// Inherits from UserInterace
    /// Implements ICardStackServiceUi
    /// </summary>
    internal class CardStackServiceUi : UserInterface, ICardStackServiceUi
    {
        /// <inheritdoc/>
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
