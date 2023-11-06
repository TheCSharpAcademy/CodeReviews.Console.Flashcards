namespace Ohshie.FlashCards.StacksManager;

public static class AskUser
{
    public static string AskTextInput(string where, string what)
    {
        string userInput = AnsiConsole.Ask<string>($"Enter new {where} [red]{what}[/]:");
        return userInput;
    }
}