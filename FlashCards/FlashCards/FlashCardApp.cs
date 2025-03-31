using System.Text.Json;

namespace FlashCards
{
    /// <summary>
    /// Represents the FlashCard application, handling initialization, user interaction, and main menu processing.
    /// </summary>
    internal class FlashCardApp
    {
        /// <summary>
        /// Gets the user interface for the application.
        /// </summary>
        IFlashCardAppUi UserInterface { get; }

        /// <summary>
        /// Gets the service for managing card stacks.
        /// </summary>
        ICardStackService CardStackRepositoryService { get; }

        /// <summary>
        /// Gets the service for managing flashcards.
        /// </summary>
        IFlashCardService FlashCardRepositoryService { get; }

        /// <summary>
        /// Gets the service for managing study sessions.
        /// </summary>
        IStudySessionService StudySessionRepositoryService { get; }

        /// <summary>
        /// Gets the file path to the default data file.
        /// </summary>
        private string PathToDefaultData { get; }

        /// <summary>
        /// Indicates whether the application should auto-fill with default data on startup.
        /// </summary>
        private bool AutoFill { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlashCardApp"/> class.
        /// </summary>
        public FlashCardApp(ICardStackService cardStackService, IFlashCardService flashCardService, IStudySessionService studySessionService, IFlashCardAppUi userInterface, string pathToDefaultData, bool autoFill = false)
        {
            UserInterface = userInterface;
            CardStackRepositoryService = cardStackService;
            FlashCardRepositoryService = flashCardService;
            StudySessionRepositoryService = studySessionService;
            PathToDefaultData = pathToDefaultData;
            AutoFill = autoFill;
        }

        /// <summary>
        /// Loads default data from a JSON file.
        /// </summary>
        private DefaultDataObject GetDefaultData()
        {
            try
            {
                if (!File.Exists(PathToDefaultData))
                {
                    Console.WriteLine("Default data file not found.");
                    return new DefaultDataObject();
                }

                var jsonData = File.ReadAllText(PathToDefaultData);
                return JsonSerializer.Deserialize<DefaultDataObject>(jsonData) ?? new DefaultDataObject();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading default data: {ex.Message}");
                return new DefaultDataObject();
            }
        }

        /// <summary>
        /// Prepares the application by loading default data and initializing repositories.
        /// </summary>
        public bool PrepareApp()
        {
            if (!AutoFill) { return true; }

            var defaultData = GetDefaultData();

            if (!CardStackRepositoryService.PrepareRepository(defaultData.Stacks))
            {
                Console.WriteLine("Error initializing Card Stack repository.");
                return false;
            }

            var stacks = CardStackRepositoryService.GetAllStacks();

            if (!FlashCardRepositoryService.PrepareRepository(stacks, defaultData.FlashCards))
            {
                Console.WriteLine("Error initializing Flash Card repository.");
                return false;
            }

            if (!StudySessionRepositoryService.PrepareRepository(stacks, defaultData.StudySessions))
            {
                Console.WriteLine("Error initializing Study Session repository.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Runs the main loop of the application.
        /// </summary>
        public void Run()
        {
            if (!PrepareApp()) { return; }

            UserInterface.PrintApplicationHeader();
            MainMenuOption mainMenuOption = UserInterface.GetMainMenuSelection();
            while (mainMenuOption != MainMenuOption.Exit)
            {
                ProcessMainMenu(mainMenuOption);
                UserInterface.ClearConsole();
                mainMenuOption = UserInterface.GetMainMenuSelection();
            }
        }

        /// <summary>
        /// Processes the selected main menu option.
        /// </summary>
        private void ProcessMainMenu(MainMenuOption mainMenuOption)
        {
            switch (mainMenuOption)
            {
                case MainMenuOption.ManageStacks:
                    HandleManageStacks();
                    break;
                case MainMenuOption.ManageFlashCards:
                    HandleManageFlashCards();
                    break;
                case MainMenuOption.Study:
                    HandleStudy();
                    break;
                case MainMenuOption.ViewStudySessions:
                    HandleViewSessions();
                    break;
                case MainMenuOption.GetReport:
                    HandleGetReport();
                    break;
            }
        }
        /// <summary>
        /// Handles the management of stacks by allowing the user to select various stack-related options.
        /// Loops until the user chooses to return to the main menu.
        /// </summary>
        private void HandleManageStacks()
        {
            StackMenuOption stackMenuOption = UserInterface.GetStackMenuSelection();
            while (stackMenuOption != StackMenuOption.ReturnToMainMenu)
            {
                ProcessStackMenu(stackMenuOption);
                UserInterface.ClearConsole();
                stackMenuOption = UserInterface.GetStackMenuSelection();
            }
        }
        /// <summary>
        /// Handles the management of flashcards in specific stack by allowing the user to select various stack-related options.
        /// Loops until the user chooses to return to the main menu.
        /// </summary>
        private void HandleManageFlashCards()
        {
            List<CardStack> stacks = CardStackRepositoryService.GetAllStacks();
            CardStack stack = UserInterface.StackSelection(stacks);

            FlashCardMenuOption flashCardMenuOption = UserInterface.GetFlashCardMenuSelection();

            while (flashCardMenuOption != FlashCardMenuOption.ReturnToMainMenu)
            {
                ProcessFlashCardMenu(flashCardMenuOption, stack);
                UserInterface.ClearConsole();
                flashCardMenuOption = UserInterface.GetFlashCardMenuSelection();
            }
        }
        /// <summary>
        /// Initiates new StudySession
        /// </summary>
        private void HandleStudy()
        {
            List<CardStack> stacks = CardStackRepositoryService.GetAllStacks();
            CardStack stack = UserInterface.StackSelection(stacks);

            List<FlashCardDto> cards = FlashCardRepositoryService.GetAllCardsInStack(stack) ?? new List<FlashCardDto>();
            StudySessionRepositoryService.NewStudySession(stack, cards);
        }
        /// <summary>
        /// Displays all study sessions associated with the available stacks.
        /// </summary>
        private void HandleViewSessions()
        {
            List<CardStack> stacks = CardStackRepositoryService.GetAllStacks();
            StudySessionRepositoryService.PrintAllSessions(stacks);
        }
        /// <summary>
        /// Generates and displays a study report for all available stacks.
        /// </summary>
        private void HandleGetReport()
        {
            List<CardStack> stacks = CardStackRepositoryService.GetAllStacks();
            StudySessionRepositoryService.PrintReport(stacks);

        }

        /// <summary>
        /// Processes the selected stack menu option and invokes the appropriate service method.
        /// </summary>
        /// <param name="stackMenuOption">The stack menu option selected by the user.</param>
        private void ProcessStackMenu(StackMenuOption stackMenuOption) 
        {
            switch (stackMenuOption) 
            {
                case StackMenuOption.ViewAllStacks:
                    CardStackRepositoryService.HandleViewAllStacks();
                    break;
                case StackMenuOption.CreateNewStack:
                    CardStackRepositoryService.HandleCreateNewStack();
                    break;
                case StackMenuOption.RenameStack:
                    CardStackRepositoryService.HandleRenameStack();
                    break;
                case StackMenuOption.DeleteStack:
                    CardStackRepositoryService.HandleDeleteStack();
                    break;
                case StackMenuOption.ReturnToMainMenu:
                    return;
            }
        }

        // <summary>
        /// Processes the selected flashcard menu option and invokes the appropriate service method.
        /// </summary>
        /// <param name="flashCardMenuOption">The flashcard menu option selected by the user.</param>
        /// <param name="stack">The currently selected stack of flashcards.</param>
        private void ProcessFlashCardMenu(FlashCardMenuOption flashCardMenuOption, CardStack stack) 
        {
            switch (flashCardMenuOption)
            {
                case FlashCardMenuOption.ViewAllCards:
                    FlashCardRepositoryService.HandleViewAllCards(stack);
                    break;
                case FlashCardMenuOption.ViewXCards:
                    FlashCardRepositoryService.HandleViewXCards(stack);
                    break;
                case FlashCardMenuOption.CreateNewFlashCard:
                    FlashCardRepositoryService.HandleCreateNewFlashCard(stack);
                    break;
                case FlashCardMenuOption.UpdateFlashCard:
                    FlashCardRepositoryService.HandleUpdateFlashCard(stack);
                    break;
                case FlashCardMenuOption.DeleteFlashCard:
                    FlashCardRepositoryService.HandleDeleteFlashCard(stack);
                    break;
                case FlashCardMenuOption.SwitchStack:
                    List<CardStack> stacks = CardStackRepositoryService.GetAllStacks();
                    stack = FlashCardRepositoryService.HandleSwitchStack(stacks);
                    break;
            }
        }
    }
}
