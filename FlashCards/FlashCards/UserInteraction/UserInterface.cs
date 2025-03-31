using Spectre.Console;
using System.Text.RegularExpressions;

namespace FlashCards
{
    /// <summary>
    /// Represents a user interface which specific interfaces inherit.
    /// Implements IUserInterface
    /// </summary>
    internal class UserInterface : IUserInterface
    {
        /// <summary>
        /// A Regex representing valid string for the user input
        /// </summary>
        private Regex validString = new Regex("^[A-Za-z0-9]+(?: [A-Za-z0-9]+)*$");

        /// <inheritdoc/>
        public void ClearConsole()
        {
            Console.Clear();
            PrintApplicationHeader();
        }

        /// <inheritdoc/>
        public void PrintApplicationHeader()
        {
            Console.WriteLine("Welcome to Flash card application");
            Console.WriteLine(new string('-', 50));
        }

        /// <inheritdoc/>
        public void PrintPressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <inheritdoc/>
        public int GetNumberFromUser(string prompt)
        {
            var count = AnsiConsole.Prompt(
                new TextPrompt<int>(prompt)
                .Validate(x => x > 0)
                );

            return count;
        }

        /// <inheritdoc/>
        public string GetStringFromUser(string prompt)
        {
            var text = AnsiConsole.Prompt(
                new TextPrompt<string>(prompt)
                .AllowEmpty()
                .Validate(x => validString.IsMatch(x))
                );

            return text;
        }

        /// <inheritdoc/>
        public CardStack StackSelection(List<CardStack> stacks)
        {
            var stack = AnsiConsole.Prompt(
                new SelectionPrompt<CardStack>()
                .WrapAround()
                .AddChoices(stacks)
                .UseConverter(x => x.StackName)

                );
            return stack;
        }
    }
}