using System.Globalization;
using Flashcards.Mateusz_Platek.Models;
using Flashcards.Mateusz_Platek.Repositories;
using Spectre.Console;

namespace Flashcards.Mateusz_Platek.Controllers;

public static class SessionController
{
    private static SessionsRepository sessionsRepository = new SessionsRepository();
    private static FlashcardsRepository flashcardsRepository = new FlashcardsRepository();
    
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
            double score = ((double)session.score / session.maxScore) * 100;
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

        List<StackSessionDTO> sessionsReport = sessionsRepository.GetSessionsReport(year);

        foreach (StackSessionDTO stackSession in sessionsReport)
        {
            table.AddRow(
                $"[bold navy]{stackSession.name}[/]",
                $"[bold green]{stackSession.sessions["January"]}[/]",
                $"[bold blueviolet]{stackSession.sessions["February"]}[/]",
                $"[bold mediumpurple]{stackSession.sessions["March"]}[/]",
                $"[bold greenyellow]{stackSession.sessions["April"]}[/]",
                $"[bold orchid]{stackSession.sessions["May"]}[/]",
                $"[bold violet]{stackSession.sessions["June"]}[/]",
                $"[bold sandybrown]{stackSession.sessions["July"]}[/]",
                $"[bold lime]{stackSession.sessions["August"]}[/]",
                $"[bold darkmagenta]{stackSession.sessions["September"]}[/]",
                $"[bold darkseagreen]{stackSession.sessions["October"]}[/]",
                $"[bold darkblue]{stackSession.sessions["November"]}[/]",
                $"[bold purple]{stackSession.sessions["December"]}[/]"
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

        List<StackScoreDTO> sessionsScore = sessionsRepository.GetSessionsScore(year);
        
        foreach (StackScoreDTO stackScore in sessionsScore)
        {
            table.AddRow(
                $"[bold navy]{stackScore.name}[/]",
                $"[bold green]{stackScore.scores["January"]}%[/]",
                $"[bold blueviolet]{stackScore.scores["February"]}%[/]",
                $"[bold mediumpurple]{stackScore.scores["March"]}%[/]",
                $"[bold greenyellow]{stackScore.scores["April"]}%[/]",
                $"[bold orchid]{stackScore.scores["May"]}%[/]",
                $"[bold violet]{stackScore.scores["June"]}%[/]",
                $"[bold sandybrown]{stackScore.scores["July"]}%[/]",
                $"[bold lime]{stackScore.scores["August"]}%[/]",
                $"[bold darkmagenta]{stackScore.scores["September"]}%[/]",
                $"[bold darkseagreen]{stackScore.scores["October"]}%[/]",
                $"[bold darkblue]{stackScore.scores["November"]}%[/]",
                $"[bold purple]{stackScore.scores["December"]}%[/]"
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
        
        List<Flashcard> flashcards = flashcardsRepository.GetFlashcardsOfStack(stack.name);
        if (flashcards.Count == 0)
        {
            AnsiConsole.Write(new Markup("[bold red]Stack is empty[/]\n"));
            return;
        }
        
        int score = 0;
        
        foreach (Flashcard flashcard in flashcards)
        {
            string input = AnsiConsole.Prompt(
                new TextPrompt<string>($"[bold green]Word:[/] {flashcard.word}\n[bold purple]Translation:[/]")
                );
            
            if (input.ToLower() == flashcard.translation.ToLower())
            {
                AnsiConsole.Write(new Markup("[bold green]Correct answer[/]\n"));
                score++;
            }
            else
            {
                AnsiConsole.Write(new Markup($"[bold red]Incorrect answer ({flashcard.translation})[/]\n")); 
            }
            Menu.ContinueInput();
            AnsiConsole.Clear();
        }

        double percent = (double) score / flashcards.Count * 100;
        AnsiConsole.Write(
            new Markup($"[bold darkorange]Your score: {score}/{flashcards.Count} ({percent:0.00}%)[/]\n")
        );
        
        sessionsRepository.AddSession(stack.stackId, score, DateTime.Now, flashcards.Count);
    }
}