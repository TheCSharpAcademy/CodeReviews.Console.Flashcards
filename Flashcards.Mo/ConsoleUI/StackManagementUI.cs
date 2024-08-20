using Flashcards.Services;
public static class StackManagementUI
{
    public static void Run(StackService stackService, FlashcardService flashcardService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Stack Management");
            Console.WriteLine("1. Create Stack");
            Console.WriteLine("2. View Stacks");
            Console.WriteLine("3. Delete Stack");
            Console.WriteLine("4. Create Flashcard");
            Console.WriteLine("5. Take a Quiz");
            Console.WriteLine("6. View Flashcards");
            Console.WriteLine("7. Delete Flashcard");
            Console.WriteLine("8. View Sessions Per Month Report");
            Console.WriteLine("9. View Average Score Per Month Report");
            Console.WriteLine("10. Back to Main Menu");
            Console.Write("Select an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateStack(stackService);
                    break;
                case "2":
                    ViewStacks(stackService, flashcardService);
                    break;
                case "3":
                    DeleteStack(stackService);
                    break;
                case "4":
                    CreateFlashcard(stackService, flashcardService);
                    break;
                case "5":
                    TakeQuiz(stackService, flashcardService);
                    break;
                case "6":
                    ViewFlashcards(stackService, flashcardService);
                    break;
                case "7":
                    DeleteFlashcard(stackService, flashcardService);
                    break;
                case "8":
                    ViewSessionsPerMonthReport(stackService);
                    break;
                case "9":
                    ViewAverageScorePerMonthReport(stackService);
                    break;
                case "10":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void ViewSessionsPerMonthReport(StackService stackService)
    {
        Console.Clear();
        Console.Write("Input a year in format YYYY: ");
        var inputYear = Console.ReadLine();
        if (int.TryParse(inputYear, out int year))
        {
            stackService.ShowSessionsPerMonth(year);
        }
        else
        {
            Console.WriteLine("Invalid year format.");
        }
        Console.ReadKey();
    }


    private static void ViewAverageScorePerMonthReport(StackService stackService)
    {
        Console.Clear();
        Console.Write("Input a year in format YYYY: ");
        var inputYear = Console.ReadLine();
        if (int.TryParse(inputYear, out int year))
        {
            stackService.ShowAverageScorePerMonth(year);
        }
        else
        {
            Console.WriteLine("Invalid year format.");
        }
        Console.ReadKey();
    }



    private static void CreateStack(StackService stackService)
    {
        Console.Clear();
        Console.Write("Enter the name of the new stack: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Stack name cannot be empty. Please try again.");
        }
        else
        {
            stackService.CreateStack(name);
            Console.WriteLine($"Stack '{name}' created successfully.");
        }

        Console.ReadKey();
    }
    private static void CreateFlashcard(StackService stackService, FlashcardService flashcardService)
    {
        Console.Clear();
        Console.Write("Enter the name of the stack to add a flashcard to: ");
        var stackName = Console.ReadLine();
        if (string.IsNullOrEmpty(stackName))
        {
            Console.WriteLine("Stack name cannot be empty.");
            Console.ReadKey();
            return;
        }
        var stack = stackService.GetStackByName(stackName);
        if (stack == null)
        {
            Console.WriteLine("Stack not found.");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter the question: ");
        var question = Console.ReadLine();
        Console.Write("Enter the answer: ");
        var answer = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(question) || string.IsNullOrWhiteSpace(answer))
        {
            Console.WriteLine("Question and answer cannot be empty. Please try again.");
        }
        else
        {
            flashcardService.CreateFlashcard(stack.Id, question, answer);
            Console.WriteLine("Flashcard created successfully.");
        }

        Console.ReadKey();
    }

    private static void ViewStacks(StackService stackService, FlashcardService flashcardService)
    {
        Console.Clear();
        var stacks = stackService.GetAllStacks();

        if (stacks.Any())
        {
            foreach (var stack in stacks)
            {
                Console.WriteLine($"Stack: {stack.Name}");
                var flashcards = flashcardService.GetFlashcardsForStack(stack.Id);
                int flashcardNumber = 1;
                foreach (var flashcard in flashcards)
                {
                    Console.WriteLine($"  {flashcardNumber}. Q: {flashcard.Question} | A: {flashcard.Answer}");
                    flashcardNumber++;
                }
            }
        }
        else
        {
            Console.WriteLine("No stacks available.");
        }

        Console.ReadKey();
    }

    private static void DeleteStack(StackService stackService)
    {
        Console.Clear();
        Console.Write("Enter the name of the stack to delete: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Stack name cannot be empty. Please try again.");
        }
        else
        {
            stackService.DeleteStackByName(name);
            Console.WriteLine($"Stack '{name}' deleted successfully.");
        }

        Console.ReadKey();
    }

