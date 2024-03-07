using Flashcards.Dejmenek.Enums;
using Flashcards.Dejmenek.Models;
using Spectre.Console;

namespace Flashcards.Dejmenek.Services
{
    public class UserInteractionService
    {
        public int GetId()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>("Enter id: ")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That is not a valid id[/]")
                    .Validate(Validation.IsPositiveNumber)
                );
        }

        public string GetFlashcardFront()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter the front of the flashcard:")
                );
        }

        public string GetFlashcardBack()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter the back of the flashcard")
                );
        }

        public string GetStackName()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter the stack name")
                ); ;
        }

        public string GetAnswer()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your answer to this card")
                );
        }

        public string GetYear()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter year: ")
                    .ValidationErrorMessage("This is not a valid year!")
                    .Validate(Validation.IsValidYear)
                );
        }

        public string GetFlashcard(List<FlashcardDTO> flashcards)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose your flashcard")
                    .AddChoices(flashcards.Select(f => f.Front))
                );
        }

        public string GetStack(List<StackDTO> stacks)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose your stack")
                    .AddChoices(stacks.Select(f => f.Name))
                );
        }

        public MenuOptions GetMenuOption()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                    .Title("What would you like to do?")
                    .AddChoices(Enum.GetValues<MenuOptions>())
                );
        }

        public ManageStackOptions GetManageStackOption(string currentStack)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<ManageStackOptions>()
                    .Title($"Current working stack: {currentStack}")
                    .AddChoices(Enum.GetValues<ManageStackOptions>())
                );
        }

        public ManageFlashcardsOptions GetManageFlashcardsOption()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<ManageFlashcardsOptions>()
                    .Title($"What would you like to do with flashcards?")
                    .AddChoices(Enum.GetValues<ManageFlashcardsOptions>())
                );
        }

        public void GetUserInputToContinue()
        {
            AnsiConsole.MarkupLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
