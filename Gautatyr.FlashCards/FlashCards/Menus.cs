using static FlashCards.Helpers;
using static FlashCards.DataValidation;
using static FlashCards.DataAccess;
using ConsoleTableExt;
using FlashCards.Models;

namespace FlashCards;

public static class Menus
{
    public static void MainMenu(string message = "")
    {
        Console.Clear();

        if (message != "") Console.WriteLine($"\n{message}");

        Console.WriteLine("\nMAIN MENU\n");
        Console.WriteLine("- Type 1 to access the Stacks menu");
        Console.WriteLine("- Type 2 to access the Study Sessions menu");
        Console.WriteLine("- Type 0 to close the application");

        switch (GetNumberInput())
        {
            case 0:
                Environment.Exit(0);
                break;
            case 1:
                StacksMenu();
                break;
            case 2:
                StudySessionsMenu();
                break;
            default:
                string error = "Wrong input ! Please type a number between 0 and 2";
                MainMenu(DisplayError(error));
                break;
        }
    }

    private static void StacksMenu(string message = "")
    {
        Console.Clear();

        if (message != "") Console.WriteLine($"\n{message}");

        Console.WriteLine("\nSTACKS\n");
        Console.WriteLine("- Type 1 Create a new stack");
        Console.WriteLine("- Type 2 to View your stacks");
        Console.WriteLine("- Type 3 to Update stacks");
        Console.WriteLine("- Type 4 to Delete stacks");
        Console.WriteLine("- Type 0 Return to the main menu");

        switch (GetNumberInput())
        {
            case 0:
                MainMenu();
                break;
            case 1:
                CreateStack();
                break;
            case 2:
                int stackId = GetStackIdInput("\nType the ID of the stack you want to inspect, or type 0 to return to the stack menu.\n");

                if (stackId == 0) StacksMenu();

                while (stackId != 0)
                {
                    InspectStack(stackId);

                    Console.WriteLine("\nPress Enter to go back.\n");
                    Console.ReadLine();

                    DisplayStacks();

                    stackId = GetStackIdInput("\nType the ID of the stack you want to inspect, or type 0 to return to the stack menu.\n");
                }

                StacksMenu();
                break;
            case 3:
                stackId = GetStackIdInput("\nType the ID of the stack you want to Update, or type 0 to return to the stack menu.\n");

                while (stackId != 0)
                {
                    UpdateStack(stackId);
                    stackId = GetStackIdInput("\nType the ID of the stack you want to Update, or type 0 to return to the stack menu.\n");
                }

                StacksMenu();
                break;
            case 4:
                DisplayStacks();

                stackId = GetStackIdInput("\nType the ID of the stack you want to Delete, or type 0 to return to the stack menu.");

                while (stackId != 0)
                {
                    DeleteStack(stackId);
                    DisplayStacks();
                    stackId = GetNumberInput("\nType the ID of the stack you want to Delete, or type 0 to return to the stack menu.");
                }

                StacksMenu();
                break;
            default:
                string error = "Wrong input ! Please type a number between 0 and 3";
                StacksMenu(DisplayError(error));
                break;
        }
    }

