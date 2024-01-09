using Flashcards.StevieTV.Helpers;
using Flashcards.StevieTV.Models;
using Flashcards.StevieTV.Database;
using Spectre.Console;

namespace Flashcards.StevieTV.UI;

internal static class ManageFlashCards
{
    private static Stack currentStack;
    public static void FlashCardsMenu(Stack stack)
    {
        currentStack = stack;
        var exitManageFlashCards = false;

        while (!exitManageFlashCards)
        {
            Console.Clear();
            AnsiConsole.WriteLine($"You are editing the '{currentStack.Name}' stack");
            PrintCurrentFlashCards();

            var menuSelection = new SelectionPrompt<string>();
            menuSelection.Title("Please choose an option from the list below");
            menuSelection.AddChoice("Return to Main Menu");
            menuSelection.AddChoice("Change the current stack");
            menuSelection.AddChoice("Add a Flash Card to this stack");
            menuSelection.AddChoice("Edit a Flash Card in this stack");
            menuSelection.AddChoice("Remove a Flash Card in this stack");

            var menuInput = AnsiConsole.Prompt(menuSelection);

            switch (menuInput)
            {
                case "Return to Main Menu":
                    exitManageFlashCards = true;
                    Menu.MainMenu();
                    break;
                case "Change the current stack":
                    ManageStacks.EditStack();
                    break;
                case "Add a Flash Card to this stack":
                    AddFlashCard();
                    break;
                case "Edit a Flash Card in this stack":
                    EditFlashCard();
                    break;
                case "Remove a Flash Card in this stack":
                    RemoveFlashCard();
                    break;
            }
        }
    }

    private static void PrintCurrentFlashCards()
    {
        var flashCards = FlashCardsManager.GetFlashCards(currentStack);

        var table = new Table
        {
            Border = TableBorder.Rounded,
            Title = new TableTitle(currentStack.Name)
        };

        table.AddColumn("ID");
        table.AddColumn("Front");
        table.AddColumn("Back");

        var idCounter = 1; 
        foreach (var flashCard in flashCards)
        {
            table.AddRow(idCounter++.ToString(), flashCard.Front, flashCard.Back);
        }

        AnsiConsole.Write(table);
    }

    private static void AddFlashCard()
    {
        var newFlashCardFront = AnsiConsole.Prompt(
            new TextPrompt<string>("Please enter the front of the Flash Card you would like to add (or press 0 to cancel):")
                .Validate(front => !CheckFlashCardExists(front.Trim()), "That card already exists")
        );
    
        if (newFlashCardFront.Trim() == "0") return;
    
        var newFlashCardBack = AnsiConsole.Prompt(
            new TextPrompt<string>($"Please enter the back for the '{newFlashCardFront.ToTitleCase()}' Flash Card you would like to add (or press 0 to cancel):")
        );
    
        if (newFlashCardBack.Trim() == "0") return;
        
        FlashCardsManager.Post(new FlashCardDTO
        {
            StackId = currentStack.StackId,
            Front = newFlashCardFront.ToTitleCase(),
            Back = newFlashCardBack.ToTitleCase()
        });
    }

    private static void EditFlashCard()
    {
        var flashCards = FlashCardsManager.GetFlashCards(currentStack);
    
        Console.Clear();
        
        var menuSelection = new SelectionPrompt<FlashCard>();
        menuSelection.Title("Which Flash Card would you like to edit");
        menuSelection.AddChoices(flashCards);
        menuSelection.AddChoice(new FlashCard{Id = 0, StackId = 0, Front = "Cancel and return to menu", Back = ""});
        menuSelection.UseConverter(displayName => $"{displayName.Front} {(!string.IsNullOrEmpty(displayName.Back) ? " / " + displayName.Back : "")}");
    
        var selectedFlashCard = AnsiConsole.Prompt(menuSelection);
    
        if (selectedFlashCard.StackId == 0) return;
        
        var newFlashCardFront = AnsiConsole.Prompt(
            new TextPrompt<string>("Please enter the new front of the Flash Card (or press 0 to cancel):")
                .DefaultValue(selectedFlashCard.Front)
                .Validate(front => !CheckFlashCardExists(front.Trim()), "That card already exists")
        );
    
        if (newFlashCardFront.Trim() == "0") return;
    
        var newFlashCardBack = AnsiConsole.Prompt(
            new TextPrompt<string>($"Please enter the back for the '{newFlashCardFront.ToTitleCase()}' Flash Card you would like to add (or press 0 to cancel):")
                .DefaultValue(selectedFlashCard.Back)
        );
    
        if (newFlashCardBack.Trim() == "0") return;

        
        if (AnsiConsole.Confirm($"This will update the stack ['{selectedFlashCard.Front.ToTitleCase()}' / '{selectedFlashCard.Back.ToTitleCase()}'] to ['{newFlashCardFront.ToTitleCase()}' / '{newFlashCardBack.ToTitleCase()}']".EscapeMarkup()))
        {
            FlashCardsManager.Update(selectedFlashCard, newFlashCardFront, newFlashCardBack);
        }
    }
    
    private static void RemoveFlashCard()
    {
        var flashCards = FlashCardsManager.GetFlashCards(currentStack);
    
        Console.Clear();
        
        var menuSelection = new SelectionPrompt<FlashCard>();
        menuSelection.Title("Which Flash Card would you like to remove");
        menuSelection.AddChoices(flashCards);
        menuSelection.AddChoice(new FlashCard{Id = 0, StackId = 0, Front = "Cancel and return to menu", Back = ""});
        menuSelection.UseConverter(displayName => $"{displayName.Front} {(!string.IsNullOrEmpty(displayName.Back) ? " / " + displayName.Back : "")}");
    
        var selectedFlashCard = AnsiConsole.Prompt(menuSelection);
    
        if (selectedFlashCard.StackId == 0) return;
        
        if (AnsiConsole.Confirm($"This will remove the stack '{selectedFlashCard.Front.ToTitleCase()}' / '{selectedFlashCard.Back.ToTitleCase()}'"))
        {
            FlashCardsManager.Delete(selectedFlashCard);
        }
    }

    private static bool CheckFlashCardExists(string front)
    {
        var flashCards = FlashCardsManager.GetFlashCards(currentStack);
        var newFlashCardFound = false;
    
        foreach (var flashCard in flashCards)
        {
            if (front.ToLower() == flashCard.Front.ToLower())
                newFlashCardFound = true;
        }
    
        return newFlashCardFound;
    }
}