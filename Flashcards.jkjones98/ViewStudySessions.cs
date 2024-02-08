
using Flashcards.Functions;

namespace Flashcards.jkjones98;

internal class ViewStudySessions
{
    MainMenu mainMenu = new();
    CheckUserInput checkUserInput = new();
    StudySessionController controller = new();
    internal void ViewSessionMenu()
    {
        Console.Clear();
        int stackId = checkUserInput.ChooseStack();
        Console.WriteLine("\nWhat data would you like to view?");
        Console.WriteLine("\nEnter 1 - View study sessions by year");
        Console.WriteLine("Enter 2 - View study sessions by month + year");
        Console.WriteLine("Enter 3 - View study sessions by stack");
        Console.WriteLine("Enter 4 - View all study sessions for selected stack");
        Console.WriteLine("Enter 5 - Change the stack of session data being reviewed");
        Console.WriteLine("Enter 0 - Return to main menu");
        string viewInput = Console.ReadLine();

        while(string.IsNullOrEmpty(viewInput))
        {

            Console.WriteLine("Invalid input. Please enter a valid Id.");
            viewInput = Console.ReadLine();
        }
        switch(viewInput)
        {
            case "1":
                Console.WriteLine("\nWhat year would you like you to filter by? Use YYYY format");
                string userYearInput = CheckYearFormat();
                controller.StudySessionsYear(stackId, userYearInput);
                break;
            case "2": 
                Console.WriteLine("\nWhich month and year would you like to filter by? Enter the year in YYYY format");
                string userYear = CheckYearFormat();

                Console.WriteLine("\nEnter the numerical representation of the month you would you like to filter by");
                Console.WriteLine("E.g. January would be 01");
                string userMonth = CheckMonthFormat();
                string yearMonth = userMonth + "/" + userYear;
                controller.StudySessionsYear(stackId, yearMonth);
                break;
            case "3": 
                Console.WriteLine("\nWhich stack would you like to view data for?");
                break;
            case "4": 
                controller.ViewStudySessions(stackId);
                break;
            case "5": 
                ViewSessionMenu();
                break;
            case "0":
                mainMenu.DisplayMenu();
                break;
            default:
                Console.WriteLine("Please enter a valid selection");
                break;
        }

    }

    internal string CheckYearFormat()
    {
        string year = Console.ReadLine();
        var pattern = @"(?<Year>\d{4})";
        var regexp = new System.Text.RegularExpressions.Regex(pattern);
        while(!regexp.IsMatch(year))
        {
            Console.WriteLine("Does not match specified format. Please enter year in YYYY format");
            year = Console.ReadLine();
        }

        return year;
    }
    internal string CheckMonthFormat()
    {
        string month = Console.ReadLine();
        int monthDig;
        while(!Int32.TryParse(month, out monthDig) || monthDig < 1 || monthDig > 12)
        {
            Console.WriteLine("\nUnrecognised numerical date format - please try again");
            Console.WriteLine("January = 01, February = 02, March = 03 etc.");
            month = Console.ReadLine();
        }

        return month;
        //If has digit check if between 0 and 12
        //If has char check if full name or short name
    }
}