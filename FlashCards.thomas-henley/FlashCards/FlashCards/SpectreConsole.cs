using Serilog.Core;
using Spectre.Console;

namespace FlashCards;

public class SpectreConsole
{
    public string MainMenu()
    {
        var prompt = new SelectionPrompt<string>()
            .Title("What would you like to do?")
            .AddChoiceGroup("Manage Collection", "Add Stack", "Delete Stack", "Add Card", "Delete Card")
            .AddChoiceGroup("Study", "Start Study Session","Review Sessions", "Review Sessions by Stack")
            .AddChoices("Exit");

        return AnsiConsole.Prompt(prompt);
    }

    public string AddStackMenu(List<string>? stackNames)
    {
        var prompt = new TextPrompt<string>("Enter name for the new stack (or 0 to exit):")
            .Validate(name => !stackNames.Contains(name))
            .ValidationErrorMessage("[red bold]Stack name already in use![/]");
        
        return AnsiConsole.Prompt(prompt);
    }

    public Stack SelectStackMenu(List<Stack> stacks, string message)
    {
        var prompt = new SelectionPrompt<Stack>()
            .Title(message)
            .AddChoices(stacks)
            .UseConverter(stack => stack.Name);
        
        return AnsiConsole.Prompt(prompt);
    }

    public (string, string) NewCardMenu()
    {
        var frontPrompt = new TextPrompt<string>("What is on the front of the card?")
            .Validate(frontText => !string.IsNullOrWhiteSpace(frontText))
            .ValidationErrorMessage("[red bold]Card front cannot be blank.[/]");
        
        var front = AnsiConsole.Prompt(frontPrompt);

        var backPrompt = new TextPrompt<string>("What is on the back of the card?");
        
        var back = AnsiConsole.Prompt(backPrompt);
        
        return (front, back);
    }

    public CardDTO DeleteCardMenu(List<CardDTO> cards, string message)
    {
        var prompt = new SelectionPrompt<CardDTO>()
            .Title(message)
            .AddChoices(cards)
            .UseConverter(card => $"{card.Name}:\t\t {card.Front}");
        
        var selectedCard = AnsiConsole.Prompt(prompt);

        return selectedCard;
    }

    public string StudyCard(Card card)
    {
        var prompt = new TextPrompt<string>(card.Front);
        
        return AnsiConsole.Prompt(prompt);
    }

    public void ShowSessions(List<StudySessionDto> sessions)
    {
        var table = new Table()
            .AddColumns("Date", "Stack", "Correct", "Score");

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Date,
                session.Name,
                $"{session.Correct}/{session.Total}",
                $"{ColorizeScore(session.GetScore())}"
            );
        }
        
        AnsiConsole.Write(table);
    }

    private string ColorizeScore(int score)
    {
        return score switch
        {
            > 90 => "[green bold]" + score + "%[/]",
            > 60 => "[yellow]" + score + "%[/]",
            _ => "[red]" + score + "%[/]"
        };
    }

    public bool Confirm(string message)
    {
        var prompt = new ConfirmationPrompt(message);
        
        return AnsiConsole.Prompt(prompt);
    }

    public void Red(string message)
    {
        AnsiConsole.Markup($"[red bold]\n{message}\n[/]");
    }
    
    public void Error(string message)
    {
        Red($"ERROR: {message}");
    }

    public void Success(string message)
    {
        AnsiConsole.MarkupLine($"[green bold]\n{message}\n[/]");
    }
}