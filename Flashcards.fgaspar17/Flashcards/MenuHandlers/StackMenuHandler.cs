using Flashcards.MenuEnums;
using FlashcardsLibrary;
using Spectre.Console;

namespace Flashcards;
public class StackMenuHandler
{
    public void DisplayMenu()
    {
        MenuPresentation.MenuDisplayer<StackMenuOptions>(() => "[blue]Stack Menu[/]", HandleMenuOptions);
    }

    private bool HandleMenuOptions(StackMenuOptions option)
    {
        switch (option)
        {
            case StackMenuOptions.Back:
                return false;
            case StackMenuOptions.CreateStack:
                CreateStackHandler();
                break;
            case StackMenuOptions.UpdateStack:
                UpdateStackHandler();
                break;
            case StackMenuOptions.DeleteStack:
                DeleteStackHandler();
                break;
            case StackMenuOptions.ShowStacks:
                ShowStackHandler();
                break;
            default:
                Console.WriteLine($"Option: {option} not valid");
                break;
        }

        return true;
    }

    private void CreateStackHandler()
    {
        MenuPresentation.PresentMenu("[blue]Inserting[/]");

        string name = GetStackNameFromUser(new UniqueStackNameValidator());
        if (CancelSetup.IsCancelled(name)) return;

        StackController.InsertStack(new Stack { Name = name });
    }

    private void UpdateStackHandler()
    {
        MenuPresentation.PresentMenu("[yellow]Updating[/]");
        ShowStacks();

        AnsiConsole.WriteLine("Old Name"); 
        string oldName = GetStackNameFromUser(new ExistingModelValidator<string, Stack>() { ErrorMsg = "Stack Name must exist", GetModel = StackController.GetStackByName });
        if (CancelSetup.IsCancelled(oldName)) return;
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine("New Name"); 
        string newName = GetStackNameFromUser(new UniqueStackNameValidator());
        if (CancelSetup.IsCancelled(newName)) return;

        StackController.UpdateStack(new Stack { StackId = StackController.GetStackByName(oldName).StackId, Name = newName });
    }

    private void DeleteStackHandler()
    {
        MenuPresentation.PresentMenu("[red]Deleting[/]");
        ShowStacks();

        string name = GetStackNameFromUser(new ExistingModelValidator<string, Stack>() { ErrorMsg = "Stack Name must exist", GetModel = StackController.GetStackByName });
        if (CancelSetup.IsCancelled(name)) return;

        StackController.DeleteStack(new Stack { StackId = StackController.GetStackByName(name).StackId, Name = name });
    }

    private void ShowStackHandler()
    {
        MenuPresentation.PresentMenu("[green]Showing Stacks[/]");

        ShowStacks();

        Prompter.PressKeyToContinuePrompt();
    }

    public static string GetStackNameFromUser(params IValidator[] validators)
    {
        string message = "Introduce a Stack Name";

        return Prompter.ValidatedTextPrompt(message, validations: validators);
    }

    public static void ShowStacks()
    {
        var stacks = StackService.GetStacks();

        OutputRenderer.ShowTable(stacks, title: "Stacks");
    }
}