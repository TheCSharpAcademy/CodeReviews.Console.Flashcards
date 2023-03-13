namespace AskInputs;

public class AskInput
{
    public void ClearPreviousLines(int numberOfLines)
    {
        for (int i = 1; i <= numberOfLines; i++)
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }

    public int PositiveNumber(string message)
    {
        string? input;
        bool showError = false;
        int number;
        do
        {

            if (showError)
            {
                ClearPreviousLines(2);
                Console.Write("Invalid Input. ");
            }

            Console.WriteLine(message);
            input = Console.ReadLine();
            showError = true;
        }
        while (!(Int32.TryParse(input, out number) && number >= 0));
        return number;
    }

    public void AnyKeyToContinue()
    {
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }

    public void AnyKeyToContinue(string? message)
    {
        Console.WriteLine(message);
        Console.ReadKey();
    }

    public bool ZeroOrOtherAnyKeyToContinue(string? message)
    {
        Console.WriteLine(message);
        if (Console.ReadKey().KeyChar == '0') return true;
        else return false;
    }

    public string AlphasNumbersSpecialUpToLimit(int charLimit, string message)
    {
        string? returnString;
        bool showError = false;
        do
        {
            if (showError)
            {
                ClearPreviousLines(2);
                Console.Write($"Input can't be over {charLimit} characters. ");
            }
            Console.WriteLine(message);

            returnString = Console.ReadLine();
            showError = true;
        }
        while (!(returnString.Length < charLimit));

        return returnString.Trim();
    }
}