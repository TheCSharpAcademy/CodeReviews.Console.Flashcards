using Flashcards.CoreyJordan.DTOs;
using FlashcardsLibrary.Data;
using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.Controller;

internal class CardManager : Controller
{
    internal void ManageCards()
    {
        throw new NotImplementedException();
    }

    internal void EditPack()
    {
        // TODO test UI for title placement
        UIConsole.TitleBar("PACK EDITOR");

        string packChoice = ChoosePack(PackGateway.GetPacks());

        bool exitPackEditor = false;
        while (exitPackEditor == false)
        {
            List<CardModel> cards = CardGateway.GetPackContents(packChoice);
            List<CardFaceDTO> cardList = CardFaceDTO.GetCardsDTO(cards);
            UICard.DisplayCards(cardList);
            UICard.DisplayEditListMenu();

            repeat:
            switch (UserInput.GetString("Select an option: ").ToUpper())
            {
                case "1":
                    NewCard(packChoice);
                    break;
                case "2":
                    DeleteCard(packChoice);
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
        string pack = ChoosePack(PackGateway.GetPacks());
        DeleteCard(pack);
    }

    private void DeleteCard(string packChoice)
    {
        throw new NotImplementedException();
    }

    private void CreateCard()
    {
        string pack = ChoosePack(PackGateway.GetPacks());
        NewCard(pack);
    }

    private void NewCard(string packName)
    {
        string cardFront = UICard.GetCardFace("NEW CARD");
        string cardBack = UICard.GetCardFace("NEW CARD");

        // Insert card into database (front, back, deckId? from deck name)
        CardGateway.CreateCard(cardFront, cardBack, packName);
        throw new NotImplementedException();
    }
}
