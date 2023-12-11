using ConsoleTableExt;
using Flashcards.UgniusFalze.Models;

namespace Flashcards.UgniusFalze.Menu;

public class DisplayController
{
    private Driver CurrentDriver;

    public DisplayController(Driver currentDriver)
    {
        CurrentDriver = currentDriver;
    }
    private Dictionary<int, Stacks>? DisplayStacks()
    {
        Dictionary<int, Stacks> stacksList = CurrentDriver.GetStacks();
        if (stacksList.Count == 0)
        {
            Console.WriteLine("Stack list is empty");
            return null;
        }
        ConsoleTableBuilder
            .From(stacksList.Values.ToList())
            .WithTitle("Stack list")
            .WithColumn("Stack Id", "Stack Name")
            .ExportAndWriteLine(TableAligntment.Left);
        return stacksList;
    }

    public static int? GetOptionId(bool initial, int count)
    {
        if (initial)
        {
            Console.WriteLine("0. Exit the application");
        }
        else
        {
            Console.WriteLine("0. Go back");
        }

        string? option = Console.ReadLine();
        int optionNumeric;
        if (option == null)
        {
            Console.WriteLine("Choice needs to be not null");
            return null;
        }if(int.TryParse(option, out optionNumeric) == false)
        {
            Console.WriteLine("Choice needs to be numeric");
            return null;
        }if (optionNumeric > count)
        {
            Console.WriteLine("Invalid choice");
            return null;
        } 
        return optionNumeric;
    }

    public void ManageStacks()
    {
        Dictionary<int, Stacks>? stacksList = DisplayStacks();
        if (stacksList == null)
        {
            return;
        }
        int? optionId;
        do
        {
            Console.WriteLine("Choose which stack to manage");
            optionId = GetOptionId(false, stacksList.Last().Key );
            if (optionId == 0)
            {
                return;
            }
            if (optionId != null && stacksList.ContainsKey((int)optionId))
            {
                break;
            }
            Console.WriteLine("Stack doesn't exist");

        } while (true);

        Stacks stack;
        stacksList.TryGetValue((int)optionId, out stack);
        new Menu("Choose what to do with selected stack")
            .AddOption("Update name", () => UpdateStackNameDisplay(stack))
            .AddOption("Manage flashcards", () => ManageFlashcards((int)optionId))
            .AddOption("Add flashcard", (() => AddFlashcard((int)optionId)))
            .AddExitOption("Delete stack", () => DeleteStack(stack))
            .Display(false);
    }

    public void AddStack()
    {
        do
        {
            Console.WriteLine("Enter the stacks name");
            string? stackName = Console.ReadLine();
            if (stackName != null)
            {
                if (Stacks.InsertStack(CurrentDriver.SqlConn,(string)stackName) == false)
                {
                    Console.WriteLine("Stack with this name already exist");
                }
                else
                {
                    return;
                }
            }
            else
            {
                Console.WriteLine("Stack name shouldn't be null");
            }
        } while (true);
    }
    
    private void ManageFlashcards(int stackId)
    {
        Dictionary<int, Models.Flashcards> flashCards = CurrentDriver.GetFlashCards(stackId);
        if (flashCards.Count == 0)
        {
            Console.WriteLine("This stack has no flashcards");
            return;
        }
        Dictionary<int, FlashcardDTO> flashcardDtos = new Dictionary<int, FlashcardDTO>();
        int counter = 0;
        foreach (var flashCard in flashCards)
        {
            counter++;
            flashcardDtos.Add(counter, flashCard.Value.ConvertToDto(counter));
        }
        ConsoleTableBuilder
            .From(flashcardDtos.Values.ToList())
            .WithTitle("Flashcard List")
            .WithColumn("Flashcard Id", "Front", "Back")
            .ExportAndWriteLine();
        int? optionId;
        do
        {
            Console.WriteLine("Choose which flashcard to manage");
            optionId = GetOptionId(false, flashcardDtos.Last().Key );
            if (optionId == 0)
            {
                return;
            }
            if (optionId != null && flashcardDtos.ContainsKey((int)optionId))
            {
                break;
            }
            Console.WriteLine("Flashcard doesn't exist");

        } while (true);

        FlashcardDTO flashcardDto;
        flashcardDtos.TryGetValue((int)optionId, out flashcardDto);
        int actualId = flashcardDto.GetActualId();
        Models.Flashcards flashcard;
        flashCards.TryGetValue(actualId, out flashcard);
        new Menu("Choose what to do with selected flashcard")
            .AddOption("Update front", () => UpdateFlashcardFront(flashcard))
            .AddOption("Update back", () => UpdateFlashcardBack(flashcard))
            .AddExitOption("Delete flashcard", () => DeleteFlashcard(flashcard))
            .Display(false);
    }
    
