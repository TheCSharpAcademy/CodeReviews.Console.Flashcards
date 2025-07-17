using FlashCard.SheheryarRaza.Managers;

namespace FlashCard.SheheryarRaza
{
    public class FlashcardApp
    {
        private readonly StackManager _stackManager;
        private readonly FlashcardManager _flashcardManager;
        private readonly StudySessionManager _studySessionManager;

        public FlashcardApp(StackManager stackManager, FlashcardManager flashcardManager, StudySessionManager studySessionManager)
        {
            _stackManager = stackManager;
            _flashcardManager = flashcardManager;
            _studySessionManager = studySessionManager;
        }

        public void RunApplication()
        {
            bool running = true;
            while (running)
            {
                ConsoleHelper.ClearConsole();
                ConsoleHelper.DisplayMessage("--- Flashcard Application Menu ---", ConsoleColor.Cyan);
                Console.WriteLine("1. Manage Stacks");
                Console.WriteLine("2. Manage Flashcards");
                Console.WriteLine("3. Start Study Session");
                Console.WriteLine("4. View Study Sessions");
                Console.WriteLine("5. Exit");
                ConsoleHelper.DisplayMessage("----------------------------------", ConsoleColor.Cyan);

                string choice = ConsoleHelper.GetStringInput("Enter your choice: ");

                switch (choice)
                {
                    case "1":
                        _stackManager.ManageStacks();
                        break;
                    case "2":
                        _flashcardManager.ManageFlashcards();
                        break;
                    case "3":
                        _studySessionManager.StartStudySession();
                        break;
                    case "4":
                        _studySessionManager.ViewStudySessions();
                        break;
                    case "5":
                        running = false;
                        ConsoleHelper.DisplayMessage("Exiting application. Goodbye!", ConsoleColor.Green);
                        break;
                    default:
                        ConsoleHelper.DisplayMessage("Invalid choice. Please try again.", ConsoleColor.Red);
                        ConsoleHelper.PressAnyKeyToContinue();
                        break;
                }
            }
        }
    }
}
