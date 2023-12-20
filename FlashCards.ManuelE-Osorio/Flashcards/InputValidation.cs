using System.Text.RegularExpressions;

namespace Flashcards;

class InputValidation
{
    public static string regexPattern = "(^$)|([^A-Za-z0-9$])";

    public static string? ValidateSelection(string? selection, int minValue, int maxValue)
    {
        string? errorMessage = null;

        if(int.TryParse(selection, out int selectionInt))
        {
            if(selectionInt < minValue || selectionInt > maxValue)
            {
                errorMessage += "Invalid value. ";
            }
        }
        else
        {
            errorMessage += "Invalid character. ";
        }

        return errorMessage;
    }
    public static string? ValidateNewStackName(string newStackName)
    {
        string? errorMessage = null;
        Regex stackNameRegex = new(regexPattern);

        if (newStackName.Length>50)
        {
            errorMessage += "Input has more than 50 characters. ";
        }

        if(stackNameRegex.IsMatch(newStackName))
        {
            errorMessage += "Input contains an invalid character. ";
        }

        return errorMessage;
    }

    public static string? ValidateStackSelection(List<Stacks> stacks, string? stackName)
    {
        string? errorMessage = "The stack you selected does not exists. ";
        if(stacks.Any(stacks => stacks.StackName == stackName))
        {
            errorMessage = null;
        }
        return errorMessage;
    }

    public static string? ValidateCardSelection(List<Cards> cards, string? cardID)
    {
        string? errorMessage = "The card you selected does not exists. ";
        if(cards.Any(cards => cards.CardID == Convert.ToInt32(cardID)))
        {
            errorMessage = null;
        }
        return errorMessage;
    }

    public static bool ValidateStudySessionAnswer(CardsDTO card, string answer)
    {
        bool answerIsCorrect = false;
        if(answer.Equals(card.Answer,StringComparison.OrdinalIgnoreCase))
        {
            answerIsCorrect = true;
        }
        return answerIsCorrect;
    }
}