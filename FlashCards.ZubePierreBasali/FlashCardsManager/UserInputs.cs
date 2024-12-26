using Spectre.Console;

namespace FlashCards.FlashCardsManager
{
    internal class UserInputs
    {
        public static string GetInputString(string message = "")
        {
            message = Markup.Escape($"{message} (Invalid characters: \\ [ & {{ , )");
            return AnsiConsole.Prompt(new TextPrompt<string>(message)
                .Validate(result => result.IndexOfAny(new char[] {'\\', '[', '{', '&', ',' }) == -1)
                .ValidationErrorMessage("[red bold]Invalid input[/] format, please rewrite project name using in a [blue]valid[/] format"));
        }
        public static bool Validation(string message = "", bool defVal = true, string choice1 = "y", string choice2 = "n")
        {
            return AnsiConsole.Prompt(
                                new TextPrompt<bool>(message)
                                .AddChoice(true)
                                .AddChoice(false)
                                .DefaultValue(defVal)
                                .WithConverter(choice => choice ? choice1 : choice2));
        }
    }
}
