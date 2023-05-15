using ConsoleTableExt;
using Flashcards.CRUD;
using Flashcards.Helpers;
using Flashcards.Model;

namespace Flashcards.Menus {
    internal class ChooseStudyStack {

        internal static void ChooseStacksMenu() {

            bool managing = true;

            Console.Clear();
            List<Stack> stacks = DbOperations.ShowStacksList();


            ConsoleTableBuilder.From(HelpersAndValidation.GetNames(stacks))
                .WithColumn("Name")
                .ExportAndWriteLine();

            HelpersAndValidation.InsertSeparator();
            Console.WriteLine("This is the menu to choose a stack for a study session.\n");
            Console.WriteLine("Type the name of a stack or exit (0)\n");

            while (managing) {
                string op = Console.ReadLine();

                switch (op) {
                    case "0":
                        managing = false;
                        break;
                    default:
                        if (stacks.Any(stack => stack.Name == op)) {
                            ManageStudySessionMenu.ManageStudySessions(stacks.Find(stack => stack.Name == op));
                        } else {
                            Console.Clear();
                            Console.WriteLine("Invalid stack name");
                            HelpersAndValidation.InsertSeparator();
                            stacks = DbOperations.ShowStacksList();
                        }
                        break;
                }
            }
            MainMenu.GetMainInput();
        }
    }
}