    private static void UpdateStack(int stackId)
    {
        Console.Clear();
        InspectStack(stackId);

        Console.WriteLine("\nMODIFY STACK\n");
        Console.WriteLine("- Type 1 to Modify the stack's theme");
        Console.WriteLine("- Type 2 to Add a card");
        Console.WriteLine("- Type 3 to Modify a card");
        Console.WriteLine("- Type 4 to Delete a card");
        Console.WriteLine("- Type 0 to go back to the stack menu");

        switch (GetNumberInput())
        {
            case 0:
                StacksMenu();
                break;
            case 1:
                InspectStack(stackId);

                string stackNewTheme = GetTextInput($"\nType the stack's new theme");

                UpdateStackTheme(stackId, stackNewTheme);

                UpdateStack(stackId);
                break;
            case 2:
                InspectStack(stackId);

                string stackTheme = GetStackTheme(stackId);

                AddCard(stackTheme);

                InspectStack(stackId);

                string answer = GetTextInput("\nType 1 to add a new card, or type 0 to go back.\n");

                while (answer == "1")
                {
                    InspectStack(stackId);
                    AddCard(stackTheme);
                    InspectStack(stackId);
                    answer = GetTextInput("\nType 1 to add a new card, or type 0 to go back.\n");
                }

                UpdateStack(stackId);
                break;
            case 3:
                InspectStack(stackId);

                int cardId = GetCardIdInput(stackId, "\nType in the card's id you wish to Update, or type 0 to go back.\n");

                while (cardId != 0)
                {
                    UpdateCard(stackId, cardId);
                    InspectStack(stackId);
                    cardId = GetCardIdInput(stackId, "\nType in the card's id you wish to Update, or type 0 to go back.\n");
                }

                UpdateStack(stackId);
                break;
            case 4:
                InspectStack(stackId);

                cardId = GetCardIdInput(stackId, "\nType in the card's id you wish to Delete, or type 0 to go back.\n");

                while (cardId != 0)
                {
                    DeleteCard(cardId, stackId);
                    InspectStack(stackId);
                    cardId = GetCardIdInput(stackId, "\nType in the card's id you wish to Delete, or type 0 to go back.\n");
                }

                UpdateStack(stackId);
                break;
            default:
                string error = "Wrong input ! Please type a number between 0 and 2";
                StacksMenu(DisplayError(error));
                break;
        }
    }

    private static void UpdateCard(int stackId, int cardId)
    {
        int answer = GetNumberInput("\nType 1 to update the question, 2 to update the answer or 0 to go back to the menu.\n");

        while (answer != 0)
        {
            Console.Clear();

            DisplayCard(stackId, cardId);

            switch (answer)
            {
                case 1:
                    string question = GetTextInput("\nType the new question\n");
                    UpdateCardQuestion(cardId, stackId, question);
                    break;
                case 2:
                    string questionAnswer = GetTextInput("\nType the new answer\n");
                    UpdateCardAnswer(cardId, stackId, questionAnswer);
                    break;
            }

            DisplayCard(stackId, cardId);

            answer = GetNumberInput("\nType 1 to update the question, 2 to update the answer or 0 to go back to the stack.\n");
        }
    }

    private static void CreateStack()
    {
        Console.WriteLine("\nSTACK CREATION\n");

        string stackTheme = GetTextInput("Type the theme of the new stack, or type 0 to go back to the stack menu.\n");

        while (StackExists(GetStackId(stackTheme)) && stackTheme != "0")
        {
            Console.Clear();

            Console.WriteLine(DisplayError("This theme already exists !"));

            stackTheme = GetTextInput("Type the theme of the new stack, or type 0 to go back to the stack menu.\n");
        }

        if (stackTheme == "0") StacksMenu();

        InsertStack(stackTheme);

        Console.WriteLine("\nTo insert a new card in the stack type 1, else press enter to go back to the stack menu.\n");
        string continueToAddCard = Console.ReadLine();

        int stackId = GetStackId(stackTheme);

        while (continueToAddCard == "1")
        {
            AddCard(stackTheme);
            InspectStack(stackId);
            Console.WriteLine("\nType 1 to add another card, else press enter.\n");
            continueToAddCard = Console.ReadLine();
        }

        StacksMenu();
    }

    private static void AddCard(string stackTheme)
    {
        string question = GetTextInput("\nType the card's question.\n");

        string answer = GetTextInput("\nType the card's answer.\n");

        Console.Clear();

        InsertCard(stackTheme, question, answer);
    }

    public static void DisplayStacks()
    {
        List<Stack> stackFromDatabase = GetStacks();
        
        List<StackCardsWithId> stacksWithIds = new List<StackCardsWithId>();

        int sequenceId = 1;

        foreach (var stack in stackFromDatabase)
        {
            stacksWithIds.Add(new StackCardsWithId
            {
                Theme = stack.Theme,
                Id = sequenceId
            });
            sequenceId++;
        }

        Console.Clear();

        Console.WriteLine("\nSTACKS\n");

        ConsoleTableBuilder.From(stacksWithIds).ExportAndWriteLine();
    }

