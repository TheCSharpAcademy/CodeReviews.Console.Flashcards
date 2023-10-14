using Flashcards.wkktoria.Managers;
using Flashcards.wkktoria.Managers.Helpers;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions.Helpers;

namespace Flashcards.wkktoria.UserInteractions;

internal class UserInterface
{
    private readonly ExistingStackManager _existingStackManager;
    private readonly SessionManager _sessionManager;
    private readonly StackManager _stackManager;
    private readonly StudyManager _studyManager;

    internal UserInterface(StackService stackService, CardService cardService, SessionService sessionService)
    {
        _stackManager = new StackManager(stackService, cardService);
        _existingStackManager = new ExistingStackManager(stackService, cardService);
        _studyManager = new StudyManager(stackService, cardService, sessionService);
        _sessionManager = new SessionManager(stackService, sessionService);
    }

    private static void ShowMenu()
    {
        Console.WriteLine("""
                          0 - Exit
                          1 - Manage stacks
                          2 - Manage cards
                          3 - Study
                          4 - Show study sessions
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
                case "3":
                    _studyManager.Run();
                    break;
                case "4":
                    _sessionManager.Run();
                    break;
                default:
                    UserOutput.ErrorMessage("Invalid option.");
                    ConsoleHelpers.PressToContinue();
                    break;
            }
        }
    }
}