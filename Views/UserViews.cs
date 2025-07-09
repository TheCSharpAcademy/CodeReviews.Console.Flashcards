namespace DotNETConsole.Flashcards.Views;

using Spectre.Console;
using DTO;

public class UserViews()
{
    public void PrintStackTable(List<StackViewDto> stacks)
    {
        var table = new Table();
        table.AddColumns(new[] { "No", "Title" });
        int i = 1;
        foreach (StackViewDto stack in stacks)
        {
            table.AddRow($"{i}", $"{stack.Name}");
            i++;
        }
        AnsiConsole.Write(table);
    }


    public void PrintCardTable(List<CardViewDto> cards)
    {
        var table = new Table();
        table.AddColumns(new[] { "No", "Question", "Stack" });
        int i = 1;
        foreach (CardViewDto card in cards)
        {
            table.AddRow($"{i}", $"{card.Question}", $"{card.Stack}");
            i++;
        }
        AnsiConsole.Write(table);
    }


    public void Tost(string message, string type = "default")
    {
        switch (type)
        {
            case "defalut":
                AnsiConsole.MarkupLine(message);
                break;
            case "error":
                AnsiConsole.MarkupLine($"[red bold]Error: {message}[/]");
                break;
            case "success":
                AnsiConsole.MarkupLine($"[green bold]Success: {message}[/]");
                break;
            case "info":
                AnsiConsole.MarkupLine($"[yellow bold]Info: {message}[/]");
                break;
        }
    }

    public void ContentSummary(string text)
    {
        AnsiConsole.Write(new Align(new Markup($"[Green bold italic]{text}[/]"), HorizontalAlignment.Center, VerticalAlignment.Top));
    }
}
