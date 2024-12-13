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
      .AddColumn("[bold blue]Stack Name[/]")
      .AddColumn("[bold yellow]Date[/]")
      .AddColumn("[bold green]Score(%)[/]");

    List<Session> sessions = sessionsRepository.GetSessions();

    foreach (Session session in sessions)
    {
      double score = session.maxScore / (double)session.score * 100;
      Console.WriteLine($"Score: {score}, {session.score}, {session.maxScore}");
      table.AddRow(
        $"[bold blue]{session.stackName}[/]",
        $"[bold yellow]{session.dateTime.ToString(CultureInfo.CurrentCulture)}[/]",
        $"[bold green]{score:0.00}%[/]"
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
        .AddColumn("[bold navy]January[/]")
        .AddColumn("[bold navy]February[/]")
        .AddColumn("[bold navy]March[/]")
        .AddColumn("[bold navy]April[/]")
        .AddColumn("[bold navy]May[/]")
        .AddColumn("[bold navy]June[/]")
        .AddColumn("[bold navy]July[/]")
        .AddColumn("[bold navy]August[/]")
        .AddColumn("[bold navy]September[/]")
        .AddColumn("[bold navy]October[/]")
        .AddColumn("[bold navy]November[/]")
        .AddColumn("[bold navy]December[/]");

    List<SessionDto> sessionsReport = sessionsRepository.GetSessionsReport(year);

    foreach (SessionDto stackSession in sessionsReport)
    {
      table.AddRow(
          $"[bold blueviolet]{stackSession.Name}[/]",
          $"[bold blueviolet]{stackSession.Sessions["January"]}[/]",
          $"[bold blueviolet]{stackSession.Sessions["February"]}[/]",
          $"[bold blueviolet]{stackSession.Sessions["March"]}[/]",
          $"[bold blueviolet]{stackSession.Sessions["April"]}[/]",
          $"[bold blueviolet]{stackSession.Sessions["May"]}[/]",
          $"[bold blueviolet]{stackSession.Sessions["June"]}[/]",
          $"[bold blueviolet]{stackSession.Sessions["July"]}[/]",
          $"[bold blueviolet]{stackSession.Sessions["August"]}[/]",
          $"[bold blueviolet]{stackSession.Sessions["September"]}[/]",
          $"[bold blueviolet]{stackSession.Sessions["October"]}[/]",
          $"[bold blueviolet]{stackSession.Sessions["November"]}[/]",
          $"[bold blueviolet]{stackSession.Sessions["December"]}[/]"
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
        .AddColumn("[bold blueviolet]Stack name[/]")
        .AddColumn("[bold blueviolet]January[/]")
        .AddColumn("[bold blueviolet]February[/]")
        .AddColumn("[bold blueviolet]March[/]")
        .AddColumn("[bold blueviolet]April[/]")
        .AddColumn("[bold blueviolet]May[/]")
        .AddColumn("[bold blueviolet]June[/]")
        .AddColumn("[bold blueviolet]July[/]")
        .AddColumn("[bold blueviolet]August[/]")
        .AddColumn("[bold blueviolet]September[/]")
        .AddColumn("[bold blueviolet]October[/]")
        .AddColumn("[bold blueviolet]November[/]")
        .AddColumn("[bold blueviolet]December[/]");

    List<ScoreDto> sessionsScore = sessionsRepository.GetSessionsScore(year);

    foreach (ScoreDto stackScore in sessionsScore)
    {
      table.AddRow(
          $"[bold navy]{stackScore.Name}[/]",
          $"[bold navy]{stackScore.Scores["January"]}%[/]",
          $"[bold navy]{stackScore.Scores["February"]}%[/]",
          $"[bold navy]{stackScore.Scores["March"]}%[/]",
          $"[bold navy]{stackScore.Scores["April"]}%[/]",
          $"[bold navy]{stackScore.Scores["May"]}%[/]",
          $"[bold navy]{stackScore.Scores["June"]}%[/]",
          $"[bold navy]{stackScore.Scores["July"]}%[/]",
          $"[bold navy]{stackScore.Scores["August"]}%[/]",
          $"[bold navy]{stackScore.Scores["September"]}%[/]",
          $"[bold navy]{stackScore.Scores["October"]}%[/]",
          $"[bold navy]{stackScore.Scores["November"]}%[/]",
          $"[bold navy]{stackScore.Scores["December"]}%[/]"
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
