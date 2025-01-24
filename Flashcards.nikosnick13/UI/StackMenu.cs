using Flashcards.nikosnick13.Controllers;
using Flashcards.nikosnick13.DTOs;
using Flashcards.nikosnick13.Models;
using Spectre.Console;
using static System.Console;
using static Flashcards.nikosnick13.Enums.Enums;
using static Flashcards.nikosnick13.UI.MenuManager;
namespace Flashcards.nikosnick13.UI;

internal class StackMenu
{
    private readonly StackController _stackController;

    public StackMenu()
    {
        _stackController = new StackController();
    }

    public void ShowStackMenu()
    {
       
        bool isStackMenuRunning = true;

        while (isStackMenuRunning)
        {
            Clear();
            var stackMenu = AnsiConsole.Prompt(
                new SelectionPrompt<StacksMenuOptions>()
                .Title("What would you like to do?")
                .AddChoices(
                    StacksMenuOptions.ViewStack,
                    StacksMenuOptions.ViewAllStacks,
                    StacksMenuOptions.AddStack,
                    StacksMenuOptions.DeleteStack,
                    StacksMenuOptions.EditStacks,
                    StacksMenuOptions.ReturnToMainMenu
                    ));

            switch (stackMenu)
            {
                case StacksMenuOptions.ViewStack:
                    PocessecViewOneStack();
                    break;
                case StacksMenuOptions.ViewAllStacks:
                    _stackController.ViewAllStacks();                   
                    break;
                case StacksMenuOptions.AddStack:
                    ProssesAdd();
                    break;
                case StacksMenuOptions.DeleteStack:
                    PorssesDelete();
                    Clear();
                    break;
                case StacksMenuOptions.EditStacks:
                    ProcessesEdit();
                    break;
                case StacksMenuOptions.ReturnToMainMenu:
                    isStackMenuRunning = false;
                    break;
            }
        }
    }

    private void PocessecViewOneStack()
    {
        _stackController.ViewAllStacks();

        AnsiConsole.WriteLine("\nPlease enter the ID of the stack you want to view (or 0 to return to the main menu).");
        string? userInput = ReadLine();

        if (userInput == "0")
        {
            ShowStackMenu();
            return;
        }

        while (!Validation.isValidInt(userInput))
        {
            Console.WriteLine("Invalid input. Please enter a valid integer ID.");
            userInput = Console.ReadLine();
        }

        int id = Int32.Parse(userInput);

        // Κλήση της μεθόδου που επιστρέφει το stack
        var stackDTO = _stackController.ViewStackById(id);
 

        // Εμφάνιση των δεδομένων του stack μέσω της ShowStackTable
        TableVisualisation.ShowStackTable(new List<DetailStackDTO> { stackDTO });

    }

    private void ProcessesEdit()
    {
        while (true)
        {
            _stackController.ViewAllStacks();
       
            AnsiConsole.WriteLine("\nPlease enter the ID of the category you want to edit (or 0 to return to the main menu).");
            string? userInputId = ReadLine();

          
            if (userInputId == "0")
            {
                ShowStackMenu();
                return;
            }

            if (!Validation.isValidInt(userInputId))
            {
                AnsiConsole.MarkupLine("[red]Invalid input. Please enter a valid numeric ID.[/]");
                continue; 
            }

            int id = Int32.Parse(userInputId);

            var stack = _stackController.GetById(id);

            if (stack == null)
            {
                AnsiConsole.WriteLine($"Record with ID {id} doesn't exist. Please try again.");
                continue; 
            }

            if (!Validation.ConfirmEdit("Do you want to edit the name of this stack?"))
            {
                return;  
            }

            string newName = GetStackName("Please insert the new name. Type 0 to return to the main menu.");
            if (newName == "0")
            {
                ShowStackMenu();
                return;
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                AnsiConsole.MarkupLine("[red]The name cannot be empty. Please try again.[/]");
                continue;  
            }

            var stackDto = new BasicStackDTO
            {
                Id = stack.Id,
                Name = newName
            };

           
            _stackController.EditStackById(stackDto);

            AnsiConsole.MarkupLine($"[green]Stack with ID {id} has been successfully updated to '{newName}'.[/]");

            AnsiConsole.Prompt(new TextPrompt<string>("\nPress [green]Enter[/] to return to the menu.").AllowEmpty());

            break;  
        }
    }

    private void PorssesDelete()
    {
        
        _stackController.ViewAllStacks();

        WriteLine("\nPlease enter the ID of the category you want to delete (or 0 to return to the main menu).");
        string? userInputId = ReadLine();

        if (userInputId == "0") ShowStackMenu();

        while (!Validation.isValidInt(userInputId))
        {
            AnsiConsole.MarkupLine("[red]Invalid input. Try again.[/]");

            AnsiConsole.Prompt(new TextPrompt<string>("\nPress [green]Enter[/] to continue...").AllowEmpty());
            PorssesDelete();
        }

        int id = Int32.Parse(userInputId);

        var stack = _stackController.GetById(id);

        _stackController.DeleteStackById(stack.Id);
    }

    private void ProssesAdd()
    {
        AnsiConsole.Clear();
        var stackName = GetStackName("Enter the name of the new stack, or type '0' to return to the Stack Menu.\n");

        var newStack = new Stack
        {
            Name = stackName
        };

        _stackController.InsertStack(newStack);
        Clear();
    }

    private string GetStackName(string msg)
    {
        WriteLine(msg);

        string? userInput = AnsiConsole.Prompt(new TextPrompt<string>("New stack name:"));

        if (userInput == "0") ShowStackMenu();

        while (!Validation.isValidString(userInput))
        {
            AnsiConsole.MarkupLine("[red]Invalid input. Try again.[/]");
            userInput = AnsiConsole.Ask<string>(msg);
        }

        return userInput;
    }

}