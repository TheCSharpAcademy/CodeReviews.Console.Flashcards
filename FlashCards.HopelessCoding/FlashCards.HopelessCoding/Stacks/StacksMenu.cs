using DbHelpers.HopelessCoding;
using HelperMethods.HopelessCoding;
using Spectre.Console;
using Stacks.HopelessCoding;
using FlashCards.HopelessCoding.DTOs;
using Flashcards.HopelessCoding;

namespace FlashCards.HopelessCoding.Stacks;

internal class StacksMenu
{
    internal static void StacksMainMenu()
    {
        while (true)
        {
            Console.Clear();

            AnsiConsole.Write(new Markup("[yellow1]Manage Stacks\n\n[/]"));
            var selection = Helpers.MenuSelector(["Manage existing stack", "Create a new stack", "Return to main menu"]);

            switch (selection)
            {
                case "Manage existing stack":
                    StacksMenu.SelectStackMenu();
                    return;
                case "Create a new stack":
                    StackCommands.CreateNewStack();
                    return;
                case "Return to main menu":
                    return;
            }
        }
    }

    internal static void SelectStackMenu()
    {
        Console.Clear();

        StackService stackService = new StackService(DatabaseHelpers.connectionString);
        stackService.PrintAllStacks();

        Console.WriteLine("\n-------------------\nInput a stack name which you want to manage or input 0 to return main menu");

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
                StacksCommandsMenu(input);
                break;
            }
            else
            {
                AnsiConsole.Write(new Markup($"[red]Invalid input, try again.[/]\n"));
            }
        }
    }

    private static void StacksCommandsMenu(string stackName)
    {
        Console.Clear();

        AnsiConsole.Write(new Markup($"[yellow1]Current working stack: {stackName}\n\n[/]"));
        var selection = Helpers.MenuSelector(["Change working stack", "Edit stack", "Delete stack", "View all flashcards in stack", "Return to main menu"]);

        switch (selection)
        {
            case "Change working stack":
                SelectStackMenu();
                return;
            case "Edit stack":
                StackCommands.EditStack(stackName);
                return;
            case "Delete stack":
                StackCommands.DeleteStack(stackName);
                return;
            case "View all flashcards in stack":
                FlashcardCommands.ViewFlashcards(stackName, null);
                Helpers.WaitForUserInput();
                return;
            case "Return to main menu":
                return;
        }
    }
}