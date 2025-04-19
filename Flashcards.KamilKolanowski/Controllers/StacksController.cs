using System.Collections;
using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Handlers;
using Flashcards.KamilKolanowski.Helpers;
using Flashcards.KamilKolanowski.Models;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Controllers;

public class StacksController
{
    internal static void AddNewStack(DatabaseManager databaseManager)
    {
        var newStack = UserInputHandler.CreateStack();
        // var stack = databaseManager.ReadStacks().FirstOrDefault(s => s.StackId == stackId); FIX THE BUGS here
        
        VerifyIfStackExists(null, newStack.StackName, "added");
        databaseManager.AddStack(newStack);
    }
    
    internal static void EditStack(DatabaseManager databaseManager)
    {
        var updateStackDto = new UpdateStackDto();
    
        var stackId = StackChoice.GetStackChoice(databaseManager);
        var stack = databaseManager.ReadStacks().FirstOrDefault(s => s.StackId == stackId);

        if (stack == null)
            return;
    
        updateStackDto.StackId = stack.StackId;
        updateStackDto.StackName = stack.StackName;

        updateStackDto.ColumnToUpdate = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose the column to edit")
                .AddChoices("StackName", "Description"));
        
        updateStackDto.NewValue = AnsiConsole.Prompt(
            new TextPrompt<string>($"Provide new value for {updateStackDto.ColumnToUpdate}: "));

        if (VerifyIfStackExists(stack.StackName, updateStackDto.NewValue, "updated"))
        {
            return; // Task updated successfully even after it checks that exists
        }
        databaseManager.UpdateStack(updateStackDto);

        InformUserWithStatus("updated");
    }
    
    internal static void ViewStacksTable(DatabaseManager databaseManager)
    {
        var stacksTable = GetStacksDtoTable(databaseManager);
        var table = BuildStackTable(stacksTable);
        
        AnsiConsole.Write(table);
        
        AnsiConsole.MarkupLine("Press any key to go back to the main menu.");
        Console.ReadKey();
    }

    private static List<StacksDto> GetStacksDtoTable(DatabaseManager databaseManager)
    {
        var stacks = databaseManager.ReadStacks();
        
        return stacks.Select(stack => new StacksDto    
        {
            StackId = stack.StackId,
            StackName = stack.StackName,
            Description = stack.Description
        }).ToList();

    }
    
    private static Table BuildStackTable(List<StacksDto> stackDtos)
    {
        var stacksTable = new Table();
        
        stacksTable.Title("[bold yellow]Stacks[/]");
        stacksTable.Border(TableBorder.Rounded);
        stacksTable.BorderColor(Color.HotPink3);
        
        stacksTable.AddColumn("[darkorange3_1]Stack Id[/]");
        stacksTable.AddColumn("[darkorange3_1]Stack Name[/]");
        stacksTable.AddColumn("[darkorange3_1]Stack Description[/]");
        
        var idx = 1;
        foreach (var stack in stackDtos)
        {
            stacksTable.AddRow(
                $"[grey69] {idx}[/]",
                $"[grey69] {stack.StackName}[/]",
                $"[grey69] {stack.Description}[/]"
            );
            idx++;
        }
    
        foreach (var column in stacksTable.Columns)
        {
            column.Centered();
        }
        
        return stacksTable;
    }
    
    private static bool VerifyIfStackExists(string oldStack, string newStack, string operation)
    {
        if (oldStack.ToLower().Equals(newStack.ToLower()))
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[red]Stack already exists[/]");
            AnsiConsole.MarkupLine("Press any key to go back to Main Menu");
            Console.ReadKey();
            
            return false;
        }
        InformUserWithStatus(operation);
        return true;

    }
    
    private static void InformUserWithStatus(string option)
    {
        Console.Clear();
        AnsiConsole.MarkupLine($"[springgreen2_1]Stack {option} successfully.[/] \nPress any key to go back to Main Menu.");
        Console.ReadKey();
    }
}