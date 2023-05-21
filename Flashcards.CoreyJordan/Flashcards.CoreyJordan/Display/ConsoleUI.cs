namespace Flashcards.CoreyJordan.Display;
internal class ConsoleUI

{
    public int Width { private get; set; }

    public ConsoleUI()
    {
        Width = Console.WindowWidth;
    }

    internal void WriteCenter(string output)
    {
        int stringLength = output.Length;
        string centered = "";

        for (int i = 0; i < (Width - stringLength) / 2; i++)
        {
            centered += " ";
        }
        centered += output;

        Console.Write(centered);
    }

    internal void WriteCenterLine(string output)
    {
        WriteCenter(output);
        Console.WriteLine();
    }

    internal void Seperator()
    {
        string seperator = "";

        for (int i = 0; i < Width; i++)
        {
            seperator += "-";
        }

        Console.WriteLine(seperator);
        Console.WriteLine();
    }

    internal void Prompt()
    {
        Console.WriteLine();
        WriteCenter("Press any key to continue...");
        Console.ReadKey();
    }

    internal void Prompt(string message)
    {
        Console.WriteLine();
        WriteCenterLine(message);
        Prompt();
    }

    internal void TitleBar(string message)
    {
        Console.Clear();
        Seperator();
        WriteCenterLine(message);
        Console.WriteLine();
        Seperator();
    }

    internal void PromptAndReset(string message)
    {
        int prompts = 4;
        Prompt(message);

        Console.SetCursorPosition(0, Console.GetCursorPosition().Top - prompts);
        for (int i = 0;i < prompts; i++)
        {
            Console.WriteLine(new string(' ', Console.WindowWidth));
        }
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.GetCursorPosition().Top - prompts);
    }
}
