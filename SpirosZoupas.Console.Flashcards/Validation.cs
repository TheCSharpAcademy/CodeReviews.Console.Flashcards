using Flashcards.DAL;
using Flashcards.DAL.DTO;
using Flashcards.DAL.Model;
using Spectre.Console;
using System.Globalization;

namespace Flashcards
{
    public class Validation
    {
        private readonly Controller _controller;
        public Validation(Controller controller)
        {
            _controller = controller;
        }

        public int GetExistingFlashcardID(string message)
        {
            int id = AnsiConsole.Prompt(
                new TextPrompt<int>(message)
                .Validate((n) => _controller.FlashcardIDExists(n) switch
                {
                    true => ValidationResult.Success(),
                    false => ValidationResult.Error("[white on red]Flashcard with that ID does not exist.[/]")
                }));

            return id;
        }

        public string GetExistingStackName(string message)
        {
            string name = AnsiConsole.Prompt(
                new TextPrompt<string>(message)
                .Validate((s) => _controller.StackNameExists(s) switch
                {
                    true => ValidationResult.Success(),
                    false => ValidationResult.Error("[white on red]Stack with that name does not exist.[/]")
                }));

            return name;
        }

        public string GetUniqueStackName(string message)
        {
            string name = AnsiConsole.Prompt(
                new TextPrompt<string>(message)
                .Validate((n) => _controller.StackNameExists(n) switch
                {
                    true => ValidationResult.Error("[white on red]A stack with that name already exists! Stack names must be unique.[/]"),
                    false => ValidationResult.Success()
                }));

            return name;
        }

        public int GetValidatedInteger(string message)
        {
            int number = AnsiConsole.Prompt(
                new TextPrompt<int>(message));

            return number;
        }

        public int GetValidYear()
        {
            int year = AnsiConsole.Prompt(
                new TextPrompt<int>("[darkcyan]Please input a valid year[/]")
                .Validate(input =>
                {
                    return input > 999 && input < 9999;
                }));

            return year;
        }

    }
}
