using Models;

namespace Flashcards;

public class StudyGame
{
    public int Play(PlayStack stack) 
    {
        int maxScore = stack.Flashcards.Count;
        int score = 0;

        foreach(var flashcard in stack.Flashcards)
        {
            var userAnswer = UI.StringResponse("Front - " + flashcard.Front + " [grey]Enter your guess[/]");
            if (userAnswer == flashcard.Back)
            {
                score++;
                UI.ConfirmationMessage("Correct!");
            } else{
                UI.ConfirmationMessage("Incorrect. Correct answer was '" + flashcard.Back + "'.");
            }
        }
        var finalScore = (int) ((float) score / (float) maxScore * 100);
        UI.ConfirmationMessage("You got " + score.ToString() + " out of " + maxScore.ToString() + " correct for a score of " + finalScore.ToString() + ".");
        return finalScore;
    }
}
