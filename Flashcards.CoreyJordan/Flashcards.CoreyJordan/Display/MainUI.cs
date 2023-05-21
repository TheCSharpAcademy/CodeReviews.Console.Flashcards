using ConsoleTableExt;

namespace Flashcards.CoreyJordan.Display;
internal class MainUI
{
    private ConsoleUI UI { get; set; } = new();
    private List<MenuModel> MainMenu { get; } = new()
    {
        new MenuModel("1", "New Study Session"),
        new MenuModel("2", "Pack Menu"),
        new MenuModel("3", "Card Menu"),
        new MenuModel("4", "Report Card"),
        new MenuModel("Q", "Quit")
    };
    private string[] SplashScreen { get; } =
    {
        "Welcome to Flash Card Study",
        "Written by Corey Jordan",
        "Developed for the C# Academy",
        "",
        "Create packs of flashcards and use them to",
        "carry out study sessions.",
        "Flash Card Study will track and report each",
        "session and generate reports to track prtogress.",
        "",
        "Enjoy!"
    };

    public void DisplayMainMenu()
    {
        UI.TitleBar("MAIN MENU");

        ConsoleTableBuilder
            .From(MainMenu)
            .WithColumn("", "")
            .WithFormat(ConsoleTableBuilderFormat.MarkDown)
            .ExportAndWriteLine(TableAligntment.Center);
    }

    public void DisplaySplashScreen()
    {
        UI.Seperator();

        foreach (string s in SplashScreen)
        {
            UI.WriteCenterLine(s);
        }
        Console.WriteLine();

        UI.Seperator();

        UI.Prompt();
    }
}
