using Spectre.Console;

namespace Flashcards;
public static class MenuPresentation
{
    public static void PresentMenu(string ruleMessage)
    {
        AnsiConsole.Clear();

        var rule = new Rule(ruleMessage);
        AnsiConsole.Write(rule);
    }

    public static void MenuDisplayer<T>(Func<string> getTitle, Func<T, bool> HandleMenuOption) where T : struct, Enum
    {
        bool continueRunning = true;

        while (continueRunning)
        {
            PresentMenu(getTitle());
            T option = Prompter.EnumPrompt<T>();
            continueRunning = HandleMenuOption(option);
        }
    }
}