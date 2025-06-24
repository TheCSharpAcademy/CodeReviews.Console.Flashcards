using Flashcards.GoldRino456.Database;
using Flashcards.GoldRino456.Database.Models;
using Flashcards.GoldRino456.UI;

public class StudyAid()
{
    private static bool _isAppRunning = true;

    private static void Main()
    {
        DatabaseManager.Instance.InitializeDatabase();

        while (_isAppRunning)
        {
            DisplayEngine.ClearScreen();
            ProcessMainMenu();
        }
    }

    private static void ProcessMainMenu()
    {
        var menuSelection = DisplayEngine.DisplayMainMenu();

        switch (menuSelection)
        {
            case MenuOptions.StartStudy:
                ProcessStudySession();
                break;

            case MenuOptions.EditMaterials:
                ProcessEditSelected();
                break;

            case MenuOptions.ViewMaterials:
                ProcessViewMaterials();
                break;

            case MenuOptions.ViewStudySession:
                ProcessViewStudySessions();
                break;

            case MenuOptions.Quit:
                _isAppRunning = false;
                break;
        }
    }

    #region Study Session Functions

    private static void ProcessStudySession()
    {
        Stack? selectedStack;
        if (HandleStackSelection("Which stack would you like to study?", out selectedStack))
        {
            var cardsList = DatabaseManager.Instance.FlashcardCtrl.ReadAllEntriesFromStack(selectedStack.StackId);
            if (CheckIfAnyElementsExist<Flashcard>(cardsList))
            {
                cardsList = cardsList.OrderBy(n => Random.Shared.Next()).ToList();
                Queue<Flashcard> cardQueue = new Queue<Flashcard>(cardsList);
                StartStudySession(cardQueue, selectedStack.StackId);
            }
            else
            {
                DisplayEngine.DisplayErrorToUser("Stack has no flashcards in it!");
                DisplayEngine.PressAnyKeyToContinue();
            }
        }
        else
        {
            DisplayEngine.DisplayErrorToUser("No stacks exist to study!");
            DisplayEngine.PressAnyKeyToContinue();
        }
    }

    private static void ProcessViewStudySessions()
    {
        var stackList = DatabaseManager.Instance.StackCtrl.ReadAllEntries();
        if(!CheckIfAnyElementsExist<Stack>(stackList))
        {
            DisplayEngine.DisplayErrorToUser("No card stacks currently exist!");
            DisplayEngine.PressAnyKeyToContinue();
            return;
        }

        var sessionsList = DatabaseManager.Instance.StudySessionCtrl.ReadAllEntries();
        if (!CheckIfAnyElementsExist<StudySession>(sessionsList))
        {
            DisplayEngine.DisplayErrorToUser("No study session records to retrieve!");
            DisplayEngine.PressAnyKeyToContinue();
            return;
        }

        Dictionary<int, string> stackDisplayNames = new();
        foreach (var stack in stackList)
        {
            stackDisplayNames[stack.StackId] = stack.StackName;
        }

        DisplayEngine.DisplayStudySessions(sessionsList, stackDisplayNames);
        DisplayEngine.PressAnyKeyToContinue();
    }

    /// <summary>
    /// Handles running and displaying the actual study session the user interacts with. Takes a queue of cards to display as well as the id number of the current stack.
    /// </summary>
    /// <param name="cardQueue"></param>
    /// <param name="stackId"></param>
    private static void StartStudySession(Queue<Flashcard> cardQueue, int stackId)
    {
        int score = 0;
        int maxScore = cardQueue.Count;

        while(cardQueue.Count > 0)
        {
            var currentCard = cardQueue.Dequeue();

            bool isCorrect = DisplayEngine.PromptForFlashcard(currentCard, true); //Add feature to randomize front/back or one specifically
            score += isCorrect ? 1 : 0;

            DisplayEngine.PressAnyKeyToContinue();
            DisplayEngine.ClearScreen();
        }

        StudySession session = new() { StackId = stackId, SessionDate = DateTime.Now.Date, Score = score };
        DatabaseManager.Instance.StudySessionCtrl.CreateEntry(session);
        DisplayEngine.DisplayFinalScore(score, maxScore);
    }

