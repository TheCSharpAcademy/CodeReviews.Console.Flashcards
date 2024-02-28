using Flash.Launching;

namespace Flash.Helper.MainHelper;

internal class ReturnComment
{
    internal static void MainMenuReturnComments()
    {
        Console.WriteLine("Press Any Key To Return To MainMenu");
        Console.ReadLine();

        MainMenu.GetMainMenu();
    }
}
