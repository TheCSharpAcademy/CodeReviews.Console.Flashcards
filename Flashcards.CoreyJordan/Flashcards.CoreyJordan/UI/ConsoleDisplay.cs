using static System.Console;

namespace Flashcards.CoreyJordan.UI;
internal class ConsoleDisplay
{
    public int Width { get; set; }

    public ConsoleDisplay(int consoleWidth)
    {
        Width = consoleWidth;
    }

    public string Bar()
    {
        string bar = string.Empty;
        for (int i = 0; i < Width; i++)
        {
            bar += "-";
        }
        bar += "\n";
        return bar;
    }

    public string Tab(int tabs)
    {
        string tab = string.Empty;
        for (int i = 0; i < tabs; i++)
        {
            for (int j = 0; j < Width / 8; j++)
            {
                tab += " ";
            }
        }
        return tab;
    }

    public void WriteCenter(string message)
    {
        int stringLength = message.Length;
        string centered = string.Empty;
        for (int i = 0; i < (Width - stringLength) / 2; i++)
        {
            centered += " ";
        }
        centered += message;

        Write(centered);
    }

    public void WriteCenter(string message, int carriageReturns)
    {
        WriteCenter(message);

        for (int i = 0; i < carriageReturns; i++)
        {
            WriteLine();
        }
    }

    public void PromptUser()
    {
        WriteCenter("Press any key to continue...");
        ReadKey();
    }

    public void TitleBar(string title)
    {
        Write(Bar());
        WriteCenter(title, 1);
        Write(Bar());
    }
}