    #endregion

    #region Edit Menu Functions
    private static void ProcessEditSelected()
    {
        var choice = DisplayEngine.DisplayEditMenu();

        switch(choice)
        {
            case EditOptions.CreateStack:
                ProcessCreateStack();
                break;
            case EditOptions.CreateFlashcard:
                ProcessCreateFlashcard();
                break;
            case EditOptions.EditStack:
                ProcessEditStack();
                break;
            case EditOptions.EditFlashcard:
                ProcessEditFlashcard();
                break;
            case EditOptions.DeleteStack:
                ProcessDeleteStack();
                break;
            case EditOptions.DeleteFlashcard:
                ProcessDeleteFlashcard();
                break;
            case EditOptions.Quit:
                break;
        }
    }

    private static void ProcessCreateStack()
    {
        var stackNames = DatabaseManager.Instance.StackCtrl.ReadAllEntryNames();

        Stack newStack = new();
        DisplayEngine.PromptUserForStackInfo(newStack, stackNames);
        DatabaseManager.Instance.StackCtrl.CreateEntry(newStack);
    }

    private static void ProcessCreateFlashcard()
    {
        var stackList = DatabaseManager.Instance.StackCtrl.ReadAllEntries();

        if (!CheckIfAnyElementsExist(stackList))
        {
            DisplayEngine.DisplayErrorToUser("No card stacks exist! Please create a stack before making a flashcard!");
            return;
        }

        Flashcard newFlashcard = new();
        DisplayEngine.PromptUserForFlashcardInfo(newFlashcard, stackList);
        DatabaseManager.Instance.FlashcardCtrl.CreateEntry(newFlashcard);
    }

    private static void ProcessEditStack()
    {
        Stack? selectedStack;
        if (HandleStackSelection("Which stack would you like to edit?", out selectedStack))
        {
            var stackNames = DatabaseManager.Instance.StackCtrl.ReadAllEntryNames();
            stackNames.Remove(selectedStack.StackName);

            Stack updatedStack = new();
            DisplayEngine.PromptUserForStackInfo(updatedStack, stackNames);
            DatabaseManager.Instance.StackCtrl.UpdateEntry(selectedStack.StackId, updatedStack);
        }
    }

    private static void ProcessEditFlashcard()
    {
        //Prompt User For What Stack they want to look at...
        Stack? selectedStack;
        if(HandleStackSelection("Which stack would you like to edit?", out selectedStack))
        {
            //Check and Select a FlashCard
            Flashcard? selectedCard;

            if(HandleFlashcardSelection("Please select a card to edit: ", selectedStack, out selectedCard))
            {
                Flashcard updatedCard = new();
                var stackList = DatabaseManager.Instance.StackCtrl.ReadAllEntries();
                DisplayEngine.PromptUserForFlashcardInfo(updatedCard, stackList);
                DatabaseManager.Instance.FlashcardCtrl.UpdateEntry(selectedCard.CardId, updatedCard);
            }
        }
    }
    #endregion

    #region Delete Functions
    private static void ProcessDeleteStack()
    {
        Stack? selectedStack;
        if (HandleStackSelection("Which stack would you like to delete? (WARNING: This cannot be undone and will delete all associated Flashcards and Study Sessions.)", out selectedStack))
        {
            var cardsList = DatabaseManager.Instance.FlashcardCtrl.ReadAllEntriesFromStack(selectedStack.StackId);
            if (CheckIfAnyElementsExist<Flashcard>(cardsList))
            {
                foreach (var card in cardsList)
                {
                    DatabaseManager.Instance.FlashcardCtrl.DeleteEntry(card.CardId);
                }
            }

            var studySessionList = DatabaseManager.Instance.StudySessionCtrl.ReadAllEntriesFromStack(selectedStack.StackId);
            if(CheckIfAnyElementsExist<StudySession>(studySessionList))
            {
                foreach (var studySession in studySessionList)
                {
                    DatabaseManager.Instance.StudySessionCtrl.DeleteEntry(studySession.SessionId);
                }
            }

            DatabaseManager.Instance.StackCtrl.DeleteEntry(selectedStack.StackId);
        }
    }

