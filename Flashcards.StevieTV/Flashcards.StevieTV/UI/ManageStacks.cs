using Flashcards.StevieTV.Helpers;
using Flashcards.StevieTV.Models;
using Flashcards.StevieTV.Database;
using Spectre.Console;

namespace Flashcards.StevieTV.UI;

internal static class ManageStacks
{
    public static void StacksMenu()
    {
        var exitManageStacks = false;

        while (!exitManageStacks)
        {
            Console.Clear();
            AnsiConsole.WriteLine("These are your current stacks:");
            PrintCurrentStackNames();

            var menuSelection = new SelectionPrompt<string>();
            menuSelection.Title("Please choose an option from the list below");
            menuSelection.AddChoice("Return to Main Menu");
            menuSelection.AddChoice("Add a Stack");
            menuSelection.AddChoice("Remove a Stack");
            menuSelection.AddChoice("Rename a Stack");
            menuSelection.AddChoice("Edit Flash Cards in a Stack");

            var menuInput = AnsiConsole.Prompt(menuSelection);

            switch (menuInput)
            {
                case "Return to Main Menu":
                    exitManageStacks = true;
                    break;
                case "Add a Stack":
                    AddStack();
                    break;
                case "Remove a Stack":
                    RemoveStack();
                    break;
                case "Rename a Stack":
                    RenameStack();
                    break;
                case "Edit Flash Cards in a Stack":
                    EditStack();
                    break;
            }
        }
    }

    private static void PrintCurrentStackNames()
    {
        var stacks = StacksDatabaseManager.GetStacks();

        var table = new Table()
        {
            Border = TableBorder.Rounded
        };
        table.AddColumn("Stack Name");

        foreach (var stack in stacks)
        {
            table.AddRow(stack.Name);
        }

        AnsiConsole.Write(table);
    }

    private static void AddStack()
    {
        var newStackName = AnsiConsole.Prompt(
            new TextPrompt<string>("Please enter the name of the Stack you would like to add (or press 0 to cancel):")
                .Validate(name => !CheckStackExists(name.Trim()), "That stack already exists")
        );

        if (newStackName.Trim() == "0") return;

        StacksDatabaseManager.Post(new StackDTO
        {
            Name = newStackName.ToTitleCase()
        });
    }
       
    private static void RemoveStack()
    {
        var stacks = StacksDatabaseManager.GetStacks();

        Console.Clear();

        var menuSelection = new SelectionPrompt<Stack>();
        menuSelection.Title("Which stack would you like to remove");
        menuSelection.AddChoices(stacks);
        menuSelection.AddChoice(new Stack {StackId = 0, Name = "Cancel and return to menu"});
        menuSelection.UseConverter(stack => stack.Name);

        var selectedStack = AnsiConsole.Prompt(menuSelection);

        if (selectedStack.StackId == 0) return;

        if (AnsiConsole.Confirm($"This will remove the stack '{selectedStack.Name.ToTitleCase()}' and all associated Flash Cards"))
        {
            StacksDatabaseManager.Delete(selectedStack);
        }
    }
    
    private static void RenameStack()
    {
        var stacks = StacksDatabaseManager.GetStacks();

        Console.Clear();

        var menuSelection = new SelectionPrompt<Stack>();
        menuSelection.Title("Which stack would you like to rename");
        menuSelection.AddChoices(stacks);
        menuSelection.AddChoice(new Stack {StackId = 0, Name = "Cancel and return to menu"});
        menuSelection.UseConverter(stack => stack.Name);

        var selectedStack = AnsiConsole.Prompt(menuSelection);

        if (selectedStack.StackId == 0) return;
        
        var newStackName = AnsiConsole.Prompt(
            new TextPrompt<string>($"Please enter the new name for the {selectedStack.Name} stack (or press 0 to cancel):")
                .Validate(name => !CheckStackExists(name.Trim()), "That stack already exists")
        );

        if (newStackName.Trim() == "0") return;

        if (AnsiConsole.Confirm($"This will rename the stack '{selectedStack.Name.ToTitleCase()}' to '{newStackName.ToTitleCase()}'"))
        {
            StacksDatabaseManager.Update(selectedStack, newStackName.ToTitleCase());
            
        }
    }
 
    public static void EditStack()
    {
        var stacks = StacksDatabaseManager.GetStacks();

        Console.Clear();

        var menuSelection = new SelectionPrompt<Stack>();
        menuSelection.Title("Which stack would you like to edit");
        menuSelection.AddChoices(stacks);
        menuSelection.AddChoice(new Stack {StackId = 0, Name = "Cancel and return to menu"});
        menuSelection.UseConverter(stack => stack.Name);

        var selectedStack = AnsiConsole.Prompt(menuSelection);

        if (selectedStack.StackId != 0) ManageFlashCards.FlashCardsMenu(selectedStack);
    }

    private static bool CheckStackExists(string newStackName)
    {
        var currentStacks = StacksDatabaseManager.GetStacks();
        var newStackFound = false;

        foreach (var stack in currentStacks)
        {
            if (newStackName.ToLower() == stack.Name.ToLower())
                newStackFound = true;
        }

        return newStackFound;
    }
}