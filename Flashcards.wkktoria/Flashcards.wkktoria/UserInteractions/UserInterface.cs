using Flashcards.wkktoria.Managers;
using Flashcards.wkktoria.Services;

namespace Flashcards.wkktoria.UserInteractions;

internal class UserInterface
{
    private readonly StackManager _stackManager;

    internal UserInterface(StackService stackService, CardService cardService)
    {
        _stackManager = new StackManager(stackService, cardService);
    }

    private static void ShowMenu()
    {
        Console.WriteLine("""
                          0 - Exit
                          1 - Manage stacks
                          2 - Manage cards
                          """);
    }

    internal void Run()
    {
        var quit = false;

        while (!quit)
        {
            Console.Clear();
            ShowMenu();

            Console.Write("> ");
            var option = Console.ReadLine();

            switch (option!.Trim())
            {
                case "0":
                    quit = true;
                    break;
                case "1":
                    _stackManager.Run();
                    break;
                case "2":
                    UserOutput.InfoMessage("NOT IMPLEMENTED");
                    break;
                default:
                    UserOutput.ErrorMessage("Invalid option.");
                    UserOutput.InfoMessage("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}