using Flash.Helper.MainHelper;
using Spectre.Console;

namespace Flash.Launching.SubManageStacks;
internal class WorkingStackMenu
{
    internal static void GetWorkingStackMenu(string currentWorkingStack)
    {
        ShowBanner.GetShowBanner("Current Stack Menu", Color.Gold1);

        try
        {
            CurrentStackMenuOptions.GetCurrentStackMenuOptions(currentWorkingStack);

            string command = Console.ReadLine();

            ExecuteCurrentStackMenuOptions.GetExecuteCurrentStackMenuOptions(command, currentWorkingStack);
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        ReturnComment.MainMenuReturnComments();
    }

}
