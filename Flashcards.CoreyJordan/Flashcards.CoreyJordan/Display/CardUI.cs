using ConsoleTableExt;
using Flashcards.CoreyJordan.DTOs;
using FlashcardsLibrary;

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

    internal void DisplayFlashCard(CardFaceDTO cardFaceDTO, Face face)
    {
        int cardWidth = 28;
        string text = face switch
        {
            Face.Front => cardFaceDTO.Question,
            Face.Back => cardFaceDTO.Answer,
            _=> "Error"
        };

        string border = "+";
        for (int i = 0; i < cardWidth ;i++)
        {
            border += "-";
        }
        border += "+";

        string field = "|";
        for (int i = 0; i < cardWidth; i++)
        {
            field += " ";
        }
        field += "|";

        string cardText = "|";
        for (int i = 0; i < (cardWidth - text.Length) / 2; i++)
        {
            cardText += " ";
        }
        cardText += text;
        for (int i = 0; i < cardWidth - text.Length - (cardWidth - text.Length) / 2 ; i++)
        {
            cardText += " ";
        }
        cardText += "|";

        UIConsole.WriteCenterLine(border);
        UIConsole.WriteCenterLine(field);
        UIConsole.WriteCenterLine(field);
        UIConsole.WriteCenterLine(cardText);
        UIConsole.WriteCenterLine(field);
        UIConsole.WriteCenterLine(field);
        UIConsole.WriteCenterLine(border);
    }
}
