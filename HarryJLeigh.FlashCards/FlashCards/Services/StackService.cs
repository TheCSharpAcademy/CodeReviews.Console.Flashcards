using FlashCards.Controllers;
using FlashCards.Data;
using FlashCards.Utilities;
using FlashCards.Views;
using Spectre.Console;

namespace FlashCards.Services;

public static class StackService
{
    private static readonly StackController _stackController = new StackController(new DatabaseService());
    
    internal static void InsertStack()
    {
        string stackName = StackExtensions.GetName();
        while (StackExtensions.CheckIfStackExists(stackName))
        {
            AnsiConsole.MarkupLine($"[red]{stackName} Stack already exists! Enter a different stack name.[/]");
            stackName = StackExtensions.GetName();
        }
        _stackController.InsertStack(stackName);
    }
    
    internal static void ViewAll()
    {
        List<StackDto> stacks = _stackController.GetAllStackNames();
        TableVisualisation.ShowStacks(stacks);
    }
    
    internal static void UpdateStack()
    {   
        string stackNameToUpdate = StackExtensions.GetStackNameFromTable();
        string newStackName = StackExtensions.GetName("new");
        _stackController.UpdateStack(stackNameToUpdate, newStackName);
        AnsiConsole.MarkupLine($"[green]Stack name updated from [/][cyan]{stackNameToUpdate}[/] [green]to [/][cyan]{newStackName}[/]");
            
    }
    
    internal static void DeleteStack()
    {
        string stackNameToDelete = StackExtensions.GetStackNameFromTable();
        FlashcardService.DeleteAllFlashcards(stackNameToDelete);
        int stack_id = FlashcardExtensions.GetStack_id(stackNameToDelete);
        _stackController.DeleteStack(stackNameToDelete);
        StudyService.DeleteAllStudySessions(stack_id);
        AnsiConsole.MarkupLine($"[green]Deleted stack: [/][cyan]{stackNameToDelete}[/]");
    }
    
    internal static List<string> GetAllStackNames()
    {
        List<StackDto> stacks = _stackController.GetAllStackNames();
        List<string> stackNames = new List<string>();
        
        foreach (StackDto stack in stacks) stackNames.Add(stack.name);
        return stackNames;
    }
}