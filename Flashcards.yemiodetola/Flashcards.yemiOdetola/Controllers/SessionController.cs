using System.Globalization;
using Flashcards.yemiOdetola.Models;
using Flashcards.yemiOdetola.Repositories;
using Spectre.Console;

namespace Flashcards.yemiOdetola.Controllers;

public static class SessionController
{

  private static SessionsRepository sessionsRepository = new SessionsRepository();
  private static FlashCardsRepository flashcardsRepository = new FlashCardsRepository();

  public static void DisplaySessions()
  {
    Table table = new Table()
        .Title("[bold red]Sessions[/]")
        .AddColumn("[bold blue]stack name[/]")
        .AddColumn("[bold green]score[/]")
        .AddColumn("[bold yellow]date[/]");

    List<Session> sessions = sessionsRepository.GetSessions();

    foreach (Session session in sessions)
    {
      double score = (double)session.score / session.maxScore * 100;
      table.AddRow(
          $"[bold blue]{session.stackName}[/]",
          $"[bold green]{score:0.00}%[/]",
          $"[bold yellow]{session.dateTime.ToString(CultureInfo.CurrentCulture)}[/]"
          );
    }

    AnsiConsole.Clear();
    AnsiConsole.Write(table);
  }

  public static void DisplaySessionsReport()
  {
    int year = AnsiConsole.Prompt(
        new TextPrompt<int>("[bold darkorange]Insert year:[/]")
            .ValidationErrorMessage("Incorrect year")
    );

    Table table = new Table()
        .Title($"[bold red]Sessions per month: {year}[/]")
        .AddColumn("[bold navy]Stack name[/]")
        .AddColumn("[bold green]January[/]")
        .AddColumn("[bold blueviolet]February[/]")
        .AddColumn("[bold mediumpurple]March[/]")
        .AddColumn("[bold greenyellow]April[/]")
        .AddColumn("[bold orchid]May[/]")
        .AddColumn("[bold violet]June[/]")
        .AddColumn("[bold sandybrown]July[/]")
        .AddColumn("[bold lime]August[/]")
        .AddColumn("[bold darkmagenta]September[/]")
        .AddColumn("[bold darkseagreen]October[/]")
        .AddColumn("[bold darkblue]November[/]")
        .AddColumn("[bold purple]December[/]");

    List<SessionDto> sessionsReport = sessionsRepository.GetSessionsReport(year);

    foreach (SessionDto stackSession in sessionsReport)
    {
      table.AddRow(
          $"[bold navy]{stackSession.Name}[/]",
          $"[bold green]{stackSession.Sessions["January"]}[/]",
          $"[bold blueviolet]{stackSession.Sessions["February"]}[/]",
          $"[bold mediumpurple]{stackSession.Sessions["March"]}[/]",
          $"[bold greenyellow]{stackSession.Sessions["April"]}[/]",
          $"[bold orchid]{stackSession.Sessions["May"]}[/]",
          $"[bold violet]{stackSession.Sessions["June"]}[/]",
          $"[bold sandybrown]{stackSession.Sessions["July"]}[/]",
          $"[bold lime]{stackSession.Sessions["August"]}[/]",
          $"[bold darkmagenta]{stackSession.Sessions["September"]}[/]",
          $"[bold darkseagreen]{stackSession.Sessions["October"]}[/]",
          $"[bold darkblue]{stackSession.Sessions["November"]}[/]",
          $"[bold purple]{stackSession.Sessions["December"]}[/]"
      );
    }

    AnsiConsole.Clear();
    AnsiConsole.Write(table);
  }

  public static void DisplayAverageScore()
  {
    int year = AnsiConsole.Prompt(
        new TextPrompt<int>("[bold darkorange]Insert year:[/]")
            .ValidationErrorMessage("Incorrect year")
    );

    Table table = new Table()
        .Title($"[bold red]Average scores: {year}[/]")
        .AddColumn("[bold navy]Stack name[/]")
        .AddColumn("[bold green]January[/]")
        .AddColumn("[bold blueviolet]February[/]")
        .AddColumn("[bold mediumpurple]March[/]")
        .AddColumn("[bold greenyellow]April[/]")
        .AddColumn("[bold orchid]May[/]")
        .AddColumn("[bold violet]June[/]")
        .AddColumn("[bold sandybrown]July[/]")
        .AddColumn("[bold lime]August[/]")
        .AddColumn("[bold darkmagenta]September[/]")
        .AddColumn("[bold darkseagreen]October[/]")
        .AddColumn("[bold darkblue]November[/]")
        .AddColumn("[bold purple]December[/]");

    List<ScoreDto> sessionsScore = sessionsRepository.GetSessionsScore(year);

    foreach (ScoreDto stackScore in sessionsScore)
    {
      table.AddRow(
          $"[bold navy]{stackScore.Name}[/]",
          $"[bold green]{stackScore.Scores["January"]}%[/]",
          $"[bold blueviolet]{stackScore.Scores["February"]}%[/]",
          $"[bold mediumpurple]{stackScore.Scores["March"]}%[/]",
          $"[bold greenyellow]{stackScore.Scores["April"]}%[/]",
          $"[bold orchid]{stackScore.Scores["May"]}%[/]",
          $"[bold violet]{stackScore.Scores["June"]}%[/]",
          $"[bold sandybrown]{stackScore.Scores["July"]}%[/]",
          $"[bold lime]{stackScore.Scores["August"]}%[/]",
          $"[bold darkmagenta]{stackScore.Scores["September"]}%[/]",
          $"[bold darkseagreen]{stackScore.Scores["October"]}%[/]",
          $"[bold darkblue]{stackScore.Scores["November"]}%[/]",
          $"[bold purple]{stackScore.Scores["December"]}%[/]"
      );
    }

    AnsiConsole.Clear();
    AnsiConsole.Write(table);
  }

  public static void AddSession()
  {
    Stack? stack = StackController.SelectStack();
    if (stack == null)
    {
      return;
    }

    List<FlashCard> flashcards = flashcardsRepository.GetCardsStack(stack.Name);
    if (flashcards.Count == 0)
    {
      AnsiConsole.Write(new Markup("[bold red]Stack is empty[/]\n"));
      return;
    }

    int score = 0;

    foreach (FlashCard flashcard in flashcards)
    {
      string input = AnsiConsole.Prompt(
          new TextPrompt<string>($"[bold green]Word:[/] {flashcard.Word}\n[bold purple]Category:[/]")
          );

      if (input.ToLower() == flashcard.Category.ToLower())
      {
        AnsiConsole.Write(new Markup("[bold green]Correct answer[/]\n"));
        score++;
      }
      else
      {
        AnsiConsole.Write(new Markup($"[bold red]Incorrect answer ({flashcard.Category})[/]\n"));
      }
      MenuOptions.ContinueInput();
      AnsiConsole.Clear();
    }

    double percent = (double)score / flashcards.Count * 100;
    AnsiConsole.Write(
        new Markup($"[bold darkorange]Your score: {score}/{flashcards.Count} ({percent:0.00}%)[/]\n")
    );

    sessionsRepository.AddSession(stack.Id, score, DateTime.Now, flashcards.Count);
  }

}
