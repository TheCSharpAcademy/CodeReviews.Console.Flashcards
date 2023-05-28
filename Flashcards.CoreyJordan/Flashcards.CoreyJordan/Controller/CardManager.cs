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
            UICard.Menu(UICard.CardManagerMenu);

            try
            {
            repeat:
                switch (UserInput.GetString("Select an option: ").ToUpper())
                {
                    case "1":
                        NewCard();
                        break;
                    case "2":
                        ;
                        DeleteCard();
                        break;
                    case "3":
                        List<CardModel> cards = CardGateway.GetAllCards();
                        List<CardDTO> list = CardDTO.GetListDTO(cards);
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
        List<PackNamesDTO> packs = DisplayPacksList(PackGateway.GetPacks());
        string packChoice = UIPack.GetPackChoice(packs);

        bool exitPackEditor = false;
        while (exitPackEditor == false)
        {
            List<CardModel> cards = CardGateway.GetPackContents(packChoice);
            List<CardFaceDTO> cardList = CardFaceDTO.GetCardsDTO(cards);
            UICard.DisplayCards(cardList);
            UICard.Menu(UICard.EditListMenu);

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
        List<PackNamesDTO> packs = DisplayPacksList(PackGateway.GetPacks());
        string packChoice = UIPack.GetPackChoice(packs);

        List<CardFaceDTO> cards = DisplayCardList(CardGateway.GetPackContents(packChoice));
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
        List<PackNamesDTO> packs = DisplayPacksList(PackGateway.GetPacks());
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
