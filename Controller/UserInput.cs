using System.Text.RegularExpressions;
using Flashcards.TwilightSaw.Models;
using Spectre.Console;

namespace Flashcards.TwilightSaw.Controller
{
    static class UserInput
    {
        public static int CreateSpecifiedInt(int bound, string message)
        {
            int inputInt;
            var input = Console.ReadLine();
            while (!int.TryParse(input, out inputInt))
            {
                Console.Write(message);
                input = Console.ReadLine();
            }
            while (int.Parse(input) > bound || int.Parse(input) < 1)
            {
                Console.Write(message);
                input = Console.ReadLine();
                inputInt = int.Parse(input);
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

        public static Flashcard ChooseFlashcard(List<Flashcard> flashcards)
        {
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<Flashcard>()
                    .Title("[blue]Please, choose an option from the list below:[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more categories[/]")
                    .AddChoices(flashcards));
            return input;
        }

        public static string CreateChoosingList(string[] variants)
        {
            var select = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Please, choose an option from the list below:[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more categories[/]")
                    .AddChoices(variants));
            return select;
        }

        public static Table CreateFlashcardTable(List<Flashcard> flashcards)
        {
            var table = new Table();
            table.AddColumn("Number")
                .AddColumn("Front")
                .AddColumn("Back")
                .Centered();
            
            for (var index = 0; index < flashcards.Count; index++)
            {
                var flashcard = flashcards[index];
                table.AddRow(@$"{index + 1}",
                    $"{FlashcardMapper.ConvertToDto(flashcard).Front}",
                    $"{FlashcardMapper.ConvertToDto(flashcard).Back}");
            }

            return table;
        }
    }
}
