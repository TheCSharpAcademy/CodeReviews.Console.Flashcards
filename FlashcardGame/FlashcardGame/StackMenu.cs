namespace FlashcardGame
{
    public static class StackMenu
    {
        public static void RunStackMenu()
        {
            bool runStackMenu = true;
            while (runStackMenu)
            {
                Console.Clear();
                Console.WriteLine("What you want to do with stacks?");

                Console.WriteLine("Options: ");
                Console.WriteLine("a. Manage already existing stack");
                Console.WriteLine("b. Create new stack");
                Console.WriteLine("c. Delete stack");
                Console.WriteLine("d. Return to main menu");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "a":
                        ManageStackMenu.ManageStacks();
                        break;
                    case "b":
                        DatabaseHelpers.AddStack();
                        break;
                    case "c":
                        DatabaseHelpers.DeleteStack();
                        break;
                    case "d":
                        MainMenu.Get1UserInput();
                        break;
                    default:
                        Console.WriteLine("Wrong input. Please try again");
                        Console.WriteLine("Press any key to continue: ");
                        Console.ReadLine();
                        break;

                }
            }
        }
        
    }
}
