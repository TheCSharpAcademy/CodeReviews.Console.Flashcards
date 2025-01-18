using FlashCards.Database;
using FlashCards.Models;
using Microsoft.IdentityModel.Tokens;

namespace FlashCards.UI;

internal static class Menu
{
    public static void ShowMainMenu()
    {
        Console.WriteLine("Welcome to Flashcards App");
        PauseForUser();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Please Enter your choice");
            Console.WriteLine("1- Add Stack");
            Console.WriteLine("2- Manage Stacks");
            Console.WriteLine("3- Study");
            Console.WriteLine("4- View StudySessions");
            Console.WriteLine("5- View year summary");
            Console.WriteLine("0- Exit");
            switch(Console.ReadLine()?.Trim())
            {
                case "1":
                    AddStack();
                    break;
                case "2":
                    if (ViewStacks())
                    {
                        var Stacks = DatabaseManager.GetStacks();
                        var SelectedStack = new Stack();
                        Console.WriteLine("Please enter a stack name to manage or 0 to go back to the main menu.");
                        string? stackName;
                        while (true)
                        {
                            stackName = Console.ReadLine()?.Trim();

                            if (string.IsNullOrEmpty(stackName))
                            {
                                Console.WriteLine("Input cannot be empty. Please try again.");
                                continue;
                            }

                            if (stackName == "0")
                                break;

                            if (!Stacks.Any(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase)))
                            {
                                Console.WriteLine("Invalid stack name. Please enter a valid name from the list.");
                                continue;
                            }
                            else
                            {
                                SelectedStack = Stacks.First(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase));
                            }

                            ManageStacksMenu(SelectedStack);
                            break;
                        }
                    }
                    break;
                case "3":
                    StudyMenu();
                    break;
                case "4":
                    StudySessionsMenu();
                    break;
                case "5":
                    ViewYearSummaryMenu();
                    break;
                case "0":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice, Try again.");
                    PauseForUser();
                    break;
            }
        }
    }

    private static void PauseForUser()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void AddStack()
    {
        Console.WriteLine("Enter the name of the stack. 0 To Back");
        var stackName = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(stackName))
        {
            Console.WriteLine("Stack name cannot be empty. Please try again.");
            PauseForUser();
            return;
        }
        if (stackName == "0")
            return;

        var Stacks = DatabaseManager.GetStacks();
        if (Stacks.Any(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Stack with the same name already exists. Please try again.");
            PauseForUser();
            return;
        }
        var stack = new Stack { Name = stackName };

        DatabaseManager.AddStack(stack);
        Console.WriteLine("Stack added successfully.");
        PauseForUser();
    }

    private static bool ViewStacks()
    {
        var Stacks = DatabaseManager.GetStacks();

        if (Stacks == null || Stacks.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("No stacks available. Please add a stack first.");
            Console.WriteLine("Please Enter Another choice.");
            PauseForUser();
            return false;
        }
        Console.Clear();
        Console.WriteLine("Stacks:");

        var DtoStacks = Stacks.Select(s => new StackDto
        {
            Name = s.Name
        }).ToList();

        foreach (var stack in DtoStacks)
        {
            Console.WriteLine($"- {stack.Name}");
        }

        return true;
    }

    private static void ManageStacksMenu(Stack stack)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Managing {stack.Name} Stack\n");
            Console.WriteLine("Please Enter your choice");
            Console.WriteLine("1- Add Flashcard");
            Console.WriteLine("2- View Flashcards");
            Console.WriteLine("3- Edit Flashcard");
            Console.WriteLine("4- Delete Flashcard");
            Console.WriteLine("5- Delete Stack");
            Console.WriteLine("0- Back to main menu");
            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    AddFlashcard(stack);
                    PauseForUser();
                    break;
                case "2":
                    ViewFlashcards(stack);
                    PauseForUser();
                    break;
                case "3":
                    EditFlashcard(stack);
                    break;
                case "4":
                    DeleteFlashcardMenu(stack);
                    break;
                case "5":
                    DatabaseManager.DeleteStack(stack);
                    Console.WriteLine("Stack deleted successfully.");
                    PauseForUser();
                    return;
                case "0":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choice, Try again.");
                    PauseForUser();
                    break;
            }
        }
    }

    private static void AddFlashcard(Stack stack)
    {
        Console.Clear();
        var flashcard = new Flashcard { StackId = stack.StackId};
        Console.Write("Enter Question : ");
        string? question = Console.ReadLine()?.Trim();
        Console.Write("Enter Answer : ");
        string? answer = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(answer) || string.IsNullOrEmpty(question))
        {
            Console.WriteLine("Question and Answer cannot be empty. Please try again.");
            return;
        }

        var flashCards = DatabaseManager.GetFlashcards(stack.StackId);
        if (flashCards != null && flashCards.Any(f => f.Question.Equals(question, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Flashcard with the same question already exists. Please try again.");
            PauseForUser();
            return;
        }

        flashcard.Answer = answer;
        flashcard.Question = question;
        DatabaseManager.AddFlashcard(flashcard);
        Console.WriteLine("Flashcard added successfully.");
        PauseForUser();
    }

    private static List<Flashcard>? ViewFlashcards(Stack stack)
    {
        Console.Clear();
        var flashcards = new List<Flashcard>();
        flashcards = DatabaseManager.GetFlashcards(stack.StackId);
        var DTOStack = new StackDto { Name = stack.Name };
        if (flashcards == null || flashcards.Count == 0)
        {
            Console.WriteLine("No flashcards available. Please add a flashcard first.");
            Console.WriteLine("Please Enter Another choice.");
            return flashcards;
        }
        Console.WriteLine($"Flash Cards for {DTOStack.Name} :");

        var DtoFlashcards = flashcards.Select(f => new FlashcardDto
        {
            Answer = f.Answer,
            Question = f.Question
        }).ToList();

        for (int i = 0; i < DtoFlashcards.Count; i++)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Flashcard Id: {i+1}");
            Console.WriteLine($"Question: {DtoFlashcards[i].Question}");
            Console.WriteLine($"Answer: {DtoFlashcards[i].Answer}");
            Console.WriteLine("-------------------------------------------------");
        }
        return flashcards;
    }

    private static void EditFlashcard(Stack stack)
    {
        var flashcards = ViewFlashcards(stack);
        if (flashcards == null || flashcards.Count == 0)
        {
            PauseForUser();
            return;
        }

        Console.WriteLine("Enter the ID of the flashcard you want to edit");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID. Please try again.");
            PauseForUser();
            return;
        }
        if (id < 1 || id > flashcards.Count)
        {
            Console.WriteLine("Invalid ID. Please try again.");
            PauseForUser();
            return;
        }
        Console.WriteLine("Enter the new question");
        string? question = Console.ReadLine()?.Trim();
        Console.WriteLine("Enter the new answer");
        string? answer = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(question) || string.IsNullOrEmpty(answer))
        {
            Console.WriteLine("Question and Answer cannot be empty. Please try again.");
            PauseForUser();
            return;
        }
        var newFlashcard = new Flashcard
        {
            FlashcardId = flashcards[id - 1].FlashcardId,
            StackId = stack.StackId,
            Question = question,
            Answer = answer
        };
        DatabaseManager.EditFlashcard(newFlashcard);
        Console.WriteLine("Flashcard edited successfully.");
        PauseForUser();
    }

    private static void DeleteFlashcardMenu(Stack stack)
    {
        var flashcards = ViewFlashcards(stack);

        if (flashcards == null || flashcards.Count == 0)
        {
            PauseForUser();
            return;
        }

        Console.WriteLine("Enter the ID of the flashcard you want to delete");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID. Please try again.");
            PauseForUser();
            return;
        }
        if (id < 1 || id > flashcards.Count)
        {
            Console.WriteLine("Invalid ID. Please try again.");
            PauseForUser();
            return;
        }
        DatabaseManager.DeleteFlashcard(flashcards[id - 1]);
        Console.WriteLine("Flashcard deleted successfully.");
        PauseForUser();
    }

    private static void StudyMenu()
    {
        Console.Clear();
        ViewStacks();
        string? stackName;
        while (true)
        {
            Console.WriteLine("Please Enter Name of the stack you want to study from. Enter 0 to back");
            stackName = Console.ReadLine()?.Trim();
            if (stackName == "0")
                return;

            if (string.IsNullOrEmpty(stackName))
            {
                Console.WriteLine("Input cannot be empty. Please try again.");
                continue;
            }

            var Stacks = DatabaseManager.GetStacks();

            if (!Stacks.Any(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Invalid stack name. Please enter a valid name from the list.");
                continue;
            }
            else
            {
                var SelectedStack = Stacks.First(s => s.Name.Equals(stackName, StringComparison.OrdinalIgnoreCase));
                StudyStack(SelectedStack);
                break;
            }
        }
    }

    private static void StudyStack(Stack selectedStack)
    {
        Console.WriteLine($"Studying {selectedStack.Name}. \n");
        var flashcards = DatabaseManager.GetFlashcards(selectedStack.StackId);
        if (flashcards == null || flashcards.Count == 0)
        {
            Console.WriteLine("No flashcards available. Please add a flashcard first.");
            Console.WriteLine("Please Enter Another choice.");
            PauseForUser();
            return;
        }

        var DtoFlashcards = flashcards.Select(s => new FlashcardDto
        {
            Question = s.Question,
            Answer = s.Answer
        }).ToList();

        int correctAnswers = 0;
        for (int i = 0; i < DtoFlashcards.Count; i++)
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Question no. : {i + 1}");
            Console.WriteLine($"Question: {DtoFlashcards[i].Question}");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Please enter your answer");

            string? answer = Console.ReadLine()?.Trim();
            if (answer != null)
            {
                if (answer.Equals(DtoFlashcards[i].Answer, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Correct Answer");
                    correctAnswers++;
                    PauseForUser();
                }
                else
                {
                    Console.WriteLine("Incorrect Answer");
                    Console.WriteLine($"Correct Answer is : {DtoFlashcards[i].Answer}");
                    PauseForUser();
                }
            }
            else
            {
                Console.WriteLine("Answer cannot be empty");
                i--;
                PauseForUser();
                continue;
            }
        }
        Console.Clear();
        Console.WriteLine($"You have answerd {correctAnswers} of {DtoFlashcards.Count} questions correctly.");
        var StudySession = new StudySession
        {
            StackId = selectedStack.StackId,
            SessionDate = DateTime.UtcNow,
            Score = correctAnswers
        };
        DatabaseManager.AddStudySession(StudySession);
        PauseForUser();
    }

    private static void StudySessionsMenu()
    {
        var sessions = DatabaseManager.GetStudySessions();
        var stacks = DatabaseManager.GetStacks();
        Console.Clear();
        if (sessions.IsNullOrEmpty())
        {
            Console.WriteLine("No study sessions available.");
            PauseForUser();
            return;
        }
        Console.WriteLine("Study Sessions:");

        foreach (var session in sessions)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Date : {session.SessionDate}");
            var stack = stacks.First(s => s.StackId == session.StackId);
            Console.WriteLine($"Stack : {stack.Name}");
            Console.WriteLine($"Score : {session.Score}");
            Console.WriteLine("-------------------------------------------------\n");
        }
        PauseForUser();
    }

    private static void ViewYearSummaryMenu()
    {
        Console.Clear();
        var sessions = DatabaseManager.GetStudySessions();
        var stacks = DatabaseManager.GetStacks();

        Console.WriteLine("Enter the year. 0 to back");
        string? year = Console.ReadLine()?.Trim();
        if (year == "0")
            return;

        if (string.IsNullOrEmpty(year))
        {
            Console.WriteLine("Year cannot be empty. Please try again.");
            PauseForUser();
            return;
        }
        if (!int.TryParse(year, out int yearInt))
        {
            Console.WriteLine("Invalid year. Please try again.");
            PauseForUser();
            return;
        }

        var results = sessions
            .Where(s => s.SessionDate.Year == yearInt)
            .GroupBy(s => new { s.StackId, s.SessionDate.Month })
            .Select(g => new
            {
                StackId = g.Key.StackId,
                Month = g.Key.Month,
                AverageScore = g.Average(s => s.Score)
            })
            .OrderBy(r => r.Month)
            .ThenByDescending(r => r.AverageScore);

        if (results.IsNullOrEmpty())
        {
            Console.WriteLine("No results for this year");
            PauseForUser();
            return;
        }
        Console.Clear();
        Console.WriteLine($"Year Summary for {yearInt}:\n");
        Console.WriteLine("Stack Name       | Month | Average Score");
        Console.WriteLine("-----------------|-------|---------------");
        foreach (var result in results)
        {
            var stack = stacks.First(s => s.StackId == result.StackId);
            string monthName = new DateTime(yearInt, result.Month, 1).ToString("MMM");

            Console.WriteLine($"{stack.Name,-16} | {monthName,-5} | {result.AverageScore,13:F2}");
        }

        PauseForUser();
    }
}
