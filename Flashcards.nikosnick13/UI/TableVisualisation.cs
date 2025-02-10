using Flashcards.nikosnick13.Controllers;
using Flashcards.nikosnick13.DTOs;
using Flashcards.nikosnick13.Models;
using Spectre.Console;
using static System.Console;


namespace Flashcards.nikosnick13.UI;

internal class TableVisualisation
{
    private readonly FlashcardController _flashcardController;

    public TableVisualisation()
    {
        _flashcardController = new FlashcardController();
    }

    internal static void ShowTable(List<DetailStackDTO> recordsList)
    {

        /*
        var StackWithSequentialIds = recordsList.Select((stacks, index) => new DetailStackDTO
        {
            Id = index + 1,
            Name = stacks.Name

        });*/

        var table = new Table();
        table.Title("Your Stacks");
        table.AddColumn("Id");
        table.AddColumn("Name");

        foreach (var record in recordsList)
        {
            table.AddRow(record.Id.ToString(), record.Name ?? "[red]NULL[/]");

        }

        AnsiConsole.Write(table);
        AnsiConsole.Prompt(new TextPrompt<string>("\nPress [green]Enter[/] to continue...").AllowEmpty());

    }

    internal static void ShowStackTable(List<DetailStackDTO> recordsList)
    {
        if (recordsList == null || recordsList.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No stacks to display.[/]");
            return;
        }

        var table = new Table();
        table.Title("Your Stack");
        table.AddColumn("Id");
        table.AddColumn("Name");

        foreach (var record in recordsList)
        {
            table.AddRow(record.Id.ToString(), record.Name ?? "[red]NULL[/]");
        }

        AnsiConsole.Write(table);
        AnsiConsole.Prompt(new TextPrompt<string>("\nPress [green]Enter[/] to continue...").AllowEmpty());
    }

    public static void DisplayFlashcards(List<DetailFlashcardDTO> recordList)
    {
       
        if (recordList == null || !recordList.Any())
        {
            AnsiConsole.MarkupLine("[red]No flashcards found in the database.[/]");
            return;
        }

        /*
        var flashcardsWithSequentialIds = recordList.Select((flashcard, index) => new DetailFlashcardDTO
        {
            Id = index + 1,
            Question = flashcard.Question,
            Answer = flashcard.Answer,
            StackId = flashcard.StackId
        }) ;*/

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Question");
        table.AddColumn("Answer");
        table.AddColumn("Stack ID");

        foreach (var flashcard in recordList)
        {
            table.AddRow(
                flashcard.Id.ToString(),
                flashcard.Question,
                flashcard.Answer,
                flashcard.StackId.ToString());
        }

        AnsiConsole.Write(table);
    }

    public static void DisplayOneFlashcard(List<DetailFlashcardDTO> recordList) {


        if (recordList == null || recordList.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No Flashcard to display.[/]");
            return;
        }
        var flashcardsWithSequentialIds = recordList.Select((flashcard, index) => new DetailFlashcardDTO
        {
            Id = index + 1,                    
            Question = flashcard.Question,
            Answer = flashcard.Answer,
            StackId = flashcard.StackId,       
        }).ToList();

        var table = new Table();
        table.AddColumn("Display ID");
        table.AddColumn("Question");
        table.AddColumn("Answer");
        table.AddColumn("Stack ID");

        foreach (var flashcard in flashcardsWithSequentialIds)
        {
            table.AddRow(
                flashcard.Id.ToString(),
                flashcard.Question,
                flashcard.Answer,
                flashcard.StackId.ToString()
            );
        }
        AnsiConsole.Write(table);
        AnsiConsole.Prompt(new TextPrompt<string>("\nPress [green]Enter[/] to continue...").AllowEmpty());

    }

}