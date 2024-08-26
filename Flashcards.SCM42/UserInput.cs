using System.Globalization;

namespace Flashcards;

public class Input
{
    internal static string? GetString(string? prompt)
    {
        string? input = null;
        bool validInput = false;

        Views.ShowMessage($"{prompt}\n");

        while (!validInput)
        {
            input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                Views.ShowErrorMessage("Input was null. Please input a valid entry.");
            }
            else
            {
                validInput = true;
            }
        }

        return input;
    }

    internal static string? GetDate()
    {
        string? input = null;
        bool validDate = false;

        while (!validDate)
        {
            input = GetString("Please input a year (Format: yyyy). Press 0 to cancel.");

            if (input == "0")
            {
                validDate = true;
            }
            else if (!DateTime.TryParseExact(input, "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Views.ShowErrorMessage("Incorrect format.");
            }
            else
            {
                validDate = true;
            }
        }

        return input;
    }

    internal static string? GetNumber()
    {
        string? input = null;
        bool validInput = false;
        bool isNumber;

        while (!validInput)
        {
            input = Console.ReadLine()?.Trim();
            isNumber = Validation.ValidateNumber(input);

            if (string.IsNullOrEmpty(input))
            {
                Views.ShowErrorMessage("Input was null or empty. Please input a valid entry.");
            }
            else if (!isNumber)
            {
                Views.ShowErrorMessage("Not a number. Please input a valid number.");
            }
            else
            {
                validInput = true;
            }
        }

        return input;
    }
}

public class Validation
{
    internal static bool ValidateMenuSelection(string? input)
    {
        bool valid;

        if (string.IsNullOrEmpty(input))
        {
            valid = false;
        }
        else
        {
            valid = true;
        }

        return valid;
    }

    internal static bool ValidateNumber(string number)
    {
        bool valid;

        if (int.TryParse(number, out _))
        {
            valid = true;
        }
        else
        {
            valid = false;
        }

        return valid;
    }
}