using static FlashCards.Helpers;
using static FlashCards.DataAccess;
using static FlashCards.Menus;

namespace FlashCards;

internal class DataValidation
{
    public static int GetNumberInput(string message = "")
    {
        if (message != "") Console.WriteLine(message);

        string userInput = Console.ReadLine();
        int number;

        while (!int.TryParse(userInput, out number))
        {
            string error = $"{userInput} is not a valid number. Please try again.";
            DisplayError(error);
            userInput = Console.ReadLine();
        }
        return number;
    }

    public static string GetTextInput(string message = "")
    {
        Console.WriteLine(message);
        string textInput = Console.ReadLine();

        while (string.IsNullOrEmpty(textInput))
        {
            DisplayError("Input can't be empty !");
            Console.WriteLine(message);
            textInput = Console.ReadLine();
        }

        return textInput;
    }

    // Return only valid stack Id or 0
    public static int GetStackIdInput(string message = "")
    {
        DisplayStacks();

        int stackId = GetNumberInput(message);

        while (!StackExists(stackId) && stackId != 0)
        {
            DisplayStacks();

            Console.WriteLine(DisplayError($"The id: {stackId} is invalid"));

            stackId = GetNumberInput(message);
        }

        return stackId;
    }

    // Return only valid card Id or 0
    public static int GetCardIdInput(int stackId, string message = "")
    {
        InspectStack(stackId);

        int cardId = GetNumberInput(message);

        while (!CardExists(cardId, stackId) && cardId != 0)
        {
            InspectStack(stackId);

            Console.WriteLine(DisplayError($"The id: {cardId} is invalid"));

            cardId = GetNumberInput(message);
        }

        return cardId;
    }
}
