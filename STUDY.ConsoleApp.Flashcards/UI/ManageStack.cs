using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using STUDY.ConsoleApp.Flashcards.Controllers;
using STUDY.ConsoleApp.Flashcards.Enums;
using STUDY.ConsoleApp.Flashcards.Models;
using STUDY.ConsoleApp.Flashcards.Models.DTOs;

namespace STUDY.ConsoleApp.Flashcards.UI;

public class ManageStack
{
    public static void Menu()
    {
        StackController stackController = new();
        
        while (true)
        {
            AnsiConsole.Clear();
            var menuOptions = AnsiConsole.Prompt(
                new SelectionPrompt<ManageStacksOptions>()
                    .Title("Manage Stacks")
                    .AddChoices(Enum.GetValues<ManageStacksOptions>())
            );
            
            var stackList = stackController.ListAllStacks();

            switch (menuOptions)
            {
                case ManageStacksOptions.CreateStack:
                    AnsiConsole.MarkupLine("Creating a new stack...");
                    var name = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter stack name (or 0 to go back): ").Validate(
                            name => name.Length <= 50, "Too long stack name."));
                    if (name == "0")
                        break;

                    stackController.CreateStack(name);

                    AnsiConsole.Markup("[green]Stack created successfully.[/]\n");
                    break;
                
                case ManageStacksOptions.ListAllStacks:

                    if (stackList.IsNullOrEmpty())
                    {
                        AnsiConsole.MarkupLine("[red]There are no stacks to list.[/]");
                        break;
                    }

                    Table table = new();

                    table.AddColumn("Name");

                    foreach (var stack in stackList)
                    {
                        table.AddRow(stack.Name);
                    }

                    AnsiConsole.Write(table);
                    break;

                case ManageStacksOptions.EditStackName:

                    if (stackList.IsNullOrEmpty())
                    {
                        AnsiConsole.MarkupLine("[red]There are no stacks to edit.[/]");
                        break;
                    }
                    
                    var updateSelection = AnsiConsole.Prompt(new SelectionPrompt<Stack>().Title("Editing stack name...\nChoose one of the next stacks to edit its name:").AddChoices(stackList));
                    var updatedName = AnsiConsole.Prompt(
                            new TextPrompt<string>("Enter new stack name(or 0 to go back): ").Validate(s => s.Length <= 50,
                                "Too long stack name."));
                    if (updatedName == "0")
                        break;

                    stackController.EditStackName(updateSelection.Id, updatedName);
                    AnsiConsole.Markup("[green]Stack name updated successfully.[/]\n");
                    break;
                
                case ManageStacksOptions.DeleteStack:
                    if (stackList.IsNullOrEmpty())
                    {
                        AnsiConsole.MarkupLine("[red]There are no stacks to delete.[/]");
                        break;
                    }

                    var deleteSelection = AnsiConsole.Prompt(new SelectionPrompt<Stack>().Title("Deleting a stack...\nChoose one of the next stacks delete:").AddChoices(stackList));

                    var confirmation = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title($"[red]Are you sure you want to delete '{deleteSelection.Name}' stack?[/]")
                        .AddChoices("Yes", "No"));
                    if (confirmation == "Yes")
                    {
                        stackController.DeleteStack(deleteSelection.Id);
                        AnsiConsole.Markup("[green]Stack deleted successfully.[/]\n");
                    }
                    else
                    {
                        AnsiConsole.Markup("[cyan]Stack won't be deleted.[/]\n");
                    }
                    break;
                
                case ManageStacksOptions.BackToMainMenu:
                    Menus.MainMenu();
                    break;
            }

            AnsiConsole.Markup("Press Any key to continue...\n");
            Console.ReadKey();
        }
    }
}