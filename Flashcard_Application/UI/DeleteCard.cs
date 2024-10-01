using Flashcard_Application.DataServices;
using Flashcards.UI;
using Spectre.Console;

namespace Flashcard_Application.UI;

public class DeleteCard
{
    public static void DeleteCardPrompt()
    {
        Console.Clear();

        var stacks = StackDatabaseServices.GetAllStacks();
        string[] stackNameArray = new string[stacks.Count];

        for (int i = 0; i < stackNameArray.Length; i++)
        {
            stackNameArray[i] = stacks[i].StackName;
        }

        var stackSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Please select the stack you wish to delete the card from: [/]")
                .PageSize(10)
                .AddChoices(stackNameArray)
                );

        var cards = FlashcardDatabaseServices.GetFlashCardsInStack(stackSelection);
        string[] cardNameArray = new string[cards.Count];
        for (int i = 0; i < cardNameArray.Length; i++)
        {
            cardNameArray[i] = cards[i].Question;
        }

        var cardSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Please select the card you wish to delete: [/]")
                .PageSize(10)
                .AddChoices(cardNameArray)
                );

        FlashcardDatabaseServices.DeleteCard(cardSelection);

        Console.Clear();
        MainMenu.MainMenuPrompt();
    }
}
