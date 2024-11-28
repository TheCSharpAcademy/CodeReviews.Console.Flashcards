using FlashCards.Services;
using Spectre.Console;

namespace FlashCards.Utilities;

public static class StackExtensions
{
    internal static string GetName(string type = "")
    {
        AnsiConsole.MarkupLine($"[green]Enter {type}[/] [yellow]stack[/] [green]name:[/]");
        string input = Console.ReadLine();
        return input;
    }

    internal static string GetStackNameFromTable(string text = "")
    {
        string input = "";
        StackService.ViewAll();

        while (true)
        {
            if (text == "")
            {
                AnsiConsole.MarkupLine("[green]Enter name from table:[/]");
                input = Console.ReadLine();
            }
            if (text != "")
            {
                AnsiConsole.MarkupLine($"[green]{text}[/]");
                input = Console.ReadLine();
            }
            if (StackService.GetAllStackNames().Contains(input))
            {
                return input;
            }
            if (!StackService.GetAllStackNames().Contains(input))
            {
                AnsiConsole.MarkupLine("[red]Invalid stack name[/]");
            }
        }
    }
    
    internal static string ChooseStack()
    {
        return StackExtensions.GetStackNameFromTable("Choose a stack of flashcards to interact with:");
    }

    internal static bool CheckIfStackExists(string stackName) => StackService.GetAllStackNames().Contains(stackName);

    internal static bool IsStacksEmpty()
    {
        List<string> stacks = StackService.GetAllStackNames();
        if (stacks.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No stacks found![/]");
            return true;
        }
        return false;
    }
}