using ConsoleTableExt;
using Flashcards.CoreyJordan.DTOs;
using System.Xml.Linq;

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
            .WithColumn("FRONT", "BACK")
            .ExportAndWriteLine(TableAligntment.Center);
        Console.WriteLine();
    }

    internal void DisplayEditListMenu()
    {
        ConsoleTableBuilder
            .From(EditListMenu)
            .ExportAndWriteLine(TableAligntment.Center);
        Console.WriteLine();
    }

    internal string GetCardFace(string title)
    {
        UIConsole.TitleBar(title);
        return UserInput.GetString("Enter a question for this card: ");
    }
}
