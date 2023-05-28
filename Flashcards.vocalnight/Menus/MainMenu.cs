using Flashcards.CRUD;
using Flashcards.Helpers;
using Flashcards.Model;

namespace Flashcards.Menus {
    internal class MainMenu {

        internal static void GetMainInput() {

            bool isRunning = true;
            while (isRunning) {
                Console.WriteLine("Welcome to the flashCard app");
                Console.WriteLine("What would you like to do today?");
                Console.WriteLine(@"1 - Add a new card stack
2 - Manage your stacks
3 - Study
0 - Exit");

                string op = Console.ReadLine();
                Console.Clear();

                switch (op) {
                    case "1":
                        try {
                            CreateStack();
                        } catch (Exception ex) {
                            HelpersAndValidation.DealWithError(ex);
                        }
                        break;
                    case "2":
                        ChooseStackMenu.ChooseStacksMenu();
                        break;
                    case "3":
                        ChooseStudyStack.ChooseStacksMenu();
                        break;
                    case "0":
                        Console.WriteLine("\nGoodbye");
                        isRunning = false;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input");
                        break;
                }
                HelpersAndValidation.InsertSeparator();
            }
        }

        internal static void CreateStack() {
            Console.WriteLine("Please type the name of the stack you wish to add");
            HelpersAndValidation.InsertSeparator();

            string op = Console.ReadLine();

            while (StackExists(op)) {
                Console.WriteLine("This stack name is already in use, please use another one");
                op = Console.ReadLine();
            }

            DbOperations.AddStack(op);
        }

        internal static bool StackExists( string op ) {
            List<Stack> stacks = DbOperations.ShowStacksList();

            return stacks.Any(stack => stack.Name == op);
        }
    }
}
