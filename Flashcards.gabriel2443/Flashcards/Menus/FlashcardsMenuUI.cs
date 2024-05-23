using Flashcards.Database;
using Flashcards.Helpers;
using Flashcards.Menus;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.FlashcardsMenu;

internal class FlashcardsMenuUI
{
    private FlashcardDatabaseManager flashcardDatabaseManager = new FlashcardDatabaseManager();
    private StackDatabaseManager stackDatabaseManager = new StackDatabaseManager();

    internal void FlashCardsMenu(CardStack stack)

    {
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
                    AddFlashCard(stack);
                    break;

                case "View flashcards":
                    ViewFlashcards(stack);
                    break;

                case "Update a flash card":
                    UpdateFlashcards(stack);
                    break;

                case "Delete a flashcard":
                    DeleteFlashcard(stack);
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
        ViewFlashcards(selectedCardStack);
        UpdateFlashcards(selectedCardStack);
        DeleteFlashcard(selectedCardStack);
        AddFlashCard(selectedCardStack);
    }

    internal void AddFlashCard(CardStack stack)
    {
        Console.Clear();
        var flashcards = new FlashCards();

        if (stack.CardstackId == 0)
        {
            Console.WriteLine($"No stacks found");
            return;
        }
        flashcards.CardstackId = Convert.ToInt32(stack.CardstackId);

        var inputFront = AnsiConsole.Prompt(new TextPrompt<string>("Please enter the front of the card").Validate(input => !ValidateFlashcardsStacks.InputFront(input), "This question exists"));
        var inputBack = AnsiConsole.Prompt(new TextPrompt<string>("Please enter the back of the card").Validate(input => !ValidateFlashcardsStacks.InputBack(input), "This answer exists"));

        flashcards.Answer = inputBack;
        flashcards.Question = inputFront;

        flashcardDatabaseManager.AddFlashard(flashcards);
    }

    internal void ViewFlashcards(CardStack stack)
    {
        Console.Clear();

        var getFlashcards = flashcardDatabaseManager.ReadFlashcardsDTO(stack);
        int id = 1;

        if (getFlashcards.Count() == 0) Console.WriteLine("No flashcards found");
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

    internal void UpdateFlashcards(CardStack stack)
    {
        var getFlashcards = flashcardDatabaseManager.ReadFlahcards(stack);
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
            var inputFront = AnsiConsole.Prompt(new TextPrompt<string>("Please enter the updated front of the card").Validate(input => !ValidateFlashcardsStacks.InputFront(input), "This question exists"));

            var inputBack = AnsiConsole.Prompt(new TextPrompt<string>("Please enter the updated back of the card").Validate(input => !ValidateFlashcardsStacks.InputFront(input), "This answer exists"));
            flashcardDatabaseManager.UpdateFlashcards(selectedFlashcard, inputFront, inputBack);
        }
    }

    internal void DeleteFlashcard(CardStack stack)
    {
        Console.Clear();
        var getFlashcards = flashcardDatabaseManager.ReadFlahcards(stack);
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
}