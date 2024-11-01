namespace Flashcards.wkktoria.UserInteractions;

internal static class UserInput
{
    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        string? numberInput;

        do
        {
            Console.Write("> ");
            numberInput = Console.ReadLine();

            if (!int.TryParse(numberInput, out _)) UserOutput.ErrorMessage("Enter an integer.");
        } while (!int.TryParse(numberInput, out _));

        return int.Parse(numberInput);
    }

    internal static string GetStringInput(string message)
    {
        Console.WriteLine(message);
        Console.Write("> ");

        var stringInput = Console.ReadLine();

        return stringInput!;
    }
}