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

        public double GetValidatedDouble()
        {
            double number = AnsiConsole.Prompt(
                new TextPrompt<double>(string.Empty));

            return number;
        }

        public DateTime GetValidatedDateTimeValue()
        {
            string dateTime = AnsiConsole.Prompt(
                new TextPrompt<string>(string.Empty)
                .Validate(input =>
                {
                    return DateTime.TryParseExact(input, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                        ? ValidationResult.Success()
                        : DateTime.TryParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) 
                        ? ValidationResult.Success() 
                        : ValidationResult.Error($"[white on red]Invalid format. Please enter any DateTime values in dd-MM-yyyy HH:mm:ss foramt. Example: 20-01-2025 13:00:00.[/]");
                }));

            if (DateTime.TryParseExact(dateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }

            result = DateTime.ParseExact(dateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            AnsiConsole.MarkupLine("[yellow3_1]Time of DateTime value not set; Defaulting to 00:00:00.[/]");
            return result.Date;
        }
    }
}
