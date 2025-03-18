using Spectre.Console;

namespace FlashCards
{
    internal class UserInterface
    {

        public void PrintMainMenu()
        {
            var mainMenuInput = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .WrapAround()
                .AddChoices(s)
                ));

            Console.WriteLine(mainMenuInput);
        }
    }
}
