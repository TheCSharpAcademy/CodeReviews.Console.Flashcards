namespace Flashcards;

public class Controller
{
    public static void Run()
    {
        bool appRunning = true;

        while (appRunning)
        {
            Console.Clear();
            Views.ShowHeader();

            string? input = Views.MainMenu();
            bool validInput = Validation.ValidateMenuSelection(input);

            if (validInput)
            {
                switch (input)
                {
                    case "Exit":
                        appRunning = false;
                        Environment.Exit(0);
                        break;
                    case "Manage Stacks":
                        StackHandler();
                        break;
                    case "Manage Flashcards":
                        FlashcardHandler();
                        break;
                    case "Start Study Session":
                        SessionController.StudySessionHandler();
                        break;
                    case "View Session Data":
                        SessionController.ViewSessionData();
                        break;
                    default:
                        Views.ShowErrorMessage("Please select a valid menu option.");
                        break;
                }
            }
            else
            {
                Views.ShowErrorMessage("Input was null or empty. Closing application.");
                Environment.Exit(0);
            }
        }
    }

    // Stacks methods
    internal static void StackHandler()
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            Console.Clear();

            string? input = StacksView.StacksMenu();
            bool validInput = Validation.ValidateMenuSelection(input);

            if (validInput)
            {
                var stacksList = FlashcardsModel.FetchStacks();

                switch (input)
                {
                    case "View Stacks":
                        ViewStacks(stacksList);
                        break;
                    case "Create Stack":
                        CreateNewStack();
                        break;
                    case "Delete Stack":
                        DeleteStack();
                        break;
                    default:
                        exitMenu = true;
                        break;
                }
            }
            else
            {
                Views.ShowErrorMessage("Input was null or empty. Returning to main menu.");
                exitMenu = true;
            }
        }
    }

    internal static void ViewStacks(List<Stack> stackList)
    {
        if (stackList.Count == 0)
        {
            Views.ShowMessage("Stack list is empty.");
        }
        else
        {
            DisplayConvertedStacks(stackList);
        }

        Views.ShowMessage("Press any key to continue.\n");
        Console.ReadLine();
    }

    internal static int CountStackTable()
    {
        int count;

        var stackList = FlashcardsModel.FetchStacks();
        count = stackList.Count;

        return count;
    }

    internal static string? CreateNewStack()
    {
        string? input = null;
        bool exitMenu = false;

        while (!exitMenu)
        {
            Console.Clear();

            List<Stack> stacks = FlashcardsModel.FetchStacks();
            DisplayConvertedStacks(stacks);

            Views.ShowMessage("Create new stack?\n");
            input = Views.SelectYesOrNo().ToLower();

            if (input == "yes")
            {
                string? stackName = GetStackName(stacks);

                if (stackName != null)
                {
                    FlashcardsModel.InsertStack(stackName);
                }
            }
            else
            {
                exitMenu = true;
            }
        }

        return input;
    }

    internal static bool SearchStacksForName(List<Stack> stackList, string? stackName)
    {
        bool stackFound = false;

        foreach (var stack in stackList)
        {
            if (stackName?.ToLower() == stack.StackName?.ToLower())
            {
                stackFound = true;
                break;
            }
        }

        return stackFound;
    }

    internal static string? GetStackName(List<Stack> stackList)
    {
        string? stackName = null;
        bool stackExist = true;

        while (stackExist)
        {
            stackName = Input.GetString("Enter stack name. Press 0 to cancel.");

            if (stackName == "0")
            {
                return null;
            }
            else if (stackList.Count == 0)
            {
                stackExist = false;
            }
            else
            {
                stackExist = SearchStacksForName(stackList, stackName);

                if (stackExist)
                {
                    Views.ShowErrorMessage($"There is already a stack with the name {stackName}.");
                }
            }
        }

        return stackName;
    }

    internal static void DeleteStack()
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            Console.Clear();

            List<Stack> stacks = FlashcardsModel.FetchStacks();
            int count = stacks.Count;

            if (count != 0 && stacks != null)
            {
                DisplayConvertedStacks(stacks);

                string? stackName;
                stackName = StacksView.SelectStackMenu(stacks, count);

                if (stackName == "Exit")
                {
                    exitMenu = true;
                }
                else
                {
                    ConfirmDelete(stacks, stackName);
                }
            }
            else
            {
                Views.ShowMessage("Stack list is empty.");
                Views.ShowMessage("Press any key to continue.");
                Console.ReadLine();

                exitMenu = true;
            }

        }
    }

    internal static void ConfirmDelete(List<Stack> stackList, string? stackName)
    {
        bool stackFound = SearchStacksForName(stackList, stackName);

        if (stackFound)
        {
            Views.ShowMessage($"Are you sure you want to delete '{stackName}' stack?\n");

            string? input = Views.SelectYesOrNo().ToLower();

            if (input == "yes")
            {
                FlashcardsModel.DeleteStack(stackName);
            }
        }
    }


    // Flashcard methods
    internal static void FlashcardHandler()
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            Console.Clear();

            int count;
            count = CountStackTable();

            if (count == 0)
            {
                Views.ShowMessage("Currently no stacks to work with.");
                Views.ShowMessage("Press any key to continue.");
                Console.ReadLine();
                exitMenu = true;
                continue;
            }
            else
            {
                bool changeStack;
                int stackId;
                string? stackName;

                var stackList = FlashcardsModel.FetchStacks();
                DisplayConvertedStacks(stackList);

                stackName = StacksView.SelectStackMenu(stackList, count);

                if (stackName == "Exit")
                {
                    exitMenu = true;
                }
                else
                {
                    foreach (var stack in stackList)
                    {
                        if (stackName?.ToLower() == stack.StackName?.ToLower())
                        {
                            stackId = stack.StackId;
                            stackName = stack.StackName;
                            changeStack = RunWorkingStackMenu(stackId, stackName);

                            if (!changeStack)
                            {
                                exitMenu = true;
                            }
                            break;
                        }
                        else
                        {
                            Views.ShowErrorMessage($"Something went wrong. {stackName} wasn't found. Returning to main.");
                        }
                    }
                }
            }
        }
    }

    internal static bool RunWorkingStackMenu(int stackId, string? stackName)
    {
        bool exitMenu = false;
        bool changeStack = false;

        while (!exitMenu)
        {
            Console.Clear();
            Views.ShowWorkingStack(stackName);

            string? input = FlashcardViews.FlashcardMenu();
            bool validInput = Validation.ValidateMenuSelection(input);

            if (validInput)
            {
                switch (input)
                {
                    case "View All Flashcards in Stack":
                        ViewCardsInStack(stackId, stackName);
                        Views.ShowMessage("Press enter to return to menu.");
                        Console.ReadLine();
                        break;
                    case "View X Amount of Flashcards in Stack":
                        ViewXAmount(stackId, stackName);
                        break;
                    case "Create Flashcard in Stack":
                        CreateFlashcard(stackId, stackName);
                        break;
                    case "Update Flashcard in Stack":
                        UpdateFlashcard(stackId, stackName);
                        break;
                    case "Delete Flashcard in Stack":
                        DeleteCard(stackId, stackName);
                        break;
                    case "Change Working Stack":
                        exitMenu = true;
                        changeStack = true;
                        break;
                    default:
                        exitMenu = true;
                        break;
                }
            }
            else
            {
                Views.ShowErrorMessage("Input was null or empty. Returning to main menu.");
                exitMenu = true;
            }
        }

        return changeStack;
    }

    internal static List<Flashcard> ViewCardsInStack(int stackId, string? stackName)
    {
        List<Flashcard> list = FlashcardsModel.FetchCardsInStack(stackId);
        DisplayConvertedFlashcards(list, stackName);

        return list;
    }

    internal static void ViewXAmount(int stackId, string? stackName)
    {
        int amount;
        bool validInput = false;

        Views.ShowMessage("Input amount of flashcards you'd like to see. Press 0 to cancel.\n");

        while (!validInput)
        {
            amount = Convert.ToInt32(Input.GetNumber());

            if (amount < 0)
            {
                Views.ShowMessage("Please input a number above 0.\n");
            }
            else if (amount == 0)
            {
                validInput = true;
            }
            else
            {
                List<Flashcard> list = FlashcardsModel.FetchXAmountCard(stackId, amount);
                DisplayConvertedFlashcards(list, stackName);
                validInput = true;

                Views.ShowMessage("Press enter to return to menu.\n");
                Console.ReadLine();
            }
        }
    }

    internal static void CreateFlashcard(int stackId, string? stackName)
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            bool exitSubMenu = false;
            Console.Clear();

            // Displays cards in stack
            Views.ShowWorkingStack(stackName);            
            ViewCardsInStack(stackId, stackName);

            Views.ShowMessage("Create new flashcard?\n");

            while (!exitSubMenu)
            {
                string? userInput = Views.SelectYesOrNo().ToLower();

                if (userInput == "yes")
                {
                    // Counts stack
                    int count = FlashcardsModel.GetCardQuantity(stackId);

                    // Fills out card and inserts it
                    string? cardFront = Input.GetString("Fill out front of card.");
                    string? cardBack = Input.GetString("Fill out back of card.");
                    FlashcardsModel.InsertCard(cardFront, cardBack, stackId);

                    // Recounts stack
                    int newCount = FlashcardsModel.GetCardQuantity(stackId);

                    // If count shows card was added, updates card quantity
                    if (newCount == count + 1)
                    {
                        FlashcardsModel.UpdateCardQuantity(newCount, stackId);
                        Views.ShowMessage("New card added.");
                    }
                    else
                    {
                        Views.ShowMessage("Something went wrong. Card wasn't added.");
                    }

                    Views.ShowMessage("Press any key to continue.\n");
                    Console.ReadLine();

                    exitSubMenu = true;
                }
                else
                {
                    exitSubMenu = true;
                    exitMenu = true;
                }
            }
        }
    }

    internal static void UpdateFlashcard(int stackId, string? stackName)
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            bool validInput = false;

            Console.Clear();

            // Displays stack
            Views.ShowWorkingStack(stackName);
            var cardsList = ViewCardsInStack(stackId, stackName);

            Views.ShowMessage($"Select a flashcard number to update. Press 0 to return to menu.\n");

            while (!validInput)
            {
                // Gets card number from user
                string? userInput = Input.GetNumber();

                if (userInput == "0")
                {
                    validInput = true;
                    exitMenu = true;
                }
                else
                {
                    // Looks for card in stack. Card not found if it returns -1
                    int cardId = SearchForCard(cardsList, userInput);

                    if (cardId == -1)
                    {
                        Views.ShowErrorMessage("Flashcard not found. Please enter an existing card number.\n");
                    }
                    else
                    {
                        // Updates card
                        UpdateCardDetails(cardId);

                        validInput = true;
                    }
                }
            }
        }
    }

    internal static void UpdateCardDetails(int cardId)
    {
        string cardSide = FlashcardViews.SelectColumnMenu();
        string? newValue = Input.GetString("Input updated text.");

        FlashcardsModel.UpdateCard(cardId, cardSide, newValue);
    }

    internal static int SearchForCard(List<Flashcard> cardList, string? userInput)
    {
        int parsedInput = Convert.ToInt32(userInput);
        int cardId = -1;

        foreach (var card in cardList)
        {
            if (parsedInput == card.RowNumber)
            {
                cardId = card.CardId;
                break;
            }
        }

        return cardId;
    }

    internal static void DeleteCard(int stackId, string? stackName)
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            bool exitSubMenu = false;
            Console.Clear();

            // Displays cards in stack
            Views.ShowWorkingStack(stackName);
            var cardsList = ViewCardsInStack(stackId, stackName);

            Views.ShowMessage("Select the flashcard number you'd like to delete. Press 0 to cancel.\n");

            while (!exitSubMenu)
            {
                // Gets card number from user
                string? userInput = Input.GetNumber();

                if (userInput == "0")
                {
                    exitSubMenu = true;
                    exitMenu = true;
                }
                else
                {
                    // Looks for card in stack. Card not found if it returns -1
                    int cardId = SearchForCard(cardsList, userInput);

                    if (cardId == -1)
                    {
                        Views.ShowErrorMessage("Flashcard not found. Please enter an existing card number.\n");
                    }
                    else
                    {
                        Views.ShowMessage("Are you sure you want to delete flashcard?\n");

                        string? input = Views.SelectYesOrNo().ToLower();

                        /* Counts the stack, deletes the card and recounts it.
                           If deleted, updates card quantity.*/ 
                        if (input == "yes")
                        {                           
                            int count = FlashcardsModel.GetCardQuantity(stackId);
                            
                            FlashcardsModel.DeleteCard(cardId);

                            int newCount = FlashcardsModel.GetCardQuantity(stackId);

                            if (newCount == count - 1)
                            {
                                Views.ShowMessage("Card was deleted.");
                                FlashcardsModel.UpdateCardQuantity(newCount, stackId);
                            }
                            else
                            {
                                Views.ShowErrorMessage("Something went wrong. Card was not deleted.");
                            }

                            Views.ShowMessage("Press any key to continue.\n");
                            Console.ReadLine();
                        }

                        exitSubMenu = true;
                    }
                }
            }
        }
    }

    // Card DTO methods
    internal static List<FlashcardDTO> RunFlashcardMapper(List<Flashcard> cardsList)
    {
        var dtoList = new List<FlashcardDTO>();

        foreach (Flashcard card in cardsList)
        {
            FlashcardDTO cardDTO = ConvertToDTO(card);
            dtoList.Add(cardDTO);
        }

        return dtoList;
    }

    internal static FlashcardDTO ConvertToDTO(Flashcard card)
    {
        return new FlashcardDTO
        {
            CardFront = card.CardFront,
            CardBack = card.CardBack,
            RowNumber = card.RowNumber
        };
    }

    internal static void DisplayConvertedFlashcards(List<Flashcard> cardList, string? stackName)
    {
        if (cardList != null && cardList.Count != 0)
        {
            List<FlashcardDTO> dtoList = RunFlashcardMapper(cardList);
            VisualizationEngine.DisplayFlashcards(dtoList, stackName);
        }
        else
        {
            Views.EmptyTable(stackName);
        }
    }

    //Stack DTO methods
    internal static List<StackDTO> RunStackMapper(List<Stack> stackList)
    {
        var dtoList = new List<StackDTO>();

        foreach (Stack stack in stackList)
        {
            StackDTO stackDTO = ConvertToDTO(stack);
            dtoList.Add(stackDTO);
        }

        return dtoList;
    }

    internal static StackDTO ConvertToDTO(Stack stack)
    {
        return new StackDTO
        {
            StackName = stack.StackName,
            CardQuantity = stack.CardQuantity,
            RowNumber = stack.RowNumber
        };
    }

    internal static void DisplayConvertedStacks(List<Stack> stackList)
    {
        if (stackList != null && stackList.Count != 0)
        {
            List<StackDTO> dtoList = RunStackMapper(stackList);
            VisualizationEngine.DisplayStacks(dtoList);
        }
        else
        {
            Views.ShowMessage("Stack list is empty.\n");
        }
    }
}