using ConsoleTableExt;
using Flashcards.Models;

namespace Flashcards;

internal class Program
{
    private static readonly DataAccessor dataAccessor = new();
    public static void Main(string[] args)
    {
        Viewer.DisplayTitle();
        while (true)
        {
            Viewer.DisplayOptionsMenu();
            string userInput = UserInput.GetUserOption();
            ProcessInput(userInput);
        }
    }

    private static void ProcessInput(string userInput)
    {
        switch (userInput)
        {
            case "s":
                Study();
                break;
            case "v":
                ViewStudies();
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
                Exit();
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
            List<StackDTO> stacks = ViewStacks();
            Console.WriteLine("Which stack do you want to update?");
            int index = UserInput.GetIndex(stacks) - 1;
            if (index == -1) { return; }
            StackDTO selectedStack = stacks[index];
            Console.WriteLine("What do you want to rename the stack to?");
            string newName = UserInput.GetDbFriendlyString();
            dataAccessor.UpdateStackName(newName, selectedStack.Name);
            Console.WriteLine($"\nSuccessfully renamed stack {selectedStack.Name} to {newName}.");
        }
        else if (option == "f")
        {
            List<StackDTO> stacks = ViewStacks();
            Console.WriteLine("Which stack do you want to update a flashcard from?");
            int index = UserInput.GetIndex(stacks) - 1;
            if (index == -1) { return; }
            StackDTO selectedStack = stacks[index];
            Console.WriteLine("Which flashcard do you want to update?");
            List<FlashcardDTO> flashcards = ViewFlashcards(selectedStack.Name);
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
    }

    private static void Delete()
    {
        Console.WriteLine("Woud you like to delete a stack or a flashcard? (Type s or f)");
        string option = UserInput.ChooseStackOrFlashcard();
        if (option == "s")
        {
            List<StackDTO> stacks = ViewStacks();
            Console.WriteLine("Which stack do you want to delete?");
            int index = UserInput.GetIndex(stacks);
            if (index == -1) { return; }
            StackDTO selectedStack = stacks[index - 1];
            dataAccessor.DeleteStack(selectedStack.Name);
            Console.WriteLine($"\nSuccessfully deleted stack {selectedStack.Name}.");
        }
        else if (option == "f")
        {
            List<StackDTO> stacks = ViewStacks();
            Console.WriteLine("Which stack do you want to delete a flashcard from?");
            int index = UserInput.GetIndex(stacks);
            if (index == -1) { return; }
            StackDTO selectedStack = stacks[index - 1];
            Console.WriteLine("Which flashcard do you want to delete?");
            List<FlashcardDTO> flashcards = ViewFlashcards(selectedStack.Name);
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
    }

    private static void Add()
    {
        Console.WriteLine("Woud you like to add a new stack or a new flashcard? (Type s or f)");
        string option = UserInput.ChooseStackOrFlashcard();
        if (option == "s")
        {
            Console.WriteLine("What is the name of your new stack?");
            string name = UserInput.GetDbFriendlyString();
            dataAccessor.AddStack(name);
            Console.WriteLine($"\nSuccessfully created a new stack called {name}.");
        }
        else if (option == "f")
        {
            List<StackDTO> stacks = ViewStacks();
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
    }

    private static void ViewStudies()
    {
        List<StudyDTO> studies = dataAccessor.GetStudies();
        var tableData = new List<List<object>>();
        foreach (StudyDTO study in studies)
        {
            tableData.Add(new List<object> { study.Stack, Math.Round(study.Score * 100, 2), study.Date });
        }
        ConsoleTableBuilder.From(tableData).WithTitle("Your Studies").WithColumn("Stack", "Score (%)", "Date").ExportAndWriteLine();
    }

    private static void Study()
    {
        List<StackDTO> stacks = ViewStacks();
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

    private static List<StackDTO> ViewStacks()
    {
        List<StackDTO> stacks = dataAccessor.GetStacks();
        var tableData = new List<List<object>>();
        foreach (StackDTO stack in stacks)
        {
            tableData.Add(new List<object> { stacks.IndexOf(stack) + 1, stack.Name });
        }
        ConsoleTableBuilder.From(tableData).WithTitle("Your Stacks").WithColumn("Id", "Name").ExportAndWriteLine();
        return stacks;
    }

    private static List<FlashcardDTO>? ViewFlashcards(string stackName)
    {
        List<FlashcardDTO> flashcards = dataAccessor.GetFlashcardsInStack(stackName);
        if (flashcards == null) { return null; }
        var tableData = new List<List<object>>();
        foreach (FlashcardDTO flashcard in flashcards)
        {
            tableData.Add(new List<object> { flashcards.IndexOf(flashcard) + 1, flashcard.Prompt, flashcard.Answer });
        }
        ConsoleTableBuilder.From(tableData).WithTitle($"Flashcards in {stackName}").WithColumn("Id", "Prompt", "Answer").ExportAndWriteLine();
        return flashcards;
    }
}
