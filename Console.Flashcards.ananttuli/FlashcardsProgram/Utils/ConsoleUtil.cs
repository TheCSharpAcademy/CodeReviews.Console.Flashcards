namespace FlashcardsProgram.Utils;

public class ConsoleUtil
{
    public static void PressAnyKeyToClear()
    {
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
        Console.Clear();
    }
}