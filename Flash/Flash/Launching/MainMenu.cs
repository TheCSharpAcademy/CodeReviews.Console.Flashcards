using Spectre.Console;
using Flash.Helper.MainHelper;

namespace Flash.Launching;
internal static class MainMenu
{
    internal static void GetMainMenu()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            ShowBanner.GetShowBanner("MAIN MENU", Color.White);

            ShowMainMenuOptions.GetShowMainMenuOptions();

            string command = Console.ReadLine();

            ExecuteMainMenuOptions.GetExecuteMainMenuOptions(command, closeApp);
        }
    }
}
