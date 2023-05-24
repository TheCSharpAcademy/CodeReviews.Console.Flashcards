using Flashcards.CoreyJordan.DTOs;
using FlashcardsLibrary;
using FlashcardsLibrary.Data;

namespace Flashcards.CoreyJordan.Controller;
internal class StudySession : Controller
{
    internal void StartNew()
    {
        string player = UserInput.GetString("Enter a user name for this session: ");

        List<PackNamesDTO> packs = DisplayPacksList(PackGateway.GetPacks());
        string packChoice = UIPack.GetPackChoice(packs);

        List<CardFaceDTO> correct = new();
        List<CardFaceDTO> unanswered = CardFaceDTO.GetCardsDTO(CardGateway.GetPackContents(packChoice));

        bool quitGame = false;
        Random rand = new();

        while (quitGame == false)
        {
            // Show card
            int randomCard = rand.Next();
            UICard.DisplayFlashCard(unanswered[randomCard], Face.Front);

            // Get answer
            string userGuess = UserInput.GetString("Answer: ");

            // Show back
            UICard.DisplayFlashCard(unanswered[randomCard], Face.Back);

            // Report result


            // If correct, add to correct list and remove from incorrect
            // Loop until incorrect is empty or user quits
        }

        // Report results
        // Store results in db
        throw new NotImplementedException();

        // TODO create session table
        // user, 
    }
}