    private void AddFlashcard(int stackId)
    {
        string front = GetStringInput("Enter the flashcards question.");
        string back = GetStringInput("Enter the flashcards answer");
        Models.Flashcards.InsertFlashcard(CurrentDriver.SqlConn, front, back, stackId);
    }

    private string GetStringInput(string message)
    {
        do
        {
            Console.WriteLine(message);
            string? input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                return (string)input;
            }
            else
            {
                Console.WriteLine("Input shouldn't be empty");
            }
        } while (true);
    }
    
    private void UpdateStackNameDisplay(Stacks stack)
    {
        do
        {
            Console.WriteLine("Enter the new stack name");
            string? stackName = Console.ReadLine();
            if (stackName != null)
            {
                if (stack.UpdateStackName(stackName, CurrentDriver.SqlConn) == false)
                {
                    Console.WriteLine("Stack with this name already exist");
                }
                else
                {
                    return;
                }
            }
            else
            {
                Console.WriteLine("Stack name shouldn't be null");
            }
        } while (true);
    }
    
    private void DeleteStack(Stacks stack)
    {
        stack.DeleteStack(CurrentDriver.SqlConn);
    }
    
    private void UpdateFlashcardFront(Models.Flashcards flashcard)
    {
        do
        {
            Console.WriteLine("Enter the new flashcard question");
            string? flashcardFront = Console.ReadLine();
            if (flashcardFront != null)
            {
                flashcard.Front = flashcardFront;
                flashcard.UpdateFlashcard(CurrentDriver.SqlConn);
                return;
            }
            Console.WriteLine("Flashcard question shouldn't be null");
        } while (true);
    }
    
    private void UpdateFlashcardBack(Models.Flashcards flashcard)
    {
        do
        {
            Console.WriteLine("Enter the new flashcard answer");
            string? flashcardBack = Console.ReadLine();
            if (flashcardBack != null)
            {
                flashcard.Back = flashcardBack;
                flashcard.UpdateFlashcard(CurrentDriver.SqlConn);
                return;
            }
            Console.WriteLine("Flashcard question shouldn't be null");
        } while (true);
    }
    
    private void DeleteFlashcard(Models.Flashcards flashcard)
    {
        flashcard.DeleteFlashcard(CurrentDriver.SqlConn);
    }

    public void Study()
    {
        Console.WriteLine("Choose which stack you want to study from");
        Dictionary<int, Stacks>? stacksList = DisplayStacks();
        if (stacksList == null)
        {
            return;
        }
        int? optionId;
        do
        {
            optionId = GetOptionId(false, stacksList.Last().Key );
            if (optionId == 0)
            {
                return;
            }
            if (optionId != null && stacksList.ContainsKey((int)optionId))
            {
                break;
            }
            Console.WriteLine("Stack doesn't exist");

        } while (true);

        Random rand = new Random();
        List<FlashcardDTO> flashcardDtos = CurrentDriver
            .GetFlashCards((int)optionId)
            .OrderBy(_ => rand.Next())
            .ToList()
            .ConvertAll<FlashcardDTO>(keyValuePair => keyValuePair.Value.ConvertToDto(0));
        if (flashcardDtos.Count == 0)
        {
            Console.WriteLine("This stack doesn't have any flashcard to study from");
            return;
        }
        int correct = 0;
        foreach (var flashcardDto in flashcardDtos)
        {
            ConsoleTableBuilder
                .From(new List<string> { flashcardDto.Front })
                .WithColumn("Front")
                .ExportAndWriteLine();
            string? input = GetStringInput("Input your answer to this card\nOr 0 to exit the session");
            if (input == flashcardDto.Back)
            {
                Console.WriteLine("You are correct");
                correct++;
            }
            else if (input == "0")
            {
                Console.WriteLine($"Correctly guessed {correct} out of {flashcardDtos.Count}");
                Console.WriteLine("Exiting Study Session");
                return;
            }
            else
            {
                Console.WriteLine($"You are not correct, the correct answer is {flashcardDto.Back}");
            }
        }
        StudySession.InsertStudySession(CurrentDriver.SqlConn, (int)Math.Ceiling(((float)correct / (float)flashcardDtos.Count)*100), (int)optionId);
        Console.WriteLine($"Stack is complete, your score is {correct} out of {flashcardDtos.Count}");
    }

    public void DisplaySessions()
    {
        List<StudySessionDTO> studySessions = CurrentDriver.GetStudySessions();
        if (studySessions.Count == 0)
        {
            Console.WriteLine("Study session list is empty.");
            return;
        }
        ConsoleTableBuilder
            .From(studySessions)
            .WithColumn("Date Time", "Score", "Stack")
            .WithTitle("Study Sessions")
            .ExportAndWriteLine();
        
    }
}