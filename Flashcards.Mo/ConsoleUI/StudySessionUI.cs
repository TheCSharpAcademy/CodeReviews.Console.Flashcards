using Flashcards.Domain.DTO;
using Flashcards.Domain.Entities;
using Flashcards.Services;

public static class StudySessionUI
{
    public static void Run(StackService stackService, StudySessionService studySessionService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Study Sessions");
            Console.WriteLine("1. Start a Study Session");
            Console.WriteLine("2. View Study Session History");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Select an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StartStudySession(stackService, studySessionService);
                    break;
                case "2":
                    ViewStudyHistory(stackService, studySessionService);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void StartStudySession(StackService stackService, StudySessionService studySessionService)
    {
        Console.Clear();
        Console.Write("Enter the name of the stack to study: ");
        var stackName = Console.ReadLine();

        
        if (string.IsNullOrWhiteSpace(stackName))
        {
            Console.WriteLine("Stack name cannot be empty. Please try again.");
            Console.ReadKey();
            return;
        }

        var stack = stackService.GetStackByName(stackName);

        if (stack != null)
        {
            var flashcards = stackService.GetFlashcardsForStack(stack.Id)?.ToList() ?? new List<Flashcard>(); 
            int correctAnswers = 0;

            foreach (var flashcard in flashcards)
            {
                Console.Clear();
                Console.WriteLine($"Question: {flashcard.Question}");
                Console.Write("Your Answer: ");
                var userAnswer = Console.ReadLine();

                if (userAnswer?.Trim().Equals(flashcard.Answer, StringComparison.OrdinalIgnoreCase) == true)
                {
                    correctAnswers++;
                    Console.WriteLine("Correct!");
                }
                else
                {
                    Console.WriteLine($"Incorrect. The correct answer is: {flashcard.Answer}");
                }

                Console.ReadKey();
            }

            int score = flashcards.Any() ? (int)((double)correctAnswers / flashcards.Count() * 100) : 0;
            studySessionService.RecordStudySession(stack.Id, score);
            Console.WriteLine($"Study session completed. Your score: {score}%");
        }
        else
        {
            Console.WriteLine("Stack not found.");
        }

        Console.ReadKey();
    }

    private static void ViewStudyHistory(StackService stackService, StudySessionService studySessionService)
    {
        Console.Clear();
        Console.Write("Enter the name of the stack to view history: ");
        var stackName = Console.ReadLine();

        
        if (string.IsNullOrWhiteSpace(stackName))
        {
            Console.WriteLine("Stack name cannot be empty. Please try again.");
            Console.ReadKey();
            return;
        }

        var stack = stackService.GetStackByName(stackName);

        if (stack != null)
        {
            var sessions = studySessionService.GetStudySessionsForStack(stack.Id)?.ToList() ?? new List<StudySessionDTO>(); 

            if (sessions.Any())
            {
                foreach (var session in sessions)
                {
                    Console.WriteLine($"Date: {session.Date}, Score: {session.Score}%");
                }
            }
            else
            {
                Console.WriteLine("No study sessions found for this stack.");
            }
        }
        else
        {
            Console.WriteLine("Stack not found.");
        }

        Console.ReadKey();
    }
}
