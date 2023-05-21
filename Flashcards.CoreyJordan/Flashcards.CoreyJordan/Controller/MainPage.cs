using Flashcards.CoreyJordan.Display;

namespace Flashcards.CoreyJordan.Controller;
public class MainPage
{
    private ConsoleUI UI { get; set; } = new();
    private InputModel User { get; set; } = new();
    private MainUI Page { get; set; } = new();


    public bool RunMainMenu()
    {
        Page.DisplayMainMenu();

        bool quit = false;
        string choice = User.GetString("Select an option: ");
        
        switch (choice.ToUpper())
        {
            case "1":
                StudySession session = new();
                session.StartNew();
                break;
            case "2":
                PackManager packManager = new();
                packManager.ManagePacks();
                break;
            case "3":
                CardManager cardManager = new();
                cardManager.ManageCards();
                break;
            case "4":
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

    public void DisplaySplashScreen()
    {
        Page.DisplaySplashScreen();
    }
}
