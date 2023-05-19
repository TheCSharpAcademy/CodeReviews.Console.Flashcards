using FlashcardsLibrary.Display;

namespace FlashcardsLibrary.Controller;
public class MainPage
{
    internal UserInterface UI { get; set; } = new();
    internal InputModel User { get; set; } = new();
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

    public bool RunMainMenu()
    {
        bool quit = false;
        string choice = User.GetStringInput("\tSelect an option: ");
        
        switch (choice.ToUpper())
        {
            case "N":
                StudySession session = new();
                session.StartNew();
                break;
            case "P":
                PackManager packManager = new();
                packManager.ManagePacks();
                break;
            case "C":
                CardManager cardManager = new();
                cardManager.ManageCards();
                break;
            case "R":
                ReportManager reportManager = new();
                reportManager.ManageReports();
                break;
            case "Q":
                quit = true;
                break;
            default:
                UI.Prompt("Invalid Selection. Please try again.");
                break;
        }

        return quit;
    }
}
