using Flashcards.RyanW84.Models;
using Spectre.Console;
using static Flashcards.RyanW84.Enums;

namespace Flashcards.RyanW84;

internal class UserInterface
{
    internal static void MainMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuChoices>()
                    .Title("What would you like to do?")
                    .AddChoices(
                        MainMenuChoices.ManageStacks,
                        MainMenuChoices.ManageFlashcards,
                        MainMenuChoices.Quit
                    )
            );

            switch (usersChoice)
            {
                case MainMenuChoices.ManageStacks:
                    StacksMenu();
                    break;
                case MainMenuChoices.ManageFlashcards:
                    FlashcardsMenu();
                    break;
                case MainMenuChoices.Quit:
                    System.Console.WriteLine("Goodbye");
                    isMenuRunning = false;
                    break;
            }
        }
    }

    internal static void StacksMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<StackChoices>()
                    .Title("What would you like to do?")
                    .AddChoices(
                        StackChoices.ViewStacks,
                        StackChoices.AddStack,
                        StackChoices.UpdateStack,
                        StackChoices.DeleteStack,
                        StackChoices.ReturnToMainMenu
                    )
            );

            switch (usersChoice)
            {
                case StackChoices.ViewStacks:
                    ViewStacks();
                    break;
                case StackChoices.AddStack:
                    AddStack();
                    break;
                case StackChoices.DeleteStack:
                    DeleteStack();
                    break;
                case StackChoices.UpdateStack:
                    UpdateStack();
                    break;
                case StackChoices.ReturnToMainMenu:
                    isMenuRunning = false;
                    break;
            }
        }
    }

    private static void UpdateStack()
    {
        throw new NotImplementedException();
    }

    private static void DeleteStack()
    {
        throw new NotImplementedException();
    }

    private static void AddStack()
    {
        Stack stack = new();

        stack.Name = AnsiConsole.Ask<string>("Insert Stack's Name.");

        while (string.IsNullOrEmpty(stack.Name))
        {
            stack.Name = AnsiConsole.Ask<string>("Stack's name can't be empty. Try again.");
        }

        var dataAccess = new DataAccess();
        dataAccess.InsertStack(stack);
    }

    private static int ChooseStack()
    {
        var dataAccess = new DataAccess();
        var stacks = dataAccess.GetAllStacks();

        var stacksArray = stacks.Select(x => x.Name).ToArray();
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("Choose Stack").AddChoices(stacksArray)
        );

        var stackId = stacks.Single(x => x.Name == option).Id;

        return stackId;
    }

    private static void ViewStacks()
    {
        throw new NotImplementedException();
    }

    internal static void FlashcardsMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<FlashcardChoices>()
                    .Title("What would you like to do?")
                    .AddChoices(
                        FlashcardChoices.ViewFlashcards,
                        FlashcardChoices.AddFlashcard,
                        FlashcardChoices.UpdateFlashcard,
                        FlashcardChoices.DeleteFlashcard,
                        FlashcardChoices.ReturnToMainMenu
                    )
            );

            switch (usersChoice)
            {
                case FlashcardChoices.ViewFlashcards:
                    ViewFlashcards();
                    break;
                case FlashcardChoices.AddFlashcard:
                    AddFlashcard();
                    break;
                case FlashcardChoices.DeleteFlashcard:
                    DeleteFlashcard();
                    break;
                case FlashcardChoices.UpdateFlashcard:
                    UpdateFlashcard();
                    break;
                case FlashcardChoices.ReturnToMainMenu:
                    isMenuRunning = false;
                    break;
            }
        }
    }

    private static void UpdateFlashcard()
    {
        throw new NotImplementedException();
    }

    private static void DeleteFlashcard()
    {
        throw new NotImplementedException();
    }

    private static void AddFlashcard()
    {
        Flashcard flashcard = new();

        flashcard.StackId = ChooseStack();
        flashcard.Question = AnsiConsole.Ask<string>("Insert Question.");

        while (string.IsNullOrEmpty(flashcard.Question))
        {
            flashcard.Question = AnsiConsole.Ask<string>("Question can't be empty. Try again.");
        }

        flashcard.Answer = AnsiConsole.Ask<string>("Insert Answer.");

        while (string.IsNullOrEmpty(flashcard.Answer))
        {
            flashcard.Answer = AnsiConsole.Ask<string>("Answer can't be empty. Try again.");
        }

        var dataAccess = new DataAccess();
        dataAccess.InsertFlashcard(flashcard);
    }

    private static void ViewFlashcards()
    {
        throw new NotImplementedException();
    }
}