    public static void DisplayCard(int stackId, int cardId)
    {
        stackId = StackIdToRealId(stackId);
        cardId = CardIdToRealId(cardId, stackId);

        Console.Clear();

        List<CardNoId> card = new List<CardNoId>
        {
            GetCard(stackId, cardId)
        };

        ConsoleTableBuilder.From(card).ExportAndWriteLine();
    }

    public static void InspectStack(int stackId)
    {
        List<Stack> stackFromDatabase = GetStacks();

        int sequenceId = 1;

        string chosenStackTheme = "";

        foreach (var stack in stackFromDatabase)
        {
          if(sequenceId == stackId)
            {
                chosenStackTheme = stack.Theme;
            }
            sequenceId++;
        }

        StackCardsDto stackCards = GetStack(GetStackId(chosenStackTheme));

        Console.Clear();

        Console.WriteLine($"\n{stackCards.Theme.ToUpper()}\n");

        List<CardDto> cards = stackCards.CardsDto;
        List<CardDto> cardsWithId = new List<CardDto>();

        sequenceId = 1;

        foreach (var card in cards)
        {
            cardsWithId.Add(new CardDto
            {
                Id = sequenceId,
                Question = card.Question,
                Answer = card.Answer,
            });
            sequenceId++;
        }

        Console.Clear();

        Console.WriteLine($"\n{chosenStackTheme.ToUpper()}\n");

        ConsoleTableBuilder.From(cardsWithId)
            .ExportAndWriteLine();
    }

    private static void StudySessionsMenu(string message = "")
    {
        Console.Clear();

        if (message != "") Console.WriteLine($"\n{message}");

        Console.WriteLine("\nSTUDY SESSIONS\n");
        Console.WriteLine("- Type 1 to Start a Study Session");
        Console.WriteLine("- Type 2 to View Sessions history");
        Console.WriteLine("- Type 0 Return to the main menu");

        switch (GetNumberInput())
        {
            case 0:
                MainMenu();
                break;
            case 1:
                StartStudySession();
                break;
            case 2:
                ViewStudySessions();

                Console.WriteLine("\nPress enter to go back\n");
                Console.ReadLine();

                StudySessionsMenu();
                break;
            default:
                string error = "Wrong input ! Please type a number between 0 and 3";
                StudySessionsMenu(DisplayError(error));
                break;
        }
    }

    private static void ViewStudySessions()
    {
        Console.Clear();

        List<StudySessionDto> studySessions = GetStudySessions();

        ConsoleTableBuilder.From(studySessions)
           .ExportAndWriteLine();
    }

    private static void StartStudySession()
    {
        DisplayStacks();

        int stackId = GetStackIdInput("\nType the id of the stack you want to study or type 0 to go back\n");

        if (stackId == 0) StudySessionsMenu();

        stackId = StackIdToRealId(stackId);

        int score = Quiz(stackId);

        string formatedScore = $"{score}/{GetNumberOfCards(stackId)}";

        CreateStudySession(stackId, formatedScore);

        Console.WriteLine($"Your score is {formatedScore}, press enter to continue");
        Console.ReadLine();

        StartStudySession();
    }

    private static int Quiz(int stackId)
    {
        int score = 0;

        List<CardDto> cards = GetStack(stackId).CardsDto;

        for (int q = 0; q < GetNumberOfCards(stackId); q++)
        {
            Console.Clear();

            Console.WriteLine($"\n{cards[q].Question}\n");

            if (GetTextInput().ToLower().Trim() == cards[q].Answer.ToLower().Trim())
            {
                score++;
                Console.WriteLine("\nCorrect !\n");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"\nWrong. The correct answer was: {cards[q].Answer}\n");
                Console.ReadLine();
            }
        }

        return score;
    }
}
