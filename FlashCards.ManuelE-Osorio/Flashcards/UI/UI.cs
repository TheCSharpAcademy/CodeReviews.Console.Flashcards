namespace Flashcards;

class UI
{
    public static void WelcomeMessage(string? errorMessage)
    {
        Helpers.ClearConsole();
        if(errorMessage != null)
        {
            Console.WriteLine("Error: " + errorMessage + "\n");
        }
        else
        {
        Console.WriteLine("Welcome to the Flash Cards App!\n");
        }
        Thread.Sleep(2000);
    }
    public static void MainMenu(string? stackName, string? errorMessage)
    {
        Helpers.ClearConsole();
        if(stackName == null)
        {
            Console.WriteLine("You haven't selected a stack\n");
        }
        else
        {
            Console.WriteLine($"Currently you have selected the stack \"{stackName}\"\n");
        }
        
        if (errorMessage != null)
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }

        Console.WriteLine(
            "1) Manage stacks\n"+
            "2) Manage Flashcards\n"+
            "3) Start a study session\n"+
            "4) View study sessions data\n"+
            "0) Exit the application\n");
    }

    public static void Stacks(string? errorMessage)
    {
        Helpers.ClearConsole();
        if (errorMessage != null)
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }
        
        Console.WriteLine(
            "1) Create a new stack\n"+
            "2) Select a stack\n"+
            "3) Modify a stack\n"+
            "4) Delete a stack\n"+
            "0) Return to the main menu\n");
    }

    public static void FlashCards(string? stackName, string? errorMessage)
    {
        Helpers.ClearConsole();
        Console.WriteLine($"Currently you have selected the stack \"{stackName}\"\n");
        if (errorMessage != null)
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }

        Console.WriteLine(
            "1) View the cards in the current stack\n"+
            "2) Create a new card in the current stack\n"+
            "3) Edit a card in the current stack\n"+
            "4) Delete a card in the current stack\n"+
            "0) Return to the main menu\n"  
        );

    }

    public static void StudySession(string? errorMessage)
    {
        Helpers.ClearConsole();
        if (errorMessage != null)
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }

        Console.WriteLine(
            "1) Start study session\n"+
            "2) Set quantity of questions\n"+
            "0) Return to the main menu\n"
        );

    }

    public static void StudySessionData(string? errorMessage)
    {
        Helpers.ClearConsole();
        if(errorMessage != null)
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }

        Console.WriteLine(
            "1) View all the study sessions records\n"+
            "2) View total and average score of study sessions by month and stack\n"+
            "0) Return to the main menu\n"
        );
    }

    public static void ExitMessage()
    {
        Helpers.ClearConsole();
        Console.WriteLine("Thank you for using the Flash Cards App!\n");
        Thread.Sleep(2000);
    }

    public static void NewStackName(string? errorMessage, string? action)
    {
        Helpers.ClearConsole();
        if(errorMessage != null) 
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }

        Console.WriteLine($"Please write the {action} name of the stack. Valid characters are a to z, A to Z and 0 to 9.\n"+
        "The maximum length of the name is 50 characters.\n");
    }

    public static void SelectOrDeleteStack(List<StacksDto> currentStacksUI, string? errorMessage, string action)
    {
        Helpers.ClearConsole();
        TableUI.PrintStacksTable(currentStacksUI);
        if(errorMessage != null) 
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }

        Console.WriteLine($"Please write the name of the stack that you want to {action}:\n");
    }

    public static void DisplayCards(List<CardsDto> currentCardsUI, string? stackName, string? errorMessage, string? action)
    {
        Helpers.ClearConsole();
        TableUI.PrintCardsTable(currentCardsUI, stackName);
        if(errorMessage != null)
        {
            Console.WriteLine("Error: " + errorMessage+"\n");    
        }

        if(action != null)
        {
            Console.WriteLine($"Please write the ID of the card that you want to {action}:\n");
        }
        else
        {
            Console.WriteLine("Press any key to return");
        }
    }

    public static void NewCard(string? errorMessage, string? type, string? action)
    {
        Helpers.ClearConsole();
        if(errorMessage != null) 
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }

        Console.WriteLine($"Please write the {action} {type} of the card. The maximum length of the {type} is 300 characters.\n");
    }

    public static void StudySessionQuestion(CardsDto card)
    {
        Helpers.ClearConsole();
        Console.WriteLine("What is the answer to the following card:\n");
        TableUI.PrintCardQuestion(card);
        Console.WriteLine("\n");
    }

    public static void StudySessionAnswer(CardsDto card, bool answerIsCorrect)
    {
        Helpers.ClearConsole();
        Console.WriteLine("What is the answer to the following card:\n");
        TableUI.PrintCardAnswer(card);
        Console.WriteLine("\n");
    
        if(answerIsCorrect)
        {
            Console.WriteLine("Correct answer. Press any key to continue");
        }
        else
        {
            Console.WriteLine("Wrong answer. Press any key to continue");
        }
    }

    public static void StudySessionFinished(double score)
    {
        Helpers.ClearConsole();
        Console.WriteLine($"The study session has finished. Your score is {(score*100).ToString("0.##")}\n");
        Thread.Sleep(3000);
    }

    public static void ModifyStudySessionQuestions(string? errorMessage)
    {
        Helpers.ClearConsole();
        if(errorMessage != null) 
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }
        Console.WriteLine("Please write the quantity of questions for the study session. A maximum of 100 questions is allowed.");
    }

    public static void DisplayStudySessions(List<StudySessionDto> currentStudySessionsToUI)
    {
        Helpers.ClearConsole();
        TableUI.PrintStudySessionsTable(currentStudySessionsToUI);
        Console.WriteLine("Press any key to return");
    }

    public static void DisplayStudySessionsReport(List<List<object>> reportData)
    {
        Helpers.ClearConsole();
        TableUI.PrintTable(reportData, TableUI.ReportTitle, TableUI.ReportHeader);
        Console.WriteLine("Press any key to return");
    }
}