namespace Flashcards.wkktoria;

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

            if (!int.TryParse(numberInput, out _)) Console.WriteLine("Enter an integer.");
        } while (!int.TryParse(numberInput, out _));

        return int.Parse(numberInput);
    }

    internal static string GetStringInput(string message)
    {
        Console.WriteLine(message);
        Console.Write("> ");

        var stringInput = Console.ReadLine();

        // do
        // {
        //     if (stringInput.IsNullOrEmpty()) Console.WriteLine("Enter non-empty string.");
        // } while (stringInput.IsNullOrEmpty());

        return stringInput!;
    }
}