    private static void ProcessDeleteFlashcard()
    {
        //Prompt User For What Stack they want to look at...
        Stack? selectedStack;
        if (HandleStackSelection("Which stack would you like to delete a card from?", out selectedStack))
        {
            //Check and Select a FlashCard
            Flashcard? selectedCard;

            if (HandleFlashcardSelection("Please select a card to delete: ", selectedStack, out selectedCard))
            {
                DatabaseManager.Instance.FlashcardCtrl.DeleteEntry(selectedCard.CardId);
            }
        }
    }
    #endregion

    private static void ProcessViewMaterials()
    {
        //Prompt User For What Stack they want to view
        var stackList = DatabaseManager.Instance.StackCtrl.ReadAllEntries();
        if (!CheckIfAnyElementsExist(stackList))
        {
            DisplayEngine.DisplayErrorToUser("No stack exists to view!");
            DisplayEngine.PressAnyKeyToContinue();
            return;
        }
        var stackListIndex = DisplayEngine.PromptUserForStackSelection("Which stack of cards would you like to view?", stackList);

        //Check that the stack actually DOES have cards in it.
        var cardList = DatabaseManager.Instance.FlashcardCtrl.ReadAllEntriesFromStack(stackList[stackListIndex].StackId);
        if (!CheckIfAnyElementsExist(cardList))
        {
            DisplayEngine.DisplayErrorToUser("No cards found in stack!");
            DisplayEngine.PressAnyKeyToContinue();
            return;
        }

        DisplayEngine.DisplayFlashcards(cardList);
        DisplayEngine.PressAnyKeyToContinue();
    }

    /// <summary>
    /// Prompts the user to select a stack from all available.
    /// </summary>
    /// <param name="prompt">Customizable message displayed to the user.</param>
    /// <param name="stack">Output stack that the user selected.</param>
    /// <returns></returns>
    private static bool HandleStackSelection(string prompt, out Stack? stack)
    {
        var stacksList = DatabaseManager.Instance.StackCtrl.ReadAllEntries();
        var chosenStack = -1;

        if (!CheckIfAnyElementsExist(stacksList))
        {
            DisplayEngine.DisplayErrorToUser("No stacks exist!");
            DisplayEngine.PressAnyKeyToContinue();

            stack = null;
            return false;
        }

        chosenStack = DisplayEngine.PromptUserForStackSelection(prompt, stacksList);
        stack = stacksList[chosenStack];
        return true;
    }

    /// <summary>
    /// Prompts the user to select a flashcard from all available in a given stack of cards.
    /// </summary>
    /// <param name="prompt">Customizable prompt displayed to the user.</param>
    /// <param name="stack">The stack of cards that the user will choose from.</param>
    /// <param name="card">Output flashcard data that the user selected.</param>
    /// <returns></returns>
    private static bool HandleFlashcardSelection(string prompt, Stack stack, out Flashcard? card)
    {
        //Check that the stack actually DOES have cards in it.
        var cardsList = DatabaseManager.Instance.FlashcardCtrl.ReadAllEntriesFromStack(stack.StackId);
        var chosenCard = -1;

        if (!CheckIfAnyElementsExist(cardsList))
        {
            DisplayEngine.DisplayErrorToUser("No cards found in stack!");
            DisplayEngine.PressAnyKeyToContinue();

            card = null;
            return false;
        }

        //Prompt user for which card and the changes they want to make to it.
        chosenCard = DisplayEngine.PromptUserForFlashcardSelection(prompt, cardsList);
        card = cardsList[chosenCard];
        return true;
    }

    /// <summary>
    /// Takes a generic list of any type and determines if there are any elements within that list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns>True if list has any elements, false otherwise.</returns>
    private static bool CheckIfAnyElementsExist<T>(List<T> list)
    {
        return list != null && list.Count > 0;
    }
}