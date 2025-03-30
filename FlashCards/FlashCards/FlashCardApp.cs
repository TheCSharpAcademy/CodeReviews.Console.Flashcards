using FlashCards.Models;
using Spectre.Console;
using System.Text.Json;

namespace FlashCards
{
    internal class FlashCardApp
    {
        UserInterface UserInterface { get; }

        CardStackRepositoryService CardStackRepositoryService { get; }
        FlashCardRepositoryService FlashCardRepositoryService { get; }
        StudySessionRepositoryService StudySessionRepositoryService { get; }

        string pathToDefaultData {  get; }

        public FlashCardApp(CardStackRepositoryService cardStackService, FlashCardRepositoryService flashCardService, StudySessionRepositoryService studySessionService, UserInterface userInterface, string pathToDefaultData)
        {
            UserInterface = userInterface;
            CardStackRepositoryService = cardStackService;
            FlashCardRepositoryService = flashCardService;
            StudySessionRepositoryService = studySessionService;
            this.pathToDefaultData  = pathToDefaultData;
        }
        private DefaultDataObject GetDefaultData()
        {
            var defaultData = JsonSerializer.Deserialize<DefaultDataObject>(File.ReadAllText(pathToDefaultData));
            return defaultData!;
        }
        public void PrepareApp()
        {
            var defaultData = GetDefaultData();

            CardStackRepositoryService.PrepareRepository(defaultData.Stacks);
            List<CardStack> stacks = CardStackRepositoryService.GetAllStacks();

            FlashCardRepositoryService.PrepareRepository(stacks, defaultData.FlashCards);

            StudySessionRepositoryService.PrepareRepository(stacks, defaultData.StudySessions);


        }
        public void Run()
        {
            PrepareApp();

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
                    StudySessionRepositoryService.PrintReport();
                    break;
                case MainMenuOption.Exit:
                    return;
            }

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
            List<FlashCardDto> cards = FlashCardRepositoryService.GetAllCardsInStack(stack);

            StudySessionRepositoryService.NewStudySession(stack, cards);
        }

        private void HandleManageStacks()
        {
            StackMenuOption stackMenuOption = UserInterface.GetStackMenuSelection();
            while (stackMenuOption != StackMenuOption.ReturnToMainMenu)
            {
                ProcesStackMenu(stackMenuOption);
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

            ProcessFlashCardMenu(flashCardMenuOption, stack);
        }
        
        private void ProcesStackMenu(StackMenuOption stackMenuOption) 
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
