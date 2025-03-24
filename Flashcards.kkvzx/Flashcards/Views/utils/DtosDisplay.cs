using Flashcards.Models.Dtos;
using Spectre.Console;

namespace Flashcards.Views.utils;

public static class DtosDisplay
{
    public static void DisplayStacks(List<StackDto> stacks)
    {
        AnsiConsole.Clear();
        if (stacks.Count == 0)
        {
            AnsiConsole.Markup(
                "[red]Currently there are no stacks to manage.[/]\nFirstly create stacks in order to manage them\n");
        }
        else
        {
            var table = new Table().LeftAligned();
            table.Border = TableBorder.Rounded;

            table.Title = new TableTitle("[darkcyan]Stacks[/]");
            table.AddColumn(new TableColumn("Idx").RightAligned());
            table.AddColumn(new TableColumn("Name").LeftAligned());

            for (var i = 0; i < stacks.Count; i++)
            {
                table.AddRow($"{i}", stacks[i].Name);
            }

            AnsiConsole.Write(table);
        }
    }

    public static void DisplayFlashcards(List<FlashcardDto> flashcards, string currentStack)
    {
        if (flashcards.Count == 0)
        {
            AnsiConsole.Markup(
                "[red]Stack is empty and there are no flashcards to manage.[/]\nFirstly create flashcards in order to manage them\n");
        }
        else
        {
            var table = new Table().LeftAligned();
            table.Border = TableBorder.Rounded;
            table.Title = new TableTitle($"[darkcyan]{currentStack} Flashcards[/]");
            table.AddColumn(new TableColumn("Idx").RightAligned());
            table.AddColumn(new TableColumn("Front").Centered());
            table.AddColumn(new TableColumn("Back").Centered());

            for (var i = 0; i < flashcards.Count; i++)
            {
                table.AddRow($"{i}", flashcards[i].FrontText, flashcards[i].BackText);
            }

            AnsiConsole.Write(table);
        }
    }

    public static void DisplaySessions(List<SessionDto> sessions, List<StackDto> stacks)
    {
        if (sessions.Count == 0)
        {
            AnsiConsole.Markup(
                "[red]There are no sessions yet.[/]\nBegin your learning in order to view sessions\n");
        }
        else
        {
            var table = new Table().LeftAligned();
            table.Border = TableBorder.Rounded;
            table.Title = new TableTitle($"[darkcyan]Learning sessions[/]");
            table.AddColumn(new TableColumn("Idx").RightAligned());
            table.AddColumn(new TableColumn("Language").Centered());
            table.AddColumn(new TableColumn("Date").Centered());
            table.AddColumn(new TableColumn("Score").Centered());

            for (var i = 0; i < sessions.Count; i++)
            {
                var idx = $"{i}";
                var shortDateTime = sessions[i].OccurenceDate.ToString("dd/MM/yyyy");
                var name = stacks.Find(stack => stack.Id == sessions[i].StackId)?.Name ?? "Unknown";
                var score = sessions[i].Score.ToString();

                table.AddRow(idx, name, shortDateTime, score);
            }

            AnsiConsole.Write(table);
        }
    }
}