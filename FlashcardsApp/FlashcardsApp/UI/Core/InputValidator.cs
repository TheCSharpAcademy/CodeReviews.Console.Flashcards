using Spectre.Console;

namespace FlashcardsApp.UI.Core
{
    public class InputValidator
    {
        public string GetStackName()
        {
            while (true)
            {
                Console.Write("\nEnter a name for the stack of flashcards OR 0 to return: ");
                string? name = Console.ReadLine();

                if (name == "0")
                    return string.Empty;

                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("\nName cannot be empty.");
                    continue;
                }

                if (name.Length > 100)
                {
                    Console.WriteLine("\nName cannot be longer than 100 characters.");
                    continue;
                }

                return name;
            }
        }

        public string GetStackDescription()
        {
            Console.Write("\nIf you would like to add a description type it here or press enter: ");
            string? description;

            do
            {
                description = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(description))
                {
                    return "No description provided";
                }

                if (description.Length > 500)
                {
                    Console.WriteLine("\nDescription cannot be longer than 500 characters");
                    Console.Write("\nEnter description again: ");
                }
            } while (description.Length > 500);

            return description;
        }

        public string GetFlashcardContent(string side)
        {
            while (true)
            {
                Console.Write($"\n\nEnter the content for the {side} of the card OR 0 to return: ");
                string? content = Console.ReadLine();

                if (content == "0")
                    return string.Empty;

                if (string.IsNullOrEmpty(content))
                {
                    Console.WriteLine($"\n{side} cannot be empty.");
                    continue;
                }

                if (content.Length > 500)
                {
                    Console.WriteLine($"\n{side} cannot be longer than 500 characters.");
                    continue;
                }

                return content;
            }
        }

        public bool GetConfirmation(string message)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(message)
                    .AddChoices(new[] { "Yes", "No" }));

            return choice == "Yes";
        }
    }
}

