// See https://aka.ms/new-console-template for more information


using Flashcards;
using FlashcardsProgram.Database;
using FlashcardsProgram.Flashcards;
using FlashcardsProgram.Stacks;

// FlashcardService.Display("Spanish practice", new FlashcardDTO(1, "hola amigos in the asdasd asdasd asdasd world", "hello"), 1);
// FlashcardService.Display("Spanish practice", new FlashcardDTO(1, "hola", "hello"), 1, true);


// ConnectionManager.Init();

// // SqlConnection connection = ConnectionManager.Connection;

// var flashCardsRepo = new BaseRepository<FlashcardDAO>("FlashCards");
// var stacksRepo = new StacksRepository(StackDAO.TableName);
// var createStackDto = new CreateStackDTO("Spanish to English6");

// var existingStack = stacksRepo.FindByName(createStackDto.Name);
// Console.WriteLine(existingStack != null ? $"Existing Stack {existingStack.Id}\t{existingStack.Name}" : $"Stack with name {createStackDto.Name} not found");

// var stack = existingStack ?? stacksRepo.Create(createStackDto);

// if (stack == null)
// {
//     Console.WriteLine("Could not create or stack");
//     Environment.Exit(1);
// }

// var updatedStack = stacksRepo.Update(stack.Id, new UpdateStackDTO("NEW NAME OF UPDATED STACKISTS 1"));

// Console.WriteLine(updatedStack != null ? $"UPDATED Stack {updatedStack.Id}\t{updatedStack.Name}" : $"");


// var createFlashcardDto = new CreateFlashcardDTO("hola", "hello", stack.Id);

// var createdFlashcard = flashCardsRepo.Create(createFlashcardDto);

// if (createdFlashcard != null)
// {
//     Console.WriteLine($"Created Stack {stack.Id}\t{stack.Name}");
//     Console.WriteLine($"Created Flashcard {createdFlashcard.Id}\t{createdFlashcard.Front}\t{createdFlashcard.Back}");

//     flashCardsRepo.Delete(createdFlashcard.Id);
//     var allFlashCards = flashCardsRepo.List();

//     foreach (var card in allFlashCards)
//     {
//         Console.WriteLine($"# Flashcard {card.Id}\t{card.Front}\t{card.Back}");
//     }

//     stacksRepo.Delete(stack.Id);
//     Console.WriteLine("After stack delete");

//     var allFlashCardsAfterStackDelete = flashCardsRepo.List();

//     foreach (var card in allFlashCardsAfterStackDelete)
//     {
//         Console.WriteLine($"# Flashcard {card.Id}\t{card.Front}\t{card.Back}");
//     }

// }


namespace FlashcardsProgram;

public class Program
{
    public static void Main()
    {
        bool isAppRunning = true;

        ShowWelcomeMessage();
        do
        {
            isAppRunning = StartApp();
        } while (isAppRunning);

        ShowExitMessage();
    }

    public static void ShowExitMessage()
    {
        Console.WriteLine("Bye!");
    }

    public static void ShowWelcomeMessage()
    {
        Console.WriteLine("Welcome");
    }

    public static bool StartApp()
    {
        bool isRunning = true;

        var selected = Menu.ShowMainMenu();

        switch (selected)
        {
            case Menu.CREATE_SESSION:
                break;
            case Menu.VIEW_SESSIONS:
                break;
            case Menu.MANAGE_STACK:
                break;
            case Menu.CREATE_STACK:
                break;
            case Menu.EXIT:
                isRunning = false;
                break;
        }

        return isRunning;
    }
}