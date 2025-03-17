using System.ComponentModel.DataAnnotations;

using Flashcards.RyanW84.Models;
using Spectre.Console;
using static Flashcards.RyanW84.Enums;

namespace Flashcards.RyanW84;

internal class UserInterface
{
    //Menu Methods
    private static string GetEnumDisplayName(Enum enumValue) //Enums weren't showing their display name, this fixes it
        {
        var displayAttribute =
            enumValue
                .GetType()
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() as DisplayAttribute;

        return displayAttribute != null ? displayAttribute.Name : enumValue.ToString();
        }

    internal static void MainMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
            {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuChoices>()
                    .Title("What would you like to do?")
                     .AddChoices(Enum.GetValues(typeof(MainMenuChoices)).Cast<MainMenuChoices>())
                    .UseConverter(choice => GetEnumDisplayName(choice))

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
                     .AddChoices(Enum.GetValues(typeof(StackChoices)).Cast<StackChoices>())
                    .UseConverter(choice => GetEnumDisplayName(choice))

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

    internal static void FlashcardsMenu()
        {
        var isMenuRunning = true;

        while (isMenuRunning)
            {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<FlashcardChoices>()
                    .Title("What would you like to do?")
                     .AddChoices(Enum.GetValues(typeof(FlashcardChoices)).Cast<FlashcardChoices>())
                    .UseConverter(choice => GetEnumDisplayName(choice))

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

    //Stacks Methods
    private static int ChooseStack(string message)
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

    private static void DeleteStack()
        {
        var id = ChooseStack("Choose stack to delete: ");

        if (!AnsiConsole.Confirm("Are you sure?"))
            return;

        var dataAccess = new DataAccess();
        dataAccess.DeleteStack(id);
        }

    private static void UpdateStack()
        {
        var stack = new Stack();

        stack.Id = ChooseStack("Choose stack to update");
        stack.Name = AnsiConsole.Ask<string>("Insert Stack's Name.");

        var dataAccess = new DataAccess();
        dataAccess.UpdateStack(stack);
        }

    //Flashcards Methods
    private static int ChooseFlashcard(string message, int stackId)
        {
        var dataAccess = new DataAccess();
        var flashcards = dataAccess.GetFlashcards(stackId);

        var flashcardsArray = flashcards.Select(x => x.Question).ToArray();
        var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title(message)
            .AddChoices(flashcardsArray));

        var flashcardId = flashcards.Single(x => x.Question == option).Id;

        return flashcardId;
        }

    private static void ViewFlashcards()
        {
        throw new NotImplementedException();
        }

    private static void AddFlashcard()
        {
        Flashcard flashcard = new();

        flashcard.StackId = ChooseStack("Choose a stack for the new flashcard: ");
        flashcard.Question = GetQuestion();
        flashcard.Answer = GetAnswer();

        var dataAccess = new DataAccess();
        dataAccess.InsertFlashcard(flashcard);
        }

    private static void DeleteFlashcard()
        {
        var stackId = ChooseStack("Where's the flashcard you want to delete?");
        var flashcard = ChooseFlashcard("Choose flashcard to delete", stackId);

        if (!AnsiConsole.Confirm("Are you sure?"))
            return;

        var dataAccess = new DataAccess();
        dataAccess.DeleteFlashcard(flashcard);
        }

    private static string GetQuestion()
        {
        var question = AnsiConsole.Ask<string>("Insert Question.");

        while (string.IsNullOrEmpty(question))
            {
            question = AnsiConsole.Ask<string>("Question can't be empty. Try again.");
            }

        return question;
        }

    private static string GetAnswer()
        {
        var answer = AnsiConsole.Ask<string>("Insert answer.");

        while (string.IsNullOrEmpty(answer))
            {
            answer = AnsiConsole.Ask<string>("Answer can't be empty. Try again.");
            }

        return answer;
        }

    private static void UpdateFlashcard()
        {
        var stackId = ChooseStack("Choose stack where flashcard is:");
        var flashcardId = ChooseFlashcard("Choose flashcard to update", stackId);

        var propertiesToUpdate = new Dictionary<string, object>();

        if (AnsiConsole.Confirm("Would you like to update question?"))
            {
            var question = GetQuestion();
            propertiesToUpdate.Add("Question", question);
            }

        if (AnsiConsole.Confirm("Would you like to update answer?"))
            {
            var answer = GetAnswer();
            propertiesToUpdate.Add("Answer", answer);
            }

        if (AnsiConsole.Confirm("Would you like to update stack?"))
            {
            var stack = ChooseStack("Choose new stack for flashcard");

            propertiesToUpdate.Add("StackId", stack);
            }

        var dataAccess = new DataAccess();
        dataAccess.UpdateFlashcard(flashcardId, propertiesToUpdate);
        }

    }
