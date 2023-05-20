namespace FlashcardsLibrary.Display;
internal class MainPageDisplay
{
    private UserInterface UI { get; set; } = new();
    private string[] MainMenu { get; } =
    {
        "N: New Study Session",
        "P: Pack Menu",
        "C: Card Menu",
        "R: Report Card",
        "Q: Quit"
    };
    private string[] SplashScreen { get; } =
    {
        "Welcome to Flash Card Study",
        "Written by Corey Jordan",
        "Developed for the C# Academy"
    };

    public void DisplayMainMenu()
    {
        UI.TitleBar("MAIN MENU");

        foreach (string s in MainMenu)
        {
            Console.WriteLine($"\t{s}");
        }
        Console.WriteLine();
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
