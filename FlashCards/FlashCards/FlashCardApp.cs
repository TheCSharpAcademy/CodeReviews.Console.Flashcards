using System.Text.Json;

namespace FlashCards
{
    internal class FlashCardApp
    {
        IFlashCardAppUi UserInterface { get; }
        ICardStackService CardStackRepositoryService { get; }
        IFlashCardService FlashCardRepositoryService { get; }
        IStudySessionService StudySessionRepositoryService { get; }

        private string PathToDefaultData {  get; }
        private bool AutoFill { get; }
        public FlashCardApp(ICardStackService cardStackService, IFlashCardService flashCardService, IStudySessionService studySessionService, IFlashCardAppUi userInterface, string pathToDefaultData, bool autoFill = false)
        {
            UserInterface = userInterface;
            CardStackRepositoryService = cardStackService;
            FlashCardRepositoryService = flashCardService;
            StudySessionRepositoryService = studySessionService;
            PathToDefaultData = pathToDefaultData;
            AutoFill = autoFill;
        }
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
        public bool PrepareApp()
        {
            if (!AutoFill) { return true; }

            var defaultData = GetDefaultData();

            if (defaultData == null)
            {
                Console.WriteLine("Failed to load default data.");
                return false;
            }

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
        public void Run()
        {

            if(!PrepareApp()) { return; }

            UserInterface.PrintApplicationHeader();

            MainMenuOption mainMenuOption = UserInterface.GetMainMenuSelection();
            while(mainMenuOption != MainMenuOption.Exit)
            {
                ProcessMainMenu(mainMenuOption);
                UserInterface.ClearConsole();
                mainMenuOption = UserInterface.GetMainMenuSelection();
            }
        }
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
                case MainMenuOption.Exit:
                    return;
            }

        }
        private void HandleGetReport()
        {
            List<CardStack> stacks = CardStackRepositoryService.GetAllStacks();
            StudySessionRepositoryService.PrintReport(stacks);
        }
        private void HandleViewSessions()
        {
            List<CardStack> stacks = CardStackRepositoryService.GetAllStacks();
            StudySessionRepositoryService.PrintAllSessions(stacks);
        }
        private  void HandleStudy()
        {
            List<CardStack> stacks = CardStackRepositoryService.GetAllStacks();
            CardStack stack = UserInterface.StackSelection(stacks);

            List<FlashCardDto> cards = FlashCardRepositoryService.GetAllCardsInStack(stack) ?? new List<FlashCardDto>();
            StudySessionRepositoryService.NewStudySession(stack, cards);
        }

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
