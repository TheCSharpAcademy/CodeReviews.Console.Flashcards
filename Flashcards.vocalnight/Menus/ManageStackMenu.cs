using ConsoleTableExt;
using Flashcards.CRUD;
using Flashcards.Helpers;
using Flashcards.Model;

namespace Flashcards.Menus {
    internal class ManageStackMenu {

        internal static void ManageStack( Stack stack ) {

            bool managing = true;

            while (managing) {

                Console.WriteLine($"Currently managing {stack.Name} stack");
                Console.WriteLine(@"Select what you would like to do?
1 - Choose another stack
2 - View all flashcards in this stack
3 - Insert a flashcard
4 - Manage the flashcards in this stack
5 - Delete this stack
0 - Go back to the main menu");

                string op = Console.ReadLine();
                Console.Clear();

                switch (op) {
                    case "0":
                        managing = false;
                        break;
                    case "1":
                        ChooseStackMenu.ChooseStacksMenu();
                        break;
                    case "2":
                        ViewFlashcards(stack);
                        break;
                    case "3":
                        InsertFlashcards(stack);
                        break;
                    case "4":
                        ChooseFlashcard(stack);
                        break;
                    case "5":
                        DeleteChoiceStack(stack);
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
            MainMenu.GetMainInput();
        }

        static void DeleteChoiceStack( Stack stack ) {
            Console.Clear();

            Console.WriteLine("Are you sure you want to delete this stack? y/n");
            string input = Console.ReadLine();

            if (input == "y") {
                Console.Clear();
                DbOperations.DeleteStack(stack);
                MainMenu.GetMainInput();
            } else {
                Console.Clear();
                ManageStack(stack);
            }
        }

        static void ChooseFlashcard( Stack stack ) {
            Console.Clear();
            List<CardDto> DtoList = DbOperations.GetFlashcardsWithId(stack);
            List<CardDto> cards = DbOperations.GetFlashcards(stack);
            HelpersAndValidation.InsertSeparator();

            bool managing = true;

            while (managing) {

                ConsoleTableBuilder.From(DtoList).ExportAndWriteLine();
                Console.WriteLine("Select the id of the card to edit");

                string op = Console.ReadLine();

                switch (op) {
                    case "0":
                        managing = false;
                        break;
                    default:
                        CardDto card = FindIdSelect(op, cards);
                        if (card != null) {
                            ManageCardMenu.ManageCard(card, stack);
                        } else {
                            Console.Clear();
                            Console.WriteLine("Invalid card id");
                            HelpersAndValidation.InsertSeparator();
                        }
                        break;
                }
            }
            ManageStack(stack);
        }

        static CardDto? FindIdSelect( string op, List<CardDto> cards ) {
            int index = int.Parse(op);
            index -= 1;

            if (index >= 0 && index <= cards.Count) {
                return cards[index];
            }
            return null;
        }

        static void ViewFlashcards( Stack stack ) {

            bool managing = true;

            while (managing) {

                Console.Clear();

                var list = DbOperations.GetFlashcardsWithId(stack);
                if (list.Count == 0) {
                    Console.WriteLine("Nothing here!");
                }
                ConsoleTableBuilder.From(list)
                .ExportAndWriteLine();

                HelpersAndValidation.InsertSeparator();
                Console.WriteLine("Press 0 to go back");

                string op = Console.ReadLine();

                if (op == "0") {
                    Console.Clear();
                    managing = false;
                } else {
                    Console.WriteLine("Invalid input");
                }
            }
        }

        static void InsertFlashcards( Stack stack ) {

            Console.Clear();
            Console.WriteLine("What would be the front of the card?");
            string front = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("What about the back?");
            string back = Console.ReadLine();

            DbOperations.CreateCard(front, back, stack);
            Console.WriteLine("Card Created!");
            HelpersAndValidation.InsertSeparator();
        }
    }
}
