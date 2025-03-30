using Spectre.Console;
using System.Text.RegularExpressions;

namespace FlashCards
{
    internal class UserInterface : IUserInterface
    {

        private Regex validString = new Regex("^[A-Za-z0-9]+(?: [A-Za-z0-9]+)*$");
        public void ClearConsole()
        {
            Console.Clear();
            PrintApplicationHeader();
        }
        public void PrintApplicationHeader()
        {
            Console.WriteLine("Welcome to Flash card application");
            Console.WriteLine(new string('-', 50));
        }
        public void PrintPressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public int GetNumberFromUser(string prompt)
        {
            var count = AnsiConsole.Prompt(
                new TextPrompt<int>(prompt)
                .Validate(x => x > 0)
                );

            return count;

        }
        public string GetStringFromUser(string prompt)
        {
            var text = AnsiConsole.Prompt(
                new TextPrompt<string>(prompt)
                .AllowEmpty()
                .Validate(x => validString.IsMatch(x))
                );

            return text;
        }
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
