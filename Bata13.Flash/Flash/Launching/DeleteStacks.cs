using Flash.Helper.MainHelper;
using Spectre.Console;
using System.Data.SqlClient;

namespace Flash.Launching;
internal class DeleteStacks
{    
    internal static void GetDeleteStacks()
    {
        Console.Clear();

        ShowBanner.GetShowBanner("Manage Stacks", Color.RosyBrown);

        int stacksTableCount = CheckStacksTableExists.GetCheckStacksTableExists();

        if (stacksTableCount == 0)
        {
            Console.WriteLine("Cannot delete a stack. Stacks Table does not exist.");
            ReturnComment.MainMenuReturnComments();
        }
        else
        {
            Console.WriteLine("Stacks Table alreaday exists");
        }

        Console.WriteLine("This is all the stacks in your Stacks Table: ");

        AllExistingStacks.ShowAllExistingStacks();

        int stackIdToDelete = StackIdToDelete.GetStackIdToDelete();

        Console.WriteLine($"The stack ID to delete is: {stackIdToDelete}");

        DeleteAStack.ExecuteDeleteAStack(stackIdToDelete);

        ReturnComment.MainMenuReturnComments();
    }
}
