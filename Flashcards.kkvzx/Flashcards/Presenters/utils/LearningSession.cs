using Flashcards.Models.Dtos;
using Flashcards.Views;

namespace Flashcards.Presenters.utils;

public class LearningSession(MainView view, int stackId, Func<FlashcardDto> getRandomFlashcardFromStack)
{
    private bool _isSessionInProgress = true;
    private const string EndSessionCommand = "x";

    public SessionDto StartSession()
    {
        var score = 0;
        
        while (_isSessionInProgress)
        {
            view.Clear();

            var flashcardDto = getRandomFlashcardFromStack();
            view.ShowMessage($"Type '{EndSessionCommand}' to end current learning session.");
            view.ShowMessage("Score: " + score);
            view.ShowMessage("Front is: " + flashcardDto.FrontText);
            var userGuess = view.Input.GetText("What is on the back: ");

            if (userGuess == flashcardDto.BackText)
            {
                view.ShowSuccess("Great! Correct answer");
                view.ShowSuccess("+10 points");
                view.PressKeyToContinue();

                score += 10;
            }
            else if (userGuess == EndSessionCommand)
            {
                _isSessionInProgress = false;
            }
            else
            {
                view.ShowMessage("Unfortunately correct answer is: " + flashcardDto.BackText);
                view.ShowError("-2 points");

                view.PressKeyToContinue();
                score -= 2;
            }
        }

        return new SessionDto(stackId, score, DateTime.Now);
    }
}