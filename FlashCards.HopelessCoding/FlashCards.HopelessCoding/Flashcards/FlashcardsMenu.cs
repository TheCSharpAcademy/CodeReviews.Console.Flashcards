using DbHelpers.HopelessCoding;
using Flashcards.HopelessCoding;
using FlashCards.HopelessCoding.DTOs;
using HelperMethods.HopelessCoding;
using Spectre.Console;

namespace FlashCards.HopelessCoding.Flashcards;

internal class FlashcardsMenu
{
    private static void FlashcardCommandsMenu(string stackName)
    {
        Console.Clear();

        AnsiConsole.Write(new Markup($"[yellow1]Current working stack: {stackName}\n\n[/]"));
        var selection = Helpers.MenuSelector(["Change working stack", "View all flashcards in stack", "View X amount of flashcards in stack",
                                              "Create a flashcard in current stack", "Edit a flashcard", "Delete a flashcard", "Return to main menu"]);

        switch (selection)
        {
            case "Change working stack":
                SelectStackForFlashcardsMenu();
                return;
            case "View all flashcards in stack":
                FlashcardCommands.ViewFlashcards(stackName, null);
                Helpers.WaitForUserInput();
                return;
            case "View X amount of flashcards in stack":
                Console.Write("How many flashcards you want to see: ");
                int amount = Helpers.ValidateIntegerInput();
                FlashcardCommands.ViewFlashcards(stackName, amount);
                Helpers.WaitForUserInput();
                return;
            case "Create a flashcard in current stack":
                FlashcardCommands.CreateNewFlashcard(stackName);
                return;
            case "Edit a flashcard":
                FlashcardCommands.EditFlashcard(stackName);
                return;
            case "Delete a flashcard":
                FlashcardCommands.DeleteFlashcard(stackName);
                return;
            case "Return to main menu":
                return;
        }
    }

    internal static void SelectStackForFlashcardsMenu()
    {
        Console.Clear();
        StackService stackService = new StackService(DatabaseHelpers.connectionString);
        stackService.PrintAllStacks();

        Console.WriteLine("\n-------------------\nInput a stack of flashcards name which you want to interact with or input 0 to return main menu");

        while (true)
        {
            Console.WriteLine("-------------------");

            var input = Console.ReadLine();

            if (input == "0")
            {
                Console.Clear();
                return;
            }

            if (DatabaseHelpers.StackExists(input))
            {
                FlashcardCommandsMenu(input);
                break;
            }
            else
            {
                AnsiConsole.Write(new Markup($"[red]Invalid input, try again.[/]\n"));
            }
        }
    }
}