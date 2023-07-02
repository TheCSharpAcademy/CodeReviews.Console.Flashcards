using Ohshie.FlashCards.CardsManager;

namespace Ohshie.FlashCards.Menus;

public class EditFlashCardMenu : MenuBase
{
    public EditFlashCardMenu(FlashCardDto flashCardDto, bool lastFlashCard)
    {
        FlashCardDto = flashCardDto;

        if (lastFlashCard)
        {
            MenuItems = new[] { "Rewrite Flashcard", "Go back" };
        }
    }

    private FlashCardDto FlashCardDto;
    private readonly FlashcardEditor _flashcardEditor = new();
    
    protected override string[] MenuItems { get; } =
    {
        "Rewrite Flashcard",
        "Delete Flashcard",
        "Go back"
    };
    
    protected override bool Menu()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("Settings"));
        AnsiConsole.Write(FlashcardDisplay());
        
        string userChoice = MenuBuilder(5);
        
        switch (userChoice)
        {
            case "Rewrite Flashcard":
            {
                _flashcardEditor.RewriteFlashcard(FlashCardDto);
                break;
            }
            case "Delete Flashcard":
            {
                FlashcardService flashcardService = new();
                flashcardService.DeleteFlashcard(FlashCardDto);
                return true;
            }
            case "Go back":
            {
                return true;
            }
        }

        return false;
    }

    private Table FlashcardDisplay()
    {
        var table = new Table();
        
        table.AddColumn("Name");
        table.AddColumn("Content");

        table.AddRow($"{FlashCardDto.Name}",
            $"{FlashCardDto.Content}");

        return table;
    }
}