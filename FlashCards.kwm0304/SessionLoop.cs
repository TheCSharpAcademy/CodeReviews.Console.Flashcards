
using FlashCards.kwm0304.Config;
using FlashCards.kwm0304.Services;
using FlashCards.kwm0304.Views;
using Spectre.Console;

namespace FlashCards.kwm0304;

public class SessionLoop
{
  private readonly StackService _stackService;
  private readonly StudySessionService _studyService;
  private readonly ReportUI _reportUI;
  private readonly DatabaseConfiguration _dbConfig;
  public SessionLoop(DatabaseConfiguration dbConfig)
  {
    _stackService = new StackService();
    _studyService = new StudySessionService();
    _reportUI = new ReportUI();
    _dbConfig = dbConfig;
  }
  internal void OnStart()
  {
    _dbConfig.DatabaseActionsOnStart();

    while (true)
    {
      Console.Clear();
      string choice = SelectionPrompt.MainMenu();
      HandleChoice(choice);
    }
  }

  private void HandleChoice(string choice)
  {
    switch (choice)
    {
      case "Go to stacks":
        _stackService.HandleStack();
        break;
      case "Study":
        _studyService.HandleStudy();
        break;
      case "Reports":
        _reportUI.HandleReports();
        break;
      case "Exit":
        AnsiConsole.WriteLine("Goodbye");
        Environment.Exit(0);
        break;
      default:
        break;
    }
  }
}