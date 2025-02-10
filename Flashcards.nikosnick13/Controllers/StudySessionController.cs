using System;
using System.Collections.Generic;
using FlashStudy.Models;
using Flashcards.nikosnick13.DTOs;
using FlashStudy.Data;
using Spectre.Console;
using Flashcards.nikosnick13;
using Flashcards.nikosnick13.Controllers;

namespace FlashStudy.Controllers;

public class StudyController
{
    private readonly StackController _stackController;
    private readonly FlashcardController _flashcardController;

    public StudyController()
    {
        _stackController = new StackController();
        _flashcardController = new FlashcardController();
    }

    public static void StartStudySession() 
    {


        var stacks = _stackController.ViewAllStacks();


        if (stacks.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No stacks available to study.[/]");
            return;
        }

        // Εμφάνιση των διαθέσιμων στοίβων
        AnsiConsole.MarkupLine("\n[green]Choose a stack to study:[/]");
        for (int i = 0; i < stacks.Count; i++)
            AnsiConsole.MarkupLine($"[yellow]{i + 1}[/] {stacks[i].Name}");

        int selectedIndex = Validation.GetValidNumber("\nEnter the stack number: ", 1, stacks.Count);
        int stackId = stacks[selectedIndex - 1].Id;

        // Ανάκτηση flashcards για τη στοίβα
        var flashcards = _flashcardController.GetFlashcardById(stackId);

        if (flashcards.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]This stack has no flashcards.[/]");
            return;
        }

        int correctAnswers = 0;

        foreach (var card in flashcards)
        {
            AnsiConsole.MarkupLine($"\n[yellow]Question:[/] {card.Question}");
            string userAnswer = Console.ReadLine().Trim().ToLower();

            if (userAnswer == card.Answer.ToLower())
            {
                correctAnswers++;
                AnsiConsole.MarkupLine("[green]Correct![/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Wrong![/] The correct answer was: [blue]{card.Answer}[/]");
            }
        }

        int score = (int)((double)correctAnswers / flashcards.Count * 100);
        AnsiConsole.MarkupLine($"\n[green]Your score: {score}%[/]");

        StudySessionRepository.AddStudySession(new StudySession(score, stackId));

        AnsiConsole.MarkupLine("[green]Study session saved![/]");
    }

    public static void ViewStudySessions()
    {
        var sessions = StudySessionRepository.StudySessions;

        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No study sessions found.[/]");
            return;
        }

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Score");
        table.AddColumn("Date");
        table.AddColumn("Stack");

        foreach (var session in sessions)
            table.AddRow(session.Id.ToString(), $"{session.Score}%", session.Date.ToString("yyyy-MM-dd HH:mm"), session.StackName);

        AnsiConsole.Write(table);
    }
}
