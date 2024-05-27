using Flashcards.Database;
using Flashcards.Helpers;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Menus;

internal class StackMenuUI
{
    private StackDatabaseManager stackDatabaseManager = new();

    internal void StackMenu()
    {
        Console.Clear();
        var mainMenu = new MainMenuUI();
        bool isRunning = true;

        while (isRunning)
        {
            var select = new SelectionPrompt<string>();
            select.Title("   [bold]STACKS MENU[/]\n");
            select.AddChoice("Go back to main menu");
            select.AddChoice("Add a stack");
            select.AddChoice("View stacks");
            select.AddChoice("Delete stacks");
            select.AddChoice("Update stacks");

            var userInput = AnsiConsole.Prompt(select);

            switch (userInput)
            {
                case "Go back to main menu":
                    mainMenu.MainMenu();
                    break;

                case "Add a stack":
                    AddStack();
                    break;

                case "View stacks":
                    ReadStack();
                    break;

                case "Delete stacks":
                    DeleteStack();
                    break;

                case "Update stacks":
                    EditStack();
                    break;
            }
        }
    }

    private void AddStack()
    {
        Console.Clear();
        var cardStack = new CardStack();

        var stack = AnsiConsole.Prompt(new TextPrompt<string>("Please enter the name of the stack you want to add or type 0 to go back to main menu").Validate(stackName => !ValidateFlashcardsStacks.StackExists(stackName.Trim()), "This stack already exists"));
        if (stack == "0") StackMenu();
        cardStack.CardstackName = stack;

        stackDatabaseManager.InsertStack(cardStack);
    }

    internal void ReadStack()
    {
        Console.Clear();
        var cardStacks = stackDatabaseManager.GetStacks();

        if (cardStacks.Any())
        {
            Console.WriteLine("  List of stacks:\n");
            foreach (var card in cardStacks)
            {
                AnsiConsole.Write(new Rows(
         new Text($"   {card.CardstackName}\n")));
            }
        }
        else
        {
            Console.WriteLine("No stacks found.");
        }
    }

    internal void EditStack()
    {
        Console.Clear();
        var stacks = stackDatabaseManager.GetStacks();
        var select = new SelectionPrompt<CardStack>();
        select.Title("  Select a stack you want to edit");
        select.AddChoices(stacks);
        select.AddChoice(new CardStack { CardstackName = "Go back to menu" });
        select.UseConverter(stackName => stackName.CardstackName);
        var selectedStack = AnsiConsole.Prompt(select);

        var stackName = AnsiConsole.Prompt(new TextPrompt<string>($"Please enter the updated name").Validate(name => !ValidateFlashcardsStacks.StackExists(name), "This stack already exists please enter another stack"));

        stackDatabaseManager.UpdateStack(selectedStack, stackName);
    }

    private void DeleteStack()
    {
        Console.Clear();
        var stacks = stackDatabaseManager.GetStacks();
        var select = new SelectionPrompt<CardStack>();
        select.Title("Select which stack you want to delete");
        select.AddChoices(stacks);
        select.AddChoice(new CardStack { CardstackName = "Return to stack menu" });
        select.UseConverter(stackName => stackName.CardstackName);
        var stackSelected = AnsiConsole.Prompt(select);

        if (stackSelected.CardstackName == "Return to stack menu") StackMenu();
        if (AnsiConsole.Confirm($"Are you sure that you want to delete the stack with name [bold]{stackSelected.CardstackName}[/]\n"))
        {
            AnsiConsole.MarkupLine($" [bold]{stackSelected.CardstackName.ToUpper()}[/] was deleted\n\n");
        }

        stackDatabaseManager.DeleteStack(stackSelected);
    }
}