    private static void TakeQuiz(StackService stackService, FlashcardService flashcardService)
    {
        Console.Clear();
        Console.Write("Enter the name of the stack you want to take a quiz on: ");
        var stackName = Console.ReadLine();
        if (string.IsNullOrEmpty(stackName))
        {
            Console.WriteLine("Stack name cannot be empty.");
            Console.ReadKey();
            return;
        }


        var stack = stackService.GetStackByName(stackName);
        if (stack == null)
        {
            Console.WriteLine("Stack not found.");
            Console.ReadKey();
            return;
        }

        var flashcards = flashcardService.GetFlashcardsForStack(stack.Id).ToList();
        if (!flashcards.Any())
        {
            Console.WriteLine("No flashcards found for this stack.");
            Console.ReadKey();
            return;
        }

        int correctAnswers = 0;

        foreach (var flashcard in flashcards)
        {
            Console.Clear();
            Console.WriteLine($"Question: {flashcard.Question}");
            Console.Write("Your Answer: ");
            var userAnswer = Console.ReadLine();

            if (string.Equals(userAnswer, flashcard.Answer, StringComparison.OrdinalIgnoreCase))
            {
                correctAnswers++;
                Console.WriteLine("Correct!");
            }
            else
            {
                Console.WriteLine($"Incorrect! The correct answer is: {flashcard.Answer}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        Console.Clear();
        Console.WriteLine($"Quiz completed! You got {correctAnswers} out of {flashcards.Count} correct.");
        Console.ReadKey();
    }

    private static void DeleteFlashcard(StackService stackService, FlashcardService flashcardService)
    {
        Console.Clear();
        Console.Write("Enter the name of the stack: ");
        var stackName = Console.ReadLine();
        if (string.IsNullOrEmpty(stackName))
        {
            Console.WriteLine("Stack name cannot be empty.");
            Console.ReadKey();
            return;
        }
        var stack = stackService.GetStackByName(stackName);
        if (stack == null)
        {
            Console.WriteLine("Stack not found.");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter the flashcard number to delete: ");
        if (int.TryParse(Console.ReadLine(), out int flashcardNumber))
        {
            var flashcards = flashcardService.GetFlashcardsForStack(stack.Id).ToList();
            if (flashcardNumber < 1 || flashcardNumber > flashcards.Count)
            {
                Console.WriteLine("Invalid flashcard number.");
            }
            else
            {
                var flashcard = flashcards[flashcardNumber - 1]; 
                flashcardService.DeleteFlashcard(flashcard.FlashcardNumber); 
                Console.WriteLine($"Flashcard {flashcardNumber} deleted successfully.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }

        Console.ReadKey();
    }

    private static void ViewFlashcards(StackService stackService, FlashcardService flashcardService)
    {
        Console.Clear();
        Console.Write("Enter the name of the stack to view flashcards: ");
        var stackName = Console.ReadLine();
        if (string.IsNullOrEmpty(stackName))
        {
            Console.WriteLine("Stack name cannot be empty.");
            Console.ReadKey();
            return;
        }
        var stack = stackService.GetStackByName(stackName);
        if (stack == null)
        {
            Console.WriteLine("Stack not found.");
            Console.ReadKey();
            return;
        }

        var flashcards = flashcardService.GetFlashcardsForStack(stack.Id).ToList();
        if (!flashcards.Any())
        {
            Console.WriteLine("No flashcards found for this stack.");
        }
        else
        {
            foreach (var flashcard in flashcards)
            {
                Console.WriteLine($"{flashcard.FlashcardNumber}. Q: {flashcard.Question} | A: {flashcard.Answer}");
            }
        }

        Console.ReadKey();
    }


}
