using Spectre.Console;
public static class Menu
{
    public static string userInput = "";
    private static DatabaseManager db = new DatabaseManager();
    public static void DisplayMenu()
    {
        Console.WriteLine("--------------------------------------------------");

        Console.WriteLine("exit");
        Console.WriteLine("Manage Stacks");
        Console.WriteLine("Manage Flashcards");
        Console.WriteLine("Study");
        Console.WriteLine("view study session data");

        Console.WriteLine("--------------------------------------------------");

        

        while (userInput != "exit")
        {
            userInput = Console.ReadLine();

            // Switch statement for menu options
            switch (userInput.ToLower())
            {
                case "manage stacks":

                    SelectionPrompt<string> prompt = new SelectionPrompt<string>();

                    // Build the prompt
                    foreach (Stack stack in db.GetStacks())
                    {
                        prompt.AddChoice(stack.name);
                    }
                    // Display the prompt
                    string menuChoice = AnsiConsole.Prompt(prompt);

                    break;
            }
        }
    }

}