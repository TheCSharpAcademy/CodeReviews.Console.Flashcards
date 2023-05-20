using FlashcardsLibrary.Display;

namespace FlashcardsLibrary.Controller;
public class MainPage
{
    private UserInterface UI { get; set; } = new();
    private InputModel User { get; set; } = new();
    private MainPageDisplay Page { get; set; } = new();


    public bool RunMainMenu()
    {
        Page.DisplayMainMenu();

        bool quit = false;
        string choice = User.GetMenuChoice();
        
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

    public void DisplaySplashScreen()
    {
        Page.DisplaySplashScreen();
    }
}
