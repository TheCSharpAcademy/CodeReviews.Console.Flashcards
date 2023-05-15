using Flashcards.Models;

namespace Flashcards;

internal class Program
{
    private static readonly DataAccessor dataAccessor = new();
    private static bool exitApp = false;
    public static void Main(string[] args)
    {
        Viewer.DisplayTitle();
        while (!exitApp)
        {
            Viewer.DisplayOptionsMenu();
            string userInput = UserInput.GetUserOption();
            ProcessInput(userInput);
        }
        Exit();
    }

    private static void ProcessInput(string userInput)
    {
        switch (userInput)
        {
            case "s":
                Study();
                break;
            case "v":
                Viewer.ViewStudies();
                break;
            case "a":
                Add();
                break;
            case "d":
                Delete();
                break;
            case "u":
                Update();
                break;
            case "0":
                exitApp = true;
                break;
            default:
                break;
        }
    }

    private static void Exit()
    {
        Environment.Exit(0);
    }

    private static void Update()
    {
        Console.WriteLine("Woud you like to update a stack or a flashcard? (Type s or f)");
        string option = UserInput.ChooseStackOrFlashcard();
        if (option == "s")
        {
            UpdateStack();
        }
        else if (option == "f")
        {
            UpdateFlashcard();
        }
    }

    private static void UpdateFlashcard()
    {
        List<StackDTO> stacks = Viewer.ViewStacks();
        Console.WriteLine("Which stack do you want to update a flashcard from?");
        int index = UserInput.GetIndex(stacks);
        if (index == -1) { return; }
        StackDTO selectedStack = stacks[index - 1];

        Console.WriteLine("Which flashcard do you want to update?");
        List<FlashcardDTO> flashcards = Viewer.ViewFlashcards(selectedStack.Name);
        if (flashcards.Count == 0 || flashcards == null)
        {
            Console.WriteLine("This stack does not contain any flashcards yet! Try adding some from the menu.");
            return;
        }
        int flashcardIndex = UserInput.GetIndex(flashcards) - 1;
        if (flashcardIndex == -1) { return; }

        Console.WriteLine("What will be the new prompt/question on the flashcard?");
        string prompt = UserInput.GetDbFriendlyString();

        Console.WriteLine("What will be the new answer on the flashcard?");
        string answer = UserInput.GetDbFriendlyString();

        dataAccessor.UpdateFlashcard(prompt, answer, flashcards[flashcardIndex].Prompt);
        Console.WriteLine($"\nSuccessfully updated a flashcard in the {selectedStack.Name} stack.");
    }

    private static void UpdateStack()
    {
        List<StackDTO> stacks = Viewer.ViewStacks();
        if (stacks.Count == 0 || stacks == null)
        {
            Console.WriteLine("You don't have any stacks yet! Try adding some from the menu.");
            return;
        }
        Console.WriteLine("Which stack do you want to update?");
        int index = UserInput.GetIndex(stacks);
        if (index == -1) { return; }
        StackDTO selectedStack = stacks[index - 1];

        Console.WriteLine("What do you want to rename the stack to?");
        string newName = UserInput.GetDbFriendlyString();

        dataAccessor.UpdateStackName(newName, selectedStack.Name);
        Console.WriteLine($"\nSuccessfully renamed stack {selectedStack.Name} to {newName}.");
    }

    private static void Delete()
    {
        Console.WriteLine("Woud you like to delete a stack or a flashcard? (Type s or f)");
        string option = UserInput.ChooseStackOrFlashcard();
        if (option == "s")
        {
            DeleteStack();
        }
        else if (option == "f")
        {
            DeleteFlashcard();
        }
    }

