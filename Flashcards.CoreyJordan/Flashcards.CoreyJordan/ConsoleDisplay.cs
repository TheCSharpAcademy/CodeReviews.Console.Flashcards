using static System.Console;

namespace Flashcards.CoreyJordan;
internal static class ConsoleDisplay
{
    public static string Bar(int width)
    {
        string bar = String.Empty;
        for (int i = 0; i < width; i++)
        {
            bar += "-";
        }
        bar += "\n";
        return bar;
    }

    public static string Tab(int tabs, int consoleWidth)
    {
        string tab = String.Empty;
        for (int i = 0; i < tabs; i++)
        {
            for (int j = 0; j < consoleWidth / 8; j++)
            {
                tab += " ";
            }
        }
        return tab;
    }

    public static void WriteCenter(string message, int consoleWidth)
    {
        int stringLength = message.Length;
        string centered = string.Empty;
        for (int i = 0; i < (consoleWidth - stringLength) / 2; i++)
        {
            centered += " ";
        }
        centered += message;

        Write(centered);
    }

    public static void WriteCenter(string message, int consoleWidth, int carriageReturns)
    {
        WriteCenter(message, consoleWidth);

        for (int i = 0; i < carriageReturns; i++)
        {
            WriteLine();
        }
    }

    public static void PromptUser()
    {
        WriteLine("Press any key to continue...");
        ReadKey();
    }
    
}
