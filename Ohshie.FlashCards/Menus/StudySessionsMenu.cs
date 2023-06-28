using Ohshie.FlashCards.StudySessionManager;

namespace Ohshie.FlashCards.Menus;

public class StudySessionsMenu : MenuBase
{
    private readonly StudySessionService _sessionService = new();
    
    protected override string[] MenuItems { get; } = 
        {"Go back"};

    public new void Initialize()
    {
        AnsiConsole.Clear();

        if(!Verify.SessionsExist("go back")) return;
        
        bool chosenExit = false;
        while (!chosenExit)
        {
            chosenExit = Menu();
        }
    }
    
    protected override bool Menu()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("Your previous studying sessions"));
        
        AnsiConsole.Write(SessionTable());
        
        var userChoice = MenuBuilder(3);
        
        switch (userChoice)
        {
            case "Go back":
            {
                return true;
            }
        }

        return false;
    }
    
    private Table SessionTable()
    {
        var table = new Table();

        table.AddColumn("Study date");
        table.AddColumn("Deck name");
        table.AddColumn("Solved cards");

        var sessionsList = _sessionService.SessionsToDisplay();

        foreach (var session in sessionsList)
        {
            table.AddRow($"{session.Date}",
                $"{session.DeckName}",
                $"{session.FlashcardsSolved}");
        }

        return table;
    }
}