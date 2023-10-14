using Flashcards.wkktoria.Managers.Helpers;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions;
using Flashcards.wkktoria.UserInteractions.Helpers;

namespace Flashcards.wkktoria.Managers;

internal class StackManager
{
    private readonly AllStackManager _allStackManager;
    private readonly ExistingStackManager _existingStackManager;

    internal StackManager(StackService stackService, CardService cardService, SessionService sessionService)
    {
        _allStackManager = new AllStackManager(stackService, cardService, sessionService);
        _existingStackManager = new ExistingStackManager(stackService, cardService);
    }

    private static void ShowMenu()
    {
        UserOutput.InfoMessage("""
                               0 - Return
                               1 - Manage all stacks
                               2 - Manage existing stack
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
                    _allStackManager.Run();
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