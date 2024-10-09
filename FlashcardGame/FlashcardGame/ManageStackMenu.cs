using Microsoft.VisualBasic.FileIO;

namespace FlashcardGame
{
    internal class ManageStackMenu
    {
        public static void ManageStacks()
        {
            Console.Clear();
            List<Stack> stacks = DataAccess.GetStacks();

            if (stacks.Count == 0)
            {
                Console.WriteLine("No stacks were found");
                Console.WriteLine("Press any key to return");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Choose with what stack you want to interact(write full name of it) or press 0 to exit");
            
            for (int i = 0; i < stacks.Count; i++)
            {
                Stack stack = stacks[i];
                Console.WriteLine($"{i + 1}. {stack.stack_name}");
            }

            string stackName = Console.ReadLine();

            if (stackName == "0")
            {
                return;
            }

            while (!stacks.Any(s => s.stack_name == stackName))
            {
                Console.WriteLine("Wrong name try again!");
                stackName = Console.ReadLine();
            }

            int id = 0;
            foreach (var stack in stacks.Where(stack => stack.stack_name == stackName))
            {
                id = stack.stack_id;
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Current stack: {stackName}");
                Console.WriteLine("Options:");
                Console.WriteLine("a. Change current stack");
                Console.WriteLine("b. View X amount of cards in stack");
                Console.WriteLine("c. Create a flashcard in current stack");
                Console.WriteLine("d. Edit a flashcard");
                Console.WriteLine("e. Delete a flashcard");
                Console.WriteLine("f. Return to main menu");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "a":
                        StackMenu.RunStackMenu();
                        break;
                    case "b":
                        DatabaseHelpers.ViewFlashcard(id);
                        break;
                    case "c":
                        DatabaseHelpers.AddFlashcard(id);
                        break;
                    case "d":
                        DatabaseHelpers.UpdateFlashcard(id);
                        break;
                    case "e":
                        DatabaseHelpers.DeleteFlashcard(id);
                        break;
                    case "f":
                        MainMenu.Get1UserInput();
                        break;
                    default:
                        Console.WriteLine("Wrong input. Please try again");
                        break;
                }
            }
        }
    }
}
