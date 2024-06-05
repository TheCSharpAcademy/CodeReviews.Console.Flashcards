using Flashcards.Database;
using Flashcards.Menus;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.FlashcardsMenu;

internal class FlashcardsMenuUI
{
    private FlashcardDatabaseManager flashcardDatabaseManager = new FlashcardDatabaseManager();
    private StackDatabaseManager stackDatabaseManager = new StackDatabaseManager();
    private CardStack currStack = new CardStack();

    internal void FlashCardsMenu(CardStack stack)

    {
        currStack = stack;
        Console.Clear();
        var mainMenu = new MainMenuUI();
        bool isRunning = true;
        while (isRunning)
        {
            var select = new SelectionPrompt<string>();
            select.Title("\n   [bold]FLASHCARDS MENU[/]\n");
            select.AddChoice("Go back to main menu");
            select.AddChoice("View flashcards");
            select.AddChoice("Add flashcard");
            select.AddChoice("Update a flash card");
            select.AddChoice("Delete a flashcard");
            var input = AnsiConsole.Prompt(select);
            switch (input)
            {
                case "Go back to main menu":
                    mainMenu.MainMenu();
                    break;

                case "Add flashcard":
                    AddFlashCard();
                    break;

                case "View flashcards":
                    ViewFlashcards();
                    break;

                case "Update a flash card":
                    UpdateFlashcards();
                    break;

                case "Delete a flashcard":
                    DeleteFlashcard();
                    break;
            }
        }
    }

    internal void StackSelection()
    {
        var cardStacks = stackDatabaseManager.GetStacks();

        var select = new SelectionPrompt<CardStack>();
        select.Title("Select a stack");
        select.AddChoices(cardStacks);
        select.AddChoice(new CardStack { CardstackId = 0, CardstackName = "Go back to menu" });
        select.UseConverter(stackName => stackName.CardstackName);
        var selectedCardStack = AnsiConsole.Prompt(select);
        FlashCardsMenu(selectedCardStack);
    }

    internal void AddFlashCard()
    {
        Console.Clear();
        var flashcards = new FlashCards();

        if (currStack.CardstackId == 0)
        {
            Console.WriteLine($"No stacks found");
            return;
        }

        var inputFront = AnsiConsole.Prompt(new TextPrompt<string>("Please enter the updated front of the card").Validate(input => !InputFront(input), "This question exists"));
        var inputBack = AnsiConsole.Prompt(new TextPrompt<string>("Please enter the back of the card").Validate(input => !InputBack(input), "This answer exists"));

        flashcards.StackId = currStack.CardstackId;
        flashcards.Answer = inputBack;
        flashcards.Question = inputFront;

        flashcardDatabaseManager.AddFlashard(flashcards);
    }

    internal void ViewFlashcards()
    {
        Console.Clear();

        var getFlashcards = flashcardDatabaseManager.ReadFlashcardsDto(currStack);
        int id = 1;

        if (getFlashcards.Count() == 0) Console.WriteLine("There are no flashcards in this stack");
        else
        {
            Console.WriteLine("List of flashcards:\n");
            foreach (var flashcard in getFlashcards)
            {
                AnsiConsole.Write(new Rows(
                new Text($"{id++}\t {flashcard.Question} \t {flashcard.Answer}")));
            }
        }
    }

    internal void UpdateFlashcards()
    {
        var getFlashcards = flashcardDatabaseManager.ReadFlashcards(currStack);
        if (getFlashcards == null || getFlashcards.Count() == 0)
        {
            AnsiConsole.WriteLine("There are no flashcards in this stack");
            return;
        }
        var select = new SelectionPrompt<FlashCards>();
        select.AddChoices(getFlashcards);
        select.UseConverter(flashcardName => $"{flashcardName.Question} {flashcardName.Answer}");
        var selectedFlashcard = AnsiConsole.Prompt(select);
        if (selectedFlashcard != null)
        {
            var inputFront = AnsiConsole.Prompt(new TextPrompt<string>("Please enter the updated front of the card").Validate(input => !InputFront(input), "This question exists, press 'k' if you want to keep the same card"));
            if (inputFront == "k") inputFront = selectedFlashcard.Question;
            var inputBack = AnsiConsole.Prompt(new TextPrompt<string>("Please enter the updated back of the card").Validate(input => !InputBack(input), "This answer exists, press 'k' if you want to keep the same card"));
            if (inputBack == "k") inputBack = selectedFlashcard.Answer;
            flashcardDatabaseManager.UpdateFlashcards(selectedFlashcard, inputFront, inputBack);
        }
    }

    internal void DeleteFlashcard()
    {
        Console.Clear();
        var getFlashcards = flashcardDatabaseManager.ReadFlashcards(currStack);
        if (getFlashcards == null || getFlashcards.Count() == 0)
        {
            AnsiConsole.WriteLine("There are no flashcards  in this stack");
            return;
        }
        var select = new SelectionPrompt<FlashCards>();
        select.AddChoices(getFlashcards);
        select.UseConverter(flashcardName => $"{flashcardName.Question} {flashcardName.Answer}");
        var selectedFlashcard = AnsiConsole.Prompt(select);
        if (AnsiConsole.Confirm($"Are you sure that you want to delete this flashcard: [bold]{selectedFlashcard.Question}[/] [bold]{selectedFlashcard.Answer}[/] \n"))
        {
            AnsiConsole.MarkupLine($" [bold]{selectedFlashcard.Question}[/] [bold]{selectedFlashcard.Answer}[/] was deleted\n\n");
        }

        flashcardDatabaseManager.DeleteFlashcard(selectedFlashcard);
    }

    internal bool InputFront(string flashCardQuestion)
    {
        var flashcards = flashcardDatabaseManager.ReadFlashcards(currStack);
        var isSameStack = false;

        foreach (var flashcard in flashcards)
        {
            if (flashCardQuestion.ToLower() == flashcard.Question.ToLower()) isSameStack = true;
        }
        return isSameStack;
    }

    internal bool InputBack(string flashCardAnswer)
    {
        var flashcards = flashcardDatabaseManager.ReadFlashcards(currStack);
        var isSameStack = false;

        foreach (var flashcard in flashcards)
        {
            if (flashCardAnswer.ToLower() == flashcard.Answer.ToLower()) isSameStack = true;
        }
        return isSameStack;
    }
}