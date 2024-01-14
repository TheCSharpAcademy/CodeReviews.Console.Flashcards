using Flashcards.StevieTV.Database;
using Flashcards.StevieTV.Helpers;
using Flashcards.StevieTV.Models;
using Spectre.Console;

namespace Flashcards.StevieTV.UI;

internal static class StudyGame
{
    internal static void ChooseStudyStack()
    {
        var stacks = StacksDatabaseManager.GetStacks();

        Console.Clear();

        var menuSelection = new SelectionPrompt<Stack>();
        menuSelection.Title("Which stack would you like to study");
        menuSelection.AddChoices(stacks);
        menuSelection.AddChoice(new Stack {StackId = 0, Name = "Cancel and return to menu"});
        menuSelection.UseConverter(stack => stack.Name);

        var selectedStack = AnsiConsole.Prompt(menuSelection);

        if (selectedStack.StackId == 0) return;
        
        StudyStack(selectedStack);
    }

    private static void StudyStack(Stack selectedStack)
    {
        var flashCards = new List<FlashCardDTO>();

        foreach (var flashCard in FlashCardsDatabaseManager.GetFlashCards(selectedStack))
        {
            flashCards.Add(FlashCardMapper.FlashCardMapToDTO(flashCard));
        }
        
        AnsiConsole.WriteLine($"There are {flashCards.Count} Flash Cards in the {selectedStack.Name} Stack.");

        var studyLength = AnsiConsole.Prompt(
            new TextPrompt<int>("How many Flash Cards would you like to study?")
                .DefaultValue(flashCards.Count)
                .Validate(count => count <= flashCards.Count));

        var randomCard = new Random();
        var score = 0;

        for (int i = 0; i < studyLength; i++)
        {
            int randomCardIndex = randomCard.Next(flashCards.Count);
            var flashCard = flashCards[randomCardIndex];

            if (GuessCard(flashCard))
            {
                score++;
                AnsiConsole.Prompt(new ConfirmationPrompt("Correct. Press enter to continue"));
            }
            else
            {
                AnsiConsole.WriteLine($"Incorrect. The correct answer is {flashCard.Back.ToTitleCase()}");
                AnsiConsole.Prompt(new ConfirmationPrompt("Press enter to continue"));
            }
            flashCards.Remove(flashCard);
        }

        switch ((score * 100) / studyLength)
        {
            case 0:
                AnsiConsole.WriteLine($"Oh dear, you got all the answers incorrect, time to study more!");
                break;
            case < 50:
                AnsiConsole.WriteLine($"Good work, you got {score} correct answers!");
                break;
            case 100:
                AnsiConsole.WriteLine($"Amazing, you got every answer correct, great work!");
                break;
            default:
                AnsiConsole.WriteLine($"Great work, you got {score} correct answers!");
                break;
        }

        var studySession = new StudySession
        {
            DateTime = DateTime.Now,
            Stack = selectedStack,
            Score = score,
            QuantityTested = studyLength
        };
        
        StudySessionsDatabaseManager.Post(studySession);

        AnsiConsole.Prompt(new ConfirmationPrompt("Press enter to return to the main menu"));
    }

    private static bool GuessCard(FlashCardDTO flashCard)
    {
        AnsiConsole.Clear();
        var cardFront = new Panel(flashCard.Front)
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(5, 1, 5, 1),
            Header = new PanelHeader("Card Front")
        };
        
        AnsiConsole.Write(cardFront);
        
        var guess = AnsiConsole.Prompt(new TextPrompt<string>("What is the back of the above card?:"));

        return guess.ToLower() == flashCard.Back.ToLower();
    }
}