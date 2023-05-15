using static System.Console;

namespace Flashcards.CoreyJordan;
internal class ConsoleDisplay
{

    public int ConsoleWidth { get; set; } = 80;

    public string Bar()
    {
        string bar = String.Empty;
        for (int i = 0; i < ConsoleWidth; i++)
        {
            bar += "-";
        }
        bar += "\n";
        return bar;
    }

    public void WriteCenter(string message)
    {
        int stringLength = message.Length;
        string centered = string.Empty;
        for (int i = 0; i < (ConsoleWidth - stringLength) / 2; i++)
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

    public void WelcomeMenu()
    {
        WriteLine(Bar());
        WriteCenter("Welcome to Flash Card Study", 1);
        WriteCenter("Written by Corey Jordan", 2);
        WriteLine(Bar());
    }
}
