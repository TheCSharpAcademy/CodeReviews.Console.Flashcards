namespace Flashcards.StevieTV.Helpers;

public class InputValidation
{
    public static bool TestValidMenuOption(int menuOption, int lowerBound, int upperBound)
    {
        return (menuOption >= lowerBound && menuOption <= upperBound);
    }
}