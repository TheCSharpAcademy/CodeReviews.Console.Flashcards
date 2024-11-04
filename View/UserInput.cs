using System.Text.RegularExpressions;
using Flashcards.TwilightSaw.Models;
using Spectre.Console;

namespace Flashcards.TwilightSaw.View
{
    static class UserInput
    {
        public static int CreateSpecifiedInt(int bound, string message)
            {
                int inputInt;
                var input = Console.ReadLine();
                while (!Int32.TryParse(input, out inputInt))
                {
                    Console.Write(message);
                    input = Console.ReadLine();
                }
                while (int.Parse(input) > bound || int.Parse(input) < 1)
                {
                    Console.Write(message);
                    input = Console.ReadLine();
                    inputInt = Int32.Parse(input);
                }
                return inputInt;
            }

            public static string CreateRegex(string regexString, string messageStart, string messageError)
            {
                Regex regex = new Regex(regexString);
                var input = AnsiConsole.Prompt(
                    new TextPrompt<string>($"[green]{messageStart}[/]")
                        .Validate(value => regex.IsMatch(value)
                            ? ValidationResult.Success()
                            : ValidationResult.Error(messageError)));

                return input;
            }

            public static string Create(string messageStart)
            {
                var input = AnsiConsole.Prompt(
                    new TextPrompt<string>($"[green]{messageStart}[/]"));
                return input;
            }

            public static CardStack ChooseStack(List<CardStack> stack)
            {
                var input = AnsiConsole.Prompt(
                    new SelectionPrompt<CardStack>()
                        .Title("[blue]Please, choose an option from the list below:[/]")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more categories[/]")
                        .AddChoices(stack));
                return input;
            }
    }
}
