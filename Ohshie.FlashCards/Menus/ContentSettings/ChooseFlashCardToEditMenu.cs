using Ohshie.FlashCards.CardsManager;
using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.Menus;

public class ChooseFlashCardToEditMenu
{
    private readonly FlashcardService _flashcardService = new();
    private List<FlashCardDto> _flashCardDtos = new();
    private readonly DeckDto _deckDto;

    public ChooseFlashCardToEditMenu(DeckDto deckDto)
    {
        _deckDto = deckDto;
    }

    public void Initialize()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("Settings"));

        if (!Verify.DeckExist(where: "go back")) return;

        AnsiConsole.Write(FlashcardsDisplay());

        var userChoice = Menu();

        switch (userChoice)
        {
            case "Create new Flashcard":
            {
                FlashcardCreator flashcardCreator = new FlashcardCreator(_deckDto.Id);
                flashcardCreator.Create();
                break;
            }
            default:
            {
                OpenEditFlashcardMenu(userChoice);
                break;
            }
        }
    }
    
    private string Menu()
    {
        var userChoice = Selector();
        return userChoice;
    }
    
    private void OpenEditFlashcardMenu(string userChoice)
    {
        var chosenCard = ConvertIdAndNameToDeckDto(userChoice);

        EditFlashCardMenu editFlashCardMenu;
        if (_flashCardDtos.Count < 2)
        {
            editFlashCardMenu = new EditFlashCardMenu(chosenCard!, true);
        }
        else
        {
            editFlashCardMenu = new EditFlashCardMenu(chosenCard!, false);
        }

        editFlashCardMenu.Initialize();
    }

    private string Selector()
    {
        return AnsiConsole.Prompt
        (
            new SelectionPrompt<string>()
                .PageSize(5)
                .Title("Select flashcard to edit")
                .AddChoices(CreateMenuChoices())
        );
    }

    private List<string> CreateMenuChoices()
    {
        List<string> menuChoicesList = new();
        
        menuChoicesList.Add("Create new Flashcard");
        foreach (var flashCard in _flashCardDtos)
        {
            menuChoicesList.Add(flashCard.DtoId+". "+flashCard.Name);
        }

        return menuChoicesList;
    }

    private FlashCardDto? ConvertIdAndNameToDeckDto(string idAndName)
    {
        string[] splittedIdAndName = idAndName.Split(".");
        return _flashCardDtos.FirstOrDefault(dd => dd.DtoId == Convert.ToInt32(splittedIdAndName[0]));
    }
    
    private Table FlashcardsDisplay()
    {
        var table = new Table();

        table.AddColumn("Id");
        table.AddColumn("Name");
        table.AddColumn("Content");

        _flashCardDtos = _flashcardService.FlashCardDtoList(_deckDto);

        foreach (var flashCard in _flashCardDtos)
        {
            table.AddRow($"{flashCard.DtoId}",
                $"{flashCard.Name}",
                $"{flashCard.Content}");
        }

        return table;
    }
}