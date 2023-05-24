using Flashcards.CoreyJordan.DTOs;
using FlashcardsLibrary;
using FlashcardsLibrary.Data;

namespace Flashcards.CoreyJordan.Controller;
internal class SessionManager : Controller
{
    internal void StartNew()
    {
        string player = UserInput.GetString("Enter a user name for this session: ");

        List<PackNamesDTO> packs = DisplayPacksList(PackGateway.GetPacks());
        string packChoice = UIPack.GetPackChoice(packs);

        List<CardFaceDTO> correctList = new();
        List<CardFaceDTO> unansweredList = CardFaceDTO.GetCardsDTO(CardGateway.GetPackContents(packChoice));

        Random rand = new();
        int correct = 0;
        int cardsShown = 0;


        bool quitGame = false;
        while (quitGame == false)
        {
            // Show card
            int randomCard = rand.Next(unansweredList.Count - 1);
            UICard.DisplayFlashCard(unansweredList[randomCard], Face.Front);
            cardsShown++;

            // Get answer
            string userGuess = UserInput.GetString("Answer: ");

            // Show back
            UICard.DisplayFlashCard(unansweredList[randomCard], Face.Back);

            // Check guess
            if (userGuess == unansweredList[randomCard].Answer)
            {
                correctList.Add(unansweredList[randomCard]);
                unansweredList.Remove(unansweredList[randomCard]);
                correct++;
                UIConsole.Prompt("CORRECT!!!");
            }
            else
            {
                UIConsole.Prompt("Sorry, that's not quite right. We'll try that again in a moment.");
            }

            // Report result


            // Loop until incorrect is empty or user quits
            if (unansweredList.Count == 0)
            {
                quitGame= true;
            }
        }

        // Report results
        // Store results in db
        throw new NotImplementedException();

        // TODO create session table
        // user, 
    }
}
