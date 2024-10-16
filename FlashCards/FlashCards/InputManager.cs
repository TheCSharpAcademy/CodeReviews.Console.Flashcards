using FlashCardsLibrary;
using FlashCardsLibrary.Models;
namespace FlashCards;

public class InputManager
{

    /// <param name="firstOption">inclusive</param>
    /// <param name="lastOption">inclusive</param>
    public static int GetOption(int firstOption, int lastOption, bool enterToContinue = false, string quantity = "Option")
    {
        int option = -1;
        bool success = false;
        do
        {
            if (enterToContinue)
                Console.Write($"Enter {quantity} or press enter to continue: ");
            else
                Console.Write($"Enter {quantity}: ");
            var readLine = Console.ReadLine();
            if (string.IsNullOrEmpty(readLine) && enterToContinue)
                break;
            success = int.TryParse(readLine, out option);
            if (!success)
            {
                Console.WriteLine("Invalid Input. Check input is a Number");
                continue;
            }
            if (option < firstOption || option > lastOption)
            {
                Console.WriteLine($"Out of range. {firstOption}->{lastOption}");
                continue;
            }

        } while (option < firstOption || option > lastOption || !success);
        return option;
    }
    public static string NewStack()
    {
        string name = string.Empty;
        do
        {
            Console.WriteLine("Enter Name (or press enter to cancel)");
            var readLine = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(readLine))
            {
                name = string.Empty;
                break;
            }
            name = readLine;
            if (StackController.GetStackNames().Contains(new Stack(name)))
            {
                Console.WriteLine($"Sorry {name} already exist");
                continue;
            }

        } while (StackController.GetStackNames().Contains(new Stack(name)));
        return name.Trim();
    }
    public static FlashCardCreate NewFlashCard(Stack stack)
    {
        string front = string.Empty;
        string back = string.Empty;
        do
        {
            Console.Write("Front: ");
            front = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrEmpty(front))
            {
                Console.WriteLine("Front Cant Be empty");
                continue;
            }
            break;
        } while (string.IsNullOrEmpty(back));
        do
        {
            Console.Write("Back: ");
            back = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrEmpty(front))
            {
                Console.WriteLine("Front Cant Be empty");
                continue;
            }
            if (back.ToLower() == front.ToLower())
            {
                Console.WriteLine("Front cant be same as back");
                continue;
            }
        } while (string.IsNullOrEmpty(back));
        return new FlashCardCreate(stack.Name,front.Trim(),back.Trim());
    }
   
}
