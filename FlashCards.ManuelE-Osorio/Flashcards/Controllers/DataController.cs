namespace Flashcards;

class DataController
{
    public bool RunFlashCardsProgram;
    public bool RunStacksController;
    public bool RunFlashCardsController;
    public bool RunStudySessionController;
    public bool RunStudySessionDataController;
    public Stacks? SelectedStack;
    public int StudySessionQuestions;
    public DataController()
    {
        RunFlashCardsProgram = true;
        RunStacksController = false;
        RunFlashCardsController = false;
        RunStudySessionController = false;
        RunStudySessionDataController = false;
        SelectedStack = null;
        StudySessionQuestions = 5;
    }

    public void MainMenuController()
    {
        UI.WelcomeMessage();
        string? errorMessage = null; //DB Init missing
        string selection;
        do
        {
            UI.MainMenu(SelectedStack?.StackName, errorMessage);
            selection = Console.ReadLine() ?? "";
            errorMessage = InputValidation.ValidateSelection(selection, 0, 4);
            errorMessage ??= ConstraintsValidation.MainMenuConstraints(selection, SelectedStack);
            if(errorMessage == null)
            {
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
                        RunStudySessionDataController = true;
                        StudySessionsDataController();
                        break;
                    case("0"):
                        RunFlashCardsProgram = false;
                        break;
                }
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
            errorMessage ??= ConstraintsValidation.StacksMenuConstraints(selection);
            if(errorMessage == null)
            {
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
            errorMessage ??= ConstraintsValidation.FlashCardsMenuConstraints(selection, SelectedStack);
            if(errorMessage == null)
            {
                switch(selection)
                {
                    case("1"):
                        SelectOrDeleteCardController(null);
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
                    ModifyStudySessionQuestions();
                    break;
                case("0"):
                    RunStudySessionController = false;
                    break;
            }
        }
        while(RunStudySessionController);
    }

    public void StudySessionsDataController()
    {
        string? errorMessage = null;
        string selection;

        do
        {
            UI.StudySessionData(errorMessage);
            selection = Console.ReadLine() ?? "";
            errorMessage = InputValidation.ValidateSelection(selection, 0, 3);
            switch(selection)
            {
                case("1"):
                    StudySessionGetData();
                    break;
                case("2"):
                    StudySessionGetReport();
                    break;
                case("0"):
                    RunStudySessionDataController = false;
                    break;
            }
        }
        while(RunStudySessionDataController);
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

    public void NewOrModifyCardController(Cards? oldCard = null, string? action = null)
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
            UI.DisplayCards(currentCardsToUI,SelectedStack?.StackName, errorMessage, selectionString);
            cardID = Console.ReadLine() ?? "";
            if(selectionString != null)
            {
                errorMessage = InputValidation.ValidateSelection(cardID,currentCardsToUI[0].CardID, 
                currentCardsToUI[^1].CardID);
                errorMessage ??= InputValidation.ValidateCardSelection(currentCardsToUI, cardID);
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
        List<CardsDTO> studySessionCardsToUI = CardsToCardsDTO(studySessionCards);
        string answer;
        bool answerIsCorrect;
        int score = 0;

        foreach (CardsDTO studySessionCardDTO in studySessionCardsToUI)
        {
            UI.StudySessionQuestion(studySessionCardDTO);
            answer = Console.ReadLine() ?? "";
            answerIsCorrect = InputValidation.ValidateStudySessionAnswer(studySessionCardDTO, answer);
            UI.StudySessionAnswer(studySessionCardDTO, answerIsCorrect);
            Console.ReadLine();
            if (answerIsCorrect)
            {
                score++;
            }
        }
        StudySession currentStudySession = new(SelectedStack?.StackName ?? "", DateTime.Today, 
            score/(double)studySessionCards.Count);
        DBController.InsertNewStudySession(currentStudySession);
        UI.StudySessionFinished(currentStudySession.Score);
    }

    public void ModifyStudySessionQuestions()
    {
        string questionsQuantity;
        string? errorMessage = null;
        do
        {
            UI.ModifyStudySessionQuestions(errorMessage);
            questionsQuantity = Console.ReadLine() ?? "";
            errorMessage = InputValidation.ValidateSelection(questionsQuantity,1,100);
        }
        while(errorMessage != null);
        int questionsQuantityInt = Convert.ToInt32(questionsQuantity);
        StudySessionQuestions = questionsQuantityInt;
    }

    public static void StudySessionGetData()
    {
        List<StudySession> studySessions = DBController.SelectStudySessions();
        List<StudySessionDTO> studySessionsToUI = StudySessionToStudySessionsDTO(studySessions);
        UI.DisplayStudySessions(studySessionsToUI);
        Console.ReadLine();
    }

    public static void StudySessionGetReport()
    {
        List<List<object>> reportDate = DBController.GetStudySessionsReports();
        UI.DisplayStudySessionsReport(reportDate);
        Console.ReadLine();
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

    public static List<StudySessionDTO> StudySessionToStudySessionsDTO(List<StudySession> studySessions)
    {
        List<StudySessionDTO> studySessionsToUI = [];

        for(int i =0; i<studySessions.Count; i++)
        {
            studySessionsToUI.Add(new StudySessionDTO(studySessions[i],i+1));
        }
        return studySessionsToUI;        
    }
}