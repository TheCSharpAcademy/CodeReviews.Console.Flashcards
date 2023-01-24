
using Lonchanick9427.FlashCard.ExcutionFolder;

namespace Lonchanick9427;

public static class Program
{
    public static void Main(string[] arg)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\t--- Main Menu ---");
            Console.WriteLine("\t STACK MENU");
            Console.WriteLine("1. Create a new Stack");/*READY*/
            Console.WriteLine("2. Show Stacks");/*READY*/
            Console.WriteLine("3. Delete an Stack");/*READY*/
            Console.WriteLine("4. Update an Stack\n");/*READY*/

            Console.WriteLine("\t CARDS MENU");
            Console.WriteLine("5. Create a new Card");/*READY*/
            Console.WriteLine("6. Delete a card");/*READY*/
            Console.WriteLine("7. Update a Card\n");

            Console.WriteLine("\t STUDY SESSIONS MENU");
            Console.WriteLine("8. Start a New Study Session");
            Console.WriteLine("9. Show all Study Sessions");
            Console.WriteLine("10. Delete a Study Session");
            Console.WriteLine("0. Exit\n");
            Console.Write("Enter your choice: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Clear();
                    StackOperations.NewStack();
                    break;
                case "2":
                    Console.Clear();
                    StackOperations.ShowStackContent();
                    break;
                case "3":
                    Console.Clear();
                    StackOperations.DeleteStack();
                    break;
                case "4":
                    StackOperations.UpdateStack();
                    break;
                case "5":
                    CardOperations.NewCard();
                    break;
                case "6":
                    CardOperations.DeleteCard();
                    break;
                case "7":
                    CardOperations.UpdateCard();
                    break;
                case "8":
                    SessionOperations.New();
                    break;
                case "9":
                    SessionOperations.ShowAll();
                    break;
                case "10":
                    //DELETE SESSION STUDY
                    break;
                case "0":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }


    }
}
