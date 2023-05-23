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
                    string card = UICard.ChooseCard(cardList);
                    DeleteCard(card);
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
        string packChoice = ChoosePack(PackGateway.GetPacks());
        List<CardModel> cards = CardGateway.GetPackContents(packChoice);
        string card = ChooseCard(cards);
        DeleteCard(card);
    }

    private void DeleteCard(string card)
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

        CardGateway.CreateCard(cardFront, cardBack, packName);
        UIConsole.Prompt("Card added successfully");
    }
}
