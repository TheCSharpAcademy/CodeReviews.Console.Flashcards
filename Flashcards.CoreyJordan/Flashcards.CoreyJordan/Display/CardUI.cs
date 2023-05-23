using ConsoleTableExt;
using Flashcards.CoreyJordan.DTOs;

namespace Flashcards.CoreyJordan.Display;
internal class CardUI
{
    private ConsoleUI UIConsole { get; set; } = new();
    private InputModel UserInput { get; set; } = new();
    private List<MenuModel> EditListMenu { get; } = new()
    {
        new MenuModel("1", "Add New Card"),
        new MenuModel("2", "Delete Card"),
        new MenuModel("X", "Exit List Editor"),
    };

    internal void DisplayCards(List<CardFaceDTO> cards)
    {
        UIConsole.TitleBar("PACKS");

        ConsoleTableBuilder
            .From(cards)
            .WithColumn("#","FRONT", "BACK")
            .WithFormat(ConsoleTableBuilderFormat.MarkDown)
            .ExportAndWriteLine(TableAligntment.Center);
        Console.WriteLine();
    }

    internal void DisplayEditListMenu()
    {
        ConsoleTableBuilder
            .From(EditListMenu)
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
