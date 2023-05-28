using Flashcards.CRUD;
using Flashcards.Model;

namespace Flashcards.Menus {
    internal class ManageCardMenu {

        internal static void ManageCard( CardDto card, Stack stack ) {

            bool managing = true;

            while (managing) {

                Console.Clear();
                DbOperations.GetCard(card);

                Console.WriteLine(@"What would you like to do?
1 - Edit the front of the card
2 - Edit the back of the card
3 - Delete the card
0 - Go back to the stack manager");

                string op = Console.ReadLine();

                switch (op) {
                    case "0":
                        Console.Clear();
                        managing = false;
                        break;
                    case "1":
                        EditFront(card);
                        break;
                    case "2":
                        EditBack(card);
                        break;
                    case "3":
                        DeleteChoicecard(card, stack);
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
            ManageStackMenu.ManageStack(stack);
        }

        static void EditFront( CardDto card ) {
            Console.Clear();
            Console.WriteLine("What would be the new front?");

            string front = Console.ReadLine();

            DbOperations.UpdateCardText(true, front, card);
        }

        static void EditBack( CardDto card ) {
            Console.Clear();
            Console.WriteLine("What would be the new back?");

            string back = Console.ReadLine();

            DbOperations.UpdateCardText(false, back, card);
        }

        static void DeleteChoicecard( CardDto card, Stack stack ) {
            Console.Clear();

            Console.WriteLine("Are you sure you want to delete this card? y/n");
            string input = Console.ReadLine();

            if (input == "y") {
                DbOperations.DeleteCard(card.Id);
                ManageStackMenu.ManageStack(stack);
            }
        }
    }
}
