using System.Collections;
using System.ComponentModel.Design;

namespace Flashcards;

class DataController
{
    public bool RunFlashCardsProgram;
    public bool RunStacksController;
    public bool RunFlashCardsController;
    public bool RunStudySessionController;
    public Stacks? SelectedStack;

    public int StudySessionQuestions;
    public DataController()
    {
        RunFlashCardsProgram = true;
        RunStacksController = false;
        RunFlashCardsController = false;
        RunStudySessionController = false;
        SelectedStack = null;
        StudySessionQuestions = 5;
    }

    public void MainMenuController()
    {
        UI.WelcomeMessage();
        string? errorMessage = null;
        string selection;
        do
        {
            UI.MainMenu(SelectedStack?.StackName, errorMessage);
            selection = Console.ReadLine() ?? "";
            errorMessage = InputValidation.ValidateSelection(selection, 0, 4);
            switch(selection)
            {
                case("1"):
                    RunStacksController = true;
                    StacksController();
                    break;
                case("2"):
                    if(SelectedStack == null)
                    {
                        errorMessage += "You have not selected a stack. "; //Pending
                    }
                    else
                    {
                        RunFlashCardsController = true;
                        FlashCardsController();
                    }
                    break;
                case("3"):
                    RunStudySessionController = true; //Table Empty constrain, selected stack constrain
                    StudySessionController();
                    break;
                case("4"):
                    break;
                case("0"):
                    RunFlashCardsProgram = false;
                    break;
            }
        }
        while(RunFlashCardsProgram);

        UI.ExitMessage();
    }

    public void StacksController()
    {
        string selection;
        string? errorMessage = null;
        do
        {
            UI.Stacks(errorMessage);
            selection = Console.ReadLine() ?? "";
            errorMessage = InputValidation.ValidateSelection(selection, 0, 4);
            switch(selection)
            {
                case("1"):
                    NewOrModifyStackController();
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
            }
        }
        while(RunStacksController);
    }

    public void FlashCardsController()
    {
        string? errorMessage = null;
        string selection;

        do
        {
            UI.FlashCards(SelectedStack?.StackName, errorMessage);
            selection = Console.ReadLine() ?? "";
            errorMessage = InputValidation.ValidateSelection(selection, 0, 4);
            switch(selection)
            {
                case("1"):
                    SelectOrDeleteCardController(null); //table empty constrain
                    break;
                case("2"):
                    NewOrModifyCardController();
                    break;
                case("3"):
                    SelectOrDeleteCardController("modify");
                    break;
                case("4"):
                    SelectOrDeleteCardController("delete");
                    break;
                case("0"):
                    RunFlashCardsController = false;
                    break;
            }
        }
        while(RunFlashCardsController);
    }

    public void StudySessionController()
    {
        string? errorMessage = null;
        string selection;
        
        do
        {
            UI.StudySession(errorMessage);
            selection = Console.ReadLine() ?? "";
            errorMessage = InputValidation.ValidateSelection(selection, 0, 2);
            switch(selection)
            {
                case("1"):
                    RunStudySession();
                    break;
                case("2"):
                    // Modify Parameters
                    break;
                case("0"):
                    RunStudySessionController = false;
                    break;
            }
        }
        while(RunStacksController);
    }

    public void StudySessionsDataController()
    {

    }

    public void NewOrModifyStackController(string modifyStackName = "", string? action = null)
    {
        string stackName;
        string? errorMessage = null;
        do
        {
            UI.NewStackName(errorMessage, action);
            stackName = Console.ReadLine() ?? "";
            errorMessage = InputValidation.ValidateNewStackName(stackName);

        }
        while(errorMessage != null);

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
                NewOrModifyStackController(stackName, "new");
                if (SelectedStack?.StackName == stackName)
                    SelectedStack = null;
                break;
        }
    }

    public void NewOrModifyCardController(Cards? oldCard = null, string? action = null)  //Delete?
    {
        string cardQuestion;
        string cardAnswer;
        string? errorMessage = null;
        do
        {
            UI.NewCard(errorMessage, "question", action);
            cardQuestion = Console.ReadLine() ?? "";
            errorMessage = InputValidation.ValidateNewStackName(cardQuestion);
        }
        while(errorMessage != null);

        do
        {
            UI.NewCard(errorMessage, "answer", action);
            cardAnswer = Console.ReadLine() ?? "";
            errorMessage = InputValidation.ValidateNewStackName(cardAnswer);
        }
        while(errorMessage != null);


        if (action == null)
        {
            DBController.InsertNewCard(new Cards(SelectedStack?.StackID ?? 0, cardQuestion, cardAnswer));
        }
        else if (action == "new")
        {
            DBController.ModifyCard(
                new Cards(SelectedStack?.StackID ?? 0, cardQuestion, 
                cardAnswer, oldCard?.CardID ?? 0));
        }
    }

    public void SelectOrDeleteCardController(string? selectionString)
    {
        List<Cards> currentCards = DBController.SelectFlashcards(SelectedStack);
        List<CardsDTO> currentCardsToUI = CardsToCardsDTO(currentCards);
        string cardID;
        string? errorMessage = null;

        do
        {
            UI.DisplayCards(currentCardsToUI,SelectedStack, errorMessage, selectionString);
            cardID = Console.ReadLine() ?? "";
            if(selectionString != null)
            {
                errorMessage = InputValidation.ValidateSelection(cardID,currentCardsToUI[0].CardID, 
                currentCardsToUI[^1].CardID);
                errorMessage ??= InputValidation.ValidateCardSelection(currentCards, cardID);
            }
        }
        while(errorMessage != null);

        switch(selectionString)
        {
            case("delete"):
                Cards deleteCard = currentCards[
                    currentCardsToUI.FindIndex(cards => cards.CardID == Convert.ToInt32(cardID))];
                DBController.DeleteCard(deleteCard);
                break;
            case("modify"):
                Cards oldCard = currentCards[
                    currentCardsToUI.FindIndex(cards => cards.CardID == Convert.ToInt32(cardID))];
                NewOrModifyCardController(oldCard,"new");
                break;
        }
    }

    public void RunStudySession()
    {
        List<Cards> studySessionCards = DBController.SelectCardsStudySession(StudySessionQuestions, SelectedStack);
        string answer;
        bool answerIsCorrect;
        int score = 0;

        foreach (Cards studySessionCard in studySessionCards)
        {
            UI.StudySessionQuestion(studySessionCard);
            answer = Console.ReadLine() ?? "";
            answerIsCorrect = InputValidation.ValidateStudySessionAnswer(studySessionCard, answer);
            UI.StudySessionAnswer(studySessionCard, answerIsCorrect);
            Console.ReadLine();
            if (answerIsCorrect)
            {
                score++;
            }
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

    public static List<CardsDTO> CardsToCardsDTO(List<Cards> cards)
    {
        List<CardsDTO> cardsToUI = [];

        for(int i=0; i<cards.Count; i++)
        {
            cardsToUI.Add(new CardsDTO(cards[i], i+1));
        }
        return cardsToUI;
    }

}