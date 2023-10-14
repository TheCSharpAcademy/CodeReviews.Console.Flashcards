using Flashcards.wkktoria.Managers;
using Flashcards.wkktoria.Managers.Helpers;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions.Helpers;

namespace Flashcards.wkktoria.UserInteractions;

internal class UserInterface
{
    private readonly ExistingStackManager _existingStackManager;
    private readonly StackManager _stackManager;

    internal UserInterface(StackService stackService, CardService cardService)
    {
        _stackManager = new StackManager(stackService, cardService);
        _existingStackManager = new ExistingStackManager(stackService, cardService);
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
                    _existingStackManager.Run();
                    break;
                default:
                    UserOutput.ErrorMessage("Invalid option.");
                    ConsoleHelpers.PressToContinue();
                    break;
            }
        }
    }
}