    private static void DeleteFlashcard()
    {
        List<StackDTO> stacks = Viewer.ViewStacks();
        Console.WriteLine("Which stack do you want to delete a flashcard from?");
        int index = UserInput.GetIndex(stacks);
        if (index == -1) { return; }
        StackDTO selectedStack = stacks[index - 1];

        Console.WriteLine("Which flashcard do you want to delete?");
        List<FlashcardDTO> flashcards = Viewer.ViewFlashcards(selectedStack.Name);
        if (flashcards.Count == 0 || flashcards == null)
        {
            Console.WriteLine("This stack does not contain any flashcards yet! Try adding some from the menu.");
            return;
        }
        int flashcardIndex = UserInput.GetIndex(flashcards);
        if (flashcardIndex == -1) { return; }

        dataAccessor.DeleteFlashcard(flashcards[flashcardIndex - 1].Prompt);
        Console.WriteLine($"\nSuccessfully deleted a flashcard from the {selectedStack.Name} stack.");
    }

    private static void DeleteStack()
    {
        List<StackDTO> stacks = Viewer.ViewStacks();
        Console.WriteLine("Which stack do you want to delete?");
        int index = UserInput.GetIndex(stacks);
        if (index == -1) { return; }
        StackDTO selectedStack = stacks[index - 1];

        dataAccessor.DeleteStack(selectedStack.Name);
        Console.WriteLine($"\nSuccessfully deleted stack {selectedStack.Name}.");
    }

    private static void Add()
    {
        Console.WriteLine("Woud you like to add a new stack or a new flashcard? (Type s or f)");
        string option = UserInput.ChooseStackOrFlashcard();
        if (option == "s")
        {
            AddStack();
        }
        else if (option == "f")
        {
            AddFlashcard();
        }
    }

    private static void AddFlashcard()
    {
        List<StackDTO> stacks = Viewer.ViewStacks();
        Console.WriteLine("Which stack do you want to add a flashcard to?");
        int index = UserInput.GetIndex(stacks);
        if (index == -1) { return; }
        StackDTO selectedStack = stacks[index - 1];

        Console.WriteLine("What will be the prompt/question on the flashcard?");
        string prompt = UserInput.GetDbFriendlyString();

        Console.WriteLine("What will be the answer on the flashcard?");
        string answer = UserInput.GetDbFriendlyString();

        int stackId = dataAccessor.GetStackId(selectedStack.Name);
        dataAccessor.AddFlashcard(stackId, prompt, answer);
        Console.WriteLine($"\nSuccessfully created a new flashcard in the {dataAccessor.GetStackById(stackId).Name} stack.");
    }

    private static void AddStack()
    {
        Console.WriteLine("What is the name of your new stack?");
        string name = UserInput.GetDbFriendlyString();

        dataAccessor.AddStack(name);
        Console.WriteLine($"\nSuccessfully created a new stack called {name}.");
    }

    private static void Study()
    {
        List<StackDTO> stacks = Viewer.ViewStacks();
        Console.WriteLine("Which stack would you like to revise?");
        int index = UserInput.GetIndex(stacks);
        if (index == -1) { return; }
        StackDTO selectedStack = stacks[index - 1];
        List<FlashcardDTO> flashcards = dataAccessor.GetFlashcardsInStack(selectedStack.Name);
        if (flashcards.Count == 0)
        {
            Console.WriteLine("This stack does not contain any flashcards yet! Try adding some from the menu.");
            return;
        }
        int score = StartStudySession(flashcards);
        dataAccessor.AddStudy(selectedStack.Name, DateTime.Now, GetScoreOutOfMaximum(score, flashcards.Count));
    }

    private static int StartStudySession(List<FlashcardDTO> flashcards)
    {
        int score = 0;
        foreach (FlashcardDTO flashcard in flashcards)
        {
            Console.WriteLine(flashcard.Prompt);
            Console.Write("Your answer?: ");
            string answer = Console.ReadLine();
            if (answer == flashcard.Answer)
            {
                Console.WriteLine("Correct!\n");
                score++;
            }
            else
            {
                Console.WriteLine($"Sorry, the correct answer was: {flashcard.Answer}\n");
            }
        }
        Console.WriteLine($"You have completed all the flashcards! You got {GetScoreOutOfMaximum(score, flashcards.Count) * 100}%");
        return score;
    }

    private static float GetScoreOutOfMaximum(int score, int maximum) => ((float)score / (float)maximum);
}
