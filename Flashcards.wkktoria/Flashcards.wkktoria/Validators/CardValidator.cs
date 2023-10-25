using Flashcards.wkktoria.Models.Dtos;

namespace Flashcards.wkktoria.Validators;

internal static class CardValidator
{
    private static bool CheckWord(string word)
    {
        return word.Length is > 0 and <= 50;
    }

    internal static bool Check(CardDto card)
    {
        return CheckWord(card.Front) && CheckWord(card.Back);
    }
}