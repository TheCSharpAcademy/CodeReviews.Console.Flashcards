using FlashCards.Data;
using FlashCards.Models;
using FlashCards.Dtos;
using FlashCards.Mappers;
using Spectre.Console;
using Microsoft.Data.SqlClient;

namespace FlashCards.Managers;

internal class StacksManager : ModelManager<Stack>
{
    public enum StackManagementOptions
    {
        AddNewStack,
        ManageCurrentStacks
    }
    public enum StackOperations
    {
        DeleteSelectedStack,
        UpdateSelectedStack
    }

    private List<StackDto> stacksDtos;
    private List<Stack> stacks;
    readonly StacksDBController stacksDBController;
    public StacksManager()
    {
        stacksDBController = new StacksDBController();
        LoadStacks();
    }

    private void LoadStacks()
    {
        stacks = stacksDBController.ReadAllRows();

        stacksDtos = stacks.Select(
                s => s.ToStackDto())
                .ToList();
    }

    public void ShowOptions()
    {
        var userOption = AnsiConsole.Prompt(
            new SelectionPrompt<StackManagementOptions>()
            .AddChoices(Enum.GetValues<StackManagementOptions>()));

        switch (userOption)
        {
            case StackManagementOptions.AddNewStack:
                AddNewModel();
                break;
            case StackManagementOptions.ManageCurrentStacks:
                int stackId = ChooseStackMenu();
                if (stackId == -1)
                    break;
                ChooseOperationMenu(stackId);
                break;
        }
    }

    private void ChooseOperationMenu(int stackId)
    {
        var selectedOperation = AnsiConsole.Prompt(
            new SelectionPrompt<StackOperations>()
            .Title("[yellow]Choose what operation you want to perform[/]")
            .AddChoices(Enum.GetValues<StackOperations>()));

        switch (selectedOperation)
        {
            case StackOperations.DeleteSelectedStack:
                DeleteModel(stackId);
                break;
            case StackOperations.UpdateSelectedStack:
                var stackName = AnsiConsole.Ask<string>("[yellow]Enter the new name for the stack: [/]");
                UpdateModel(stackId, new Stack { name = stackName });
                Console.ReadKey();
                break;
        }

    }

    private void ShowAvailableStacks()
    {
        List<string> columnNames = ["Name"];
        TableVisualisationEngine<StackDto>.ViewAsTable(stacksDtos, ConsoleTableExt.TableAligntment.Left, columnNames);
    }

    internal int ChooseStackMenu()
    {
        bool exitMenu = false;
        do
        {
            Console.Clear();
            ShowAvailableStacks();
            AnsiConsole.MarkupLine("[yellow]Enter a stack name (Or enter 'quit' to exit)[/]");
            string? readResult = Console.ReadLine();

            if (string.IsNullOrEmpty(readResult))
            {
                AnsiConsole.MarkupLine("[red]Error- Invalid input[/]");
                continue;
            }
            string userEntry = readResult.Trim();
            exitMenu = userEntry.Equals("quit", StringComparison.CurrentCultureIgnoreCase);
            if (exitMenu)
                continue;

            if (!StackNameExist(userEntry))
            {
                AnsiConsole.MarkupLine("[red]Error- Invalid input, please make sure you entered a correct stack name - Case Matters[/]");
                continue;
            }
            var stackId = FetchModelId(userEntry);
            AnsiConsole.MarkupLine($"[green]Current working stack = {userEntry}![/]");
            return stackId;
        }
        while (!exitMenu);
        return -1;
    }

    private bool StackNameExist(string userEntry)
    {
        foreach (StackDto stackDto in stacksDtos)
        {
            if (userEntry == stackDto.name)
                return true;
        }
        return false;
    }

    private int FetchModelId(string stackName)
    {
        foreach (Stack stack in stacks)
        {
            if (stackName == stack.name)
                return stack.id;
        }
        return -1;
    }

    protected override void DeleteModel(int stackId)
    {
        AnsiConsole.MarkupLine("[red]Are you sure you want to delete the stack?\n[white](To Confirm Deletion Pres Enter)[/][/]");
        if (Console.ReadKey().Key == ConsoleKey.Enter)
        {
            stacksDBController.DeleteRow(stackId);
            AnsiConsole.MarkupLine("[green]Stack Deleted Succesfully![/]");
            return;
        }
        AnsiConsole.MarkupLine("[red]Stack Deletion Cancelled! (Press Any Key To Continue)[/]");
        Console.ReadKey();
    }

    protected override void UpdateModel(int stackId, Stack modifiedStack)
    {
        modifiedStack.id = stackId;
        try
        {
            stacksDBController.UpdateRow(modifiedStack);
            AnsiConsole.MarkupLine("[green]Stack Updated Successfully![/]");
        }
        catch (SqlException ex)
        {
            if (!ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                throw;
            AnsiConsole.MarkupLine("[red]Unique constraint violated - A Stack with that name already exists![/]");
        }


    }

    protected override void AddNewModel()
    {
        var newStack = new Stack();

        var stackName = AnsiConsole.Ask<string>("[yellow]Enter the stack name: [/]");
        newStack.name = stackName;

        try
        {
            stacksDBController.InsertRow(newStack);
            AnsiConsole.MarkupLine("[green]Stack Added Successfully![/]\n (Press Any Key To Continue)");
            Console.ReadKey();
        }
        catch (SqlException ex)
        {
            if (!ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                throw;
            AnsiConsole.MarkupLine("[red]Unique constraint violated - Stack already exists![/]");
        }

    }
}

