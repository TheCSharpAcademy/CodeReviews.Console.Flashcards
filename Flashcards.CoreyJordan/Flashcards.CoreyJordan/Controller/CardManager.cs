using Flashcards.CoreyJordan.DTOs;
using FlashcardsLibrary.Data;
using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.Controller;

internal class CardManager : Controller
{
    internal void ManageCards()
    {
        bool exitCardManager = false;
        while (exitCardManager == false)
        {
            UIConsole.TitleBar("CARD MANAGER");
            UICard.DisplayMenu(UICard.CardManagerMenu);

            try
            {
            repeat:
                switch (UserInput.GetString("Select an option: ").ToUpper())
                {
                    case "1":
                        NewCard();
                        break;
                    case "2":
                        DeleteCard();
                        break;
                    case "3":
                        List<CardModel> cards = CardGateway.GetAllCards();
                        List<CardDto> list = CardDto.GetListDto(cards);
                        UICard.DisplayCards(list);
                        UIConsole.Prompt();
                        break;
                    case "X":
                        exitCardManager = true;
                        break;
                    default:
                        UIConsole.PromptAndReset("Invalid selection. Try again.");
                        goto repeat;
                }
            }
            catch (Exception ex) 
            {
                UIConsole.Prompt(ex.Message);
            }
        }
    }

    internal void EditPack()
    {
        List<PackNamesDto> packs = DisplayPacksList(PackGateway.GetPacks());
        string packChoice = UIPack.GetPackChoice(packs);

        bool exitPackEditor = false;
        while (exitPackEditor == false)
        {
            List<CardModel> cards = CardGateway.GetPackContents(packChoice);
            List<CardFaceDto> cardList = CardFaceDto.GetCardsDto(cards);
            UICard.DisplayCards(cardList);
            UICard.DisplayMenu(UICard.EditListMenu);

            repeat:
            switch (UserInput.GetString("Select an option: ").ToUpper())
            {
                case "1":
                    NewCard(packChoice);
                    break;
                case "2":
                    string cardChoice = UICard.GetCardChoice(cardList);
                    DeleteCard(cardChoice);
                    break;
                case "X":
                    exitPackEditor = true;
                    break;
                default:
                    UIConsole.PromptAndReset("Invalid selection. Try again.");
                    goto repeat;
            }
        }
    }

    private void DeleteCard()
    {
        List<PackNamesDto> packs = DisplayPacksList(PackGateway.GetPacks());
        string packChoice = UIPack.GetPackChoice(packs);

        List<CardFaceDto> cards = DisplayCardList(CardGateway.GetPackContents(packChoice));
        string cardChoice = UICard.GetCardChoice(cards);

        DeleteCard(cardChoice);
    }

    private void DeleteCard(string card)
    {
        int successfulDelete = CardGateway.DeleteCard(card);
        if (successfulDelete != 0)
        {
            UIConsole.Prompt("Card deleted successfully");
        }
        else
        {
            UIConsole.Prompt("Card not found");
        }
    }

    private void NewCard()
    {
        List<PackNamesDto> packs = DisplayPacksList(PackGateway.GetPacks());
        string packChoice = UIPack.GetPackChoice(packs);

        NewCard(packChoice);
    }

    private void NewCard(string packName)
    {
        string cardFront = UICard.GetCardFace("NEW CARD");
        string cardBack = UICard.GetCardFace("NEW CARD");

        CardGateway.CreateCard(cardFront, cardBack, packName);
        UIConsole.Prompt("Card added successfully");
    }
}
