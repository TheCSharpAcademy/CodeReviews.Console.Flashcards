using Spectre.Console;
using Flash.Launching.SubManageStacks;
using Flash.Helper.ManageStacksHelper;
using Flash.Helper.MainHelper;

namespace Flash.Launching;
internal class ManageStacks
{
    internal static void GetManageStacks()
    {
        Console.Clear();

        ShowBanner.GetShowBanner("Manage Stacks", Color.Green);

        int stacksTableCount = CheckStacksTable.GetCheckStacksTable();
        
        if (stacksTableCount == 0)
        {
            CreateStacksTable.GetCreateStacksTable();
        }
        else
        {
            Console.WriteLine("StacksTable already exists\n");
        }           

        AllExistingStacks.ShowAllExistingStacks();

        AnsiConsole.Markup("\nInput [red]Name[/] of the Stack you want to work with Or Input 0 to Return to MainMenu");
        AnsiConsole.Markup("\nIf you add a [red]Stack Name[/] that doesn't exist, you'll be creating a new Stack under that Name.\n");

        string currentWorkingStack = Console.ReadLine(); 

        if (currentWorkingStack == "0")
        {
            ReturnComment.MainMenuReturnComments();
        }

        else
        {
            CheckExistingStacks.GetCheckExistingStacks(currentWorkingStack);

            WorkingStackMenu.GetWorkingStackMenu(currentWorkingStack);
        }

        ReturnComment.MainMenuReturnComments();
    }
}
