using Flashcard_Application.DataServices;
using Flashcards.Models;
using Flashcards.UI;
using Spectre.Console;

namespace Flashcard_Application.UI;

internal class CreateCard
{
    public static void CreateCardPrompt()
    {
        string cardQuestion = AnsiConsole.Prompt<string>(new TextPrompt<string>("Please enter the question for the Card."));
        string cardAnswer = AnsiConsole.Prompt<string>(new TextPrompt<string>("Please enter the answer for the Card."));

        List<CardStack> stacks = StackDatabaseServices.GetAllStacks();
        string[] stackNameArray = new string[stacks.Count];

        for (int i = 0; i < stackNameArray.Length; i++)
        {
            stackNameArray[i] = stacks[i].StackName;
        }

        var stackSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]\n\nPlease select the stack to add the card to: [/]")
                .PageSize(10)
                .AddChoices(stackNameArray)
                );

        Flashcard flashcard = new();
        flashcard.Question = cardQuestion;
        flashcard.Answer = cardAnswer;

        for (int i = 0; i < stacks.Count; i++)
        {
            if (stackNameArray[i] == stackSelection)
                flashcard.StackId = stacks[i].StackId;
        }

        FlashcardDatabaseServices.InsertFlashcard(flashcard);
        Console.Clear();
        MainMenu.MainMenuPrompt();
    }
}
