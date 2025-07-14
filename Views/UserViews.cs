namespace DotNETConsole.Flashcards.Views;

using Spectre.Console;
using DTO;
using Helper;

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

    public void PrintSessionTable(List<SessionDto> sessions)
    {
        var table = new Table();
        table.AddColumns(new[] { "No", "Stack", "Date", "Score" });
        int i = 1;
        foreach (SessionDto session in sessions)
        {
            table.AddRow($"{i}", $"{session.Stack}", $"{session.LogDate.ToLocalTime().Date}", $"{session.Score}");
            i++;
        }
        AnsiConsole.Write(table);
    }

    public int CheckKnowledge(List<CardViewDto> cards, string stack)
    {
        int score = 0;
        int count = 1;
        UserInput input = new UserInput();
        UserViews view = new UserViews();
        foreach (CardViewDto card in cards)
        {
            Console.Clear();
            this.ContentSummary($"{stack} - Q({count}/{cards.Count}) - Score:{score}");
            AnsiConsole.WriteLine(card.Question);
            string ans = input.UserAnswer();
            if (String.Equals(ans.ToLower(), card.Answer.ToLower()))
            {
                score += 1;
                input.ContinueInput("Correct!!!");
            }
            else
            {
                input.ContinueInput($"Wrong correct answer is: {card.Answer}");
            }
            count++;
        }
        return score;
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
