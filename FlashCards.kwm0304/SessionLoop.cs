
using FlashCards.kwm0304.Services;
using FlashCards.kwm0304.Views;
using Spectre.Console;

namespace FlashCards.kwm0304;

public class SessionLoop
{
  private readonly StackUI _stackUI;
  private readonly StudySessionUI _studyUI;
  private readonly ReportUI _reportUI;
  public SessionLoop()
  {
    _stackUI = new StackUI();
    _studyUI = new StudySessionUI();
    _reportUI = new ReportUI();
  }
  internal void OnStart()
  {
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
        _stackUI.HandleStack();
        break;
      case "Study":
        _studyUI.HandleStudy();
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