
using Flashcards.Functions;
using Microsoft.VisualBasic;

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
        Console.WriteLine("Enter 3 - View all study sessions for selected stack");
        Console.WriteLine("Enter 4 - Change the stack of session data being reviewed");
        Console.WriteLine("Enter 5 - View sessions per month per stack");
        Console.WriteLine("Enter 6 - View average score per month per stack ");
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
                int userYearInput = CheckYearFormat();
                controller.StudySessionsYear(stackId, userYearInput);
                break;
            case "2": 
                Console.WriteLine("\nWhich month and year would you like to filter by? Enter the year in YYYY format");
                int userYear = CheckYearFormat();

                Console.WriteLine("\nEnter the numerical representation of the month you would you like to filter by");
                Console.WriteLine("E.g. January would be 01");
                int userMonth = CheckMonthFormat();
                controller.StudySessionsMonth(stackId, userMonth, userYear);
                break;
            case "3": 
                controller.ViewStudySessions(stackId);
                break;
            case "4": 
                ViewSessionMenu();
                break;
            case "5": 
                Console.WriteLine("\nPlease enter the year to view number of sessions per month");
                int yearCount = CheckYearFormat();
                controller.PivotMonthlySessions(stackId, yearCount);
                break;
            case "6": 
                Console.WriteLine("\nPlease enter the year to view average score per month");
                int yearAverage = CheckYearFormat();
                controller.PivotMonthlyAverages(stackId, yearAverage);
                break;
            case "0":
                mainMenu.DisplayMenu();
                break;
            default:
                Console.WriteLine("Please enter a valid selection");
                break;
        }

    }

    internal int CheckYearFormat()
    {
        int year = checkUserInput.CheckForChar();
        var pattern = "^(?=.{4}$)[0-9]{4}$";
        var regexp = new System.Text.RegularExpressions.Regex(pattern);
        while(!regexp.IsMatch(Convert.ToString(year)))
        {
            Console.WriteLine("Does not match specified format. Please enter year in YYYY format");
            year = checkUserInput.CheckForChar();
        }

        return year;
    }
    internal int CheckMonthFormat()
    {
        int month = checkUserInput.CheckForChar();
        while(month < 1 || month > 12)
        {
            Console.WriteLine("\nUnrecognised numerical date format - please try again");
            Console.WriteLine("January = 01, February = 02, March = 03 etc.");
            month = checkUserInput.CheckForChar();
        }

        return month;
        //If has digit check if between 0 and 12
        //If has char check if full name or short name
    }
}