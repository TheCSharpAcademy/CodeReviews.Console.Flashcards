using Flashcards.MenuEnums;

namespace Flashcards;
public class MainMenuHandler
{
    public void DisplayMenu()
    {
        MenuPresentation.MenuDisplayer<MainMenuOptions>(() => "[blue]Main Menu[/]", HandleMenuOptions);
    }

    public bool HandleMenuOptions(MainMenuOptions option)
    {
        string stackName;
        switch (option)
        {
            case MainMenuOptions.Quit:
                return false;
            case MainMenuOptions.ManageStacks:
                StackMenuHandler stackMenuHandler = new StackMenuHandler();
                stackMenuHandler.DisplayMenu();
                break;
            case MainMenuOptions.ManageFlashcards:
                FlashcardMenuHandler flashcardMenuHandler = new FlashcardMenuHandler();
                flashcardMenuHandler.DisplayMenu();
                break;
            case MainMenuOptions.Study:
                Study study = new Study();
                study.Run();
                break;
            case MainMenuOptions.StudySessions:
                StudySessionDisplayer studySessionDisplayer = new StudySessionDisplayer();
                studySessionDisplayer.Display();
                break;
            case MainMenuOptions.MonthlyReport:
                MonthlyReportHandler monthlyReportHandler = new MonthlyReportHandler();
                monthlyReportHandler.Display();
                break;
            default:
                Console.WriteLine($"Option: {option} not valid");
                break;
        }

        return true;
    }
}