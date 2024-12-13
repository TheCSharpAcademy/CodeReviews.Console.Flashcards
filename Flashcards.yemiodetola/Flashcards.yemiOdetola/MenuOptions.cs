using Spectre.Console;
using Flashcards.yemiOdetola.Controllers;
using Flashcards.yemiOdetola.Models;


namespace Flashcards.yemiOdetola;

public static class MenuOptions
{
  public static void ContinueInput()
  {
    AnsiConsole.Prompt(new TextPrompt<string>("[green]Press enter key to continue...[/]")
    .AllowEmpty());
  }

  private static string GetMenuOption()
  {
    AnsiConsole.Clear();
    return AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold underline red]Menu[/]")
            .PageSize(6)
            .AddChoices(["Manage stacks", "Study Sessions", "Exit"])
    );
  }

  private static void InitStacks()
  {
    bool end = false;
    while (!end)
    {
      string option = GetStacksOptions();
      switch (option)
      {
        case "Go back":
          end = true;
          break;
        case "Add new stack":
          StackController.AddStack();
          ContinueInput();
          break;
        case "View stacks":
          StackController.DisplayStacks();
          ContinueInput();
          break;
        case "Update a stack":
          StackController.UpdateStack();
          ContinueInput();
          break;
        case "Manage stacks":
          Stack? stack = StackController.SelectStack();
          if (stack == null)
          {
            break;
          }
          InitStack(stack.Name);
          break;
        case "Delete stack":
          StackController.DeleteStack();
          ContinueInput();
          break;
      }
    }
  }

  private static void InitStack(string Name)
  {
    bool end = false;
    while (!end)
    {
      string option = GetOption(Name);
      switch (option)
      {
        case "Go back":
          end = true;
          break;
        case "View flashcards":
          FlashCardsController.DisplayCards(Name);
          ContinueInput();
          break;
        case "Add flashcard":
          FlashCardsController.AddCard(Name);
          ContinueInput();
          break;
        case "Update flashcard":
          FlashCardsController.UpdateCard(Name);
          ContinueInput();
          break;
        case "Delete flashcard":
          FlashCardsController.DeleteCard(Name);
          ContinueInput();
          break;
      }
    }
  }

  private static string GetStacksOptions()
  {
    AnsiConsole.Clear();
    return AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold underline red]Stacks[/]")
            .PageSize(6)
            .AddChoices(["Go back", "View stacks", "Add new stack", "Manage stacks", "Update a stack", "Delete stack"])
    );
  }

  private static string GetOption(string Name)
  {
    AnsiConsole.Clear();
    return AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title($"[bold underline red]{Name}[/]")
            .PageSize(6)
            .AddChoices(["Go back", "View flashcards", "Add flashcard", "Update flashcard", "Delete flashcard"])
    );
  }


  private static string GetStudyOption()
  {
    AnsiConsole.Clear();
    return AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold underline red]Study[/]")
            .PageSize(8)
            .AddChoices(["Go back", "View sessions", "Start new session", "View taken sessions", "View scores"])
    );
  }

  private static void InitStudy()
  {
    bool end = false;
    while (!end)
    {
      string option = GetStudyOption();
      switch (option)
      {
        case "Go back":
          end = true;
          break;
        case "View sessions":
          SessionController.DisplaySessions();
          ContinueInput();
          break;
        case "Start new session":
          SessionController.AddSession();
          ContinueInput();
          break;
        case "View taken sessions":
          SessionController.DisplaySessionsReport();
          ContinueInput();
          break;
        case "View scores":
          SessionController.DisplayAverageScore();
          ContinueInput();
          break;
      }
    }
  }

  public static void Start()
  {
    bool end = false;
    while (!end)
    {
      string option = GetMenuOption();
      switch (option)
      {
        case "Manage stacks":
          InitStacks();
          break;
        case "Study Sessions":
          InitStudy();
          break;
        case "Exit":
          end = true;
          break;
      }
    }
  }

}
