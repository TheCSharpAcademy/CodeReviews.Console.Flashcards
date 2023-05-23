using ConsoleTableExt;
using Flashcards.CoreyJordan.DTOs;

namespace Flashcards.CoreyJordan.Display;
internal class CardUI
{
    internal List<MenuModel> EditListMenu { get; } = new()
    {
        new MenuModel("1", "Add New Card"),
        new MenuModel("2", "Delete Card"),
        new MenuModel("X", "Exit Pack Editor"),
    };
    internal List<MenuModel> CardManagerMenu { get; } = new()
    {
        new MenuModel("1", "Add New Card"),
        new MenuModel("2", "Delete Card"),
        new MenuModel("3", "View All Cards"),
        new MenuModel("X", "Exit Card Manager"),
    };
    private ConsoleUI UIConsole { get; set; } = new();
    private InputModel UserInput { get; set; } = new();

    internal void DisplayCards(List<CardFaceDTO> cards)
    {
        UIConsole.TitleBar("CARDS");

        ConsoleTableBuilder
            .From(cards)
            .WithColumn("#","FRONT", "BACK")
            .WithFormat(ConsoleTableBuilderFormat.MarkDown)
            .ExportAndWriteLine(TableAligntment.Center);
        Console.WriteLine();
    }

    internal void DisplayCards(List<CardDTO> cards)
    {
        UIConsole.TitleBar("CARDS");

        ConsoleTableBuilder
            .From(cards)
            .WithColumn("#", "FRONT", "BACK", "DECK")
            .WithFormat(ConsoleTableBuilderFormat.MarkDown)
            .ExportAndWriteLine(TableAligntment.Center);
        Console.WriteLine();
    }

    internal void Menu(List<MenuModel> menu)
    {
        ConsoleTableBuilder
            .From(menu)
            .WithColumn("")
            .WithFormat(ConsoleTableBuilderFormat.MarkDown)
            .ExportAndWriteLine(TableAligntment.Center);
        Console.WriteLine();
    }

    internal string GetCardChoice(List<CardFaceDTO> cards)
    {
        int index = 0;
        bool inRange = false;

        while (inRange == false)
        {
            index = UserInput.GetInt("Select a pack number: ");
            inRange = cards.Any(x => x.CardNumber == index);

            if (inRange == false)
            {
                UIConsole.Prompt("Pack number not listed. Please try again.");
                DisplayCards(cards);
            }
        }

        List<CardFaceDTO> questions = cards.Where(x => x.CardNumber == index).ToList();
        return questions[0].Question;
    }

    internal string GetCardFace(string title)
    {
        UIConsole.TitleBar(title);
        return UserInput.GetString("Enter a question for this card: ");
    }
}
