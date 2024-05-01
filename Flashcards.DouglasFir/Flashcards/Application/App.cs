using Flashcards.Database;
using Flashcards.Services;
using Flashcards.Enums;
using Spectre.Console;

namespace Flashcards.Application;

internal class App
{
    private DatabaseContext _databaseContext;
    private AppConfigurationHandler _appConfigurationHandler;
    private InputHandler _inputHandler;
    private DatabaseSeeder _dbSeeder;
    private bool _running;

    public App() 
    {
        _running = true;

        // Configure Application
        _inputHandler = new InputHandler();
        _databaseContext = new DatabaseContext();
        _appConfigurationHandler = new AppConfigurationHandler(_databaseContext, _inputHandler);
        _appConfigurationHandler.ConfigureApplication();

        // Setup Services
        _dbSeeder = new DatabaseSeeder(_databaseContext, _inputHandler);
        
    }

    public void Run()
    {
        while (_running)
        {
            AnsiConsole.Clear();
            DisplayMainScreenBanner();
            PromptForSessionAction();
        }
    }

    private void DisplayMainScreenBanner()
    {
        AnsiConsole.Write(
            new FigletText("Study Hall")
                .LeftJustified()
                .Color(Color.SeaGreen1_1));

        Utilities.PrintNewLines(2);
    }

    private void PromptForSessionAction()
    {
        MainMenuOption selectedOption = _inputHandler.PromptMenuSelection<MainMenuOption>();
        ExecuteSelectedOption(selectedOption);
    }

    private void ExecuteSelectedOption(MainMenuOption option)
    {
        switch (option)
        {
            case MainMenuOption.StartStudySession:
                AppStudySession appStudySession = new AppStudySession(_databaseContext, _inputHandler);
                appStudySession.Run();
                break;
            case MainMenuOption.ManageStacks:
                AppManageStacks appManageStacks = new AppManageStacks(_databaseContext, _inputHandler);
                appManageStacks.Run();
                break;
            case MainMenuOption.ManageFlashCards:
                AppManageFlashCards appManageFlashCards = new AppManageFlashCards(_databaseContext, _inputHandler);
                appManageFlashCards.Run();
                break;
            case MainMenuOption.ViewStudySessionData:
                AppStudySessionReport appStudySessionReport = new AppStudySessionReport(_databaseContext, _inputHandler);
                appStudySessionReport.Run();
                break;
            case MainMenuOption.SeedDatabase:
                _dbSeeder.SeedDatabase();
                break;
            case MainMenuOption.Exit:
                CloseSession();
                break;
        }
    }

    private void CloseSession()
    {
        _running = false;
        AnsiConsole.Markup("[teal]Goodbye![/]");
        Utilities.PrintNewLines(2);
    }
}
