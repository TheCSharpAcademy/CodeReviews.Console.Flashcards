using Flashcards.CoreyJordan.Display;
using Flashcards.CoreyJordan.DTOs;
using FlashcardsLibrary;
using FlashcardsLibrary.Data;
using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.Controller;
internal class SessionManager : Controller
{
    public SessionUI UISession { get; set; } = new();

    internal void StartNew()
    {
        UIConsole.TitleBar("NEW STUDY SESSION");
        string player = UserInput.GetString("Enter a user name for this session: ");

        List<PackNamesDTO> packs = DisplayPacksList(PackGateway.GetPacks());
        string packChoice = UIPack.GetPackChoice(packs);

        List<CardFaceDTO> correctList = new();
        List<CardFaceDTO> unansweredList = CardFaceDTO.GetCardsDTO(CardGateway.GetPackContents(packChoice));

        int cardsShown = 0;
        int cycles = 0;
        Random rand = new();
        unansweredList = unansweredList.OrderBy(x => rand.Next()).ToList();

        bool quitGame = false;
        while (quitGame == false)
        {
            for (int i = 0; i < unansweredList.Count; i++)
            {
                UIConsole.TitleBar($"STUDY SESSION {packChoice.ToUpper()}");
                UIConsole.WriteCenterLine("\nEnter Q to exit game\n");

                UICard.DisplayFlashCard(unansweredList[i], Face.Front);
                cardsShown++;

                string userGuess = UserInput.GetString("Answer: ");
                if (userGuess.ToUpper() == "Q")
                {
                    quitGame = true;
                    break;
                }

                UICard.DisplayFlashCard(unansweredList[i], Face.Back);

                if (userGuess == unansweredList[i].Answer)
                {
                    correctList.Add(unansweredList[i]);
                    unansweredList.Remove(unansweredList[i]);
                    UIConsole.Prompt("CORRECT!!!");
                    i--;
                }
                else
                {
                    UIConsole.Prompt("Sorry, that's not quite right. We'll try that again in a moment.");
                }

                if (unansweredList.Count == 0)
                {
                    quitGame = true;
                    break;
                }
            }
            cycles++;
        }

        SessionModel session = new()
        {
            Player = player,
            Pack = packChoice,
            PackSize = correctList.Count,
            Date = DateTime.Now,
            Cycles = cycles,
            CardsShown = cardsShown,
            Correct = correctList.Count
        };

        UISession.DisplayEndOfSession(new SessionDTO(session));
        UIConsole.Prompt();
        SessionGateway.InsertSession(session);
    }
}
