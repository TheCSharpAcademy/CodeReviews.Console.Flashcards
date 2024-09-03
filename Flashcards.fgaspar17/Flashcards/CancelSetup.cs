namespace Flashcards;
public static class CancelSetup
{
    public static string CancelString { get; set; } = "c";
    public static string CancelPrompt = $", or enter '{CancelString}' to cancel";

    public static bool IsCancelled(string input)
    {
        return input.Trim().ToLower() == CancelString.ToLower();
    }
}