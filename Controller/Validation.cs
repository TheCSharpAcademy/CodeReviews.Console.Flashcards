using Spectre.Console;

namespace Flashcards.TwilightSaw.Controller;

public static class Validation
{
    public static string Validate(Action action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            return e.Message;
        }

        return "Executed successfully";
    }
}