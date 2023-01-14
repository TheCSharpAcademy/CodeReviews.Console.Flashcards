
using Lonchanick9427.FlashCard.ExcutionFolder;

while (true)
{
    Console.Clear();
    Console.WriteLine("\t--- Main Menu ---");
    Console.WriteLine("\t STACK MENU");
    Console.WriteLine("1. Create a new Stack");
    Console.WriteLine("2. Show Stacks");
    Console.WriteLine("3. Delete an Stack");
    Console.WriteLine("4. Update an Stack\n");

    Console.WriteLine("\t CARDS MENU");
    Console.WriteLine("5. Create a new Card");
    Console.WriteLine("6. Delete a card");
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
            StackOperations.showStacks();
            Console.ReadLine();
            break;
        case "3":
            Console.Clear();
            StackOperations.DeleteStack();
            break;
        case "4":
            Console.WriteLine("Update-stack PENDIENTE");
            break;
        case "5":
            CardOperations.NewCard();
            break;
        case "6":
            CardOperations.DeleteCard();
            //DELETE CARD
            break;
        case "7":
            //UPDATE
            break;
        case "8":
            //START A NEW SESSION STUDY
            break;
        case "9":
            //SHOW SESSION STUDY
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
