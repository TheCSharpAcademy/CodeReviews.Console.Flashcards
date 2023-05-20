namespace Flashcards.CoreyJordan.Display;
internal class UserInterface

{
    public int Width { private get; set; }

    public UserInterface()
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
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    internal void Prompt(string message)
    {
        Console.WriteLine();
        Console.WriteLine(message);
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
}
