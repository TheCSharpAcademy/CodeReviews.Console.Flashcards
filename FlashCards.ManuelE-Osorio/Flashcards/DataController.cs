using System.Collections;
using System.ComponentModel.Design;

namespace Flashcards;

class DataController
{
    public bool RunFlashCardsProgram;
    public bool RunStacksController;
    public bool RunFlashCardsController;
    public bool RunStudySessionController;
    public bool NewStackNameIsInvalid;
    public Stacks? SelectedStack;

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
            UI.MainMenu(SelectedStack?.StackName);
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
                    NewStackOrModifyController();
                    break;
                case("2"):
                    SelectOrDeleteStackController("select");
                    break;
                case("3"):
                    SelectOrDeleteStackController("modify");
                    break;
                case("4"):
                    SelectOrDeleteStackController("delete");
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
        string? selection;

        do
        {
            UI.FlashCards(SelectedStack?.StackName);
            selection = Console.ReadLine();
            switch(selection)
            {
                case("1"):
                    break;
                case("2"):
                    break;
                case("3"):
                    break;
                case("4"):
                    break;
                case("0"):
                    RunFlashCardsController = false;
                    break;
                default:
                    break;
            }
        }
        while(RunFlashCardsController);
    }

    public void StudySessionController()
    {

    }

    public void StudySessionsDataController()
    {

    }

    public void NewStackOrModifyController(string modifyStackName = "", string? action = null)
    {
        string stackName;
        string? errorMessage = null;
        do
        {
            UI.NewStackName(errorMessage, action);
            stackName = Console.ReadLine() ?? "";
            errorMessage = InputValidation.ValidateNewStackName(stackName);

            if (errorMessage == null)
            {
                NewStackNameIsInvalid = false;
            }
        }
        while(NewStackNameIsInvalid);

        if (action == null)
        {
            DBController.InsertNewStack(new Stacks(stackName));
        }
        else if (action == "new")
        {
            DBController.ModifyStack(new Stacks(modifyStackName), new Stacks(stackName));
        }
    }

    public void SelectOrDeleteStackController(string selectionString)
    {
        List<Stacks> currentStacks = DBController.SelectStacks();
        List<StacksDTO> currentStacksToUI = StacksToStacksDTO(currentStacks);
        string stackName;
        string? errorMessage = null;

        do
        {
            UI.SelectOrDeleteStack(currentStacksToUI, errorMessage, selectionString);
            stackName = Console.ReadLine() ?? "";
            errorMessage = InputValidation.ValidateStackSelection(currentStacks, stackName);
        }
        while(errorMessage != null);

        switch(selectionString)
        {
            case("select"):
                SelectedStack = currentStacks.Find(stacks => stacks.StackName == stackName);
                break;
            case("delete"):
                DBController.DeleteStack(stackName);
                if (SelectedStack?.StackName == stackName)
                    SelectedStack = null;
                break;
            case("modify"):
                NewStackOrModifyController(stackName, "new");
                if (SelectedStack?.StackName == stackName)
                    SelectedStack = null;
                break;
        }
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

}