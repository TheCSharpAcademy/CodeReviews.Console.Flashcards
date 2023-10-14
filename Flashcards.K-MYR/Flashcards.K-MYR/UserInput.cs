using Flashcards.K_MYR.Models;

namespace Flashcards.K_MYR;

internal class UserInput
{
    internal static void MainMenu()
    {
        while (true)
        {
            Console.Clear();

            string[] options = { "Manage Stacks ", "Manage Flashcards ", "Study ", "View Study Session Data ", "Exit " };

            int selected = Helpers.PrintMenu(options);

            switch (selected)
            {
                case 0:
                    ManageStacksMenu();
                    break;
                case 1:
                    //ManageFlashcards();
                    break;
                case 2:
                    //Study();
                    break;
                case 3:
                    //View Study Session Data();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
            }
        }          
    }

    internal static void ManageStacksMenu()
    {
        bool returnToMainMenu = false;

        while (!returnToMainMenu)
        {        
            var stacks = SQLController.SelectStacksFromDB();
            List<CardStackDTO> stackDTOs = new();

            foreach (var stack in stacks) 
            {
                stackDTOs.Add(new CardStackDTO
                {
                    Name = stack.Name,
                    NumberOfCards = stack.NumberOfCards,
                    CreatedDate = stack.CreatedDate
                });
            }

            Helpers.PrintStacksMenu(stackDTOs);

            int selected = 3;            
            bool actionKeyPressed = false;

            while (!actionKeyPressed)
            {
                if (stackDTOs.Count != 0)
                {
                    Console.CursorLeft = 1;
                    Console.CursorTop = selected;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(">");
                    Console.ResetColor();
                    Console.CursorTop = stackDTOs.Count * 2 + 7;                    
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (stackDTOs.Count != 0)
                        {
                            Console.CursorTop = selected;
                            Console.CursorLeft = 1;
                            Console.Write(" ");
                            selected = Math.Max(3, selected - 2);                            
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (stackDTOs.Count != 0)
                        {
                            Console.CursorTop = selected;
                            Console.CursorLeft = 1;
                            Console.Write(" ");
                            selected = Math.Min(stackDTOs.Count * 2 + 1, selected + 2);                            
                        }
                        break;
                    case ConsoleKey.A:
                        Console.CursorLeft = 0;
                        string Name = GetStringInput("Please enter a unique name for the new stack: ");
                        SQLController.InsertStack(Name);
                        actionKeyPressed = true;
                        break;
                    case ConsoleKey.D:
                        if (stackDTOs.Count != 0)
                        {
                            int row = (selected - 3) / 2;
                            var stack = stacks.Where(x => x.Name == stackDTOs[row].Name).First();
                            stack.Delete();
                            actionKeyPressed = true;
                        }                        
                        break;
                    case ConsoleKey.R:
                        if (stackDTOs.Count != 0)
                        {
                            string newName = GetStringInput("Please enter a new unique name for the stack: ");
                            int row = (selected - 3) / 2;
                            var stack = stacks.Where(x => x.Name == stackDTOs[row].Name).First();
                            stack.Rename(newName);
                            actionKeyPressed = true;
                        }
                        break;
                    case ConsoleKey.E:
                        actionKeyPressed = true;
                        returnToMainMenu = true;
                        break;
                }
            }    
        }        
    }

    internal static string GetStringInput(string message)
    {
        string Input;

        do
        {
            Console.WriteLine("---------------------------------------------------------------------------------------");
            Console.WriteLine($"| {message,-84}|");
            Console.WriteLine("---------------------------------------------------------------------------------------");
            Console.CursorTop -= 2;
            Console.CursorLeft = 49;
            Input = Console.ReadLine().Trim();
            Console.CursorTop -= 2;

        } while (string.IsNullOrEmpty(Input) || SQLController.StackNameExists(Input) == 1);

        return Input;
    }
}
