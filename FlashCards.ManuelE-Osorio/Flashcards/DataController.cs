using System.ComponentModel.Design;

namespace Flashcards;

class DataController
{
    public bool RunFlashCardsProgram;
    public bool RunStacksController;
    public bool RunFlashCardsController;
    public bool RunStudySessionController;
    public bool NewStackNameIsInvalid;
    public string? SelectedStack;

    public DataController()
    {
        RunFlashCardsProgram = true;
        RunStacksController = false;
        RunFlashCardsController = false;
        RunStudySessionController = false;
        NewStackNameIsInvalid = false;
        SelectedStack = null;
    }

    public void MainMenuController()
    {
        UI.WelcomeMessage();
        string? selection;
        do
        {
            UI.MainMenu();
            selection = Console.ReadLine();
            switch(selection)
            {
                case("1"):
                    RunStacksController = true;
                    StacksController();
                    break;
                case("2"):
                    RunFlashCardsController = true;
                    FlashCardsController();
                    break;
                case("3"):
                    RunStudySessionController = true;
                    StudySessionController();
                    break;
                case("4"):
                    break;
                case("0"):
                    RunFlashCardsProgram = false;
                    break;
                default:
                    break;
            }
        }
        while(RunFlashCardsProgram);

        UI.ExitMessage();
    }

    public void StacksController()
    {
        string? selection;
        do
        {
            UI.Stacks();
            selection = Console.ReadLine();
            switch(selection)
            {
                case("1"):
                    NewStackNameIsInvalid = true;
                    NewStackController();
                    break;
                case("2"):
                    SelectStackController();
                    break;
                case("0"):
                    RunStacksController = false;
                    break;
                default:
                    break;
            }
        }
        while(RunStacksController);
    }

    public void FlashCardsController()
    {

    }

    public void StudySessionController()
    {

    }

    public void StudySessionsDataController()
    {

    }

    public void NewStackController()
    {
        string? stackName;
        string? errorMessage = null;
        do
        {
            UI.NewStackName(errorMessage);
            
            stackName = Console.ReadLine();
            errorMessage = InputValidation.ValidateNewStackName(stackName);

            if (errorMessage == null)
            {
                NewStackNameIsInvalid = false;
            }
        }
        while(NewStackNameIsInvalid);
    }

    public void SelectStackController()
    {
        List<Stacks> currentStacks = StacksDisplayController();
        string? selection = null;
        string? errorMessage = null;
        do
        {
            UI.SelectStack(errorMessage);
            selection = Console.ReadLine();
            errorMessage = InputValidation.ValidateStackSelection(currentStacks, selection);
        }
        while(errorMessage != null);

        SelectedStack = selection;
    }

    public static List<StacksDTO> StacksToStacksDTO(List<Stacks> stacks)
    {
        List<StacksDTO> stacksToUI = [];

        for(int i=0; i<stacks.Count; i++)
        {
            stacksToUI.Add(new StacksDTO(stacks[i],i+1));
        }

        return stacksToUI;
    }

    public static List<Stacks> StacksDisplayController()
    {
        List<Stacks> currentStacks = DBController.SelectStacks();
        List<StacksDTO> currentStacksToUI = StacksToStacksDTO(currentStacks);
        List<List<object>> listToUI = [];;
        
        foreach (StacksDTO stacksDTO in currentStacksToUI)
        {
            listToUI.Add([stacksDTO.StackID, stacksDTO.StackName]);
        }

        TableUI.PrintTable(listToUI);
        return currentStacks;
    }
}