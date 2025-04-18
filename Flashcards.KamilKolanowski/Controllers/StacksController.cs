using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Handlers;
using Flashcards.KamilKolanowski.Models;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Controllers;

public class StacksController
{
    internal static void ViewStacksTable(DatabaseManager databaseManager)
    {
        var stacksTable = GetStacksDtoTable(databaseManager);
        var table = BuildFlashcardsTable(stacksTable);
        
        AnsiConsole.Write(table);
    }

    private static List<StacksDto> GetStacksDtoTable(DatabaseManager databaseManager)
    {
        var stacks = databaseManager.ReadStacks();
        
        return stacks.Select(stack => new StacksDto    
        {
            StackId = stack.StackId,
            StackName = stack.StackName
        }).ToList();

    }
    
    private static Table BuildFlashcardsTable(List<StacksDto> stackDtos)
    {
        var stacksTable = new Table();
        
        stacksTable.Title("[bold yellow]Stacks[/]");
        stacksTable.Border(TableBorder.Rounded);
        stacksTable.BorderColor(Color.HotPink3);
        
        stacksTable.AddColumn("[darkorange3_1]Stack Id[/]");
        stacksTable.AddColumn("[darkorange3_1]Stack Name[/]");
        
        var idx = 1;
        foreach (var stack in stackDtos)
        {
            stacksTable.AddRow(
                $"[grey69] {idx}[/]",
                $"[grey69] {stack.StackName}[/]"
            );
            idx++;
        }
    
        foreach (var column in stacksTable.Columns)
        {
            column.Centered();
        }
        
        return stacksTable;
    }
    
    private static void InformUserWithStatus(string option)
    {
        Console.Clear();
        Console.WriteLine($"Stack {option} successfully. \nPress any key to go back to Main Menu.");
        Console.ReadKey();
    }
}