using Flashcards.Arashi256.Classes;
using Flashcards.Arashi256.Controllers;
using Flashcards.Arashi256.Models;
using Spectre.Console;

namespace Flashcards.Arashi256.Views
{
    internal class StackView
    {
        public StackController StackController { get; private set; }

        public StackView()
        {
            StackController = new StackController();
        }

        public void AddNewStack()
        {
            bool error = false;
            string? subject = string.Empty;
            do
            {
                AnsiConsole.MarkupLine("Enter 'Q' to cancel operation");
                subject = AnsiConsole.Ask<string>("What is the stack [orange1]label[/]? ");
                if (subject.ToLower() == "q")
                {
                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                    break;
                }
                else
                {
                    var newStack = new Stack_DTO() { Subject = subject };
                    if (StackController.AddStack(newStack))
                        AnsiConsole.MarkupLine($"[yellow]New stack labelled '{subject}' added[/].");
                    else
                    {
                        AnsiConsole.MarkupLine("[deeppink2]There was an error adding this stack. Stack labels must be unique! Please try again.[/]");
                        error = true;
                    }
                }
            } while (error);
        }

        public List<Stack_DTO> ViewStacks()
        {
            Table tblStackList = new Table();
            tblStackList.AddColumn(new TableColumn("[yellow]ID[/]").LeftAligned());
            tblStackList.AddColumn(new TableColumn("[yellow]Subject[/]").RightAligned());
            List<Stack_DTO> stacks = StackController.GetAllStacks();
            if (stacks.Count > 0)
            {
                for (int i = 0; i < stacks.Count; i++)
                {
                    tblStackList.AddRow($"[white]{stacks[i].DisplayId}[/]", $"[darkorange]{stacks[i].Subject}[/]");
                }
            }
            else
            {
                tblStackList.AddRow("", "[red]No stacks found[/]");
            }
            tblStackList.Alignment(Justify.Center);
            AnsiConsole.Write(tblStackList);
            return stacks;
        }

        public void UpdateStack()
        {
            bool error = false;
            string? subject = string.Empty;
            int id = 0;
            List<Stack_DTO> stacks = ViewStacks();
            Stack_DTO? selectedStack = null;
            if (stacks != null && stacks.Count > 0)
            {
                id = CommonUI.GetNumberInput("Please select a Stack ID to update: ", 0, stacks.Count);
                if (id == -1)
                {
                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]Stack ID '{id}' selected[/].");
                    selectedStack = stacks[id - 1];
                    ViewStack(selectedStack);
                    do
                    {
                        AnsiConsole.MarkupLine("Enter 'Q' to cancel operation");
                        subject = AnsiConsole.Ask<string>("What is the new stack [orange1]label[/]? ");
                        if (subject.ToLower() == "q")
                        {
                            AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                            break;
                        }
                        else
                        {
                            selectedStack.Subject = subject;
                            if (StackController.UpdateStack(selectedStack))
                                AnsiConsole.MarkupLine($"[yellow]Stack labelled '{subject}' updated[/].");
                            else
                            {
                                AnsiConsole.MarkupLine("[deeppink2]There was an error updating this stack. Stack labels must be unique! Please try again.[/]");
                                error = true;
                            }
                        }
                    } while (error);
                }
            }
        }

        public void DeleteStack()
        {
            int id = 0;
            List<Stack_DTO> stacks = ViewStacks();
            Stack_DTO? selectedStack = null;
            if (stacks != null && stacks.Count > 0)
            {
                id = CommonUI.GetNumberInput("Please select a Stack ID to delete: ", 0, stacks.Count);
                if (id == -1)
                {
                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                }
                else
                {
                    selectedStack = stacks[id - 1];
                    ViewStack(selectedStack);
                    if (AnsiConsole.Confirm($"Are you sure you want to delete this stack '{selectedStack.Subject}'? This will also delete any flashcards associated with it!"))
                    {
                        bool result = StackController.DeleteStack(selectedStack);
                        if (result)
                        {
                            AnsiConsole.MarkupLine($"[yellow]'{selectedStack.Subject}' stack successfully deleted.[/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine($"[red]There was a problem deleting the stack '{selectedStack.Subject}' Please try again.[/]");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                    }
                }
            }
        }

        public void ViewStack(Stack_DTO theStack)
        {
            Table tblStack = new Table();
            tblStack.AddColumn(new TableColumn($"[yellow]ID[/]").LeftAligned());
            tblStack.AddColumn(new TableColumn($"[white]{theStack.DisplayId}[/]").RightAligned());
            tblStack.AddRow($"[yellow]Subject[/]", $"[white]{theStack.Subject}[/]");
            tblStack.Alignment(Justify.Center);
            AnsiConsole.Write(tblStack);
        }
    }
}
