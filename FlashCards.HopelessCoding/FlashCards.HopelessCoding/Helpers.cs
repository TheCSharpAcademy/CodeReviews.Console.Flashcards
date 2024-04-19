using DbHelpers.HopelessCoding;
using FlashCards.HopelessCoding.DTOs;
using Spectre.Console;

namespace HelperMethods.HopelessCoding;

public class Helpers
{
    public static string MenuSelector(string[] menuItems)
    {
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow1]What would you like to do?[/]")
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .HighlightStyle("olive")
                .AddChoices(menuItems));

        return selection;
    }

    internal static int GetValidIdFromUser(string stackName, string command)
    {
        Console.Write($"Enter Id of the flashcard which you want to {command}: ");
        int inputId;

        while (!int.TryParse(Console.ReadLine(), out inputId) || inputId <= 0)
        {
            AnsiConsole.Write(new Markup($"[red]\nInvalid input. Please enter a valid positive integer ID.[/]\nPress any key to continue."));
            Console.ReadLine();
            return -1;
        }

        FlashcardService flashcardService = new FlashcardService(DatabaseHelpers.connectionString);
        Dictionary<int, int> displayIdToActualId = flashcardService.GetDisplayIdToActualId();

        if (displayIdToActualId.ContainsKey(inputId))
        {
            return displayIdToActualId[inputId];
        }
        else
        {
            AnsiConsole.Write(new Markup($"[red]\nID does not exist in the specified stack.[/]\nPress any key to continue."));
            Console.ReadLine();
            return -1;
        }
    }

    internal static string GetValidYearFromUser()
    {
        while (true)
        {
            Console.Write("Input a year in format YYYY: ");
            string userInputYear = Console.ReadLine();

            if (DateTime.TryParseExact(userInputYear, "yyyy", null, System.Globalization.DateTimeStyles.None, out _))
            {
                return userInputYear;
            }
            else
            {
                AnsiConsole.Write(new Markup("[red]Invalid input. Please give time in valid format.[/]\n"));
            }
        }
    }

    internal static int ValidateIntegerInput()
    {
        int amount;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out amount) && amount > 0)
            {
                break;
            }
            else
            {
                AnsiConsole.Write(new Markup($"[red]\nInput is not a valid positive integer value, try again: [/]"));
            }
        }
        return amount;
    }

    internal static void WaitForUserInput()
    {
        Console.Write("Press any key to continue.");
        Console.ReadKey(true);
    }
}